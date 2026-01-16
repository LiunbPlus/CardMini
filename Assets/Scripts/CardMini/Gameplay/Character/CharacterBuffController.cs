using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Buff;

namespace Gameplay.Character{
	public class CharacterBuffController{
		public event Action<BuffBase> OnBuffChange;

		private readonly CharacterBase _character;
		private readonly Dictionary<int, BuffBase> _buffs = new();
		private readonly Dictionary<ModifierType, List<(BuffBase source, StatModifier modifier)>> _statModifiers = new();

		public CharacterBuffController(CharacterBase characterBase, List<int> passives = null){
			_character = characterBase;

			if(passives == null) return;
			foreach(int kind in passives){
				AddBuff(kind, 1);
			}
		}

		#region Buff
		public void AddBuff(int type, int stack){
			BuffBase buff = BuffFactory.Create(type, _character, stack);
			AddBuff(type, buff);
		}

		private void AddBuff(int type, BuffBase buff){
			if(_buffs.TryGetValue(type, out var same)){
				same.OnApplyRe();

				// 叠加层数
				same.Stack += buff.Stack;
				OnBuffChange?.Invoke(same);
			} else{
				buff.OnApply();
				_buffs.Add(type, buff);
				OnBuffChange?.Invoke(buff);
			}
		}

		public void RemoveBuff(int type){
			if(!_buffs.TryGetValue(type, out var buff)) return;
			buff.OnRemove();
			_buffs.Remove(type);
		}

		public void ClearBuff(BuffKind targetKind){
			List<int> removedList = new();
			foreach((int key, BuffBase value) in _buffs){
				if((value.BuffKind & targetKind) != 0){
					removedList.Add(key);
				}
			}

			foreach(int kind in removedList){
				RemoveBuff(kind);
			}
		}

		public BuffBase GetBuff(int type){
			return !_buffs.TryGetValue(type, out var buff) ? null : buff;
		}

		public bool ContainsBuff(int type){
			return _buffs.ContainsKey(type);
		}

		internal void OnGameStart(){
			foreach(var buff in _buffs.Values){
				buff.OnBattleStart();
			}
		}

		internal void OnTurnIn(int t){
			if(t != _character.Team) return;
			foreach(var buff in _buffs.Values){
				buff.OnTurnIn();
			}
		}

		internal void OnTurnOut(int t){
			if(t != _character.Team) return;
			List<int> removedList = new();
			foreach((int kind, BuffBase buff) in _buffs){
				buff.OnTurnOut();
				if(buff.TickDuration()){
					removedList.Add(kind);
				}

				OnBuffChange?.Invoke(buff);
			}

			foreach(int kind in removedList){
				RemoveBuff(kind);
			}
		}
		#endregion

		#region Modifier
		// --- 通用计算函数 ---
		internal int CalcModify(ModifierType type, int baseValue){
			if(!_statModifiers.ContainsKey(type)) return baseValue;

			float value = baseValue;
			var mods = _statModifiers[type].OrderBy(m => m.modifier.order).ToList(); // 按order顺序排序

			// 分组处理不同操作类型
			var additiveMods = mods.Where(m => m.modifier.operate == ModifierOperate.Additive).ToList();
			var multiplicativeMods = mods.Where(m => m.modifier.operate == ModifierOperate.Mult).ToList();
			var overrideMods = mods.Where(m => m.modifier.operate == ModifierOperate.Override).ToList();

			// 1. 应用覆盖
			if(overrideMods.Any()){
				// 按优先值取最后一个 Override 的值
				(BuffBase source, StatModifier modifier) = overrideMods.Last();
				value = modifier.GetValue();
				return (int)value;
			}

			// 2. 应用加法
			foreach((BuffBase source, StatModifier modifier) in additiveMods){
				float f = modifier.GetValue();
				value += f;
			}

			// 3. 应用乘法
			foreach((BuffBase source, StatModifier modifier) in multiplicativeMods){
				float f = 1 + modifier.GetValue();
				value *= f;
			}

			return (int)value;
		}

		internal void AddModifier(BuffBase source, StatModifier modifier){
			if(!_statModifiers.ContainsKey(modifier.type)){
				_statModifiers[modifier.type] = new List<(BuffBase, StatModifier)>();
			}

			_statModifiers[modifier.type].Add((source, modifier));
		}

		internal void RemoveModifier(BuffBase source, StatModifier modifier){
			if(!_statModifiers.ContainsKey(modifier.type)) return;
			_statModifiers[modifier.type].RemoveAll(m =>
				m.source == source && m.modifier.type == modifier.type && m.modifier.operate == modifier.operate); // 更精确的匹配
			// 如果列表为空，可以移除 Key
			if(_statModifiers[modifier.type].Count == 0){
				_statModifiers.Remove(modifier.type);
			}
		}
		#endregion
	}
}