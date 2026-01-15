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

		private readonly Dictionary<int, int> _teams;
		private readonly List<int> _teamIds;
		private int _curTeamIndex;
		private int CurTeamId => _teamIds[_curTeamIndex];
		private int _curTurn;

		private TurnController(){
			_teams = new();
			_teamIds = new();
		}

		/// <summary>
		/// 战斗开始
		/// </summary>
		public void BattleStart(){
			_curTurn = 0;
			OnBattleStart?.Invoke();
		}

		/// <summary>
		/// 下一回合
		/// </summary>
		public void NextTurn(){
			OnTurnEnd?.Invoke(CurTeamId);
			_curTeamIndex++;
			if(_curTeamIndex >= _teamIds.Count){
				_curTeamIndex = 0;
				_curTurn++;
			}
			OnTurnStart?.Invoke(CurTeamId);
		}

		/// <summary>
		/// 战斗结束
		/// </summary>
		public void BattleEnd(){
			OnBattleEnd?.Invoke();
		}

		/// <summary>
		/// 加入角色
		/// </summary>
		/// <param name="id">角色所在队伍id</param>
		public void AddCharacter(int id){
			if(!_teams.ContainsKey(id)) _teamIds.Add(id);
			_teams[id]++;
		}

		/// <summary>
		/// 获得唯一队伍ID
		/// </summary>
		/// <returns>若唯一返回id，若仍有其他队伍返回-1</returns>
		public int CheckOnlyTeam(){
			if(_teams.Keys.Count == 1) return _teams.Keys.ToArray()[0];
			return -1;
		}

		/// <summary>
		/// 删除角色
		/// </summary>
		/// <param name="id">角色所在队伍ID</param>
		/// <returns>是否是该队最后一人</returns>
		public bool RemoveCharacter(int id){
			if(_teams.ContainsKey(id)){
				_teams[id]--;
				if(_teams[id] == 0){
					_teams.Remove(id);
					_teamIds.Remove(id);
					return true;
				}
			}

			return false;
		}
	}
}