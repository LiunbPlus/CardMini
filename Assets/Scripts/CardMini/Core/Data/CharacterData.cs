using UnityEngine;

namespace Core.Data{
	public class CharacterData : ScriptableObject{
		[SerializeField] public int id;
		[SerializeField] public string cname;
		[SerializeField][TextArea] public string description;
		[SerializeField] public int initHealth = 20;
		[SerializeField] public int maxHealth = 20;
		[SerializeField] public int initDefense = 0;
		[SerializeField] public int maxDefense = 999;
	}
}
