using Controller;
using Core.Data;
using Gameplay.Card;

namespace Gameplay.Actions{
	public class ActionAddBuff : ActionBase{
		private readonly BuffData _data;
		private readonly int _stack;

		public ActionAddBuff(ActionData data) : base(data){
			_data = DataManager.BuffData.GetData(data.buff);
			_stack = data.value;
		}

		public override void Apply(){
			if(!FlagCheck()) return;
			for(int i = 0; i < count; ++i){
				target.ForEach(t => t.BuffController.AddBuff(_data.id, _stack));
			}
		}

		public override string GetPreviewText(){
			return $"{targetType.GetTypeDesc()}给予{_stack}层{_data.name}{(count > 1 ? count + "次" : "")}";
		}
	}
}