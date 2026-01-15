using Controller;
using Gameplay.Reward;
using UnityEngine;
using Util;

namespace UI.Reward{
	public class RewardView : UIBase{
		[SerializeField] private Transform content;
		[SerializeField] private RewardItem rewardItemPrefab;

		private ItemPool<RewardItem> _pool;

		private void Awake(){
			OperateLayer = 1;
			_pool = new ItemPool<RewardItem>(rewardItemPrefab, content);
		}

		protected override void Start(){
			base.Start();
			CombatController.Instance.OnRewardSpawn += SpawnReward;
		}

		private void CollectReward(RewardItem rewardItem){
			rewardItem.SetReward(null);
			_pool.ReturnItemToPool(rewardItem);
		}

		protected override void OnDestroy(){
			base.OnDestroy();
			CombatController.Instance.OnRewardSpawn -= SpawnReward;
		}

		private void SpawnReward(RewardBase reward){
			var r = _pool.GetItemFromPool();
			r.SetReward(reward);
			reward.OnCollected += ()=>CollectReward(r);
		}
	}
}