using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseUI : MonoBehaviour
{
    public Action onFinished = null;

    protected virtual List<GameEventType> EventTypeList { get; } = new List<GameEventType>();
    
    public void Init(params object[] data)
    {
        gameObject.SetActive(true);

        foreach (var e in EventTypeList)
            GameEventSubject.RegisterHandler(e, HandleGameEvent);

        initChild(data);
    }

    protected virtual void initChild(params object[] data) { }
    protected virtual void EndPanel()
    {
        ClearObj();
        gameObject.SetActive(false);

        var copy = onFinished;
        onFinished = null;
        copy?.Invoke();

    }
    private void ClearObj()
    {
        ChildClearObj();
        foreach (var e in EventTypeList)
            GameEventSubject.UnregisterHandler(e, HandleGameEvent);
    }
    protected virtual void ChildClearObj() { }
    public void HandleGameEvent(GameEvent ge) => ChildHandleGameEvent(ge);
    protected virtual void ChildHandleGameEvent(GameEvent e) { }

    private void OnDestroy()
    {
        ClearObj();
    }
}
