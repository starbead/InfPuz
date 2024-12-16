using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPopup : BaseUI
{
    protected override List<GameEventType> EventTypeList => new List<GameEventType>();
    protected override void ChildHandleGameEvent(GameEvent e)
    {
    }
    protected override void initChild(params object[] data)
    {
        InGameManager.instance.SetClickStatus(false);
    }
    public void OnClick_Close()
    {
        InGameManager.instance.SetClickStatus(true);
        EndPanel();
    }
    public void OnClick_Exit()
    {
        Application.Quit();
    }
}
