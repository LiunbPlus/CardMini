using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Card;
using UnityEngine;

namespace Controller{
	public class PileController{
		private static PileController instance;
		public static PileController Instance => instance ??= new();

		public event Action<CardBase> OnDraw;
		public event Action<CardBase> OnGive;
		public event Action<CardBase> OnRemove;
		public event Action<int> OnDropToDraw;
		public event Action<CardBase> OnDrop;
		public event Action<CardBase> OnExhaust;

		private System.Random _rng;
		private readonly List<CardBase> _allCards;
		private readonly List<CardBase> _handCards;
		private readonly List<CardBase> _drawPile;
		private readonly List<CardBase> _dropPile;
		private readonly List<CardBase> _exhaustPile;

		private int _handCount = 0;

		public void SetPlayer(int handCount, int seed){
			_handCount = handCount;
			_rng = new System.Random(seed);
		}

		private PileController(){
			_allCards = new();
			_handCards = new();
			_drawPile = new();
			_dropPile = new();
			_exhaustPile = new();

			TurnController.Instance.OnBattleStart += OnBattleStart;
			TurnController.Instance.OnBattleEnd += Reset;
		}

		private void Deposit(){
			TurnController.Instance.OnBattleStart -= OnBattleStart;
			TurnController.Instance.OnBattleEnd -= Reset;
		}

		public void InitCards(IEnumerable<int> ids){
			foreach(int id in ids){
				_allCards.Add(new CardBase(DataManager.CardData[id]));
			}
		}

		public void AddToCardBag(CardBase newCard){
			_allCards.Add(newCard);
		}

		/// <summary>
		/// 游戏开始
		/// </summary>
		private void OnBattleStart(){
			// 加入抽牌堆，洗牌
			_drawPile.AddRange(_allCards);
			Shuffle();
		}

		/// <summary>
		/// 获取抽牌堆的牌，排列顺序为id
		/// </summary>
		/// <returns>排序列表</returns>
		public List<CardBase> GetDrawPile(){
			return _drawPile.OrderBy(i => i).ToList();
		}

		public List<CardBase> GetDropPile(){
			return _dropPile;
		}

		public List<CardBase> GetHandPile(){
			return _handCards;
		}

		public List<CardBase> GetExhaustPile(){
			return _exhaustPile;
		}

		public List<CardBase> GetAllPile(){
			return _allCards;
		}

		/// <summary>
		/// 洗牌(抽牌)
		/// </summary>
		private void Shuffle(){
			int len = _drawPile.Count - 1;
			for(int r = len; r >= 0; --r){
				int l = _rng.Next(0, r + 1);
				(_drawPile[l], _drawPile[r]) = (_drawPile[r], _drawPile[l]);
			}
		}

		/// <summary>
		/// 将弃牌堆放入抽牌堆
		/// </summary>
		public void DropToDraw(){
			_drawPile.AddRange(_dropPile);
			_dropPile.Clear();
			Shuffle();
			OnDropToDraw?.Invoke(_drawPile.Count);
		}

		/// <summary>
		/// 从抽牌堆抽1张牌，放到手牌最后，若超出上限则加入弃牌堆
		/// </summary>
		/// <returns>抽到的牌</returns>
		public CardBase DrawCard(){
			if(_drawPile.Count == 0){
				if(_dropPile.Count == 0) return null; // 抽弃牌堆均空
				DropToDraw();                         // 洗牌
			}

			var c = _drawPile[0];
			if(_handCards.Count >= _handCount){
				Debug.Log("手牌满了");
				DropCard(c);
			} else{
				GiveCard(c);
			}

			_drawPile.RemoveAt(0);
			OnDraw?.Invoke(c);
			return c;
		}

		/// <summary>
		/// 在手牌中加入卡牌，若超出上限则加入弃牌堆
		/// </summary>
		public void GiveCard(CardBase card){
			_handCards.Add(card);
			OnGive?.Invoke(card);
		}

		/// <summary>
		/// 将一张牌从手中移除
		/// </summary>
		public void RemoveCard(CardBase card){
			_handCards.Remove(card);
			OnRemove?.Invoke(card);
		}

		/// <summary>
		/// 消耗一张牌
		/// </summary>
		public void ExhaustCard(CardBase card){
			_exhaustPile.Add(card);
			OnExhaust?.Invoke(card);
		}

		/// <summary>
		/// 弃一张牌
		/// </summary>
		public void DropCard(CardBase card){
			_dropPile.Add(card);
			OnDrop?.Invoke(card);
		}

		/// <summary>
		/// 换一张手牌（弃掉这张牌并抽一张牌）
		/// </summary>
		public void ExchangeCard(CardBase card){
			RemoveCard(card);
			DropCard(card);
			DrawCard();
		}

		/// <summary>
		/// 清空所有牌堆
		/// </summary>
		public void Reset(){
			_handCards.Clear();
			_drawPile.Clear();
			_dropPile.Clear();
			_exhaustPile.Clear();
		}
	}
}