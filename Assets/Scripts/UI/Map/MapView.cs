using Gameplay.Map;
using UnityEngine;
using Util;
using Util.Extension;

namespace UI.Map{
	public class MapView : UIBase{
		[SerializeField] private MapNodeDrawer nodePrefab;
		[SerializeField] private Transform content;

		private ItemPool<MapNodeDrawer> _pool;

		private void Awake(){
			_pool = new ItemPool<MapNodeDrawer>(nodePrefab, content);
		}

		private void DrawMap(){
			var map = MapController.Instance.GetMap();
			// todo : map draw, line
			foreach(MapNode mapNode in map){
				var node = _pool.GetItemFromPool();
				node.SetNode(mapNode);
				var nodeTransform = node.transform;
				nodeTransform.position = nodeTransform.position.WithAddY(100);
			}
		}
	}
}