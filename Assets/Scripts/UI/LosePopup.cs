using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LosePopup : StaticUI
{
    [SerializeField] Text ScoreLb = null;

    protected override List<GameEventType> EventTypeList => new List<GameEventType>()
    {
        GameEventType.GameEnd,
    };
    protected override void ChildHandleGameEvent(GameEvent e)
    {
        switch(e.eventType)
        {
            case GameEventType.GameEnd:
                {
                    gameObject.SetActive(true);
                    var score = e.ReadInt;
                    SetGameScore(score);
                }
                break;
        }
    }
    protected override void initChild(params object[] data)
    {
        gameObject.SetActive(false);
    }
    void SetGameScore(int score)
    {
        ScoreLb.text = $"{score}";
    }
    public void OnClick_ReStart()
    {
        InGameManager.Instance.ReSetStage();
        gameObject.SetActive(false);
    }
}
