using Gameplay.Buff;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Combat.Character{
	public class CharacterBuffImageItem: MonoBehaviour{
		[SerializeField] private Image image;
		[SerializeField] private TMP_Text stack;
		[SerializeField] private TMP_Text duration;

		public void SetBuff(BuffBase buff){
			image.sprite = buff.BuffData.image;
			stack.text = buff.Stack.ToString();
		}
	}
}