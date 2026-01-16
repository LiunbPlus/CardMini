using System.Collections.Generic;

namespace Util{
	public class ObjPool<T> where T : new(){
		private readonly Queue<T> _itemPool = new();

		public ObjPool(){}

		public T GetItemFromPool(){
			return _itemPool.Count > 0 ? _itemPool.Dequeue() : new T();
		}

		public void ReturnItemToPool(T item){
			_itemPool.Enqueue(item);
		}
	}
}