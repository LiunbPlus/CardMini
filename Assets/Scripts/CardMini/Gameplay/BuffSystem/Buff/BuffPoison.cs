using Gameplay.Character;

namespace Gameplay.Buff{
	public class BuffPoison : BuffBase{
		public BuffPoison(int id, CharacterBase owner, int stack) : base(id, owner, stack){}

		public override void OnTurnIn(){
			base.OnTurnIn();
			Owner.ChangeHealth(-Stack, Owner);
		}
	}
}