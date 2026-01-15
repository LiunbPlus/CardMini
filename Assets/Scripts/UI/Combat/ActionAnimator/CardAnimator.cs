using System.Collections;
using UI.Card;
using UI.Combat.Character;
using UnityEngine;

namespace Core.Data{
	public abstract class CardAnimator : ScriptableObject{
		protected bool isInterrupted = false;

		public IEnumerator Play(CharacterDrawer s, CardDrawer c){
			isInterrupted = false;
			yield return PlayInternal(s, c);
		}

		protected abstract IEnumerator PlayInternal(CharacterDrawer s, CardDrawer c);

		// 外部调用中断动画
		public void Interrupt() => isInterrupted = true;

		// 平滑移动协程
		internal IEnumerator MoveSmooth(Transform obj, Vector3 to, float duration){
			Vector3 from = obj.position;
			float t = 0f;
			while(t < 1f){
				if(isInterrupted) break;
				t += Time.deltaTime / duration;
				if(obj) obj.position = Vector3.Lerp(from, to, t);
				yield return null;
			}

			if(obj) obj.position = to;
		}
	}
}