#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;

namespace Gameplay.Actions.Editor{
	[InitializeOnLoad]
	public static class ActionTypeGenerator{
		// 自动在 Unity 启动时生成
		static ActionTypeGenerator(){
			GenerateEnum();
		}

		[MenuItem("Tools/生成ActionType枚举")]
		public static void GenerateEnum(){
			// 枚举生成路径
			const string filePath = "Assets/Scripts/CardMini/Gameplay/EnumType/ActionType.cs";
			const string factoryPath = "Assets/Scripts/CardMini/Gameplay/Actions/ActionFactory.cs";

			//***********枚举
			// 获取 ActionBase 类型
			Type baseType = typeof(ActionBase);

			// 扫描所有继承自 ActionBase 的类
			var actionTypes = baseType.Assembly.GetTypes().Where(t => t.IsSubclassOf(baseType)).ToList();

			// 构造枚举内容
			string enumContent = "namespace Gameplay.Actions {\n" + "\tpublic enum ActionType {\n";
			foreach(Type t in actionTypes){
				enumContent += $"\t\t{t.Name[6..]},\n";
			}

			enumContent += "\t}\n}";

			//***********工厂
			string facContent = @"using Core.Data;
namespace Gameplay.Actions{
	public class ActionFactory{
		public static ActionBase Create(ActionData data){
			return data.actionType switch{
";
			foreach(Type t in actionTypes){
				facContent += $"\t\t\t\tActionType.{t.Name[6..]} => new {t.Name}(data),\n";
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