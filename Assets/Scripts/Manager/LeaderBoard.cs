using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class LeaderBoard : MonoBehaviour
{
    public void ShowLeaderBoard()
    {
        PlayGamesPlatform.Instance.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_countup);
            }
        });
    }
}
