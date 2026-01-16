using Gameplay.Character;

namespace Gameplay.Buff{
	public class BuffStrength : BuffBase{
		public BuffStrength(int id, CharacterBase owner, int stack) : base(id, owner, stack){
			modifiers.Add(new StatModifier(ModifierType.DamageOutput, ModifierOperate.Additive, () => Stack, 1));
		}
	}
}