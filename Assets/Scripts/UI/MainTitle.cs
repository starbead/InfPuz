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
        SoundManager.Instance.PlayBGM("Sounds/Bgm/Zachz_Winner-blu");
    }

    public void OnClick_Play()
    {
        var value = PlayerPrefs.GetInt(Common.GetPlayerPrefs(App.Enum.LocalData.LOADSAVE), 0);
        if(value == 0)
        {
            PlayerPrefs.SetInt(Common.GetPlayerPrefs(App.Enum.LocalData.LOADSAVE), 0);
            SceneManager.LoadScene("Game");
        }
        else
            UIManager.instance.ShowUI(App.Enum.DynamicUI.ContinuePopup);
    }
}
