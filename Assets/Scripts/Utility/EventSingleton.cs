using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventSingleton<T> : GameEventHandler where T: MonoBehaviour
{
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<T>(true);
            if (instance == null)
                instance = new GameObject(nameof(T)).AddComponent<T>();
            return instance;
        }
    }

    protected void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = gameObject.GetComponent<T>();
        UpdateAllInfo();
        ChildAwake();
    }

    protected abstract void ChildAwake();

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (instance != null && instance.GetHashCode() == GetHashCode())
        {
            instance = null;
            ChildOnDestroy();
        }
    }
    protected abstract void ChildOnDestroy();
}
