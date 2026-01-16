using System;
using System.Collections.Generic;
using Core.Data;
using Gameplay.Card;
using Gameplay.Character;

namespace Gameplay.Actions{
	[Flags]
	public enum ActionFlag{
		None = 0,
		Source = 1,
		Target = 2,
		All = Source | Target
	}

	public abstract class ActionBase{
		private ActionFlag _actionFlag;
		protected CharacterBase source;
		protected List<CharacterBase> target;
		public readonly TargetType targetType;
		protected readonly bool isSingleTarget;
		protected int value;
		protected int count;

		protected ActionBase(ActionData data){
			_actionFlag = ActionFlag.None;
			value = data.value;
			count = data.count;
			targetType = data.targetType;
			isSingleTarget = targetType != TargetType.All;
		}

		public abstract void Apply();
		public abstract string GetPreviewText();

		public virtual void SetSource(CharacterBase s){
			source = s;
			_actionFlag = source == null ? _actionFlag & ~ActionFlag.Source : _actionFlag | ActionFlag.Source;
		}

		public virtual void SetTarget(List<CharacterBase> t){
			target = t;
			_actionFlag = target == null ? _actionFlag & ~ActionFlag.Target : _actionFlag | ActionFlag.Target;
		}

		protected bool FlagCheck(ActionFlag tar = ActionFlag.All){
			return (_actionFlag & tar) != 0;
		}
	}
}