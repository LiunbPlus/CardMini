using System.Collections;
using Core.Data;
using Gameplay.Character;
using UI.Card;
using UI.Combat.ActionAnimator;
using UnityEngine.EventSystems;

namespace UI.Combat.Character{
	public class CharacterDrawer : UIBase{
		public CharacterBase CharacterBase{get; private set;}
		private CardAnimator _cardAnimator;

		private void Awake(){
			OperateLayer = 0;
		}

		public void SetBase(CharacterBase characterBase){
			CharacterBase = characterBase;
		}

		// Draw Character Image


		// Draw Health Image


		// Draw Buff Image


		public IEnumerator UseCardOnTarget(CardDrawer c){
			if(_cardAnimator) _cardAnimator.Interrupt();
			yield return null;

			_cardAnimator = AnimLoader.Instance.GetData(c.CardBase.AnimatorId);
			if(_cardAnimator){
				yield return _cardAnimator.Play(this, c);
			}
		}
	}
}