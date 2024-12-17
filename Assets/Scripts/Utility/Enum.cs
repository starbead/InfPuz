using App.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.Enum
{
    public enum Blocks
    {
        NONE,
        RED,
        GREEN,
        BLUE,
        ORANGE,
        YELLOW,
        SKYBLUE,
    }
    public enum DynamicUI
    {
        NONE,
        ExitPopup,
        HelpPopup,
        ContinuePopup,
    }
    public enum LocalData
    {
        NONE,
        BESTSCORE,
        LOADSAVE,
    }
}

public static class Common
{
    public static string GetUIPath(DynamicUI ui)
    {
        switch(ui)
        {
            case DynamicUI.ExitPopup:
                return "Prefabs/Popups/ExitPopup";
            case DynamicUI.HelpPopup:
                return "Prefabs/Popups/HelpPopup";
            case DynamicUI.ContinuePopup:
                return "Prefabs/Popups/ContinuePopup";
        }
        return "";
    }
    public static string GetPlayerPrefs(LocalData code)
    {
        switch(code)
        {
            case LocalData.BESTSCORE:
                return "PUZZLE_BEST_SCORE";
            case LocalData.LOADSAVE:
                return "PUZZLE_LOAD_SAVEDATA";
        }
        return "";
    }
}