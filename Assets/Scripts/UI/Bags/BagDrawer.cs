using System.Collections.Generic;
using Gameplay.Card;
using UI.Card;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI.Bags{
	public class BagDrawer : UIBase{
		[SerializeField] private Button exitButton;
		[SerializeField] private Transform content;
		[SerializeField] private CardDrawer drawerPrefab;

		protected override int OperateLayer => 2;
		private ItemPool<CardDrawer> _pool;

		private void Awake(){
			_pool = new ItemPool<CardDrawer>(drawerPrefab, content);
		}

		protected override void Start(){
			base.Start();
			exitButton.onClick.AddListener(Exit);
		}

		protected override void OnDestroy(){
			base.OnDestroy();
			exitButton.onClick.RemoveListener(Exit);
		}

		private void Exit(){
			gameObject.SetActive(false);
		}

		public void SetPiles(List<CardBase> cards){
			foreach(var cardBase in cards){
				var cd = _pool.GetItemFromPool();
				cd.SetCardBase(cardBase);
			}
		}
	}
}