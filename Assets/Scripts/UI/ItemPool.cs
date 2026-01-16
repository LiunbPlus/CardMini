/*
 * 泛型对象池，T为池内对象的类型
 */

using System.Collections.Generic;
using UnityEngine;

namespace Util{
	public class ItemPool<T> where T : MonoBehaviour{
		private readonly Queue<T> _itemPool = new();
		private readonly T _prefab;
		private readonly Transform _content;

		public ItemPool(T prefab, Transform transform){
			_prefab = prefab;
			_content = transform;
		}

		public T GetItemFromPool(){
			return _itemPool.Count > 0 ? _itemPool.Dequeue() : Object.Instantiate(_prefab, _content);
		}

		public void ReturnItemToPool(T item){
			item.gameObject.SetActive(false);
			_itemPool.Enqueue(item);
		}
	}
}