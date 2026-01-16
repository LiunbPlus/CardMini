using Core.Data;
using Util;

namespace Gameplay.Card{
	public static class CardBaseContainer{
		private static readonly ObjPool<CardBase> Pool = new();

		public static CardBase GetFromPool(CardData data){
			var b = Pool.GetItemFromPool();
			b.SetData(data);
			return b;
		}

		public static void ReturnToPool(CardBase b){
			Pool.ReturnItemToPool(b);
		}
	}
}