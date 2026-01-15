using Gameplay.Character;
namespace Gameplay.Buff{
	public static class BuffFactory{
		public static BuffBase Create(BuffType kind, CharacterBase c, int s){
			return kind switch{
				BuffType.BuffPoison => new BuffPoison(c,s),
				BuffType.BuffStrength => new BuffStrength(c,s),

				_ => null
			};
		}
	}
}