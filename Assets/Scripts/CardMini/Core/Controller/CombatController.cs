using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Actions;
using Gameplay.Card;
using Gameplay.Character;
using Gameplay.Map;
using Gameplay.Reward;
using UnityEngine;
using Random = System.Random;

namespace Controller{
	public class CombatController{
		private CombatController(){}
		private static CombatController instance;
		public static CombatController Instance => instance ??= new();

		public int DrawCount{get; private set;}
		public int ChangeCount{get; private set;}

		public event Action<EnemyBase> OnEnemySpawn;
		public event Action<PlayerBase> OnPlayerSpawn;
		public event Action<RewardBase> OnRewardSpawn;

		public event Func<CharacterBase, CardBase, IEnumerator> OnUseCardOnTarget;

		private readonly TurnController _tc = TurnController.Instance;
		private Random _random;
		private PlayerBase _player;
		private EnemyBase _enemy;

		public PlayerBase EnterMap(int id, int seed){
			_random = new Random(seed);
			SpawnPlayer(id);
			return _player;
		}

		// MapNode -> Combat -> Turn -> Everything
		public EnemyBase EnterBattle(int eid){
			SpawnEnemy(eid);

			_tc.BattleStart();
			_tc.OnTurnStart += OnTurnIn;

			for(int i = 0; i < 5; i++){
				PileController.Instance.DrawCard();
			}

			return _enemy;
		}

		private void OnTurnIn(int t){
			if(t == 0){
				DrawCount += 2;
				ChangeCount += 2;
				_player.ChangeEnergy(3, null);
			}
		}

		public void EndBattle(){
			_tc.BattleEnd();
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

		private void SpawnEnemy(int id){
			var e = new EnemyBase(DataManager.EnemyData.GetData(id));
			e.OnDeath += EnemyDeath;
			_enemy = e;
			OnEnemySpawn?.Invoke(e);
		}

		private void SpawnPlayer(int id){
			var p = new PlayerBase(DataManager.PlayerData.GetData(id));
			p.OnDeath += PlayerDeath;
			_player = p;
			OnPlayerSpawn?.Invoke(p);
		}

		public IEnumerator NextTurn(){
			int team = _tc.CurTurn;
			CharacterBase source, target;
			if(team == 0){
				source = _player;
				target = _enemy;
			} else{
				source = _enemy;
				target = _player;
			}

			List<CardBase> choseCards = source.SlotCards;
			foreach(CardBase choseCard in choseCards){
				yield return OnUseCardOnTarget?.Invoke(source, choseCard);
				CommitCard(source, target, choseCard);
				PileController.Instance.DropCard(choseCard);
				PileController.Instance.RemoveCard(choseCard);

				yield return new WaitForSeconds(0.5f);
			}
			_tc.NextTurn();
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
			EndBattle();
		}

		private void PlayerDeath(CharacterBase p){
			GameManager.Instance.Lose();
		}

		public int GetSlotCount(){
			return 3;
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