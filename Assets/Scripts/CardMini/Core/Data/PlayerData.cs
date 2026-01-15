using UnityEngine;

namespace Core.Data{
	[CreateAssetMenu(menuName = "CardMini/Player Data")]
	public class PlayerData : CharacterData{
		[SerializeField] public int initCoin = 99;
		[SerializeField] public int maxHandCard = 10; // 手牌上限
	}
}