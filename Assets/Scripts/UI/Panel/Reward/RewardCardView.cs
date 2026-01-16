using System.Collections.Generic;
using Controller;
using Gameplay.Card;
using Gameplay.Reward;
using UI.Card;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI.Reward{
	public class RewardCardView : UIBase{
		[SerializeField] private Button backButton;
		[SerializeField] private CardDrawer cardDrawerPrefab;
		[SerializeField] private Transform content;

		protected override int OperateLayer => 2;
		private CardReward _cardReward;
		private ItemPool<CardDrawer> _pool;
		private readonly List<CardDrawer> _drawers = new();

		private void Awake(){
			_pool = new ItemPool<CardDrawer>(cardDrawerPrefab, content);
		}

		protected override void Start(){
			base.Start();
			backButton.onClick.AddListener(Hide);
		}

		protected override void OnDestroy(){
			base.OnDestroy();
			backButton.onClick.RemoveListener(Hide);
		}

		internal void SetReward(CardReward cardReward){
			if(cardReward == _cardReward) return;

			_cardReward = cardReward;
			List<CardBase> r = cardReward.GetCardReward();

			foreach(CardDrawer drawer in _drawers){
				_pool.ReturnItemToPool(drawer);
			}

			_drawers.Clear();

			foreach(CardBase cardBase in r){
				var d = _pool.GetItemFromPool();
				d.SetCardBase(cardBase, CardState.Reward);
				d.OnClick += Collect;
				_drawers.Add(d);
			}
		}

		void Collect(CardDrawer d){
			PileController.Instance.AddToCardBag(d.CardBase);
			foreach(CardDrawer drawer in _drawers){
				drawer.OnClick -= Collect;
				_pool.ReturnItemToPool(drawer);
			}

			_drawers.Clear();
		}
	}
}