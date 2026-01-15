using Controller;
using Gameplay.Map;
using UnityEngine;
using UnityEngine.UI;

namespace UI.State{
	public class StateView : UIBase{
		[SerializeField] private Button allPileButton; // 牌组按钮
		[SerializeField] private Button mapButton;     // 地图按钮
		[SerializeField] private Button exitButton;    // 退出按钮

		private void Awake(){
			OperateLayer = 0;
		}

		protected override void Start(){
			base.Start();

			MapController.Instance.OnMapShow += ShowMap;
			MapController.Instance.OnMapHide += HideMap;

			allPileButton.onClick.AddListener(OnAllPileButtonClick);
			mapButton.onClick.AddListener(OnMapButtonClick);
			exitButton.onClick.AddListener(OnExitButtonClick);
		}

		protected override void OnDestroy(){
			base.OnDestroy();

			MapController.Instance.OnMapShow -= ShowMap;
			MapController.Instance.OnMapHide -= HideMap;

			allPileButton.onClick.RemoveListener(OnAllPileButtonClick);
			mapButton.onClick.RemoveListener(OnMapButtonClick);
			exitButton.onClick.RemoveListener(OnExitButtonClick);
		}

		private void OnAllPileButtonClick(){
			var bv = UI.BagView;
			if(bv == null) return;
			bv.SetPiles(PileController.Instance.GetAllPile());
		}

		private void ShowMap(){
			var m = UI.MapView;
			if(m == null) return;
			m.Show();
		}

		private void HideMap(){
			var m = UI.MapView;
			if(m == null) return;
			m.Hide();
		}

		private void OnMapButtonClick(){
			var m = UI.MapView;
			if(m == null) return;
			if(m.IsVisible) m.Hide();
			else m.Show();
		}

		private void OnExitButtonClick(){
			UI.MenuView.Show();
		}
	}
}