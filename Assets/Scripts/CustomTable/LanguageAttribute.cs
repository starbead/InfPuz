using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Excel
{
    public enum LanguageList
    {
        [Language(SystemLanguage.English, LanguageEnum.Language_English)]
        EN,
        [Language(SystemLanguage.Korean, LanguageEnum.Language_Korea)]
        KO
    }

    public class LanguageAttribute : Attribute
    {
        public LanguageEnum TitleText { get; private set; }
        public SystemLanguage TargetLanguage { get; private set; }
        public LanguageAttribute(SystemLanguage TargetLanguage , LanguageEnum TitleText)
        {
            this.TargetLanguage = TargetLanguage;
            this.TitleText = TitleText;
        }
    }

    public partial class SheetList
    {
        private const string PlayerLanguageKey = "userLanguage";
        private static LanguageList language = default;
        public static LanguageList GetLanguage => language;
        public static void ChangeLanguage(LanguageList lan)
        {
            if (ReadLocalJsonData.ReadableLanguage.Contains(lan) == false)
            {
                if(ReadLocalJsonData.ReadableLanguage.Count == 0)
                {
                    ReadLocalJsonData.CreateSheetList();
                    ChangeLanguage(lan);
                    return;
                }
                lan = LanguageList.EN;
            }
            language = lan;
            PlayerPrefs.SetString(PlayerLanguageKey, lan.ToString());
        }

        private static bool FirstSet = false;
        public static LanguageList Language
        {
            get
            {
                if (FirstSet)
                    return language;

                var getLanguage = PlayerPrefs.GetString(PlayerLanguageKey);
                if (!Enum.TryParse<LanguageList>(getLanguage, out var result) || !Enum.IsDefined(typeof(LanguageList), result))
                {
                    bool findLanguage = false;
                    var enumList = Enum.GetValues(typeof(LanguageList)).Cast<LanguageList>();
                    foreach(var obj in enumList)
                    {
                        var Info = AttributeParse.GetDescription<LanguageList, LanguageAttribute>(obj);
                        if (Info.TargetLanguage.Equals(Application.systemLanguage))
                        {
                            findLanguage = true;
                            result = obj;
                            break;
                        }
                    }

                    if (!findLanguage)
                        result = LanguageList.EN;
                }

                FirstSet = true;
                ChangeLanguage(result);
                return language;
            }
        }

        public static LanguageDic Localization { get; private set; }
        public static void CreateLocal(string origin)
        {
            try
            {
                Localization = LanguageDic.CreateDic(origin);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"{e.Message}\n{origin}");
                Application.Quit();
            }
            GameEventSubject.SendGameEvent(GameEventType.EndTableParse);
        }
    }
}

public static class AttributeParse
{
    private static Dictionary<string, Dictionary<Enum, Attribute>> AttributeList = new Dictionary<string, Dictionary<Enum, Attribute>>();
    public static Q GetDescription<W, Q>(W en) where W : Enum where Q : Attribute
    {
        var type = en.GetType();
        var typeName = type.ToString();
        if (!AttributeList.TryGetValue(typeName, out var List))
        {
            List = new Dictionary<Enum, Attribute>();
            AttributeList.Add(typeName, List);
        }

        if (List.TryGetValue(en, out var result))
            return result as Q;

        var memInfo = type.GetMember(en.ToString());
        if (memInfo != null && memInfo.Length > 0)
        {
            var attrs = memInfo[0].GetCustomAttributes(typeof(Q), false);
            if (attrs != null && attrs.Length > 0)
            {
                var getValue = attrs[0] as Q;
                List.Add(en, getValue);
                return getValue;
            }
        }

        return null;
    }
}