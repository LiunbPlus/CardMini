using UnityEngine;
using UnityEngine.UI;

namespace Core.Data{
	[CreateAssetMenu(menuName = "CardMini/Image Data")]
	public class ImageData : ScriptableObject{
		[SerializeField] public Image image;
	}
}