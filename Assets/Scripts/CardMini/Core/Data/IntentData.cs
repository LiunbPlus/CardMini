using System;

namespace Core.Data{
	[Serializable]
	public class IntentData : Data{
		public int[] cards;

		public IntentData(int id, int[] cards) : base(id){
			this.cards = cards;
		}

		public override string ToString(){
			return $"{id} {cards.Length}";
		}
	}
}