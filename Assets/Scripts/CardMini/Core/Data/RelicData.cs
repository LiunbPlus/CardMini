using System;

namespace Core.Data{
	[Serializable]
	public class RelicData : Data{
		public string name;
		public string desc;
		public string img;
		public int rarity;

		public RelicData(int id, string name, string desc, string img, int rarity) : base(id){
			this.name = name;
			this.desc = desc;
			this.img = img;
			this.rarity = rarity;
		}
	}
}