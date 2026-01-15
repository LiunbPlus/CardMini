using Gameplay.Buff;
using UnityEngine;

namespace Core.Data{
	[CreateAssetMenu(fileName = "BuffData", menuName = "CardMini/Buff Data")]
	public class BuffData : ScriptableObject{
		[Header("基本信息")]
		public string Name = "名称";
		[TextArea]public string description = "简介";

		[Header("基础属性")]
		public Sprite image;
		public BuffKind kind;
		public int maxStacks = 99;
	}
}