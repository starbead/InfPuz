using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainTitle : StaticUI
{
    [SerializeField] GameObject TapObj = null;

    protected override List<GameEventType> EventTypeList => new List<GameEventType>()
    { GameEventType.InitializeProgress, GameEventType.InitializeFinish };
    protected override void ChildHandleGameEvent(GameEvent e)
    {
        switch(e.eventType)
        {
            case GameEventType.InitializeProgress:
                break;
            case GameEventType.InitializeFinish:
                TapObj.SetActive(true);
                break;
        }   
    }

    protected override void initChild(params object[] data)
    {
        TapObj.SetActive(true);
    }

    public void OnClick_Play()
    {
        SceneManager.LoadScene("Game");
    }
    public void OnClick_Help()
    {
        UIManager.instance.ShowUI(App.Enum.DynamicUI.HelpPopup);
    }
}
