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
        }
        return "";
    }
}