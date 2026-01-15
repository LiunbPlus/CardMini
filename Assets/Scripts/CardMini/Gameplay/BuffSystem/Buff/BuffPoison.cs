using Gameplay.Character;

namespace Gameplay.Buff{
	public class BuffPoison : BuffBase{
		public BuffPoison(CharacterBase owner, int stack) : base(owner, stack){}

		public override void OnTurnIn(){
			base.OnTurnIn();
			Owner.ChangeHealth(-Stack, Owner);
		}
	}
}