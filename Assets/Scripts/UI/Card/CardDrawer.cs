using System;
using Gameplay.Card;
using TMPro;
using UI.Combat.Character;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DM = Controller.DataManager;

namespace UI.Card{
	public enum CardState{
		InHand,  // 手牌
		InSlot,  // 选择
		Exhibit, // 展示
		Reward,  // 战后奖励
	}

	public class CardDrawer : UIBase, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IPointerClickHandler{
		[SerializeField] private TMP_Text cardName;
		[SerializeField] private TMP_Text cardDescription;
		[SerializeField] private TMP_Text cardCost;
		[SerializeField] private Image cardImage;
		[SerializeField] private Image cardBackImage;

		protected override int OperateLayer => 0;
		public event Action<CardDrawer> OnChoose;
		public event Action<CardDrawer> OnClick;

		public CardState CardState{get; private set;}
		public CardBase CardBase{get; private set;}

		public void SetCardBase(CardBase b, CardState state = CardState.Exhibit){
			CardBase = b;
			CardState = state;
			if(b == null){
				Hide();
				return;
			}

			Show();
			cardName.text = CardBase.Name;
			cardDescription.text = CardBase.Description;
			cardCost.text = CardBase.Cost.ToString();
			cardImage.sprite = CardImageManager.Instance.GetImage(CardBase.Id).sprite;
			cardBackImage.sprite = DM.ImageData.GetData(CardBase.CardType.ToString()).image.sprite;
		}

		private void SetScale(float x){
			transform.localScale = new Vector3(x, x, x);
		}

		public void OnDrag(PointerEventData eventData){}

		public void OnPointerClick(PointerEventData eventData){
			if(CardState == CardState.InHand){
				// todo:test
				CardState = CardState.InSlot;
				ChooseCard(UI.CombatView.GetFirstValidSlot());
			} else if(CardState==CardState.InSlot){
				CardState = CardState.InHand;
				UnChooseCard();

			}
			if(CardState == CardState.Reward){
				OnClick?.Invoke(this);
			}
		}

		private void ChooseCard(CardSlotItem item){
			CardState = CardState.InSlot;
			var c = item.SetCard(this);
			c.UnChooseCard();
			// 选择特效
			OnChoose?.Invoke(this);
		}

		public void UnChooseCard(){
			CardState = CardState.InHand;
			// 取消选择特效
			OnChoose?.Invoke(this);
		}

		public void OnPointerEnter(PointerEventData eventData){
			SetScale(1.2f);
		}

		public void OnPointerExit(PointerEventData eventData){
			SetScale(1f);
		}
	}
}