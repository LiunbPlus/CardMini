using System.Collections;
using UnityEngine;
using Util;

namespace UI.Chat{
	public class PreviewManager : MonoBehaviour{
		public static PreviewManager Instance{get; private set;}

		[Header("References")]
		[SerializeField]
		private PreviewItem previewItemPrefab;
		[SerializeField] private Transform content;

		[Header("Settings")] [SerializeField] private float displayDuration = 3f;
		[SerializeField] private float fadeDuration = 0.5f;

		private ItemPool<PreviewItem> _itemPool;

		private void Awake(){
			if(Instance != null){
				Destroy(gameObject);
				return;
			}

			Instance = this;
			_itemPool = new ItemPool<PreviewItem>(previewItemPrefab, content);
		}

		public void AddPreview(string message){
			PreviewItem item = _itemPool.GetItemFromPool();
			item.gameObject.SetActive(true);
			item.SetText(message);
			item.transform.SetAsFirstSibling();

			StartCoroutine(DisplayAndFadeRoutine(item));
		}

		private IEnumerator DisplayAndFadeRoutine(PreviewItem activeNote){
			CanvasGroup canvasGroup = activeNote.CanvasGroup;

			canvasGroup.alpha = 1f;

			float elapsedTime = 0f;
			// 持续时间
			while(elapsedTime < displayDuration){
				elapsedTime += Time.unscaledDeltaTime;
				yield return null;
			}

			elapsedTime = 0f;
			// 渐变消失
			while(elapsedTime < fadeDuration){
				elapsedTime += Time.unscaledDeltaTime;
				canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
				yield return null;
			}

			canvasGroup.alpha = 0f;
			_itemPool.ReturnItemToPool(activeNote);
		}

		public void HidePreview(){
			content.gameObject.SetActive(false);
		}

		public void ShowPreview(){
			content.gameObject.SetActive(true);
		}
	}
}