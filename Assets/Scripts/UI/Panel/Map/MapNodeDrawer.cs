using Gameplay.Map;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Map{
	public class MapNodeDrawer : UIBase{
		[SerializeField] private Button button;

		protected override int OperateLayer => 2;
		private MapNode _node;

		private void Awake(){
			button.onClick.AddListener(OnButtonClick);
		}

		protected override void OnDestroy(){
			base.OnDestroy();
			button.onClick.RemoveListener(OnButtonClick);
		}

		public void SetNode(MapNode node){
			_node = node;
		}

		void OnButtonClick(){
			if(_node == null) return;
			MapController.Instance.EnterNode(_node);
		}
	}
}