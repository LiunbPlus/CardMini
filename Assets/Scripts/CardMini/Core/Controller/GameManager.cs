using System;
using Gameplay.Character;
using Gameplay.Map;
using Gameplay.Relic;
using UnityEngine;

namespace Controller{
	public class GameManager : MonoBehaviour{
		[SerializeField] public TextAsset cardData;


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
			DataManager.Instance.Init();
			CardManager.LoadAll();
		}

		public void EnterGame(string playerName){
			RandomSeed = (int)Time.time;
			Player = CombatController.Instance.EnterMap(playerName, RandomSeed);
			PileController.Instance.SetPlayer(Player, RandomSeed);
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