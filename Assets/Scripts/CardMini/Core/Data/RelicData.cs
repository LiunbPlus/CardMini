using Gameplay.Card;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Data{
	[CreateAssetMenu(menuName = "CardMini/Relic Data")]
	public class RelicData : ScriptableObject{
		[SerializeField] public string cname;
		[SerializeField] public string desc;

		[SerializeField] public Image image;
		[SerializeField] public RarityType rarity;
	}
}