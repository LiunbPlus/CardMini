using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Combat.Character{
	public class CharacterHealthBar : MonoBehaviour{
		[Tooltip("（最上方的）动态血量")]
		[SerializeField]
		private Image hpImg;
		[Tooltip("（最底层的）扣血显示")]
		[SerializeField]
		private Image wImg;
		[Tooltip("（中层的）真实血量")]
		[SerializeField]
		private Image pImg;

		private float _health;
		private float _maxHealth;
		[Tooltip("动态持续时间")] public float effectTime = 0.8f;

		private Coroutine _updateCoroutine;

		public void Init(int maxHp){
			_health = _maxHealth = maxHp;
			UpdateHealthBar();
		}

		public void ChangeHealth(float addend){
			if(addend == 0) return;

			_health = Mathf.Clamp(_health + addend, 0f, _maxHealth);

			pImg.fillAmount = _health / _maxHealth;
			hpImg.fillAmount = Mathf.Clamp(hpImg.fillAmount, 0f, pImg.fillAmount);
			wImg.fillAmount = Mathf.Clamp(wImg.fillAmount, pImg.fillAmount, 1f);

			UpdateHealthBar();
		}

		private void UpdateHealthBar(){
			if(_updateCoroutine != null){
				StopCoroutine(_updateCoroutine);
			}

			_updateCoroutine = StartCoroutine(UpdateEffect());
		}

		private IEnumerator UpdateEffect(){
			float wItv = wImg.fillAmount - pImg.fillAmount;
			float pItv = pImg.fillAmount - hpImg.fillAmount;
			float elapsedTime = 0f;

			while(elapsedTime < effectTime && (wItv > 0 || pItv > 0)){
				elapsedTime += Time.deltaTime;

				if(wImg.fillAmount > pImg.fillAmount)
					wImg.fillAmount = Mathf.Lerp(wItv + pImg.fillAmount, pImg.fillAmount, elapsedTime / effectTime);

				if(pItv > 0)
					hpImg.fillAmount = Mathf.Lerp(pImg.fillAmount - pItv, pImg.fillAmount, elapsedTime / effectTime);

				yield return 0;
			}

			wImg.fillAmount = hpImg.fillAmount = pImg.fillAmount;
		}
	}
}