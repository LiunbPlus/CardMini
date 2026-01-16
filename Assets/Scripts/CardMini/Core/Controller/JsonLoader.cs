using System;
using System.Collections.Generic;
using Core.Data;
using UnityEngine;

namespace Controller{
	public class JsonLoader<T> where T : Data{
		[Serializable]
		private class JsonArrayWrap{
			public T[] list;
		}

		private readonly Dictionary<int, T> _idToData = new();

		public JsonLoader(){
			LoadAllData();
		}

		private void LoadAllData(){
			var text = Resources.Load<TextAsset>($"JsonData/{typeof(T).Name}");
			if(text == null){
				Debug.LogWarning($"JsonData/{typeof(T).Name} Not Found!");
				return;
			}

			var allData = JsonUtility.FromJson<JsonArrayWrap>(text.text);
			foreach(T data in allData.list){
				_idToData[data.id] = data;
			}
		}

		public T GetData(int id){
			return _idToData.TryGetValue(id, out T data) ? data : null;
		}

		public Dictionary<int, T> GetAllData() => _idToData;

		public int DataCount => _idToData.Count;

		public T this[int id] => GetData(id);
	}
}