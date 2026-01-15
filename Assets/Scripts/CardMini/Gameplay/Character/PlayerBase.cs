using System;
using Core.Data;

namespace Gameplay.Character{
	public class PlayerBase : CharacterBase{
		public int Coin{get; private set;}
		public int Energy{get; private set;}
		public int HandCount{get; private set;}

		public event Action<CharacterBase, int> OnEnergyChange;
		public event Action<CharacterBase, int> OnCoinChange;

		public PlayerBase(PlayerData data) : base(data){
			Team = 0;
			Coin = data.initCoin;
			HandCount = data.maxHandCard;
		}

		public void ChangeCoin(int amount, CharacterBase source){
			int originalValue = Coin;

			// 应用上下限
			if(Coin < 0) Coin = 0;

			// 触发事件
			int delta = Coin - originalValue;
			OnCoinChange?.Invoke(this, delta);
		}

		public void ChangeEnergy(int amount, CharacterBase source){
			int originalValue = Energy;

			// 应用上下限
			if(Energy < 0) Energy = 0;

			// 触发事件
			int delta = Energy - originalValue;
			OnEnergyChange?.Invoke(this, delta);
		}
	}
}