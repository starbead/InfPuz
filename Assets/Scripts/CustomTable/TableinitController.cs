using UnityEngine;

public class TableinitController : MonoBehaviour
{
    public enum eState
    {
        First,
        LocalTable,
        Maintenence,
        OK,

        UPDATE = 98,
        ERROR = 99,
    }

    public eState NowState = eState.First;
    string message = string.Empty;

    private void Start()
    {
        initData();
    }

    private void GoNextState()
    {
        switch (NowState)
        {
            case eState.First:
                NowState++;
                GoNextState();
                break;
            case eState.LocalTable:
                {
                    ReadLocalJsonData.CreateSheetList();
                    try
                    {
                        new Excel.SheetList(ReadLocalJsonData.TotalReadJsonString, Excel.SheetList.Language.ToString());
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"Fail to parse export.csv: {e}");
                    }
                    NowState++;
                    GoNextState();
                }
                break;
            case eState.Maintenence:
                NowState++;
                GoNextState();
                break;
            case eState.OK:
                GameEventSubject.SendGameEvent(GameEventType.InitializeFinish);
                return;
            case eState.ERROR:
                Debug.LogError($"ERROR: {message}");
                break;
            case eState.UPDATE:
                Debug.LogError($"NEED UPDATE: {message}");
                break;
        }
        GameEventSubject.SendGameEvent(GameEventType.InitializeProgress, (int)NowState);
    }
    private void Reset()
    {
        message = string.Empty;
        NowState = eState.First;
    }

    void initData()
    {
        NowState = eState.First;
        GoNextState();
    }
}
