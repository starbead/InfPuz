using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
        ChangeScore(0);
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
        if (PlayerPrefs.HasKey("PUZZLE_BEST_SCORE") == false)
            PlayerPrefs.SetInt("PUZZLE_BEST_SCORE", 0);
        else
        {
            bestScore = PlayerPrefs.GetInt("PUZZLE_BEST_SCORE");
        }

        if(curScore > bestScore)
        {
            PlayerPrefs.SetInt("PUZZLE_BEST_SCORE", curScore);
            bestScore = curScore;
        }
        bestScoreLb.text = $"{bestScore}";
    }
}
