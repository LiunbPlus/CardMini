using Core.Data;

namespace Gameplay.Character{
	public class EnemyBase : CharacterBase{
		public int IntentCount{get; private set;}

		public EnemyBase(EnemyData data) : base(data){
			Team = 1;
			IntentCount = data.intentCount;
		}
	}
}