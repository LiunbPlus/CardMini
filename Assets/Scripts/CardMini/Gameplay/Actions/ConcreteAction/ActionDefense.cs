using System.Collections.Generic;
using Core.Data;
using Gameplay.Buff;
using Gameplay.Card;
using Gameplay.Character;

namespace Gameplay.Actions{
	public class ActionDefense : ActionBase{
		public ActionDefense(ActionData data) : base(data){
			_calcValue = value;
		}

		private int _calcValue;

		public override void SetSource(CharacterBase s){
			base.SetSource(s);
			if(s == null) _calcValue = value;
			else _calcValue = source.BuffController.CalcModify(ModifierType.DefenseOutput, value);
		}

		public override void SetTarget(List<CharacterBase> t){
			base.SetTarget(t);
			if(isSingleTarget && target[0] != null)
				_calcValue = target[0].BuffController.CalcModify(ModifierType.DefenseInput, _calcValue);
		}

		public override void Apply(){
			if(!FlagCheck()) return;
			if(isSingleTarget){
				target[0]?.ChangeDefense(_calcValue, source);
			} else{
				target.ForEach(e => {
					if(e == null) return;
					int v = e.BuffController.CalcModify(ModifierType.DefenseOutput, _calcValue);
					e.ChangeDefense(v, source);
				});
			}
		}

		public override string GetPreviewText(){
			return $"{targetType.GetTypeDesc()}获得{_calcValue}格挡";
		}
	}
}