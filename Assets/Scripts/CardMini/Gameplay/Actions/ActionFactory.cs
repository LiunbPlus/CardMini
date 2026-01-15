using Core.Data;
namespace Gameplay.Actions{
	public class ActionFactory{
		public static ActionBase Create(ActionData data){
			return data.actionType switch{
				ActionType.AddBuff => new ActionAddBuff(data),
				ActionType.Damage => new ActionDamage(data),
				ActionType.Defense => new ActionDefense(data),

				_ => null
			};
		}
	}
}