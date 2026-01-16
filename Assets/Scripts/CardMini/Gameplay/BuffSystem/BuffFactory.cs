using System;
using Controller;
using Gameplay.Character;
namespace Gameplay.Buff{
	public static class BuffFactory{
		public static BuffBase Create(int id, CharacterBase c, int s){
			var kind = Enum.Parse<BuffType>(DataManager.BuffData[id].cls);
			return kind switch{
				BuffType.BuffPoison => new BuffPoison(id,c,s),
				BuffType.BuffStrength => new BuffStrength(id,c,s),

				_ => null
			};
		}
	}
}