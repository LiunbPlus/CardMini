using System;
using System.Linq;
using System.Reflection;

namespace Gameplay.Command{
	public static class CommandUsageBuilder{
		public static string Build(string commandName, Type argType){
			var fields = argType.GetFields();

			var args = fields.Select(f => {
				var attr = f.GetCustomAttribute<ArgAttribute>();
				return attr != null ? $"<{attr.Name}>" : $"<{f.Name}>";
			});

			return $"/{commandName} {string.Join(" ", args)}";
		}
	}
}