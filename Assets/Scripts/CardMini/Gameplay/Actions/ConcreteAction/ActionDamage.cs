using System.Collections.Generic;
using Core.Data;
using Gameplay.Buff;
using Gameplay.Card;
using Gameplay.Character;

namespace Gameplay.Actions{
	public class ActionDamage : ActionBase{
		public ActionDamage(ActionData data) : base(data){}
		private int _calcValue;

		public override void SetSource(CharacterBase s){
			base.SetSource(s);
			if(source == null) _calcValue = value;
			else _calcValue = source.BuffController.CalcModify(ModifierType.DamageOutput, value);
		}

		public override void SetTarget(List<CharacterBase> t){
			base.SetTarget(t);
			if(isSingleTarget && t[0]!=null){
				_calcValue = t[0].BuffController.CalcModify(ModifierType.DamageInput, _calcValue);
			}
		}

		public override void Apply(){
			if(!FlagCheck()) return;
			for(int i = 0; i < count; ++i){
				if(isSingleTarget){
					target[0]?.DealDamage(_calcValue, source);
				} else{
					target.ForEach(t => {
						if(t == null) return;
						int v = t.BuffController.CalcModify(ModifierType.DamageInput, _calcValue);
						t.DealDamage(v, source);
					});
				}
			}
		}

		public override string GetPreviewText(){
			return $"{targetType.GetTypeDesc()}造成{_calcValue}点伤害{(count > 1 ? count+"次" : "")}";
		}
	}
}