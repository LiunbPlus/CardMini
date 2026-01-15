using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Util.Tooltip;

public class InputManager : MonoBehaviour{
	public static InputManager Instance{get; private set;}

	[Header("Input Actions")] public InputActionAsset actions;
	private InputActionMap _activeMap;

	private InputActionMap _gameplayMap;
	private InputActionMap _menuMap;
	private InputActionMap _chatMap;
	private InputActionMap _previewMap;

	public event Action<Vector2> OnPointerMoved;
	public event Action<Vector2> OnLeftClick;
	public event Action<Vector2> OnLeftDoubleClick;
	public event Action<Vector2> OnRightClick;
	public event Action<Vector2> OnRightStart;
	public event Action OnRightUI; // Release from UI
	public event Action<Vector2> OnRightRelease;
	public event Action<Vector2> OnArrow;
	public event Action OnEsc;
	public event Action OnSpace;
	public event Action OnSplash;
	public event Action OnEnter;
	public event Action OnTabStart;
	public event Action OnTabEnd;
	public event Action<float> OnScroll;
	public event Action<bool> OnOverUI;
	public Vector2 pointerPosition;

	private bool _isOverUI;
	private bool _isRightUI;
	private bool _isRightPressed;
	private bool _leftClickPending;
	private bool _leftDoubleClickPending;
	private bool _rightClickPending;
	private bool _rightStartPending;
	private bool _rightReleasePending;
	private bool _pointerMovedPending;
	private bool _scrollPending;
	private float _scrollValue;
	private IHoverable _currentHover;

	// 双击检测变量
	private float _lastLeftClickTime = -1f;          // 上次左键点击的时间
	private Vector2 _lastLeftClickPosition;          // 上次左键点击的位置
	public float doubleClickInterval = 0.3f;        // 双击时间间隔阈值 (秒)
	public float doubleClickDistanceThreshold = 5f; // 双击距离阈值 (像素，转换为世界坐标后比较)
	// 右键点击 vs 拖拽 判断
	private float _rightMouseDownTime = -1f;        // 右键按下的时间
	private Vector2 _rightMouseDownScreenPos;       // 右键按下时的屏幕坐标
	public float rightClickTimeThreshold = 0.2f;   // 判断为点击的最大时间 (秒)
	public float rightClickDistanceThreshold = 3f; // 判断为点击的最大鼠标移动距离 (像素)

