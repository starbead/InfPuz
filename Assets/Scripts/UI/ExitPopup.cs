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
        if(InGameManager.Instance != null)
            InGameManager.Instance.SetClickStatus(false);
    }
    public void OnClick_Close()
    {
        if(InGameManager.Instance != null)
            InGameManager.Instance.SetClickStatus(true);

        EndPanel();
    }
    public void OnClick_Exit()
    {
        Application.Quit();
    }
}
