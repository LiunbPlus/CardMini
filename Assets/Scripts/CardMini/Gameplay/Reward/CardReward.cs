using System;
using System.Collections.Generic;
using System.Linq;
using Controller;
using Gameplay.Card;

namespace Gameplay.Reward{
	public class CardReward : RewardBase{
		public event Action<CardReward> OnClick;

		private List<CardBase> _rewards;
		private readonly int _count;

		public CardReward(int x, int w){
			_count = x;
		}

		public List<CardBase> GetCardReward(){
			if(_rewards != null && _rewards.Count == _count) return _rewards;
			if(!GameManager.Instance) return null;

			int seed = GameManager.Instance.RandomSeed;
			var random = new Random(seed);

			var ids = new List<int>(DataManager.CardData.GetAllData().Keys);

			if(ids.Count <= 3){
				_rewards = new List<CardBase>(ids.Select(i => new CardBase(DataManager.CardData[ids[i]])));
				return _rewards;
			}

			HashSet<int> set = new();
			while(set.Count < _count){
				int next = random.Next(0, ids.Count);
				set.Add(next);
			}

			_rewards = new List<CardBase>(set.Select(i => new CardBase(DataManager.CardData[ids[i]])));
			return _rewards;
		}

		public override void CollectReward(){
			OnClick?.Invoke(this);
		}
	}
}