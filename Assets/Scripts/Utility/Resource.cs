using System;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceState
{
    RemoveBeforeLeftScene,
    Popup,
    PoolObject,
    InGameUnit,
}

public static class Resource
{
    private static readonly Dictionary<string, Item> prefabs = new Dictionary<string, Item>();
    private static readonly Dictionary<string, Item> flushScenePrefabs = new Dictionary<string, Item>();
    public static void Clear()
    {
        prefabs.Clear();
        flushScenePrefabs.Clear();
        Item.ClearTerminatedParent();
    }

    public static GameObject LoadResource(string name, ResourceState state, Transform BaseParent, int maxItemCount = 0)
    {
        Item result;
        switch (state)
        {
            case ResourceState.RemoveBeforeLeftScene:
                if (!flushScenePrefabs.TryGetValue(name, out result))
                {
                    GameObject prefab = (GameObject)Resources.Load(name);
                    if (prefab == null)
                    {
                        Debug.LogError($"__not_found_prefab:{name}");
                        return LoadResource("dummy", state, BaseParent, maxItemCount);
                    }
                    result = new Item(name, prefab);
                    flushScenePrefabs.Add(name, result);
                }
                break;
            default:
                if (!prefabs.TryGetValue(name, out result))
                {
                    GameObject prefab = (GameObject)Resources.Load(name);
                    if (prefab == null)
                    {
                        Debug.LogError($"__not_found_prefab:{name}");
                        return LoadResource("dummy", state, BaseParent, maxItemCount);
                    }
                    result = new Item(name, prefab);
                    prefabs.Add(name, result);
                }
                break;
        }

        return result.Get(true, BaseParent, state, maxItemCount);
    }

    public static bool Release(GameObject go, bool bTerminated = false)
    {
        if (go == null)
            return false;

        var oid = go.GetComponent<ObjectID>();
        if (oid == null || oid.Name == null)
            return false;

        if (oid.isRemoveSceneLoad)
        {
            if (flushScenePrefabs.TryGetValue(oid.Name, out var value))
            {
                if (value != null)
                {
                    value.Free(go, bTerminated);
                    return true;
                }
                flushScenePrefabs.Remove(oid.name);
            }
        }
        else
        {
            if (prefabs.TryGetValue(oid.Name, out var value))
            {
                if (value != null)
                {
                    value.Free(go, bTerminated);
                    return true;
                }
                prefabs.Remove(oid.name);
            }
        }

        return false;
    }

    public static void FlushPrefabsWhenSceneChanged()
    {
        foreach(var item in flushScenePrefabs.Values)
        {
            item.FlushAllObject();
        }
    }

    #region load audio resource
    private static Dictionary<string, AudioClip> pathDic = new Dictionary<string, AudioClip>();
    public static AudioClip LoadAudioClip(string path)
    {
        if (!pathDic.TryGetValue(path, out var audio))
        {
            audio = Resources.Load<AudioClip>(path);
            if (audio == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning($"Sound 리소스 찾기 실패 : {path}");
#endif
                return null;
            }
            pathDic.Add(path, audio);
        }
        return audio;
    }
    #endregion
}

public class ObjectID : MonoBehaviour
{
    public string Name;
    public bool isRemoveSceneLoad;
}

public class Item
{
    private int currentCount = 0;
    private readonly string name;
    private readonly string postfix;
    private readonly GameObject prefab;
    private readonly Queue<GameObject> available = new Queue<GameObject>();
    private readonly List<GameObject> inUse = new List<GameObject>();

    public Item(string name, GameObject prefab)
    {
        this.name = name;
        this.prefab = prefab;
        string[] ary = name.Split('/');
        postfix = ary[ary.Length - 1];
    }

    public GameObject Get(bool bPooled, Transform BaseParent, ResourceState state, int maxItemCount)
    {
        GameObject po;
        if (0 < maxItemCount && maxItemCount <= inUse.Count)
        {
            if (inUse.Count <= currentCount)
                currentCount = 0;

            var currentObj = inUse[currentCount];
            if (currentObj != null && currentObj.activeInHierarchy)
            {
                currentCount++;
                return currentObj;
            }

            inUse.RemoveAt(currentCount);
        }

        if (available.Count != 0)
        {
            po = available.Dequeue();
            if (po == null)
                return Get(bPooled, BaseParent, state, maxItemCount);

            inUse.Add(po);
            po.transform.SetParent(BaseParent);
        }
        else
        {
            po = UnityEngine.Object.Instantiate(prefab, BaseParent);
            po.tag = state.ToString();
            po.name = postfix;
            if (bPooled)
            {
                var oid = po.AddComponent<ObjectID>();
                oid.Name = name;
                oid.isRemoveSceneLoad = state == ResourceState.RemoveBeforeLeftScene;
                inUse.Add(po);
            }
        }

        po.SetActive(true);
        po.transform.localPosition = Vector3.zero;
        po.transform.localScale = Vector3.one;
        return po;
    }

    private static bool FirstCall = false;
    public static Transform TerminatedParent { get; private set; } = null;
    public void Free(GameObject po, bool bTerminated)
    {
        if (po == null)
            return;

        if (!bTerminated && FirstCall && TerminatedParent == null)
            bTerminated = true;

        inUse.Remove(po);

        if (bTerminated == false)// && poolComp != null && poolComp.isNotDestory)
        {
#if UNITY_EDITOR
            if (available.Contains(po))
            {
                Debug.LogError($"{po.name}: 중복으로 Free가 호출되었습니다");
                return;
            }
#endif

            po.SetActive(false);
            available.Enqueue(po);

            if (TerminatedParent == null)
            {
                FirstCall = true;
                var newGo = new GameObject("TerminatedParent");
                newGo.SetActive(false);
                UnityEngine.Object.DontDestroyOnLoad(newGo);
                TerminatedParent = newGo.transform;
            }

            po.transform.SetParent(TerminatedParent);
        }
        else
            UnityEngine.Object.Destroy(po);
    }

    public static void ClearTerminatedParent()
    {
        FirstCall = false;
        if (TerminatedParent != null)
            UnityEngine.Object.Destroy(TerminatedParent.gameObject);
        TerminatedParent = null;
    }

    public void FlushAllObject()
    {
        while (available.Count > 0)
            GameObject.Destroy(available.Dequeue());
        foreach (var obj in inUse)
            GameObject.Destroy(obj);

        inUse.Clear();
        available.Clear();
        currentCount = 0;
    }
}