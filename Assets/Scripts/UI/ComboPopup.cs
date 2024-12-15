using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboPopup : StaticUI
{
    [SerializeField] Text tweenText = null;
    [SerializeField] RectTransform objTF = null;

    public Vector3 originPos = new Vector3(0, 0, 0);
    public Vector3 toPos = new Vector3(0, 350f, 0);
    protected override List<GameEventType> EventTypeList => new List<GameEventType>() { GameEventType.EffectCombo };
    protected override void ChildHandleGameEvent(GameEvent e)
    {
        PlayEffect();
    }
    protected override void initChild(params object[] data)
    {
        ResetPos();
        gameObject.SetActive(false);
    }
    void PlayEffect()
    {
        ReSetAlpha();
        var tween = LeanTween.moveLocal(tweenText.gameObject, toPos, 0.6f);
        tween.setEase(LeanTweenType.easeInQuad);
        tween.setOnComplete(() =>
        {
            ResetPos();
        });

        var c = tweenText.color;
        c.a = 0f;
        LeanTween.textColor(objTF, c, 0.4f);
    }
    void ResetPos()
    {
        objTF.localPosition = originPos;
    }
    void ReSetAlpha()
    {
        gameObject.SetActive(true);
        var c = tweenText.color;
        c.a = 1f;
        tweenText.color = c;
    }
}
