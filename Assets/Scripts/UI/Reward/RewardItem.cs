using Gameplay.Reward;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Reward{
	public class RewardItem : UIBase{
		[SerializeField] private Button button;

		private RewardBase _reward;

		protected override void Start(){
			base.Start();
			button.onClick.AddListener(OnButtonClick);
		}

		internal void SetReward(RewardBase reward){
			_reward = reward;
			if(_reward is CardReward cardReward){
				cardReward.OnClick += ShowCardRewardView;
			}
		}

		protected override void OnDestroy(){
			base.OnDestroy();
			button.onClick.RemoveListener(OnButtonClick);
			if(_reward is CardReward cardReward){
				cardReward.OnClick -= ShowCardRewardView;
			}
		}

		private void OnButtonClick(){
			_reward.CollectReward();
		}

		// 如果是卡牌，触发此弹出选择框
		private void ShowCardRewardView(CardReward cardReward){
			var cv = UI.RewardCardView;
			if(!cv) return;
			cv.SetReward(cardReward);
			cv.Show();
		}
	}
}