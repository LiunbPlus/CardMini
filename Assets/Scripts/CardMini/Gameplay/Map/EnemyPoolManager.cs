using System;
using System.Collections.Generic;
using Core.Data;
using DM = Controller.DataManager;
using GM = Controller.GameManager;

namespace Gameplay.Map{
	public class EnemyPoolManager{
		private readonly Dictionary<int, List<int>> _pool;
		private Random _rand;

		internal EnemyPoolManager(){
			_pool = new Dictionary<int, List<int>>();
			_rand = new Random(GM.Instance.RandomSeed);

			Dictionary<int, EnemyData> data = DM.EnemyData.GetAllData();
			foreach((int id, EnemyData enemyData) in data){
				int p = enemyData.pool;
				if(_pool.ContainsKey(p)){
					_pool[p].Add(id);
				} else{
					_pool[p] = new List<int>(){id};
				}
			}
		}

		public int GetRandomEnemyByPoolId(int id){
			if(_pool.TryGetValue(id, out List<int> l)){
				return l[_rand.Next(0, l.Count)];
			}

			return -1;
		}
	}
}