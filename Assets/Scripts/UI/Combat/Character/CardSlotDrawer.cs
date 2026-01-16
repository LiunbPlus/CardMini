using System.Collections.Generic;
using UnityEngine;
using Util;

namespace UI.Combat.Character{
	public class CardSlotDrawer : UIBase{
		[Header("Slot")] [SerializeField] private CardSlotItem cardSlotPrefab;
		[SerializeField] private Transform cardSlotContent;

		protected override int OperateLayer => 0;
		private List<CardSlotItem> _items;
		private ItemPool<CardSlotItem> _slotPool;

		private void Awake(){
			_slotPool = new ItemPool<CardSlotItem>(cardSlotPrefab, cardSlotContent);
			_items = new();
		}

		internal void Clear(){
			foreach(var t in _items){
				_slotPool.ReturnItemToPool(t);
			}

			_items.Clear();
		}

		internal void SpawnItem(int c){
			Clear();
			_items.Add(null);
			for(int i = 0; i < c; i++){
				var cardSlotDrawer = _slotPool.GetItemFromPool();
				_items.Add(cardSlotDrawer);
			}

			_items.Add(null);

			for(int i = 1; i <= c + 1; i++){
				_items[i].SetIndex(i, _items[i - 1], _items[i + 1]);
			}
		}

		internal CardSlotItem GetFirstItem(){
			for(int i = 1; i < _items.Count - 1; i++){
				if(_items[i].IsEmpty()) return _items[i];
			}

			return null;
		}
	}
}