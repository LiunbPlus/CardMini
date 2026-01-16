/*
 * 此文件的字段建议与Json对齐，在CardBase处再解析
 */

using System;
using System.Collections.Generic;
using Gameplay.Actions;
using Gameplay.Card;

namespace Core.Data{
	[Serializable]
	public class CardData : Data{
		public string name;
		public string desc;
		public int cost;
		public int rarity;
		public int cardType;
		public string anim;
		public List<RawActionData> actions;

		public CardData
		(int id, string name, string desc, int cost, int rarity, int cardType, string anim,
		 List<RawActionData> actions) : base(id){
			this.name = name;
			this.desc = desc;
			this.cost = cost;
			this.rarity = rarity;
			this.cardType = cardType;
			this.anim = anim;
			this.actions = actions;
		}

		public override string ToString(){
			return $"{id} {name}";
		}
	}

	[Serializable]
	public struct RawActionData{
		public string type;
		public int value;
		public string target;
		public int buff;
		public int count;

		public RawActionData(string type, int value, string target, int buff, int count){
			this.type = type;
			this.value = value;
			this.target = target;
			this.buff = buff;
			this.count = count;
		}
	}

	public struct ActionData{
		public readonly ActionType actionType;
		public readonly TargetType targetType;
		public readonly int value;
		public readonly int buff;
		public readonly int count;

		public ActionData(RawActionData ad){
			actionType = Enum.Parse<ActionType>(ad.type);
			value = ad.value;
			targetType = Enum.Parse<TargetType>(ad.target);
			buff = ad.buff;
			count = ad.count;
		}
	}
}