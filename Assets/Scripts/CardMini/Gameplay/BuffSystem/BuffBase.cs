using System;
using System.Collections.Generic;
using Controller;
using Core.Data;
using Gameplay.Character;
using UnityEngine;

namespace Gameplay.Buff{
	[Flags]
	public enum BuffKind{
		Neutral = 1,
		Positive = 1 << 2,
		Negative = 1 << 3,
		Passive = 1 << 4,

		All = Neutral | Positive | Negative
	}

	public abstract class BuffBase{
		public int Id => BuffData.id;
		public string Name => BuffData.name;
		public string Description => BuffData.desc;
		public BuffKind BuffKind => (BuffKind)(1 << BuffData.kind);
		public int MaxStacks => BuffData.maxStack;

		/// Buff 携带的修饰符列表
		internal readonly List<StatModifier> modifiers = new();

		public BuffData BuffData{get;}
		public int Stack{get; set;}
		protected CharacterBase Owner{get;}

		protected BuffBase(int id, CharacterBase owner, int stack){
			Owner = owner;
			Stack = stack;

			BuffData = DataManager.BuffData.GetData(id);
		}

		/// 游戏开始
		public virtual void OnBattleStart(){}

		/// 回合开始
		public virtual void OnTurnIn(){}

		/// 结束
		public virtual void OnTurnOut(){}

		/// 叠加层数
		public virtual void ChangeStack(int x){
			Stack = x;
			Stack = Mathf.Clamp(Stack, 0, MaxStacks);
		}

		/// 首次获得
		public virtual void OnApply(){
			foreach(var mod in modifiers){
				Owner.BuffController.AddModifier(this, mod);
			}
		}

		/// 再次获得
		public virtual void OnApplyRe(){}

		/// 失去
		public virtual void OnRemove(){
			foreach(var mod in modifiers){
				Owner.BuffController.RemoveModifier(this, mod);
			}
		}

		public bool TickDuration(){
			if(BuffKind == BuffKind.Passive) return false;
			Stack--;
			return Stack <= 0;
		}

		public override string ToString(){
			return $"{Name} {Stack} ({(BuffKind == BuffKind.Passive ? "∞" : Stack)})";
		}
	}
}