namespace Gameplay.Relic{
	public class RelicBase{
		public string Name{get; private set;}
		public string Desc{get; private set;}

		/// 进入新房间
		public virtual void OnEnterMapNode(){}

		/// 战斗开始
		public virtual void OnBattleStart(){}

		/// 回合开始
		public virtual void OnTurnIn(){}

		/// 结束
		public virtual void OnTurnOut(){}

		/// 战斗结束
		public virtual void OnBattleEnd(){}

	}
}