using Controller;
using Gameplay.Card;
using UI.Card;
using UnityEngine;

namespace UI.Combat.Character{
	public class CharacterSlotItem : UIBase{
		[SerializeField] private CardDrawer cardDrawer;
		protected override int OperateLayer => 0;

		public void SetCard(int id){
			var dat = DataManager.CardData[id];
			if(dat == null){
				cardDrawer.SetCardBase(null);
				return;
			}

			cardDrawer.SetCardBase(new CardBase(dat), CardState.Exhibit);
		}
	}
}