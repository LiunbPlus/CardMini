using System.Collections;
using Core.Data;
using Gameplay.Character;
using UI.Card;
using UI.Combat.Character;
using UnityEngine;
using Util.Extension;

namespace UI.Combat.ActionAnimator{
	[CreateAssetMenu(menuName = "CardMini/ActionAnimators/MoveTowards")]
	public class MoveTowardsAnimator : CardAnimator{
		[SerializeField] private float dashTime = 0.05f;
		[SerializeField] private float waitTime = 0.05f;
		[SerializeField] private float returnTime = 0.05f;
		[SerializeField] private float puncture = 0.3f;

		protected override IEnumerator PlayInternal(CharacterDrawer s, CardDrawer c){
			Vector3 start = s.transform.position;
			Vector3 targetPos = s.CharacterBase is PlayerBase ? start.WithX(10) : start.WithX(-10);
			Vector3 dashPos = Vector3.Lerp(start, targetPos, puncture);

			// 冲刺
			if(s) yield return MoveSmooth(s.transform, dashPos, dashTime);

			// 攻击挥动动画
			if(!isInterrupted){
				yield return new WaitForSeconds(waitTime);
			}

			// 回位
			if(s) yield return MoveSmooth(s.transform, start, returnTime);
		}
	}
}