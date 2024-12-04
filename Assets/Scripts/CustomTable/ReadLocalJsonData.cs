using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Cache;
using UnityEngine;

public static class ReadLocalJsonData
{
	public static Dictionary<string, string> TotalReadJsonString { get; private set; } = null;
	public static List<Excel.LanguageList> ReadableLanguage { get; private set; } = new List<Excel.LanguageList>();

	public static void CreateSheetList()
    {
		CreateSheetList(Excel.SheetList.Language);
	}

	private static void CreateSheetList(Excel.LanguageList Localization)
	{
		string value;
#if !LOCALFILEREAD
		var prefix = Path.Combine(Application.persistentDataPath, "export.csv");
		if (File.Exists(prefix))
			value = File.ReadAllText(prefix);
		else
#endif
		{
			var re = Resources.Load<TextAsset>("export");
			value = re.text;
		}
		TotalReadJsonString = NJson.Decode<Dictionary<string, string>>(Base64Zip.UnZip(value));

		ReadableLanguage.Clear();
		var enumList = Enum.GetValues(typeof(Excel.LanguageList)).Cast<Excel.LanguageList>();
		foreach(var obj in enumList)
        {
			if (TotalReadJsonString.TryGetValue(obj.ToString(), out _))
				ReadableLanguage.Add(obj);
        }

		if (TotalReadJsonString.TryGetValue(Localization.ToString(), out var readLocal))
			Excel.SheetList.CreateLocal(readLocal);
		else
			Excel.SheetList.CreateLocal(string.Empty);
	}

	public static void CreateDownloadFile()
	{
		var prefix = Path.Combine(Application.persistentDataPath, "export.csv");

		if (File.Exists(prefix))
			File.Delete(prefix);

		using (var fs = new FileStream(prefix, FileMode.Append, FileAccess.Write))
		{
			using (var sw = new StreamWriter(fs))
			{
				var jsonOb = NJson.Encode(TotalReadJsonString);
				var encrypt = Base64Zip.Zip(jsonOb);
				sw.WriteLine(encrypt);
				sw.Flush();
				sw.Close();
				sw.Dispose();
			}

			fs.Close();
			fs.Dispose();
		}

		TotalReadJsonString = null;
	}
}