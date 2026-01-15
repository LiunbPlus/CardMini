using System.Collections.Generic;
using Core.Data;
using UnityEngine;

namespace UI.Combat.ActionAnimator{
	public class AnimLoader{
		private readonly Dictionary<string, CardAnimator> _idToData = new();

		private static AnimLoader instance;
		public static AnimLoader Instance{get;} = instance ??= new();

		private AnimLoader(){
			LoadAllData();
		}

		private void LoadAllData(){
			CardAnimator[] allData = Resources.LoadAll<CardAnimator>("Data/CardAnimator");
			foreach(CardAnimator t in allData){
				_idToData[t.name] = t;
			}
		}

		public CardAnimator GetData(string id){
			return _idToData.TryGetValue(id, out CardAnimator data) ? data : null;
		}
	}
}