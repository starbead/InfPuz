using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopup : BaseUI
{
    [SerializeField] Slider effectSlider = null;
    [SerializeField] Slider bgmSlider = null;

    protected override List<GameEventType> EventTypeList => new List<GameEventType>();
    protected override void ChildHandleGameEvent(GameEvent e)
    {
    }
    protected override void initChild(params object[] data)
    {
        initData();
        if (InGameManager.Instance != null)
            InGameManager.Instance.SetClickStatus(false);
    }
    void initData()
    {
        effectSlider.value = GameOption.EffectSound;
        bgmSlider.value = GameOption.BgmSound;
    }
    public void BGMValueChanged()
    {
        GameOption.SetBgmValue(bgmSlider.value);
        SoundManager.Instance.SetBGMVolume(bgmSlider.value);
    }
    public void EffectValueChanged()
    {
        GameOption.SetEffectValue(effectSlider.value);
        SoundManager.Instance.SetEffectVolume(effectSlider.value);
    }
    public void OnClick_Close()
    {
        EndPanel();

        if (InGameManager.Instance != null)
            InGameManager.Instance.SetClickStatus(true);
    }
}
