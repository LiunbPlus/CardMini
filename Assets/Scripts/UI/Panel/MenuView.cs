using Controller;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu{
	public class MenuView : UIBase{
		[SerializeField] private Button resumeButton;
		[SerializeField] private Button exitButton;

		protected override int OperateLayer => 3;

		protected override void Start(){
			base.Start();
			resumeButton.onClick.AddListener(ToggleHv);
			exitButton.onClick.AddListener(OnExitButton);

			InputManager.Instance.OnEsc += ToggleHv;
		}

		protected override void OnDestroy(){
			base.OnDestroy();
			resumeButton.onClick.RemoveListener(ToggleHv);
			exitButton.onClick.RemoveListener(OnExitButton);

			InputManager.Instance.OnEsc -= ToggleHv;
		}

		private void ToggleHv(){
			if(IsVisible) Hide();
			else Show();
		}

		private void OnExitButton(){
			GameManager.Instance.ExitGame();
		}
	}
}