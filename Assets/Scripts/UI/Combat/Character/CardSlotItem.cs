using UI.Card;
using UnityEngine;

namespace UI.Combat.Character{
	public class CardSlotItem : UIBase{
		[SerializeField] public RectTransform rect;

		protected override int OperateLayer => 0;
		private CardDrawer _drawer;
		private int _index;
		private CardSlotItem _pre;
		private CardSlotItem _nxt;

		public void SetIndex(int index, CardSlotItem pre, CardSlotItem nxt){
			_index = index;
			_pre = pre;
			_nxt = nxt;
		}

		public CardDrawer SetCard(CardDrawer drawer){
			var pre = _drawer;
			_drawer = drawer;
			_drawer.transform.position = drawer.transform.position;
			return pre;
		}

		internal bool IsEmpty() => _drawer == null;
	}
}