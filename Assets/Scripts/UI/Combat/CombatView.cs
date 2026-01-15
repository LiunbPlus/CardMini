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

		private readonly PileController _pile = PileController.Instance;
		private readonly CombatController _combat = CombatController.Instance;
		private CardDrawer _targetingCard;

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

			TurnController.Instance.OnTurnStart += OnTurnIn;

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

			TurnController.Instance.OnTurnStart -= OnTurnIn;

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
			c.OnDeath -= OnPlayerDeath;
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
			StartCoroutine(UseCardAnim());
		}

		private IEnumerator UseCardAnim(){
			foreach(CardDrawer choseCard in _choseCards){
				yield return playerDrawer.UseCardOnTarget(choseCard);
				CombatController.CommitCard(playerDrawer.CharacterBase, enemyDrawer.CharacterBase, choseCard.CardBase);
				yield return new WaitForSeconds(0.5f);
			}

			TurnController.Instance.NextTurn();
		}

		private void OnDrawPileButtonClick(){
			var b = UI.BagView;
			if(b == null) return;
			b.SetPiles(_pile.GetDrawPile());
		}

		private void OnDropPileButtonClick(){
			var b = UI.BagView;
			if(b == null) return;
			b.SetPiles(_pile.GetDropPile());
		}

		private void OnExhaustPileButtonClick(){
			var b = UI.BagView;
			if(b == null) return;
			b.SetPiles(_pile.GetExhaustPile());
		}

		private void OnTurnIn(int x){
			if(x != 0) return;
			nextTurnButton.interactable = true;
		}
		#endregion
	}
}