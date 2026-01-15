using System;
using Core.Data;

namespace Gameplay.Character{
	public abstract class CharacterBase{
		public string Name{get; private set;}
		public string Description{get; private set;}
		public int Health{get; private set;}
		public int MaxHealth{get; private set;}
		public int Defense{get; private set;}
		public int MaxDefense{get; private set;}
		/// 所属队伍，玩家=0，敌人=1
		public int Team{get; protected set;}
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

		protected CharacterBase(CharacterData data){
			Name = data.cname;
			Description = data.description;
			MaxHealth = data.maxHealth;
			Health = data.initHealth;
			Defense = data.initDefense;
			MaxDefense = data.maxDefense;
			BuffController = new CharacterBuffController(this);
		}

		public void ChangeName(string name){
			Name = name;
			OnNameChange?.Invoke(this);
		}

		public void ChangeDescription(string d){
			Description = d;
			OnDescriptionChange?.Invoke(this);
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
			Defense = Math.Clamp(potentialNewValue, 0, MaxDefense);

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