using System;

namespace Core.Data{
	[Serializable]
	public class PlayerData : CharacterData{
		public int coin;
		public int maxHand;
		public int[] initCards;

		public PlayerData
			(int id, string name, string desc, int hp, int maxHp, int coin, int maxHand, int[] initCards, string img)
			: base(id, name, desc, hp, maxHp, img){
			this.coin = coin;
			this.maxHand = maxHand;
			this.initCards = initCards;
		}
	}
}