using Controller;
using Gameplay.Card;
using Gameplay.Character;
using NUnit.Framework;

namespace Tests.Gameplay{
	public class ActionTest{
		private PlayerBase _player;
		private EnemyBase _enemy;
		private CardBase _card;
		private const int RANDOM_SEED = 42;

		[OneTimeSetUp]
		public void DefaultSet(){
			DataManager.Init();
			Assert.NotNull(DataManager.EnemyData, "EnemyData is null");
			Assert.NotNull(DataManager.PlayerData, "PlayerData is null");
			Assert.NotNull(DataManager.BuffData, "BuffData is null");
			Assert.NotNull(DataManager.CardData, "CardData is null");
			Assert.NotNull(DataManager.IntentData, "IntentData is null");
			Assert.NotNull(DataManager.ImageData, "ImageData is null");


			var pd = DataManager.PlayerData[5001];
			Assert.NotNull(pd, "PlayerData[5001] is null");

			_player = CombatController.Instance.EnterMap(5001, RANDOM_SEED);
			Assert.NotNull(_player, "_player is null");

			PileController.Instance.SetPlayer(_player.HandCount, RANDOM_SEED);

			var ed = DataManager.EnemyData[4001];
			Assert.NotNull(ed, "EnemyData[4001] is null");

			_enemy = CombatController.Instance.EnterBattle(4001);
			Assert.NotNull(_enemy, "_enemy is null");

			Assert.AreEqual(PileController.Instance.GetHandPile().Count, 5, "Draw Count Incorrect");
		}

		[Test]
		public void DamageTest(){
			Assert.AreEqual(_enemy.Health, 20);

			var dat = DataManager.CardData[1001];
			_card = new CardBase(dat);
			var dat2 = DataManager.CardData[1002];
			var c = new CardBase(dat2);

			CombatController.CommitCard(_player, _enemy, _card);
			Assert.AreEqual(_enemy.Health, 14, "Calc1 Wrong");

			CombatController.CommitCard(_player, _enemy, c);
			Assert.True(_player.BuffController.GetBuff(8001)!=null, "Add Buff Wrong");
			CombatController.CommitCard(_player, _enemy, _card);
			Assert.AreEqual(_enemy.Health, 7, "Calc2 Wrong");
		}
	}
}