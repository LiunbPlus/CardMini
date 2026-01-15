
namespace Gameplay.Command{
	[Command("help", "显示命令帮助")]
	public sealed class HelpCommand : ICommand<HelpArgs>{
		public void Execute(HelpArgs args){
			if(string.IsNullOrEmpty(args.command))
				CommandDispatcher.Instance.ShowAllCommands();
			else
				CommandDispatcher.Instance.ShowCommandHelp(args.command);
		}
	}

	public struct HelpArgs{
		[Arg("command", "命令名（可选）", Optional = true)]
		public string command;
	}
}