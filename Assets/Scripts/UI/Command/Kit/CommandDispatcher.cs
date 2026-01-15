using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Log;

namespace Gameplay.Command{
	public sealed class CommandDispatcher{
		public static CommandDispatcher Instance{get; private set;}

		private sealed class CommandEntry{
			public string name;
			public string description;
			public string usage;
			public object instance;
			public Type argType;
			public MethodInfo execute;
		}

		private readonly Dictionary<string, CommandEntry> commands = new();

		public CommandDispatcher(){
			Instance = this;
			RegisterAll();
		}

		private void RegisterAll(){
			IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
				.Where(t => t.GetCustomAttribute<CommandAttribute>() != null);

			foreach(Type type in types){
				var attr = type.GetCustomAttribute<CommandAttribute>();
				Type argType = type.GetInterfaces()
					.First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommand<>)).GetGenericArguments()[0];
				commands[attr.Name] = new CommandEntry{
					name = attr.Name,
					description = attr.Description,
					usage = CommandUsageBuilder.Build(attr.Name, argType),
					instance = Activator.CreateInstance(type),
					argType = argType,
					execute = type.GetMethod("Execute")
				};
			}
		}

		public void Execute(string input){
			string[] parts = input[1..].Split(' ', StringSplitOptions.RemoveEmptyEntries);
			if(parts.Length == 0) return;

			string name = parts[0];
			string[] rawArgs = parts.Skip(1).ToArray();

			if(!commands.TryGetValue(name, out var cmd)){
				ChatLog.SendLog(LogLvl.Warn, $"未知的命令: {name}");
				return;
			}

			CommandEntry entry = commands[name];

			Type argType = entry.argType;

			MethodInfo parse = typeof(CommandArgParser).GetMethod(nameof(CommandArgParser.TryParse))?.MakeGenericMethod(argType);
			if(parse == null){
				ChatLog.SendLog(LogLvl.Error, $"找不到{name}的Parser");
				return;
			}

			object[] parameters ={rawArgs, null, null};
			bool ok = (bool)parse.Invoke(null, parameters); // Parse参数

			if(!ok){
				ChatLog.SendLog(LogLvl.Error, parameters[2] as string);
				ChatLog.SendLog(LogLvl.Info, $"用法: {entry.usage}");
				return;
			}

			entry.execute?.Invoke(cmd.instance, new[]{parameters[1]});
		}

		public void ShowAllCommands(){
			ChatLog.SendLog(LogLvl.Info, "可用命令:");

			foreach(CommandEntry entry in commands.Values.OrderBy(c => c.name)){
				ChatLog.SendLog(LogLvl.Info, $"/{entry.name} - {entry.description}");
			}
		}

		public void ShowCommandHelp(string name){
			if(!commands.TryGetValue(name, out var entry)){
				ChatLog.SendLog(LogLvl.Error, $"未知的命令: {name}");
				return;
			}

			ChatLog.SendLog(LogLvl.Info, $"用法: {entry.usage}");

			FieldInfo[] fields = entry.argType.GetFields();
			foreach(FieldInfo f in fields){
				var arg = f.GetCustomAttribute<ArgAttribute>();
				if(arg != null){
					ChatLog.SendLog(LogLvl.Info,
						$"  {(arg.Optional ? "[" : "<")}{arg.Name}{(arg.Optional ? "]" : ">")} : {arg.Description}");
				}
			}
		}
	}
}