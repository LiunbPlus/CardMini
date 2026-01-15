/*
 * 此文件的字段建议与Json对齐，在CardBase处再解析
 */

using System;
using System.Collections.Generic;
using Gameplay.Actions;
using Gameplay.Buff;
using Gameplay.Card;

namespace Core.Data{
	public class CardData{
		public readonly int id;
		public readonly string name;
		public readonly string desc;
		public readonly int cost;
		public readonly string rarity;
		public readonly string cardType;
		public readonly string anim;
		public readonly List<RawActionData> actions;

		public CardData
		(int id, string name, string desc, int cost, string rarity, string cardType, string anim,
		 List<RawActionData> actions){
			this.id = id;
			this.name = name;
			this.desc = desc;
			this.cost = cost;
			this.rarity = rarity;
			this.cardType = cardType;
			this.anim = anim;
			this.actions = actions;
		}
	}

	public struct RawActionData{
		public readonly string type;
		public readonly int value;
		public readonly string target;
		public readonly string buff;

		public RawActionData(string type, int value, string target, string buff){
			this.type = type;
			this.value = value;
			this.target = target;
			this.buff = buff;
		}
	}

	public struct ActionData{
		public readonly ActionType actionType;
		public readonly int value;
		public readonly TargetType targetType;
		public readonly BuffType buffType;

		public ActionData(RawActionData ad){
			actionType = Enum.Parse<ActionType>(ad.type);
			value = ad.value;
			targetType = Enum.Parse<TargetType>(ad.target);
			buffType = Enum.Parse<BuffType>(ad.buff);
		}
	}
}