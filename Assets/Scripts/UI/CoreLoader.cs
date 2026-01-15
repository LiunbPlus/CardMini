using System;
using Controller;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NueGames.NueDeck.Scripts.Utils{
	[DefaultExecutionOrder(-11)]
	public class CoreLoader : MonoBehaviour{
		private void Awake(){
			try{
				if(GameManager.Instance == null) SceneManager.LoadScene(SceneName.CORE_SCENE, LoadSceneMode.Additive);
				Destroy(gameObject);
			}
			catch(Exception){
				Debug.LogError("你需要一个名为CoreScene的场景并放入控制器");
				throw;
			}
		}
	}
}