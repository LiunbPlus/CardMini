using System.Collections.Generic;
using Controller;
using Gameplay.Character;
using NUnit.Framework;
using UI.Combat.Character;

namespace Tests.Gameplay{
	public class ActionTest{
		private CharacterBase _player;
		private List<CharacterBase> _enemy;
		private GameManager _gm = GameManager.Instance;
		private CombatController _combat = CombatController.Instance;
		private PileController _pile = PileController.Instance;
		private TurnController _turn = TurnController.Instance;

		private void DefaultSet(){
			_enemy = new List<CharacterBase>();
			_player = new PlayerBase(DataManager.Instance.PlayerData.GetData(PlayerName.P1));
			var e = new EnemyBase(DataManager.Instance.EnemyData.GetData(EnemyName.E1));
			_enemy.Add(e);
		}

		[Test]
		public void DamageTest(){
			DefaultSet();

		}
	}
}