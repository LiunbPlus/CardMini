using UnityEngine;
using UnityEngine.UI;

namespace UI.Lobby{
	public class LobbyView : UIBase{
		[SerializeField] private Button startButton;
		[SerializeField] private Button exitButton;

		protected override void Start(){
			base.Start();
			startButton.onClick.AddListener(OnStartButton);
			exitButton.onClick.AddListener(OnExitButton);
		}

		protected override void OnDestroy(){
			base.OnDestroy();
			startButton.onClick.RemoveListener(OnStartButton);
			exitButton.onClick.RemoveListener(OnExitButton);
		}

		void OnStartButton(){
			UI.ChangeScene(SceneName.COMBAT_SCENE);
		}

		void OnExitButton(){
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}