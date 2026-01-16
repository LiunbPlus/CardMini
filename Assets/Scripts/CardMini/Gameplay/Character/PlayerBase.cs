using System;
using Controller;
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
			PileController.Instance.InitCards(data.initCards);

			Coin = data.coin;
			HandCount = data.maxHand;
		}

		public void ChangeCoin(int amount, CharacterBase source){
			int originalValue = Coin;

			// 应用上下限
			Coin += amount;
			if(Coin < 0) Coin = 0;

			// 触发事件
			int delta = Coin - originalValue;
			OnCoinChange?.Invoke(this, delta);
		}

		public override void OnBattleStart(){
			base.OnBattleStart();
			Energy = 0;
		}

		public override void OnBattleEnd(){
			base.OnBattleEnd();
			Energy = 0;
		}

		public void ChangeEnergy(int amount, CharacterBase source){
			int originalValue = Energy;

			// 应用上下限
			Energy = Math.Clamp(Energy + amount, 0, 5);

			// 触发事件
			int delta = Energy - originalValue;
			OnEnergyChange?.Invoke(this, delta);
		}
	}
}