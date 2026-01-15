using UnityEngine;

namespace Util.Tooltip{
	public class TooltipPresenter{
		public static TooltipPresenter Instance{get; private set;}
		private TooltipView _view;
		private string _tipMsg;

		public TooltipPresenter(TooltipView view){
			if(Instance != null){
				Instance.SetView(view);
				return;
			}

			Instance = this;
			SetView(view);
		}

		private void SetView(TooltipView vew){
			_view = vew;
			vew.SetPresenter(this);
		}

		public void ShowTip(TooltipHolder holder){
			_tipMsg = holder.Msg;

			_view.ShowMessage(_tipMsg);
			_view.SetPosition(holder.TipPos);

			if(holder.PanelPivot == TextAnchor.UpperLeft){
				// 修正位置，使之总是处于屏幕中
				(float w, float h) = _view.GetWAndH();
				bool xM = holder.TipPos.x + w > Screen.width;
				TextAnchor p = holder.TipPos.y - h < 0 ? xM ? TextAnchor.LowerRight : TextAnchor.LowerLeft :
					xM ? TextAnchor.UpperRight : TextAnchor.UpperLeft;
				_view.SetPivot(p);
			} else{
				_view.SetPivot(holder.PanelPivot);
			}
		}

		public void HideTip(){
			if(!_view.isHide) _view.Hide();
		}
	}
}