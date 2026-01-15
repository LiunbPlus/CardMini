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
		private readonly int _rarity;

		public CardReward(int x, int y){
			_count = x;
			_rarity = y;
		}

		public List<CardBase> GetCardReward(){
			if(_rewards != null && _rewards.Count == _count) return _rewards;
			if(!GameManager.Instance) return null;

			int seed = GameManager.Instance.RandomSeed;
			var random = new Random(seed);

			var res = new List<CardBase>(_count);
			List<int> ids = CardManager.GetAllCardIdByRarity((RarityType)_rarity);
			HashSet<int> set = new();

			while(set.Count < _count){
				int next = random.Next(0, ids.Count);
				set.Add(next);
			}

			res.AddRange(set.Select(i => CardManager.GetCard(ids[i])));
			_rewards = res;
			return _rewards;
		}

		public override void CollectReward(){
			OnClick?.Invoke(this);
		}
	}
}