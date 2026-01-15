using System;

namespace Gameplay.Reward{
	public abstract class RewardBase{
		public Action OnCollected;
 		public abstract void CollectReward();
	}
}