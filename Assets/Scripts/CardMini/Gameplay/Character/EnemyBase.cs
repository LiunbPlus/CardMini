using System;
using System.Collections.Generic;
using System.Linq;
using Controller;
using Core.Data;

namespace Gameplay.Character{
	public class EnemyBase : CharacterBase{
		private readonly List<IntentWeight> _intents;
		private readonly int _weightSum;
		private readonly Random _rand;

		public EnemyBase(EnemyData data) : base(data){
			Team = 1;
			_intents = new List<IntentWeight>(data.intents);
			_weightSum = _intents.Sum(s => s.weight);
			_rand = new Random(GameManager.Instance.RandomSeed);
		}

		public int GetIntent(){
			if(_intents.Count == 0) return -1;

			int sum = 0;
			int w = _rand.Next(0, _weightSum);
			foreach(IntentWeight intentWeight in _intents){
				sum += intentWeight.weight;
				if(sum >= w) return intentWeight.id;
			}

			return _intents[^1].id;
		}
	}
}