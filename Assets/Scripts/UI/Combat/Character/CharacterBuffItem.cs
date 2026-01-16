using Controller;
using Gameplay.Buff;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Combat.Character{
	public class CharacterBuffItem: UIBase{
		[SerializeField] private Image image;
		[SerializeField] private TMP_Text stack;
		protected override int OperateLayer => 0;

		public void SetBuff(BuffBase buff){
			image.sprite = DataManager.BuffImageData.GetData(buff.BuffData.img).sprite;
			stack.text = buff.Stack.ToString();
		}
	}
}