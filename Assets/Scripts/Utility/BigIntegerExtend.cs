using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Numerics;

[Serializable]
[JsonConverter(typeof(BigIntegerExtendConveter))]
[TypeConverter(typeof(BigIntegerExtendTypeConverter))]
public class BigIntegerExtend
{
    public const ulong AddDumy = 100;
    private BigInteger bigInteger = 0;
    public BigIntegerExtend() { }
    public BigIntegerExtend(long va) { bigInteger = new BigInteger(va) * AddDumy; }
    public BigIntegerExtend(ulong va) { bigInteger = new BigInteger(va) * AddDumy; }
    public BigIntegerExtend(int va) { bigInteger = new BigInteger(va) * AddDumy; }
    public BigIntegerExtend(uint va) { bigInteger = new BigInteger(va) * AddDumy; }
    public BigIntegerExtend(float va) => ConvertToString($"{va.ToString("F")}");
    public BigIntegerExtend(double va) => ConvertToString($"{va.ToString("0." + new string('#', 339))}");
    public BigIntegerExtend(BigInteger va) { bigInteger = va * AddDumy; }
    public BigIntegerExtend(string va) => ConvertToString(va);

    private void ConvertToString(string va)
    {
        if (BigInteger.TryParse(va, out bigInteger))
        {
            bigInteger *= AddDumy;
            return;
        }

        if (string.IsNullOrEmpty(va))
            return;

        var split = va.Split('.');
        if (split.Length != 2)
        {
#if UNITY_EDITOR
            Debug.LogError($"BigInteger Parse Fail : {va}");
#endif
            return;
        }

        va = split[0];
        if (!BigInteger.TryParse(va, out bigInteger))
        {
#if UNITY_EDITOR
            Debug.LogError($"BigInteger Parse Fail : {va}");
#endif
            return;
        }

        bigInteger *= AddDumy;
        va = split[1];
        if (2 < va.Length)
            va = va.Remove(2, va.Length - 2);
        else if (va.Length < 2)
            va = $"{va}0";

        if (!BigInteger.TryParse(va, out var dumy))
        {
#if UNITY_EDITOR
            Debug.LogError($"BigInteger Parse Fail Dumy : {split[1]}");
#endif
            return;
        }

        bigInteger += dumy;
    }

    public static implicit operator BigIntegerExtend(long value)
    {
        return new BigIntegerExtend(value);
    }

    public static implicit operator BigIntegerExtend(ulong value)
    {
        return new BigIntegerExtend(value);
    }

    public static implicit operator BigIntegerExtend(int value)
    {
        return new BigIntegerExtend(value);
    }

    public static implicit operator BigIntegerExtend(uint value)
    {
        return new BigIntegerExtend(value);
    }

    public static implicit operator BigIntegerExtend(float value)
    {
        return new BigIntegerExtend(value);
    }

    public static implicit operator BigIntegerExtend(string value)
    {
        return new BigIntegerExtend(value);
    }

    public static implicit operator BigIntegerExtend(double value)
    {
        return new BigIntegerExtend(value);
    }
    public static implicit operator BigIntegerExtend(BigInteger value)
    {
        return new BigIntegerExtend(value);
    }

    public float ToFloat()
    {
        // Only Use on Safe Code
        return ((float)bigInteger / AddDumy);
    }

    public int ToInt()
    {
        // Only Use on Safe Code
        return ((int)bigInteger / (int)AddDumy);
    }

    public ulong ToLong()
    {
        // Only Use on Safe Code
        return ((ulong)bigInteger / (ulong)AddDumy);
    }

    public static implicit operator float(BigIntegerExtend value)
    {
        return ((float)value.bigInteger / AddDumy);
    }

    public static implicit operator int(BigIntegerExtend value)
    {
        return ((int)value.bigInteger / (int)AddDumy);
    }

    public static implicit operator ulong(BigIntegerExtend value)
    {
        return ((ulong)value.bigInteger / AddDumy);
    }

    public static BigIntegerExtend operator +(BigIntegerExtend bi1, BigIntegerExtend bi2)
    {
        return new BigIntegerExtend() { bigInteger = BigInteger.Add(bi1.bigInteger, bi2.bigInteger) };
    }

    public static BigIntegerExtend operator -(BigIntegerExtend bi1, BigIntegerExtend bi2)
    {
        return new BigIntegerExtend() { bigInteger = BigInteger.Subtract(bi1.bigInteger, bi2.bigInteger) };
    }

    public static BigIntegerExtend operator /(BigIntegerExtend bi1, BigIntegerExtend bi2)
    {
        return new BigIntegerExtend() { bigInteger = BigInteger.Divide(bi1.bigInteger * AddDumy, bi2.bigInteger) };
    }

