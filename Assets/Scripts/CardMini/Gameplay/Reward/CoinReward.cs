using Controller;

namespace Gameplay.Reward{
	public class CoinReward : RewardBase{
		public int Count{get;}

		public CoinReward(int i){
			Count = i;
		}

		public override void CollectReward(){
			var p = GameManager.Instance.Player;
			p?.ChangeCoin(Count, p);
			OnCollected?.Invoke();
		}
	}
}