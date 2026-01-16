using System;

namespace Core.Data{
	[Serializable]
	public class CharacterData : Data{
		public string name;
		public string desc;
		public string img;
		public int hp;
		public int maxHp;

		public CharacterData(int id, string name, string desc, int hp, int maxHp, string img) : base(id){
			this.name = name;
			this.desc = desc;
			this.hp = hp;
			this.maxHp = maxHp;
			this.img = img;
		}

		public override string ToString(){
			return $"{id} {name}";
		}
	}
}