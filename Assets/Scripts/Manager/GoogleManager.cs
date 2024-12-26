using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
public class GoogleManager : MonoBehaviour
{
    private void Start()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        SignIn();
    }
    public void SignIn()
    {
        if(PlayGamesPlatform.Instance.localUser.authenticated == false)
        {
            PlayGamesPlatform.Instance.localUser.Authenticate((bool success) =>
            {
                if (success)
                {

                }
            });
        }
    }
    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {

        }
        else
        {

        }
    }
}
