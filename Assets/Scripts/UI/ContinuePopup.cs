using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinuePopup : BaseUI
{
    protected override List<GameEventType> EventTypeList => new List<GameEventType>();
    protected override void ChildHandleGameEvent(GameEvent e)
    {
    }

    protected override void initChild(params object[] data)
    {
    }

    public void OnClick_Resume()
    {
        PlayerPrefs.SetInt(Common.GetPlayerPrefs(App.Enum.LocalData.LOADSAVE), 1);
        SceneManager.LoadScene("Game");
        EndPanel();
    }
    public void OnClick_NewGame()
    {
        PlayerPrefs.SetInt(Common.GetPlayerPrefs(App.Enum.LocalData.LOADSAVE), 0);
        SceneManager.LoadScene("Game");
        EndPanel();
    }
}
