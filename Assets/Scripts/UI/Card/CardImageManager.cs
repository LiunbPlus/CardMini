using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Card{
	public class CardImageManager{
		private Dictionary<int, Image> _images;

		private static CardImageManager instance;
		public static CardImageManager Instance{get;} = instance ??= new CardImageManager();

		private CardImageManager(){
			Load();
		}

		private void Load(){
			_images = new();
			Image[] allData = Resources.LoadAll<Image>("Image/Card");
			foreach(Image t in allData){
				_images[int.Parse(t.name)] = t;
			}
		}

		public Image GetImage(int id){
			return _images.TryGetValue(id, out Image image) ? image : null;
		}
	}
}