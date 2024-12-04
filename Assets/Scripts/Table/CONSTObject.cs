using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;

namespace Excel
{
	public partial class CONSTDic : CustomExcel
	{
		public readonly float Test;

		public static CONSTDic CreateDic(string origin)
		{
			var createDic = new Dictionary<string, float>();
			origin = Base64Zip.UnZip(origin);
			var split = origin.Split('\t');
			var valueLength = 2;
			var maxLength = split.Length / valueLength;
			for(int i = 0; i < maxLength; ++i)
			{
				var index = i * valueLength;
				createDic.Add(split[index], float.Parse(split[index + 1]));
			}

			return new CONSTDic(createDic);
		}

		private CONSTDic(Dictionary<string, float> basedic)
		{
			Test	= basedic["Test"];
			TableParseEnd();
		}
	}
}
