using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;

namespace Excel
{
	public partial class CONST_STRINGDic : CustomExcel
	{
		public readonly string TEST;

		public static CONST_STRINGDic CreateDic(string origin)
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

			return new CONST_STRINGDic(createDic);
		}

		private CONST_STRINGDic(Dictionary<string, string> basedic)
		{
			TEST	= basedic["TEST"];
			TableParseEnd();
		}
	}
}
