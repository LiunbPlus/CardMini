using System;
using Controller;
using Gameplay.Card;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

		public event Action<CardDrawer> OnChoose;

		public CardState CardState{get; private set;}
		public CardBase CardBase{get; private set;}

		public void SetCardBase(CardBase b, CardState state = CardState.Exhibit){
			CardBase = b;
			CardState = state;
			Refresh();
		}

		/// <summary>
		/// 重新读取信息，重新绘制
		/// </summary>
		private void Refresh(){
			cardName.text = CardBase.Name;
			cardDescription.text = CardBase.Description;
			cardCost.text = CardBase.Cost.ToString();
			cardImage = CardImageManager.Instance.GetImage(CardBase.Id);
			cardBackImage = DataManager.Instance.ImageData.GetData(CardBase.CardType.ToString()).image;
		}

		private void SetScale(float x){
			transform.localScale = new Vector3(x, x, x);
		}

		public void OnDrag(PointerEventData eventData){}

		public void OnPointerClick(PointerEventData eventData){
			if(CardState == CardState.Reward){
				// 加入卡组
				PileController.Instance.AddToCardBag(CardBase);
			}
		}

		private void ChooseCard(){
			CardState = CardState.InSlot;
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