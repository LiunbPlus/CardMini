using System;

namespace Core.Data{
	[Serializable]
	public class EnemyData : CharacterData{
		public int pool;
		public IntentWeight[] intents;

		public EnemyData
			(int id, string name, string desc, int hp, int maxHp, int pool, IntentWeight[] intents, string img) : base(id, name,
			desc, hp, maxHp, img){
			this.pool = pool;
			this.intents = intents;
		}

	}

	[Serializable]
	public class IntentWeight{
		public int id;
		public int weight;
	}
}
