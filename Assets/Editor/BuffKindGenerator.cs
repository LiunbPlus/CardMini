#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gameplay.Buff;
using UnityEditor;

namespace Gameplay.Buffs.Editor{
	[InitializeOnLoad]
	public static class BuffTypeGenerator{
		// 自动在 Unity 启动时生成
		static BuffTypeGenerator(){
			GenerateEnum();
		}

		[MenuItem("Tools/生成BuffType枚举")]
		public static void GenerateEnum(){
			// 枚举生成路径
			const string filePath = "Assets/Scripts/CardMini/Gameplay/EnumType/BuffType.cs";
			const string factoryPath = "Assets/Scripts/CardMini/Gameplay/BuffSystem/BuffFactory.cs";

			//***********枚举
			// 获取 BuffBase 类型
			Type baseType = typeof(BuffBase);

			// 扫描所有继承自 BuffBase 的类
			List<Type> actionTypes = baseType.Assembly.GetTypes().Where(t => t.IsSubclassOf(baseType)).ToList();

			// 构造枚举内容
			string enumContent = @"namespace Gameplay.Buff {
	public enum BuffType {
";
			foreach(Type t in actionTypes){
				enumContent += $"\t\t{t.Name},\n";
			}

			enumContent += @"
	}
}";

			//***********工厂
			string facContent = @"using System;
using Controller;
using Gameplay.Character;
namespace Gameplay.Buff{
	public static class BuffFactory{
		public static BuffBase Create(int id, CharacterBase c, int s){
			var kind = Enum.Parse<BuffType>(DataManager.BuffData[id].cls);
			return kind switch{
";
			foreach(Type t in actionTypes){
				facContent += $"\t\t\t\tBuffType.{t.Name} => new {t.Name}(id,c,s),\n";
			}

			facContent += @"
				_ => null
			};
		}
	}
}";

			// 写入文件
			File.WriteAllText(filePath, enumContent);
			File.WriteAllText(factoryPath, facContent);
			AssetDatabase.Refresh();
		}
	}
}
#endif