using System;
using System.Collections.Generic;
using Controller;

namespace Gameplay.Map{
	public class MapController{
		private static MapController instance;
		public static MapController Instance => instance ??= new();

		public event Action OnMapShow;
		public event Action OnMapHide;

		private Random _rng;
		private readonly EnemyPoolManager _poolManager;
		private readonly List<MapNode> _mapNodes;
		private MapNode _curNode;

		private MapController(){
			_mapNodes = new();
			_poolManager = new EnemyPoolManager();
		}

		public void SetRandomSeed(int seed){
			_rng = new Random(seed);
			GenerateMap();
			OnMapShow?.Invoke();
		}

		public List<MapNode> GetMap(){
			return _mapNodes;
		}

		public void EnterNode(MapNode node){
			switch(node.NodeType){
				default:
				case MapNodeType.Enemy:
					CombatController.Instance.EnterBattle(_poolManager.GetRandomEnemyByPoolId(1));
					break;
				case MapNodeType.Shop:
					CombatController.Instance.EnterBattle(_poolManager.GetRandomEnemyByPoolId(1));
					break;
				case MapNodeType.Elite:
					CombatController.Instance.EnterBattle(_poolManager.GetRandomEnemyByPoolId(2));
					break;
				case MapNodeType.Boss:
					CombatController.Instance.EnterBattle(_poolManager.GetRandomEnemyByPoolId(3));
					break;
			}

			OnMapHide?.Invoke();
		}

		public MapNodeType ExitNode(){
			_curNode.ExitNode();
			return _curNode.NodeType;
		}

		/// <summary>
		/// 生成地图
		/// </summary>
		private void GenerateMap(){
			var n7 = new MapNode(MapNodeType.Boss, null);
			var n6 = new MapNode(MapNodeType.Shop, new List<MapNode>(){n7});
			var n4 = new MapNode(MapNodeType.Elite, new List<MapNode>(){n6});
			var n2 = new MapNode(MapNodeType.Enemy, new List<MapNode>(){n4});
			var n1 = new MapNode(MapNodeType.Enemy, new List<MapNode>(){n2});

			_mapNodes.Add(n1);
			_mapNodes.Add(n2);
			_mapNodes.Add(n4);
			_mapNodes.Add(n6);
			_mapNodes.Add(n7);

			_mapNodes[0].IsValid = true;
		}
	}
}