using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneList
{
    InitScene,
    IngameScene,
    IngamePhoton,

    GrayScreen = 99,
}


public class GameManager : MonoSingleton<GameManager>
{
    //[SerializeField] private Camera Camera = null;
    [SerializeField] public Excel.LanguageList CustomLanguage = Excel.LanguageList.KO;

    public AdManager adManager = null;
    public static SceneList Mode { get; private set; }

    public bool IsMainThreadNow()
    {
        return mainThreadId == System.Threading.Thread.CurrentThread.ManagedThreadId;
    }
    int mainThreadId = 0;
    protected override void ChildAwake()
    {
        mainThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
        Input.multiTouchEnabled = true;
        Mode = SceneList.InitScene;

        DontDestroyOnLoad(gameObject);
        //DontDestroyOnLoad(Camera);
    }
    protected override void ChildOnDestroy()
    {
    }
#if !UNITY_IOS
    private float delayTime = 0f;
#endif
#if INTERNAL_TEST
    public static int CheatAddDay = 0;
#endif

    private void Update()
    {
        mainThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
        GameEventSubject.CheckQueueingEvent();

#if !UNITY_IOS
        if ((Time.realtimeSinceStartup - delayTime) > 1f && Input.GetKey(KeyCode.Escape))
        {
            UIManager.instance.ShowUI(App.Enum.DynamicUI.ExitPopup);
            delayTime = Time.realtimeSinceStartup;
        }
#endif
    }

    private void OnApplicationQuit()
    {
        Application.CancelQuit();
#if !UNITY_EDITOR
        System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
    }

    static public long GetParse_Long(object objData)
    {
        long.TryParse(objData.ToString(), out var result);
        return result;
    }

    static public int GetParse_Int(object objData)
    {
        int.TryParse(objData.ToString(), out var result);
        return result;
    }

    static public float GetParse_Float(object objData)
    {
        float.TryParse(objData.ToString(), out var result);
        return result;
    }

    static public bool GetParse_Bool(object objData)
    {
        bool.TryParse(objData.ToString(), out var result);
        return result;
    }
}