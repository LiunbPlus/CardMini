using Controller;
using Core.Data;
using Gameplay.Buff;
using Gameplay.Card;

namespace Gameplay.Actions{
	public class ActionAddBuff : ActionBase{
		private readonly BuffType _type;
		private readonly int _stack;

		public ActionAddBuff(ActionData data) : base(data){
			_type = data.buffType;
			_stack = data.value;
		}

		public override void Apply(){
			if(!FlagCheck()) return;
			target.ForEach(t => t.BuffController.AddBuff(_type, _stack));
		}

		public override string GetPreviewText(){
			BuffData data = DataManager.Instance.BuffData.GetData(_type.ToString());
			return $"{targetType.GetTypeDesc()}给予{_stack}层{data.Name})";
		}
	}
}