	private void Awake(){
		if(Instance != null && Instance != this){
			Destroy(gameObject);
			return;
		}

		Instance = this;

		DontDestroyOnLoad(this);

		_gameplayMap = actions.FindActionMap("Gameplay");
		_menuMap = actions.FindActionMap("Menu");
		_chatMap = actions.FindActionMap("Chat");
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDestroy(){
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void Update(){
		// 先处理 PointerMoved
		if(_pointerMovedPending){
			_pointerMovedPending = false;
			if(!EventSystem.current.IsPointerOverGameObject()){
				OnPointerMoved?.Invoke(pointerPosition);
				if(_isOverUI) OnOverUI?.Invoke(false);
				_isOverUI = false;
			} else{
				if(!_isOverUI) OnOverUI?.Invoke(true);
				_isOverUI = true;
			}

			IHoverable newHover = RaycastHover(pointerPosition);
			if(newHover != _currentHover){
				_currentHover?.HoverExit();
				_currentHover = newHover;
				_currentHover?.HoverEnter();
			}
		}

		// 处理点击事件
		if(_leftClickPending){
			_leftClickPending = false;

			float currentTime = Time.time;
			Vector2 currentScreenPosition = Mouse.current.position.ReadValue();

			// 检查时间间隔和距离
			if(currentTime - _lastLeftClickTime < doubleClickInterval){
				// 计算两次点击屏幕坐标的距离
				float distance = Vector2.Distance(currentScreenPosition, _lastLeftClickPosition);

				if(distance <= doubleClickDistanceThreshold){
					_leftDoubleClickPending = true; // 标记待处理
					_lastLeftClickTime = -1f;
				} else{
					ProcessSingleLeftClick(pointerPosition);
				}
			} else{
				ProcessSingleLeftClick(pointerPosition);
			}

			// 更新上次点击信息
			_lastLeftClickTime = currentTime;
			_lastLeftClickPosition = currentScreenPosition;
		}

		if(_leftDoubleClickPending){
			_leftDoubleClickPending = false;
			if(!EventSystem.current.IsPointerOverGameObject()){
				OnLeftDoubleClick?.Invoke(pointerPosition);
			}
		}

		if(_rightStartPending){
			_rightStartPending = false;
			_isRightPressed = true;
			if(!EventSystem.current.IsPointerOverGameObject()){
				OnRightStart?.Invoke(pointerPosition);
				_isRightUI = false;
			} else{
				_isRightUI = true;
			}
		}

		if(_rightClickPending){
			_rightClickPending = false;
			if(!EventSystem.current.IsPointerOverGameObject()){
				OnRightClick?.Invoke(pointerPosition);
			}
		}

		if(_rightReleasePending){
			_rightReleasePending = false;
			_isRightPressed = false;
			if(_isRightUI){
				OnRightUI?.Invoke();
			} else{
				OnRightRelease?.Invoke(pointerPosition);
			}
		}

		if(_scrollPending){
			_scrollPending = false;
			if(!EventSystem.current.IsPointerOverGameObject()){
				OnScroll?.Invoke(_scrollValue);
			}
		}
	}

	// 单击处理
	private void ProcessSingleLeftClick(Vector2 clickWorldPosition){
		_currentHover?.HoverExit();
		_currentHover = null;
		if(!EventSystem.current.IsPointerOverGameObject()){
			OnLeftClick?.Invoke(clickWorldPosition);
		}
	}

	private IHoverable RaycastHover(Vector2 worldPos){
		if(_isRightPressed) return null;

		// 1. UI 优先
		var data = new PointerEventData(EventSystem.current){position = Mouse.current.position.ReadValue()};
		var uiResults = new List<RaycastResult>();
		EventSystem.current.RaycastAll(data, uiResults);

		foreach(var r in uiResults){
			var h = r.gameObject.GetComponent<IHoverable>();
			if(h != null) return h;
		}

		if(_isOverUI) return null;

		// 2. 世界 2D 碰撞
		RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
		if(hit.collider){
			var h = hit.collider.GetComponentInParent<IHoverable>();
			if(h != null) return h;
		}

		return null;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
		switch(scene.name){
			case "Entry":
			case "Preparation":
				EnableActionMap(_menuMap);
				break;
			case "Test":
				EnableActionMap(_gameplayMap);
				break;
		}
	}

	private void EnableActionMap(InputActionMap map){
		_activeMap?.Disable();
		map.Enable();
		_activeMap = map;
	}

	public void OnInputFieldSelected(){
		_previewMap = _activeMap;
		EnableActionMap(_chatMap);
	}

	public void OnInputFieldDeselected(){
		if(_previewMap == null) return;
		EnableActionMap(_previewMap);
		_previewMap = null;
	}

	// Pointer position
	public void OnPointerPosition(InputAction.CallbackContext ctx){
		if(Camera.main != null) pointerPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		_pointerMovedPending = true;
	}

	// Left click
	public void OnPointerClick(InputAction.CallbackContext ctx){
		if(ctx.performed){
			if(Camera.main != null) pointerPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
			_leftClickPending = true;
		}
	}

	// Right click
	// Right click
	public void OnPointerRightClick(InputAction.CallbackContext ctx){
		if(Camera.main != null) pointerPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

		if(ctx.started){
			_rightMouseDownTime = Time.time;
			_rightMouseDownScreenPos = Mouse.current.position.ReadValue();

			_rightStartPending = true;
		} else if(ctx.canceled){
			_rightReleasePending = true; // 保留原有逻辑，用于结束拖拽

			//在释放时判断是点击还是拖拽
			if(_rightMouseDownTime > 0){
				float pressDuration = Time.time - _rightMouseDownTime;
				Vector2 releaseScreenPos = Mouse.current.position.ReadValue();
				float moveDistance = Vector2.Distance(_rightMouseDownScreenPos, releaseScreenPos);

				// 判断是否为快速点击
				if(pressDuration <= rightClickTimeThreshold && moveDistance <= rightClickDistanceThreshold){
					_rightClickPending = true;
				}

				_rightMouseDownTime = -1f; // 重置按下信息
			}
		}
	}

	// ESC
	public void OnCancelAction(InputAction.CallbackContext ctx){
		if(ctx.canceled) OnEsc?.Invoke();
	}

	// Space
	public void OnSpaceAction(InputAction.CallbackContext ctx){
		if(ctx.canceled) OnSpace?.Invoke();
	}

	// Tab
	public void OnTabAction(InputAction.CallbackContext ctx){
		if(ctx.performed) OnTabStart?.Invoke();
		else if(ctx.canceled) OnTabEnd?.Invoke();
	}

	// /
	public void OnSplashAction(InputAction.CallbackContext ctx){
		if(ctx.performed) OnSplash?.Invoke();
	}

	// Enter
	public void OnEnterAction(InputAction.CallbackContext ctx){
		if(ctx.performed) OnEnter?.Invoke();
	}

	// Arrow
	public void OnArrowAction(InputAction.CallbackContext ctx){
		if(ctx.performed) OnArrow?.Invoke(ctx.ReadValue<Vector2>());
	}

	// Scroll
	public void OnScrollAction(InputAction.CallbackContext ctx){
		if(ctx.performed){
			_scrollPending = true;
			_scrollValue = ctx.ReadValue<float>();
		}
	}
}