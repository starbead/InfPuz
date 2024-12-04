using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTitle : StaticUI
{
    [SerializeField] GameObject TapLb = null;

    protected override List<GameEventType> EventTypeList => new List<GameEventType>()
    { GameEventType.InitializeProgress, GameEventType.InitializeFinish };
    protected override void ChildHandleGameEvent(GameEvent e)
    {
        switch(e.eventType)
        {
            case GameEventType.InitializeProgress:
                break;
            case GameEventType.InitializeFinish:
                TapLb.SetActive(true);
                break;
        }   
    }

    protected override void initChild(params object[] data)
    {
        TapLb.SetActive(false);
    }
}
