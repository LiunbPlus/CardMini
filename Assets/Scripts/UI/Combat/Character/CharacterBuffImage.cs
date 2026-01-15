using System.Collections.Generic;
using Gameplay.Buff;
using UnityEngine;
using Util;

namespace UI.Combat.Character{
	public class CharacterBuffImage : MonoBehaviour{
		[SerializeField] private CharacterBuffImageItem imageItemPrefab;
		[SerializeField] private RectTransform content;

		private readonly Dictionary<string, CharacterBuffImageItem> _activeItems = new();
		private readonly ItemPool<CharacterBuffImageItem> _pool;

		public void SetItem(BuffBase buff){
			var key = buff.BuffData.name;

			// Buff 失效：回收
			if(buff.Stack <= 0 && _activeItems.TryGetValue(key, out var item)){
				_activeItems.Remove(key);
				_pool.ReturnItemToPool(item);
				return;
			}

			// Buff 已存在：刷新
			if(_activeItems.TryGetValue(key, out var exist)){
				exist.SetBuff(buff);
				return;
			}

			// 新 Buff：从池中取
			var ii = _pool.GetItemFromPool();
			ii.SetBuff(buff);
			_activeItems.Add(key, ii);
		}
	}
}