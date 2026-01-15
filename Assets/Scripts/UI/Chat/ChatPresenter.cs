using System.Collections.Generic;
using Gameplay.Command;
using UnityEngine;

namespace UI.Chat{
	public class ChatPresenter{
		public static ChatPresenter Instance{get; private set;}
		public bool IsVisible => _view.IsVisible;

		private ChatView _view;
		private readonly List<string> _sends = new();
		private int _sendIndex;
		private readonly CommandDispatcher _executor;

		public ChatPresenter(ChatView view){
			if(Instance != null){
				Instance.SetView(view);
				return;
			}

			Instance = this;
			SetView(view);
			_executor = new CommandDispatcher();

			InputManager.Instance.OnSplash += ShowCmd;
			InputManager.Instance.OnEnter += OnEnterPressed;
			InputManager.Instance.OnEsc += OnEscPressed;
			InputManager.Instance.OnArrow += OnArrow;
		}

		private void OnArrow(Vector2 vector2){
			if(!_view.HasFocus || _sends.Count == 0) return;

			if(vector2.y > 0){
				_sendIndex -= 1;
				if(_sendIndex < 0) _sendIndex = 0;
				_view.FocusInput(_sends[_sendIndex]);
			} else if(vector2.y < 0){
				_sendIndex += 1;
				if(_sendIndex > _sends.Count){
					_sendIndex = _sends.Count;
					_view.FocusInput();
				} else{
					_view.FocusInput(_sends[_sendIndex]);
				}
			}
		}

		private void ShowCmd(){
			if(!_view.IsVisible) _view.FocusInput("/");
		}

		private void OnEnterPressed(){
			if(!_view.IsVisible){
				PreviewManager.Instance.HidePreview();
				_view.FocusInput();
				return;
			}

			if(_view.HasFocus) _view.SendMsg();
			else _view.FocusInput();
		}

		private void OnEscPressed(){
			if(_view.IsVisible){
				_view.ClearQuit();
				PreviewManager.Instance.ShowPreview();
			}
		}

		private void SetView(ChatView view){
			this._view = view;
			view.SetPresenter(this);
			OnEscPressed();
		}

		internal void UserSendMessage(string input){
			if(input.StartsWith("/")){
				_executor.Execute(input);
				_sends.Add(input);
				_sendIndex = _sends.Count;
				return;
			}

			_sends.Add(input);
			_sendIndex = _sends.Count;

			_view.Append(input);
			PreviewManager.Instance.AddPreview(input);
		}

		public void LocalMessage(string msg){
			_view.Append(msg);
			PreviewManager.Instance.AddPreview(msg);
		}
	}
}