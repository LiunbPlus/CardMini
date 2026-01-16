using System;
using System.Collections.Generic;
using Controller;
using Core.Data;
using Gameplay.Buff;
using Gameplay.Card;

namespace Gameplay.Character{
	public abstract class CharacterBase{
		public int Id{get; private set;}
		public string Name{get; private set;}
		public string Description{get; private set;}
		public string ImagePath{get; private set;}
		public int Health{get; private set;}
		public int MaxHealth{get; private set;}
		public int Defense{get; private set;}
		/// 所属队伍，玩家=0，敌人=1
		public int Team{get; protected set;}
		public int SlotCount{get; private set;}
		public List<CardBase> SlotCards{get; private set;}
		public CharacterBuffController BuffController{get; private set;}

		public event Action<CharacterBase> OnNameChange;
		public event Action<CharacterBase> OnDescriptionChange;
		public event Action<CharacterBase, int> OnHealthChange;
		public event Action<CharacterBase, CharacterBase, int> OnGiveHealthChange;
		public event Action<CharacterBase> OnDeath;
		public event Action<CharacterBase, CharacterBase> OnGiveDeath;
		public event Action<CharacterBase, int> OnDefenseChange;
		public event Action<CharacterBase, CharacterBase, int> OnGiveDefenseChange;
		public event Action<CharacterBase> OnDefenseEmpty;
		public event Action<CharacterBase, CharacterBase> OnGiveDefenseEmpty;

		private readonly TurnController _tc = TurnController.Instance;

		protected CharacterBase(CharacterData data){
			Id = data.id;
			Name = data.name;
			Description = data.desc;
			ImagePath = data.img;
			MaxHealth = data.maxHp;
			Health = data.hp;
			Defense = 0;
			SlotCount = 3;
			SlotCards = new List<CardBase>(SlotCount);
			BuffController = new CharacterBuffController(this);

			_tc.OnBattleStart += OnBattleStart;
			_tc.OnBattleEnd += OnBattleEnd;
			_tc.OnTurnStart += BuffController.OnTurnIn;
			_tc.OnTurnEnd += BuffController.OnTurnOut;
		}

		public void Deposit(){
			_tc.OnBattleStart -= OnBattleStart;
			_tc.OnBattleEnd -= OnBattleEnd;
			_tc.OnTurnStart -= BuffController.OnTurnIn;
			_tc.OnTurnEnd -= BuffController.OnTurnOut;
		}

		public void ChangeName(string name){
			Name = name;
			OnNameChange?.Invoke(this);
		}

		public void ChangeDescription(string d){
			Description = d;
			OnDescriptionChange?.Invoke(this);
		}

		public virtual void OnBattleStart(){
			Defense = 0;
			BuffController.ClearBuff(BuffKind.All);
		}

		public virtual void OnBattleEnd(){
			Defense = 0;
			BuffController.ClearBuff(BuffKind.All);
		}

		/// <summary>
		/// 改变意图槽位数，清空持有卡牌（丢失引用！）
		/// </summary>
		/// <param name="amount">增量</param>
		public void ChangeSlotCount(int amount){
			SlotCount += amount;
			if(SlotCount < 0) SlotCount = 0;
			SlotCards = new List<CardBase>(SlotCount);
		}

		/// <summary>
		/// 造成伤害，会计算护盾
		/// </summary>
		/// <param name="amount">正数，代表伤害量</param>
		/// <param name="source">伤害来源</param>
		public void DealDamage(int amount, CharacterBase source){
			if(Defense >= amount){
				ChangeDefense(-amount, source);
			} else{
				ChangeDefense(-Defense, source);
				ChangeHealth(Defense - amount, source);
			}
		}

		/// <summary>
		/// 修改血量
		/// </summary>
		/// <param name="amount">目标=原值+此值</param>
		/// <param name="source">来源</param>
		public void ChangeHealth(int amount, CharacterBase source){
			int originalValue = Health;
			int potentialNewValue = originalValue + amount;

			// 应用上下限
			Health = Math.Clamp(potentialNewValue, 0, MaxHealth);

			// 触发事件
			int delta = Health - originalValue;
			OnHealthChange?.Invoke(this, delta);
			source?.OnGiveHealthChange?.Invoke(source, this, delta);

			// 死亡逻辑
			if(Health <= 0){
				OnDeath?.Invoke(this);
				source?.OnGiveDeath?.Invoke(source, this);
			}
		}

		/// <summary>
		/// 修改护盾
		/// </summary>
		/// <param name="amount">目标=原值+此值</param>
		/// <param name="source">来源</param>
		public void ChangeDefense(int amount, CharacterBase source){
			int originalValue = Defense;
			int potentialNewValue = originalValue + amount;

			// 应用上下限
			Defense = Math.Clamp(potentialNewValue, 0, 999);

			// 触发事件
			int delta = Defense - originalValue;
			OnDefenseChange?.Invoke(this, delta);
			source?.OnGiveDefenseChange?.Invoke(source, this, delta);

			// 死亡逻辑
			if(Defense <= 0){
				OnDefenseEmpty?.Invoke(this);
				source?.OnGiveDefenseEmpty?.Invoke(source, this);
			}
		}
	}
}