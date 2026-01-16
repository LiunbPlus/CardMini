using System.Collections.Generic;

namespace Gameplay.Map{
	public enum MapNodeType{
		Enemy,
		Shop,
		Elite,
		Boss,
	}

	public class MapNode{
		public bool IsValid{get; internal set;}
		internal MapNodeType NodeType{get;private set;}
		private readonly List<MapNode> _nextNode;

		internal MapNode(MapNodeType type, List<MapNode> nextNode){
			NodeType = type;
			IsValid = false;
			_nextNode = nextNode;
		}

		internal void ExitNode(){
			_nextNode.ForEach(e=>e.IsValid = true);
		}
	}
}