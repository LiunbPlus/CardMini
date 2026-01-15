using UnityEngine;

namespace Core.Data{
	[CreateAssetMenu(menuName = "CardMini/Enemy Data")]
	public class EnemyData : CharacterData{
		[SerializeField] public int intentCount;
	}
}

/*
 * EnemyData
 * intents [
 *	{
 *		int id
 *		int weight
 *	},
 *	{
 *		int id
 *		int weight
 *	}
 * ]
 */

/*
 * InteneData [
 *	{
 *		int id
 *		[int, int, int, int]
 *	},
 *
 * ]
 */
