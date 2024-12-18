using UnityEngine;

public class GameOption
{
    static float effectValue = 1f;
    static float bgmValue = 1f;

    public static float EffectSound
    {
        get
        {
            effectValue = PlayerPrefs.GetFloat(Common.GetPlayerPrefs(App.Enum.LocalData.EffectSound), 1f);
            return effectValue;
        }
    }
    public static float BgmSound
    {
        get
        {
            bgmValue = PlayerPrefs.GetFloat(Common.GetPlayerPrefs(App.Enum.LocalData.BGMSound), 1f);
            return bgmValue;
        }
    }

    public static void SetEffectValue(float value)
    {
        effectValue = value;
        PlayerPrefs.SetFloat(Common.GetPlayerPrefs(App.Enum.LocalData.EffectSound), value);
    }
    public static void SetBgmValue(float value)
    {
        bgmValue = value;
        PlayerPrefs.SetFloat(Common.GetPlayerPrefs(App.Enum.LocalData.BGMSound), value);
    }
}