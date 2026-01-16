using System.Collections.Generic;
using Core.Data;
using Gameplay.Actions;
using Gameplay.Character;

namespace Gameplay.Card{
	public class CardBase{
		public int Id{get; private set;}
		public string Name{get; private set;}
		public string Description{get; private set;}
		private string DefDesc{get; set;}
		public int Cost{get; private set;}
		public RarityType Rarity{get; private set;}
		public CardType CardType{get; private set;}
		public string AnimatorId{get; private set;}
		public List<ActionBase> Actions{get; private set;}

		public CardBase(CardData data){
			Id = data.id;
			Name = data.name;
			DefDesc = data.desc;
			Cost = data.cost;
			Rarity = (RarityType)data.rarity;
			CardType = (CardType)data.cardType;
			AnimatorId = data.anim;

			Actions = new List<ActionBase>();
			string defDesc = string.Empty;
			foreach(var ad in data.actions){
				var actionData = new ActionData(ad);
				// 需要瞄准

				ActionBase v = ActionFactory.Create(actionData);
				Actions.Add(v);
				defDesc += v.GetPreviewText() + "\n";
			}

			Description = DefDesc.Replace("<default>", defDesc);
		}

		public CardBase(){}

		public void UpdateDesc(){
			string defDesc = string.Empty;
			foreach(var ad in Actions){
				defDesc += ad.GetPreviewText() + "\n";
			}
			Description = DefDesc.Replace("<default>", defDesc);
		}

		public void SetSource(CharacterBase c){
			foreach(ActionBase actionBase in Actions){
				actionBase.SetSource(c);
			}
		}

		public void SetData(CardData data){
			Id = data.id;
			Name = data.name;
			DefDesc = data.desc;
			Cost = data.cost;
			Rarity = (RarityType)data.rarity;
			CardType = (CardType)data.cardType;
			AnimatorId = data.anim;

			Actions = new List<ActionBase>();
			string defDesc = string.Empty;
			foreach(var ad in data.actions){
				var actionData = new ActionData(ad);
				// 需要瞄准

				ActionBase v = ActionFactory.Create(actionData);
				Actions.Add(v);
				defDesc += v.GetPreviewText() + "\n";
			}

			Description = DefDesc.Replace("<default>", defDesc);
		}
	}
}