using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;

namespace Excel
{
	public enum LanguageEnum
	{
		none,
		Test,
		Language_Korea,
		Language_English,
	}
	public partial class LanguageDic : CustomDic<string, string>
	{
		public static LanguageDic CreateDic(string origin) 
		{
			var createDic = new Dictionary<string, string>();
			origin = Base64Zip.UnZip(origin);
			var split = origin.Split('\t');
			var valueLength = 2;
			var maxLength = split.Length / valueLength;
			for(int i = 0; i < maxLength; ++i)
			{
				var index = i * valueLength;
				createDic.Add(split[index], split[index + 1]);
			}
			return new LanguageDic(createDic);
		}

		private LanguageDic(Dictionary<string, string> basedic) : base(basedic) { }
	}

}
