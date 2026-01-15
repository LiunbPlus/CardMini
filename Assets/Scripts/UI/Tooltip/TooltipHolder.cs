/*
 * 加在任何需要使用Tooltip的物体上，可以自定义hover的时间，是否跟随鼠标，固定的位置，固定锚点等
 * 可以选择固定的信息msg，填写在Inspector中，也可以选择实现getMsg方法获得动态信息
 */

using System;
using System.Collections;
using UnityEngine;

namespace Util.Tooltip{
	public class TooltipHolder : MonoBehaviour, IHoverable{
		[SerializeField] private float holdTime;
		[SerializeField] private bool followMouse = true;
		[ShowIfFalse("followMouse")]
		[SerializeField] private Transform tipPos;
		[SerializeField] private bool fixedPivot = false;
		[ShowIfTrue("fixedPivot")]
		[SerializeField] private TextAnchor panelPivot;
		[SerializeField] private string msg = "No Info";

		public Func<string> getMsg;
		internal string Msg => getMsg?.Invoke() ?? msg;
		internal Vector3 TipPos => followMouse ? Input.mousePosition : tipPos.position;
		internal TextAnchor PanelPivot => fixedPivot ? panelPivot : TextAnchor.UpperLeft;

		private Coroutine _holding;

		private void OnDestroy(){
			if(_holding!=null) StopCoroutine(_holding);
		}

		public void HoverEnter(){
			_holding = StartCoroutine(HoldTime());
		}

		public void HoverExit(){
			if(_holding!=null) StopCoroutine(_holding);
			TooltipPresenter.Instance.HideTip();
		}

		private IEnumerator HoldTime(){
			yield return new WaitForSeconds(holdTime);
			TooltipPresenter.Instance.ShowTip(this);
		}
	}
}