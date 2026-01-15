using System;
using System.Collections.Generic;
using Gameplay.Actions;
using Gameplay.Card;
using Gameplay.Character;
using Gameplay.Map;
using Gameplay.Reward;

namespace Controller{
	public class CombatController{
		private CombatController(){}
		private static CombatController instance;
		public static CombatController Instance => instance ??= new();

		public event Action<EnemyBase> OnEnemySpawn;
		public event Action<PlayerBase> OnPlayerSpawn;

		public event Action<RewardBase> OnRewardSpawn;

		private Random _random;
		private PlayerBase _player;
		private EnemyBase _enemy;

		public PlayerBase EnterMap(string name, int seed){
			_random = new Random(seed);
			return SpawnPlayer(name);
		}

		// Click Map Node
		public void EnterBattle(){
			SpawnEnemy("Pawn");
			TurnController.Instance.BattleStart();


			for(int i = 0; i < 5; i++){
				PileController.Instance.DrawCard();
			}
		}

		public void EndBattle(){
			TurnController.Instance.BattleEnd();
			//reward
			switch(MapController.Instance.ExitNode()){
				case MapNodeType.Enemy:
					SpawnNormalReward();
					break;
				case MapNodeType.Elite:
					SpawnRichReward();
					break;
				case MapNodeType.Boss:
					// SpawnBossReward();
					GameManager.Instance.Win();
					break;
			}
		}

		private EnemyBase SpawnEnemy(string name){
			var e = new EnemyBase(DataManager.Instance.EnemyData.GetData(name));
			e.OnDeath += EnemyDeath;
			TurnController.Instance.AddCharacter(e.Team);
			_enemy = e;
			OnEnemySpawn?.Invoke(e);
			return e;
		}

		private PlayerBase SpawnPlayer(string name){
			var p = new PlayerBase(DataManager.Instance.PlayerData.GetData(name));
			p.OnDeath += PlayerDeath;
			TurnController.Instance.AddCharacter(p.Team);
			_player = p;
			OnPlayerSpawn?.Invoke(p);
			return p;
		}

		/// 结算
		public static void CommitCard(CharacterBase source,CharacterBase other, CardBase card){
			foreach(ActionBase e in card.Actions){
				List<CharacterBase> tar = new();
				switch(e.targetType){
					default:
					case TargetType.Self:
						tar.Add(source);
						break;
					case TargetType.Other:
						tar.Add(other);
						break;
					case TargetType.All:
						tar.Add(source);
						tar.Add(other);
						break;
				}

				e.SetSource(source);
				e.SetTarget(tar);
				e.Apply();
			}
		}

		private void EnemyDeath(CharacterBase e){
			bool b = TurnController.Instance.RemoveCharacter(e.Team);
			if(b) EndBattle();
		}

		private void PlayerDeath(CharacterBase p){
			GameManager.Instance.Lose();
		}

		private void SpawnNormalReward(){
			var c = new CoinReward(_random.Next(30, 60));
			OnRewardSpawn?.Invoke(c);
			var d = new CardReward(3, 0);
			OnRewardSpawn?.Invoke(d);
		}

		private void SpawnRichReward(){
			var c = new CoinReward(_random.Next(60, 130));
			OnRewardSpawn?.Invoke(c);
			var d = new CardReward(3, 1);
			OnRewardSpawn?.Invoke(d);
		}

		private void SpawnBossReward(){
			var c = new CoinReward(_random.Next(130, 250));
			OnRewardSpawn?.Invoke(c);
			var d = new CardReward(3, 2);
			OnRewardSpawn?.Invoke(d);
		}
	}
}