using System;

namespace Gameplay.Command{
	public interface ICommand<in TArgs>{
		void Execute(TArgs args);
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class CommandAttribute : Attribute{
		public string Name{get;}
		public string Description{get;}

		public CommandAttribute(string name, string description = ""){
			Name = name;
			Description = description;
		}
	}


	[AttributeUsage(AttributeTargets.Field)]
	public sealed class ArgAttribute : Attribute{
		public string Name{get;}
		public string Description{get;}
		public bool Optional{get; set;}

		public ArgAttribute(string name, string description = ""){
			Name = name;
			Description = description;
		}
	}
}