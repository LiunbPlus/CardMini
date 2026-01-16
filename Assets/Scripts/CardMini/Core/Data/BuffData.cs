using System;

namespace Core.Data{
	[Serializable]
	public class BuffData : Data{
		public string cls;
		public string name;
		public string desc;
		public int maxStack;
		public int kind;
		public string img;

		public BuffData(int id, string name, string desc, int maxStack, int kind, string img, string cls) : base(id){
			this.name = name;
			this.desc = desc;
			this.maxStack = maxStack;
			this.kind = kind;
			this.img = img;
			this.cls = cls;
		}

		public override string ToString(){
			return $"{id} {name}";
		}
	}
}