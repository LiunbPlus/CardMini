using System;
using Core.Data;

namespace Gameplay.Relic{
	public class RelicManager{
		private static RelicManager instance;
		public static RelicManager Instance{get;} = instance ??= new ();

		private Random _rng;

		private RelicManager(){}

		public void SetRandomSeed(int seed){
			_rng = new Random(seed);
		}

		public RelicData GetNextRelicData(){
			int nxt = _rng.Next();

			return null;
		}
	}
}