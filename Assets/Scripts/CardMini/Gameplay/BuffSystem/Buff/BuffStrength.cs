using Gameplay.Character;

namespace Gameplay.Buff{
	public class BuffStrength : BuffBase{
		public BuffStrength(CharacterBase owner, int stack) : base(owner, stack){
			modifiers.Add(new StatModifier(ModifierType.DamageOutput, ModifierOperate.Additive, () => Stack, 1));
		}
	}
}