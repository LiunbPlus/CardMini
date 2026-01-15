
namespace Gameplay.Command{
	[Command("kill", "删除角色")]
	public class KillCommand : ICommand<KillArgs>{
		public void Execute(KillArgs args){

		}
	}

	public struct KillArgs{
	}
}