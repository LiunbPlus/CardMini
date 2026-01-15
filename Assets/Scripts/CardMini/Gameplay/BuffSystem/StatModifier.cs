using System;

namespace Gameplay.Buff{
	[Serializable]
	public enum ModifierType{
		DamageOutput,  // 伤害输出
		DamageInput,   // 伤害承受
		HealOutput,    // 治疗输出
		HealInput,     // 治疗输入
		DefenseOutput, // 防御输出
		DefenseInput,  // 防御输入
	}

	/// 修饰符的操作类型
	[Serializable]
	public enum ModifierOperate{
		Additive, // 加
		Mult,     // 乘
		Override, // 覆盖原始值
	}

	/// 修饰符的数据结构
	[Serializable]
	public class StatModifier{
		public ModifierType type;
		public ModifierOperate operate;
		public int order;
		public Func<float> valueGetter;

		public StatModifier(ModifierType type, ModifierOperate op, Func<float> valueGetter, int order){
			this.type = type;
			operate = op;
			this.valueGetter = valueGetter;
			this.order = order;
		}

		public float GetValue(){
			return valueGetter?.Invoke() ?? 0f;
		}
	}
}