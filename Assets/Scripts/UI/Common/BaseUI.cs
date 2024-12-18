using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BaseUI : MonoBehaviour
{
    public Action onFinished = null;
    public App.Enum.DynamicUI uiType { get; private set; } = App.Enum.DynamicUI.NONE;
    protected virtual List<GameEventType> EventTypeList { get; } = new List<GameEventType>();

    private void Start()
    {
        SetButtonSound();
    }
    public void Init(params object[] data)
    {
        this.gameObject.SetActive(true);
        
        foreach (var e in EventTypeList)
            GameEventSubject.RegisterHandler(e, HandleGameEvent);

        initChild(data);
    }
    public void SetEnumData(App.Enum.DynamicUI ui) => uiType = ui;
    protected virtual void initChild(params object[] data) { }
    protected virtual void EndPanel()
    {
        ClearObj();
        this.gameObject.SetActive(false);

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

    void SetButtonSound()
    {
        var buttons = FindObjectsOfType<Button>();
        foreach(var button in buttons)
        {
            button.onClick.RemoveListener(PlayButtonSound);
            button.onClick.AddListener(PlayButtonSound);
        }
    }
    void PlayButtonSound()
    {
        SoundManager.Instance.PlayEffect("Sounds/UI/Button");
    }
}
