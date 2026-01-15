using UnityEngine;
using UnityEngine.UI;

namespace UI.Lose{
	public class LoseView : UIBase{
		[SerializeField] private Button backButton;

		protected override void Start(){
			base.Start();
			backButton.onClick.AddListener(OnBackButton);
		}

		protected override void OnDestroy(){
			base.OnDestroy();
			backButton.onClick.RemoveListener(OnBackButton);
		}

		private void OnBackButton(){
			UI.ChangeScene(SceneName.MENU_SCENE);
		}
	}
}