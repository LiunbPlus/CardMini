using System;
using System.Collections.Generic;
using Core.Data;
using Gameplay.Card;
using UnityEngine;

namespace Controller{
	public static class CardManager{
		private static readonly Dictionary<int, CardData> CardData = new();
		private static readonly Dictionary<RarityType, List<int>> RarityData = new();

		public static void LoadAll(){
			CardData[] cardDats = JsonUtility.FromJson<CardData[]>(GameManager.Instance.cardData.text);
			foreach(var cardData in cardDats){
				CardData.Add(cardData.id, cardData);

				var r = Enum.Parse<RarityType>(cardData.rarity);
				if(!RarityData.ContainsKey(r)) RarityData[r] = new();
				RarityData[r].Add(cardData.id);
			}

			Debug.Log($"Load {CardData.Count} Card Data");
		}

		public static List<int> GetAllCardIdByRarity(RarityType rarity){
			return RarityData[rarity];
		}

		public static CardBase GetCard(int cardName){
			return !CardData.ContainsKey(cardName) ? null : new CardBase(CardData[cardName]);
		}
	}
}