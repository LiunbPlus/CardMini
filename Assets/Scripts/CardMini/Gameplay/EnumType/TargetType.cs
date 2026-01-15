namespace Gameplay.Card{
	public enum TargetType{
		Self = 1,
		Other = 2,
		All = 3,
	}

	public static class TypeExtensionP{
		public static string GetTypeDesc(this TargetType targetType){
			return targetType switch{
				TargetType.Self  => "对自己",
				TargetType.Other => "对对方",
				TargetType.All   => "对所有人",
				_                => ""
			};
		}
	}
}