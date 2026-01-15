using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Util.Tooltip{
	public class TooltipView : MonoBehaviour{
		[SerializeField] private HorizontalLayoutGroup surface;
		[SerializeField] internal TMP_Text tipInfo;
		[SerializeField] private Image tipPanel;
		[SerializeField] private float fadeTime = 0.1f;

		private RectTransform _surfaceTransform;
		private TooltipPresenter _presenter;
		private Coroutine _fadeCoroutine;
		internal bool isHide;

		private void Awake(){
			_presenter = TooltipPresenter.Instance ?? new TooltipPresenter(this);
			_surfaceTransform = surface.GetComponent<RectTransform>();

			tipPanel.color = new Color(1, 1, 1, 0);
			tipInfo.alpha = 0;
			isHide = true;
		}

		internal void SetPresenter(TooltipPresenter p){
			_presenter = p;
		}

		internal void SetPosition(Vector3 pos){
			tipPanel.transform.parent.position = pos;
		}

		internal void SetPivot(TextAnchor p){
			surface.childAlignment = p;
			_surfaceTransform.pivot = p switch{
				TextAnchor.LowerLeft    => new Vector2(0, 0),
				TextAnchor.UpperLeft    => new Vector2(0, 1),
				TextAnchor.LowerRight   => new Vector2(1, 0),
				TextAnchor.UpperRight   => new Vector2(1, 1),
				TextAnchor.LowerCenter  => new Vector2(0.5f, 0),
				TextAnchor.UpperCenter  => new Vector2(0.5f, 1),
				TextAnchor.MiddleLeft   => new Vector2(0, 0.5f),
				TextAnchor.MiddleCenter => new Vector2(0.5f, 0.5f),
				TextAnchor.MiddleRight  => new Vector2(1, 0.5f),
				_                       => new Vector2(0, 1)
			};
		}

		internal (float, float) GetWAndH(){
			Rect rect = tipPanel.rectTransform.rect;
			return (rect.width, rect.height);
		}

		private void GenerateLine(string msg){
			// 临时设置一次，用于计算宽度
			tipInfo.text = msg;
			tipInfo.ForceMeshUpdate();

			float boxWidth = tipInfo.rectTransform.rect.width;
			int count = Mathf.Clamp(Mathf.FloorToInt(boxWidth / 14), 1, 25);

			string lineStr = $"<color=#808080><size=20>{new string('─', count)}</size></color>";

			// 替换 <line>
			msg = msg.Replace("<line>", lineStr);
			tipInfo.text = msg;
		}

		internal void ShowMessage(string msg){
			GenerateLine(msg);

			isHide = false;
			if(_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
			_fadeCoroutine = StartCoroutine(FadeIn());
		}

		internal void Hide(){
			isHide = true;
			if(_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
			_fadeCoroutine = StartCoroutine(FadeIn());
		}

		private IEnumerator FadeIn(){
			float t = 0f;
			float startOpacity = tipPanel.color.a;
			float endOpacity = isHide ? 0 : 1;

			while(t < fadeTime){
				t += Time.deltaTime;
				float op = Mathf.Lerp(startOpacity, endOpacity, Mathf.Clamp01(t / fadeTime));
				tipInfo.alpha = op;
				tipPanel.color = new Color(1f, 1f, 1f, op);
				yield return null;
			}

			tipInfo.alpha = endOpacity;
			tipPanel.color = new Color(1f, 1f, 1f, endOpacity);
			_fadeCoroutine = null;
		}
	}
}