using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LosePopup : StaticUI
{
    [SerializeField] Text ScoreLb = null;

    float delayButtonTimer = 1f;
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
                    delayButtonTimer = 1f;
                    var score = e.ReadInt;
                    SetGameScore(score);
                    RecordLeaderBoard(score);
                    InGameManager.Instance.DeleteData();
                    GameManager.Instance.adManager.ShowInterstitialAd();
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
    void RecordLeaderBoard(int score)
    {
        PlayGamesPlatform.Instance.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                PlayGamesPlatform.Instance.ReportScore(score, GPGSIds.leaderboard_countup, (bool success) =>
                {
                    if (success)
                    {

                    }
                });
            }
        });
    }
    public void OnClick_ReStart()
    {
        if (delayButtonTimer > 0) return;
        InGameManager.Instance.ReSetStage();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(delayButtonTimer > 0)
        {
            delayButtonTimer -= Time.deltaTime;
        }
    }
}
