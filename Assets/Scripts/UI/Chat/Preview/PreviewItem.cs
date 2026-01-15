using UnityEngine;
using TMPro;

namespace UI.Chat{
	public class PreviewItem : MonoBehaviour{
		[SerializeField] private TMP_Text messageText;
		[SerializeField] private CanvasGroup canvasGroup;
		public CanvasGroup CanvasGroup => canvasGroup;

		public void SetText(string text){
			messageText.text = text;
		}
	}
}