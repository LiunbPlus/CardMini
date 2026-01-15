using UI.Chat;

namespace Log{
	public enum LogLvl{
		Debug,
		Info,
		Warn,
		Error
	}

	public static class ChatLog{
		public static void SendLog(LogLvl lvl, string msg){
			string prefix = lvl switch{
				LogLvl.Debug => "<color=purple>[Debug] ",
				LogLvl.Info  => "<color=white>",
				LogLvl.Warn  => "<color=yellow>!",
				LogLvl.Error => "<color=red>!!",
				_            => ""
			};

			ChatPresenter.Instance?.LocalMessage(prefix + msg + "</color>");
		}
	}
}