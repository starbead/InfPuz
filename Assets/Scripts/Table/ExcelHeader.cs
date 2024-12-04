using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

class WritablePropertiesOnlyResolver : DefaultContractResolver
{
	protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
	{
		IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);
		return props.Where(p => p.Writable).ToList();
	}
}

public class NJson
{
	private static JsonSerializerSettings SerializerSettings = null;
	private static JsonSerializerSettings sSeting
	{
		get
		{
			if (SerializerSettings == null)
			{
				SerializerSettings = new JsonSerializerSettings
				{
					ContractResolver = new WritablePropertiesOnlyResolver(),
					Error = (sender, args) =>
					{
						Debug.LogWarning($"{args.ErrorContext.Error.Message}");
						args.ErrorContext.Handled = true;
					}
				};
			}

			return SerializerSettings;
		}
	}

	public static string Encode(object p_Object)
    {
		return JsonConvert.SerializeObject(p_Object, sSeting);
	}

	public static T Decode<T>(string p_Value)
    {
		return JsonConvert.DeserializeObject<T>(p_Value, sSeting);
	}
}

[Serializable]
[JsonConverter(typeof(LocalizeStringConveter))]
[TypeConverter(typeof(LocalizeStringTypeConverter))]
public class LocalizeString
{
    public readonly string Text;
	public LocalizeString(string Text)
	{
        this.Text = Text;
    }

	public static implicit operator string(LocalizeString _val) => _val.ToString();
    public override string ToString()
    {
        if (string.IsNullOrEmpty(Text) || Text.Equals("0"))
            return string.Empty;

        if (Excel.SheetList.Localization.TryGetValue(Text, out var result))
            return result;

        return Text;
    }
}

public class LocalizeStringConveter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(LocalizeString);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String)
            return new LocalizeString((string)reader.Value);

        return null;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var intvalue = (LocalizeString)value;
        writer.WriteValue(intvalue.Text);
    }
}

public class LocalizeStringTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        if (sourceType == typeof(LocalizeString))
            return true;

        return base.CanConvertFrom(context, sourceType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value is string stringValue)
            return new LocalizeString(stringValue);

        return base.ConvertFrom(context, culture, value);
    }
}

public abstract class FilterText
{
	private Regex collection = null;
	private Regex chainCollection = null;

	private const string BlankString = "0";
	protected void TableParseEnd(Dictionary<int, ReadOnlyCollection<string>> basedic)
	{
		var allString = new List<string>();
		foreach (var obj in basedic)
		{
			foreach (var value in obj.Value)
			{
				if (string.IsNullOrEmpty(value))
					continue;

				if (BlankString.Equals(value))
					continue;

				if (allString.Contains(value))
					continue;

				allString.Add(value);
			}
		}

		var sp = string.Join("|", allString.Select(word => string.Join(@"\s*", word.ToCharArray())));
		collection = new Regex($@"\b({sp})\b", RegexOptions.IgnoreCase);
		chainCollection = new Regex($@"({string.Join("|", allString)})", RegexOptions.IgnoreCase);
	}

	public string Filter(string input)
	{
		var value = collection.Replace(input, match => new string('*', match.Length));
		return chainCollection.Replace(value, match => new string('*', match.Length));
	}
}

public abstract class CustomExcel
{
	protected virtual void GenerateTableParse() { }
	protected void TableParseEnd()
	{
		GenerateTableParse();
		GameEventSubject.SendGameEvent(GameEventType.EndTableParse);
	}
}

public abstract class CustomDic<T, X> : System.Collections.ObjectModel.ReadOnlyDictionary<T, X>
{
	public CustomDic(Dictionary<T, X> basedic) : base(basedic)
	{
		TableParseEnd();
	}

	protected virtual void GenerateTableParse() { }
	private void TableParseEnd()
    {
		GenerateTableParse();
	}
}

namespace Excel
{
	public partial class SheetList
	{
		public static SheetList Inst { get; private set; } = null;
		public readonly CONSTDic dic_CONST;
		public readonly CONST_STRINGDic dic_CONST_STRING;
		public readonly SoundDic dic_Sound;

		public SheetList(Dictionary<string, string> jsonDicList, string Local)
		{
			Inst = null;
			string value;
			if (jsonDicList.TryGetValue("CONST", out value))
				dic_CONST = CONSTDic.CreateDic(value);
			if (jsonDicList.TryGetValue("CONST_STRING", out value))
				dic_CONST_STRING = CONST_STRINGDic.CreateDic(value);
			if (jsonDicList.TryGetValue("Sound", out value))
				dic_Sound = SoundDic.CreateDic(value);
			if (jsonDicList.TryGetValue(Local, out value))
				CreateLocal(value);
			CustomCreate();
			Inst = this;
		}
	}
}
