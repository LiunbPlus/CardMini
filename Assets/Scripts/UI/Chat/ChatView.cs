using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Chat{
	public class ChatView : MonoBehaviour{
		[SerializeField] private TMP_Text chatLog;
		[SerializeField] private TMP_InputField inputField;
		[SerializeField] private RectTransform scrollView;
		[SerializeField] private RectTransform chatPanel;
		private Coroutine _sizeChanging;

		private ChatPresenter _presenter;
		public float duration = 0.15f;
		internal bool IsVisible{get; private set;} = false;
		public bool HasFocus => inputField.isFocused;

		private void Start(){
			_presenter = ChatPresenter.Instance ?? new ChatPresenter(this);
		}

		internal void SetPresenter(ChatPresenter p){
			_presenter = p;
		}

		internal void SendMsg(){
			if(string.IsNullOrEmpty(inputField.text)) return;

			_presenter.UserSendMessage(inputField.text);
			inputField.text = string.Empty;
			StartCoroutine(ReFocusInput());
		}

		private IEnumerator ReFocusInput(){
			yield return null; // 等 TMP 自己 Deactivate 完
			inputField.ActivateInputField();
			inputField.Select();
		}


		internal void ClearQuit(){
			inputField.text = string.Empty;
			inputField.DeactivateInputField();
			if(InputManager.Instance != null)
				InputManager.Instance.OnInputFieldDeselected();

			Hide();
		}

		private void Show(){
			if(!IsVisible) OnHideButton();
		}

		internal void Hide(){
			if(IsVisible) OnHideButton();
		}

		private void OnHideButton(){
			IsVisible = !IsVisible;
			if(_sizeChanging != null) StopCoroutine(_sizeChanging);
			_sizeChanging = StartCoroutine(ChangeSize());
		}

		internal void Append(string msg){
			chatLog.text += msg + "\n";
		}

		internal void FocusInput(string preset = ""){
			if(!HasFocus && InputManager.Instance != null)
				InputManager.Instance.OnInputFieldSelected();

			Show();

			inputField.text = preset;
			StartCoroutine(SetCaretNextFrame(preset.Length));
		}

		private IEnumerator SetCaretNextFrame(int pos){
			yield return null; // 等一帧让 TMP 处理 ActivateInputField
			inputField.ActivateInputField();
			inputField.caretPosition = pos;
			inputField.selectionAnchorPosition = pos;
			inputField.selectionFocusPosition = pos;
		}

		private IEnumerator ChangeSize(){
			scrollView.GetComponent<ScrollRect>().verticalScrollbar.handleRect.gameObject.SetActive(IsVisible);

			float startHeight = IsVisible ? -400 : 0;
			float targetHeight = IsVisible ? 0 : -400;

			float t = 0f;

			while(t < duration){
				t += Time.deltaTime;
				float newHeight = Mathf.Lerp(startHeight, targetHeight, Mathf.Clamp01(t / duration));
				chatPanel.position = new Vector2(0, newHeight);
				yield return null;
			}

			// 强制设定最终值
			chatPanel.position = new Vector2(0, targetHeight);
		}
	}
}