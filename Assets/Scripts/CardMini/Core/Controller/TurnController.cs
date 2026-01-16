using System;
using System.Collections.Generic;
using System.Linq;

namespace Controller{
	public class TurnController{
		private static TurnController instance;
		public static TurnController Instance => instance ??= new ();

		public event Action OnBattleStart;
		public event Action OnBattleEnd;
		public event Action<int> OnTurnStart;
		public event Action<int> OnTurnEnd;

		private int _curTeam;
		public int CurTurn{get; private set;}

		private TurnController(){}

		/// <summary>
		/// 战斗开始
		/// </summary>
		public void BattleStart(){
			CurTurn = 0;
			OnBattleStart?.Invoke();
		}

		/// <summary>
		/// 下一回合
		/// </summary>
		public void NextTurn(){
			OnTurnEnd?.Invoke(_curTeam);
			_curTeam++;
			if(_curTeam > 1){
				_curTeam = 0;
				CurTurn++;
			}
			OnTurnStart?.Invoke(_curTeam);
		}

		/// <summary>
		/// 战斗结束
		/// </summary>
		public void BattleEnd(){
			OnBattleEnd?.Invoke();
		}
	}
}