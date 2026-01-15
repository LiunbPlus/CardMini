using UnityEngine;
using UnityEngine.UI;

namespace UI{
	public abstract class UIBase : MonoBehaviour{
		public bool IsVisible{get; private set;}
		protected static UIManager UI => UIManager.Instance;

		protected int OperateLayer{get; set;}
		private Button[] _buttons;
		private bool _isValid;

		protected virtual void Start(){
			_buttons = GetComponentsInChildren<Button>();
			UI.OnLayerChange += OnLayerChange;
		}

		protected virtual void OnDestroy(){
			UI.OnLayerChange -= OnLayerChange;
		}

		protected virtual void OnLayerChange(int l){
			if(_isValid == l > OperateLayer) return;
			_isValid = l > OperateLayer;

			foreach(Button button in _buttons){
				button.enabled = _isValid;
			}
		}

		public virtual void Hide(){
			IsVisible = false;
			gameObject.SetActive(false);
			UI.ChangeLayer(OperateLayer);
		}

		public virtual void Show(){
			IsVisible = true;
			gameObject.SetActive(true);
			UI.CancelLayer(OperateLayer);
		}
	}
}