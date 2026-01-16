using System;
using System.Collections.Generic;
using UI.Bags;
using UI.Combat;
using UI.Map;
using UI.Menu;
using UI.Reward;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI{
	public static class SceneName{
		public const string MENU_SCENE = "MenuScene";
		public const string CORE_SCENE = "CoreScene";
		public const string COMBAT_SCENE = "CombatScene";
	}

	public class UIManager : MonoBehaviour{
		private UIManager(){}
		public static UIManager Instance{get; private set;}

		private Stack<int> _layerStack;
		internal event Action<int> OnLayerChange;
		/// <summary>
		/// 操作层，高层UI出现时底层UI不再响应输入，直至高层UI消失
		/// </summary>
		internal int CurrentOperateLayer{get; private set;}
		/*
		 * 0 - Combat -> HandCard, Player Enemy, Shop; State
		 * 1 - Reward;
		 * 2 - Map, Bag, CardReward
		 * 3 - Menu
		 */

		public CombatView CombatView{get; private set;}
		public RewardCardView RewardCardView{get; private set;}
		public BagDrawer BagDrawer{get; private set;}
		public MapView MapView{get; private set;}
		public MenuView MenuView{get; private set;}

		private void Awake(){
			if(Instance != null){
				Destroy(this);
				return;
			}

			Instance = this;
			DontDestroyOnLoad(this);

			_layerStack = new();
			CurrentOperateLayer = 0;
		}

		private void Start(){
			MenuView = FindObjectOfType<MenuView>();
		}

		public void ChangeScene(string s){
			SceneManager.LoadScene(s);
			BagDrawer = FindObjectOfType<BagDrawer>();
			MapView = FindObjectOfType<MapView>();
			CombatView = FindObjectOfType<CombatView>();
			RewardCardView = FindObjectOfType<RewardCardView>();
		}

		internal void ChangeLayer(int x){
			if(x < CurrentOperateLayer) return;
			_layerStack.Push(x);
			CurrentOperateLayer = x;
			OnLayerChange?.Invoke(x);
		}

		internal void CancelLayer(int x){
			if(x != CurrentOperateLayer) return;
			_layerStack.Pop();
			CurrentOperateLayer = _layerStack.Peek();
			OnLayerChange?.Invoke(CurrentOperateLayer);
		}
	}
}