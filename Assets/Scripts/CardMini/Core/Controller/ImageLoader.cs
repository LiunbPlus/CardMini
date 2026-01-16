using System.Collections.Generic;
using Core.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Controller{
	public class ImageLoader<T> where T: Data{
		private readonly Dictionary<string, Image> _idToData = new();

		public ImageLoader(){
			LoadAllData();
		}

		private void LoadAllData(){
			var allData = Resources.LoadAll<Image>($"Image/{typeof(T).Name}");
			if(allData == null){
				Debug.LogWarning($"Image/{typeof(T).Name} Not Found!");
				return;
			}

			foreach(Image data in allData){
				_idToData[data.name] = data;
			}
		}

		public Image GetData(string id){
			return _idToData.TryGetValue(id, out Image data) ? data : null;
		}

		public Dictionary<string, Image> GetAllData() => _idToData;

		public int DataCount => _idToData.Count;
		public Image this[string id] => GetData(id);
	}
}