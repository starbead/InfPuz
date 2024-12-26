using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
public class GoogleManager : MonoBehaviour
{
    private void Start()
    {
        //PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        //SignIn();
    }
    public void SignIn()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
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
