using System.Collections;
using System.Collections.Generic;
using Controller;
using Gameplay.Card;
using Gameplay.Character;
using UI.Card;
using UI.Combat.Character;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI.Combat{
	public class CombatView : UIBase{
		[Header("Component")] [SerializeField] private Button nextTurnButton;
		[SerializeField] private Button drawPileButton;
		[SerializeField] private Button dropPileButton;
		[SerializeField] private Button exhaustPileButton;
		[SerializeField] private Button drawCardButton;
		[SerializeField] private Button changeCardButton;

		[SerializeField] private CharacterDrawer playerDrawer;
		[SerializeField] private CharacterDrawer enemyDrawer;

		[Header("Card")] [SerializeField] private CardDrawer cardDrawerPrefab;
		[SerializeField] private Transform cardDrawerContent;
		[SerializeField] private CardSlotDrawer slotDrawer;

		protected override int OperateLayer => 0;
		private readonly PileController _pile = PileController.Instance;
		private readonly CombatController _combat = CombatController.Instance;

		private ItemPool<CardDrawer> _cardPool;
		private List<CardDrawer> _choseCards;
		private Dictionary<CardBase, CardDrawer> _cardDrawers;

		private void Awake(){
			_cardPool = new ItemPool<CardDrawer>(cardDrawerPrefab, cardDrawerContent);
			_choseCards = new();
			_cardDrawers = new();
		}

		protected override void Start(){
			base.Start();

			_pile.OnDraw += OnDrawCard;
			_pile.OnDrop += OnDropCard;
			_pile.OnExhaust += OnExhaustCard;
			_pile.OnDropToDraw += OnDropToDraw;
			_pile.OnGive += OnGiveCard;
			_pile.OnRemove += OnRemoveCard;

			_combat.OnPlayerSpawn += OnPlayerSpawn;
			_combat.OnEnemySpawn += OnEnemySpawn;
			_combat.OnUseCardOnTarget += UseCardAnim;

			TurnController.Instance.OnTurnStart += OnTurnIn;
			TurnController.Instance.OnBattleStart += OnBattleStart;
			TurnController.Instance.OnBattleEnd += OnBattleEnd;

			drawCardButton.onClick.AddListener(OnDrawCardButton);
			changeCardButton.onClick.AddListener(OnChangeCardButton);

			nextTurnButton.onClick.AddListener(OnNextTurnClick);
			drawPileButton.onClick.AddListener(OnDrawPileButtonClick);
			dropPileButton.onClick.AddListener(OnDropPileButtonClick);
			exhaustPileButton.onClick.AddListener(OnExhaustPileButtonClick);
		}

		protected override void OnDestroy(){
			base.OnDestroy();

			_pile.OnDraw -= OnDrawCard;
			_pile.OnDrop -= OnDropCard;
			_pile.OnExhaust -= OnExhaustCard;
			_pile.OnDropToDraw -= OnDropToDraw;
			_pile.OnGive -= OnGiveCard;
			_pile.OnRemove -= OnRemoveCard;

			_combat.OnPlayerSpawn -= OnPlayerSpawn;
			_combat.OnEnemySpawn -= OnEnemySpawn;
			_combat.OnUseCardOnTarget -= UseCardAnim;

			TurnController.Instance.OnTurnStart -= OnTurnIn;
			TurnController.Instance.OnBattleStart -= OnBattleStart;
			TurnController.Instance.OnBattleEnd -= OnBattleEnd;

			drawCardButton.onClick.RemoveListener(OnDrawCardButton);
			changeCardButton.onClick.RemoveListener(OnChangeCardButton);

			nextTurnButton.onClick.RemoveListener(OnNextTurnClick);
			drawPileButton.onClick.RemoveListener(OnDrawPileButtonClick);
			dropPileButton.onClick.RemoveListener(OnDropPileButtonClick);
			exhaustPileButton.onClick.RemoveListener(OnExhaustPileButtonClick);
		}

		#region Character
		private void OnEnemySpawn(EnemyBase c){
			enemyDrawer.Show();
			enemyDrawer.SetBase(c);
			c.OnDeath += OnEnemyDeath;
		}

		private void OnPlayerSpawn(PlayerBase c){
			playerDrawer.SetBase(c);
			playerDrawer.Show();
			c.OnDeath += OnPlayerDeath;
		}

		private void OnEnemyDeath(CharacterBase c){
			enemyDrawer.Hide();
			enemyDrawer.SetBase(null);
			c.OnDeath -= OnEnemyDeath;
		}

		private void OnPlayerDeath(CharacterBase c){
			playerDrawer.Hide();
			playerDrawer.SetBase(null);
			c.OnDeath -= OnPlayerDeath;
		}

		private IEnumerator UseCardAnim(CharacterBase ch, CardBase card){
			if(ch is PlayerBase){
				yield return playerDrawer.UseCardOnTarget(card);
			} else if(ch is EnemyBase){
				yield return enemyDrawer.UseCardOnTarget(card);
			}
		}
		#endregion

		#region Card
		private void OnDrawCard(CardBase c){
			// 抽牌特效
		}

		private void OnChooseCard(CardDrawer d){
			if(d.CardState == CardState.InHand){
				_choseCards.Add(d);
			} else if(d.CardState == CardState.InSlot){
				_choseCards.Remove(d);
			}
		}

		private void OnDropCard(CardBase c){
			// 弃牌堆特效
		}

		private void OnExhaustCard(CardBase c){
			// 消耗特效
		}

		private void OnDropToDraw(int len){
			// 洗牌特效
		}

		private void OnGiveCard(CardBase c){
			CardDrawer d = _cardPool.GetItemFromPool();
			d.SetCardBase(c);
			d.OnChoose += OnChooseCard;
			_cardDrawers[c] = d;
		}

		private void OnRemoveCard(CardBase c){
			if(!_cardDrawers.TryGetValue(c, out CardDrawer d)) return;
			_cardPool.ReturnItemToPool(d);
			d.SetCardBase(null);
			d.OnChoose -= OnChooseCard;
			_cardDrawers.Remove(c);
		}

		public CardSlotItem GetFirstValidSlot(){
			return slotDrawer.GetFirstItem();
		}
		#endregion

		#region Button
		private void OnDrawCardButton(){
			_pile.DrawCard();
		}

		private void OnChangeCardButton(){
			foreach(CardDrawer choseCard in _choseCards){
				_pile.ExchangeCard(choseCard.CardBase);
			}
		}

		private void OnNextTurnClick(){
			nextTurnButton.interactable = false;
			StartCoroutine(_combat.NextTurn());
		}

		private void OnDrawPileButtonClick(){
			var b = UI.BagDrawer;
			if(b == null) return;
			b.SetPiles(_pile.GetDrawPile());
		}

		private void OnDropPileButtonClick(){
			var b = UI.BagDrawer;
			if(b == null) return;
			b.SetPiles(_pile.GetDropPile());
		}

		private void OnExhaustPileButtonClick(){
			var b = UI.BagDrawer;
			if(b == null) return;
			b.SetPiles(_pile.GetExhaustPile());
		}

		private void OnTurnIn(int x){
			if(x == 0){
				nextTurnButton.interactable = true;
			} else{
				// 敌方回合
				StartCoroutine(_combat.NextTurn());
			}
		}

		private void OnBattleEnd(){
			slotDrawer.Clear();

			foreach((CardBase c, CardDrawer d) in _cardDrawers){
				_cardPool.ReturnItemToPool(d);
				d.SetCardBase(null);
				d.OnChoose -= OnChooseCard;
			}

			_cardDrawers.Clear();
		}

		private void OnBattleStart(){
			slotDrawer.SpawnItem(_combat.GetSlotCount());
		}
		#endregion
	}
}