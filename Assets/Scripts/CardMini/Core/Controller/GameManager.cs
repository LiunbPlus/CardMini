using System;
using Gameplay.Character;
using Gameplay.Map;
using Gameplay.Relic;
using UnityEngine;

namespace Controller{
	public class GameManager : MonoBehaviour{
		public static GameManager Instance{get; private set;}
		public PlayerBase Player{get; private set;}
		public int RandomSeed{get; private set;}

		public event Action OnWin;
		public event Action OnEnter;
		public event Action OnLose;
		public event Action OnExit;

		/// <summary>
		/// Beginning of Everything
		/// </summary>
		private void Awake(){
			if(Instance != null){
				Destroy(this);
				return;
			}

			Instance = this;
			DataManager.Init();
		}

		public void EnterGame(int playerId){
			RandomSeed = (int)Time.time;
			Player = CombatController.Instance.EnterMap(playerId, RandomSeed);
			PileController.Instance.SetPlayer(Player.HandCount, RandomSeed);
			RelicManager.Instance.SetRandomSeed(RandomSeed);
			MapController.Instance.SetRandomSeed(RandomSeed);

			OnEnter?.Invoke();
		}

		public void Win(){
			Debug.Log("win");
			OnWin?.Invoke();
		}

		public void Lose(){
			Debug.Log("lose");
			OnLose?.Invoke();
		}

		public void ExitGame(){
			OnExit?.Invoke();
		}
	}
}