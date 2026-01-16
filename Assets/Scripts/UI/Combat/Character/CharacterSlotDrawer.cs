using System.Collections.Generic;
using UnityEngine;
using Util;

namespace UI.Combat.Character{
	public class CharacterSlotDrawer : UIBase{
		[SerializeField] private CharacterSlotItem slotItemPrefab;
		[SerializeField] private Transform content;

		protected override int OperateLayer => 0;
		private readonly List<CharacterSlotItem> _list = new();
		private ItemPool<CharacterSlotItem> _pool;

		private void Awake(){
			_pool = new ItemPool<CharacterSlotItem>(slotItemPrefab, content);
		}

		public void SetCard(int slot, int id){
			if(_list.Count <= slot)return;
			_list[slot].SetCard(id);
		}

		public void SetSlot(int count){
			if(_list.Count == count) return;

			foreach(CharacterSlotItem t in _list){
				_pool.ReturnItemToPool(t);
			}

			_list.Clear();

			for(int i = 0; i < count; i++){
				var d = _pool.GetItemFromPool();
				_list.Add(d);
			}
		}
	}
}