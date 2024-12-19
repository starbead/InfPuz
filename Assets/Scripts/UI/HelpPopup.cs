using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpPopup : BaseUI
{
    [SerializeField] Image[] control_img = null;

    protected override List<GameEventType> EventTypeList => new List<GameEventType>();
    protected override void ChildHandleGameEvent(GameEvent e)
    {
    }
    protected override void initChild(params object[] data)
    {
        if(InGameManager.Instance != null)
            InGameManager.Instance.SetClickStatus(false);

        foreach(var item in control_img)
        {
            var c = item.color;
            c.a = 1f;
            item.color = c;
        }

        for (int i = 0; i < control_img.Length; i++)
        {
            LeanTween.alpha(control_img[i].rectTransform, 0f, 1f).setLoopPingPong();
        }
    }
    public void OnClick_Close()
    {
        EndPanel();
        if (InGameManager.Instance != null)
            InGameManager.Instance.SetClickStatus(true);
    }
}
