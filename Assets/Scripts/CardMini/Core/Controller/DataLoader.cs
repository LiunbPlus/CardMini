using System.Collections.Generic;
using UnityEngine;

namespace Controller{
	public class DataLoader<T> where T : ScriptableObject{
		private readonly Dictionary<string, T> _idToData = new();

		public DataLoader(){
			LoadAllData();
		}

		private void LoadAllData(){
			T[] allData = Resources.LoadAll<T>($"Data/{typeof(T).Name}");
			foreach(T t in allData){
				_idToData[t.name] = t;
			}
		}

		public T GetData(string id){
			return _idToData.TryGetValue(id, out T data) ? data : null;
		}

		public Dictionary<string, T> GetAllData() => _idToData;

		public int DataCount => _idToData.Count;
		public T this[string id] => GetData(id);
	}
}