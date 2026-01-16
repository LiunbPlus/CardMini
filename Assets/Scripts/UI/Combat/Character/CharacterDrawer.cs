using System.Collections;
using Controller;
using Core.Data;
using Gameplay.Card;
using Gameplay.Character;
using UI.Combat.ActionAnimator;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Combat.Character{
	public class CharacterDrawer : UIBase{
		[SerializeField] private Image image;
		[SerializeField] private CharacterHealthBar healthBar;
		[SerializeField] private CharacterBuffDrawer buffDrawer;
		[SerializeField] private CharacterSlotDrawer slotDrawer;

		protected override int OperateLayer => 0;

		public CharacterBase CharacterBase{get; private set;}
		private CardAnimator _cardAnimator;

		public void SetBase(CharacterBase characterBase){
			if(CharacterBase != null){
				CharacterBase.OnHealthChange -= OnHealthChange;
				CharacterBase.BuffController.OnBuffChange -= buffDrawer.SetItem;
				CharacterBase.Deposit();
			}

			CharacterBase = characterBase;
			if(characterBase == null){
				Hide();
				return;
			}

			Show();
			image.sprite = DataManager.PlayerImageData[characterBase.ImagePath].sprite;
			healthBar.Init(characterBase.MaxHealth);
			slotDrawer.SetSlot(characterBase.SlotCount);
			characterBase.BuffController.OnBuffChange += buffDrawer.SetItem;
			characterBase.OnHealthChange += OnHealthChange;
		}

		public void SetCardIntent(int slot, int id){
			slotDrawer.SetCard(slot, id);
		}

		private void OnHealthChange(CharacterBase c, int amount){
			healthBar.ChangeHealth(amount);
		}

		public IEnumerator UseCardOnTarget(CardBase c){
			if(_cardAnimator) _cardAnimator.Interrupt();
			yield return null;

			_cardAnimator = AnimLoader.Instance.GetData(c.AnimatorId);
			if(_cardAnimator){
				yield return _cardAnimator.Play(this, c);
			}
		}
	}
}