    public static BigIntegerExtend operator *(BigIntegerExtend bi1, BigIntegerExtend bi2)
    {
        return new BigIntegerExtend() { bigInteger = BigInteger.Divide(BigInteger.Multiply(bi1.bigInteger, bi2.bigInteger), AddDumy) };
    }

    public static bool operator <(BigIntegerExtend bi1, BigIntegerExtend bi2)
    {
        return bi1.bigInteger.CompareTo(bi2.bigInteger) < 0;
    }

    public static bool operator >(BigIntegerExtend bi1, BigIntegerExtend bi2)
    {
        return 0 < bi1.bigInteger.CompareTo(bi2.bigInteger);
    }

    public static bool operator <=(BigIntegerExtend bi1, BigIntegerExtend bi2)
    {
        return bi1.bigInteger.CompareTo(bi2.bigInteger) <= 0;
    }

    public static bool operator >=(BigIntegerExtend bi1, BigIntegerExtend bi2)
    {
        return 0 <= bi1.bigInteger.CompareTo(bi2.bigInteger);
    }

    public static bool operator ==(BigIntegerExtend bi1, BigIntegerExtend bi2)
    {
        return bi1.bigInteger.CompareTo(bi2.bigInteger) == 0;
    }

    public static bool operator !=(BigIntegerExtend bi1, BigIntegerExtend bi2)
    {
        return bi1.bigInteger.CompareTo(bi2.bigInteger) != 0;
    }

    private string GetDumyString(bool removeDumy, string format = "")
    {
        if (bigInteger == 0)
            return "0";

        var reS = bigInteger / AddDumy;
        if (removeDumy)
        {
            if (reS < AddDumy)
                return reS.ToString();

            return reS.ToString(format);
        }

        var returnV = bigInteger.ToString();
        //가장 뒷자리가 연속으로 00 일 경우 삭제. 100 -> 1
        if (returnV.EndsWith("00"))
        {
            if (reS < AddDumy)
                return reS.ToString();

            return reS.ToString(format);
        }

        // 두번 연속 뒷자리를 삭제 안했으면 . 을 삽입한다. 101 -> 1.01
        var dumyString = returnV.Substring(returnV.Length - 2, 2);
        returnV = $"{reS.ToString(format)}.{dumyString}";
        if (returnV.EndsWith("0")) //가장 뒷자리가 0일 경우 삭제. 1.10 -> 1.1
            returnV = returnV.Remove(returnV.Length - 1, 1);

        return returnV;
    }

    public string ToOriginString(bool removeDumy = true)
    {
        return GetDumyString(removeDumy);
    }

    public new string ToString()
    {
        throw new System.Exception();
    }

    public string ToBaseString(bool removeDumy = true)
    {
        var returnString = GetDumyString(removeDumy, "0,0");
        if (!returnString.Contains(","))
            return returnString;

        return returnString;
    }

    public const string BlankString = "0";
    public string ToDisplay(bool removeDumy = true)
    {
        var origin = GetDumyString(removeDumy, "0,0");
        var sp = origin.Split(',');
        var length = sp.Length;
        if (length <= 0)
            return BlankString;

        if (length <= 1)
            return origin;

        length -= 2;
        var dpLenght = displayLabel.Length;
        var dotText = sp[1];
        while (dotText.EndsWith(BlankString) || 3 <= dotText.Length)
            dotText = dotText.Remove(dotText.Length - 1, 1);

        if (length < dpLenght)
        {
            if (0 < dotText.Length)
                return $"{sp[0]}.{dotText}{displayLabel[length]}";

            return $"{sp[0]}{displayLabel[length]}";
        }

        var front = length / dpLenght;
        var back = length % dpLenght;

        front -= 1;
        if (dpLenght <= front)
            front = back = dpLenght - 1;

        if (0 < dotText.Length)
            return $"{sp[0]}.{dotText}{displayLabel[front]}{displayLabel[back]}";

        return $"{sp[0]}{displayLabel[front]}{displayLabel[back]}";
    }

    public override bool Equals(object obj)
    {
        return obj is BigIntegerExtend extend &&
               bigInteger.Equals(extend.bigInteger);
    }

    public override int GetHashCode()
    {
        return -1222634271 + bigInteger.GetHashCode();
    }

    public static string[] displayLabel = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
}


public class BigIntegerExtendConveter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(BigIntegerExtend);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var jValue = new JValue(reader.Value);
        if (reader.TokenType == JsonToken.String)
        {
            BigIntegerExtend d_Int = (string)jValue;
            return d_Int;
        }

        return null;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        BigIntegerExtend intvalue = (BigIntegerExtend)value;
        writer.WriteValue(intvalue.ToOriginString());
    }
}

public class BigIntegerExtendTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        if (sourceType == typeof(BigIntegerExtend))
            return true;

        return base.CanConvertFrom(context, sourceType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value is string @string)
            return new BigIntegerExtend(@string);

        return base.ConvertFrom(context, culture, value);
    }
}