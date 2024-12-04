using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;

namespace Excel
{
	public enum SoundEnum
	{
		none,
		MainBgm,
	}
	public partial class Sound
	{
		public readonly string Name;
		public readonly int Type;
		public readonly string Resource;

		public Sound(string[] split, int start)
		{
			Name		= split[start + 0];
			Type		= int.Parse(split[start + 1]);
			Resource	= split[start + 2];
		}
	}

	public partial class SoundDic : CustomDic<string, Sound>
	{
		public static SoundDic CreateDic(string origin) 
		{
			var createDic = new Dictionary<string, Sound>();
			origin = Base64Zip.UnZip(origin);
			var split = origin.Split('\t');
			var valueLength = typeof(Sound).GetFields().Length;
			var maxLength = split.Length / valueLength;
			for(int i = 0; i < maxLength; ++i)
			{
				var index = i * valueLength;
				var newClass = new Sound(split, index);
				createDic.Add(newClass.Name, newClass);
			}
			return new SoundDic(createDic);
		}

		private SoundDic(Dictionary<string, Sound> basedic) : base(basedic) { }
	}

}
