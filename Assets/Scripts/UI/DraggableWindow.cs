using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableWindow : MonoBehaviour, IBeginDragHandler, IDragHandler{
	private RectTransform _rect;
	private RectTransform _parentRect;

	private Vector2 _offset;

	private void Awake(){
		_rect = transform as RectTransform;
		if(_rect != null) _parentRect = _rect.parent as RectTransform;
	}

	public void OnBeginDrag(PointerEventData eventData){
		RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, eventData.position, eventData.pressEventCamera, out _offset);
	}

	public void OnDrag(PointerEventData eventData){
		if(!RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentRect, eventData.position, eventData.pressEventCamera,
			out Vector2 localPoint))
			return;

		Vector2 targetPos = localPoint - _offset;

		_rect.anchoredPosition = ClampToParent(targetPos);
	}

	private Vector2 ClampToParent(Vector2 pos){
		Vector2 halfSize = _rect.rect.size * 0.5f;
		Vector2 parentHalf = _parentRect.rect.size * 0.5f;

		float minX = -parentHalf.x + halfSize.x;
		float maxX = parentHalf.x - halfSize.x;
		float minY = -parentHalf.y + halfSize.y;
		float maxY = parentHalf.y - halfSize.y;

		pos.x = Mathf.Clamp(pos.x, minX, maxX);
		pos.y = Mathf.Clamp(pos.y, minY, maxY);

		return pos;
	}
}