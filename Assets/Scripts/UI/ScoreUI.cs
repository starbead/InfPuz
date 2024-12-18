using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ScoreUI : StaticUI
{
    [SerializeField] Text bestScoreLb = null;
    [SerializeField] Text scoreLb = null;

    int curScore = 0;
    protected override List<GameEventType> EventTypeList => new List<GameEventType>()
    {
        GameEventType.ChangeScore,
    };
    protected override void ChildHandleGameEvent(GameEvent e)
    {
        switch (e.eventType)
        {
            case GameEventType.ChangeScore:
                {
                    var score = e.ReadInt;
                    ChangeScore(score);
                }
                break;
        }
    }

    protected override void initChild(params object[] data)
    {
        ChangeScore(InGameManager.Instance.LoadScore);
    }
    void SetScore()
    {
        scoreLb.text = $"{curScore}";
    }
    void ChangeScore(int score)
    {
        scoreLb.text = $"{score}";
        curScore = score;
        CheckBestScore();
    }

    void CheckBestScore()
    {
        int bestScore = 0;
        string localData = Common.GetPlayerPrefs(App.Enum.LocalData.BESTSCORE);

        if (PlayerPrefs.HasKey(localData) == false)
            PlayerPrefs.SetInt(localData, 0);
        else
        {
            bestScore = PlayerPrefs.GetInt(localData);
        }

        if(curScore > bestScore)
        {
            PlayerPrefs.SetInt(localData, curScore);
            bestScore = curScore;
        }
        bestScoreLb.text = $"{bestScore}";
    }
}
