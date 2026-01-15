using System;
using System.Reflection;

namespace Gameplay.Command{
	public static class CommandArgParser{
		public static bool TryParse<T>(string[] raw, out T result, out string error) where T : struct{
			result = default;
			error = null;

			var fields = typeof(T).GetFields();
			int rawIndex = 0;

			for(int i = 0; i < fields.Length; i++){
				var field = fields[i];
				var attr = field.GetCustomAttribute<ArgAttribute>();
				bool optional = attr is{Optional: true};

				// 判断 raw 是否还有输入
				if(rawIndex >= raw.Length){
					if(optional){
						// 可选字段，保持默认值
						continue;
					} else{
						error = $"还需要{i + 1}个参数, 检测到{raw.Length}";
						return false;
					}
				}

				string rawValue = raw[rawIndex];
				if(!TryConvert(rawValue, field.FieldType, out object value)){
					error = $"{field.Name}的类型与期望不符";
					return false;
				}

				field.SetValueDirect(__makeref(result), value);
				rawIndex++;
			}

			return true;
		}


		private static bool TryConvert(string raw, Type type, out object value){
			value = null;

			if(type == typeof(string)){
				value = raw;
				return true;
			}

			if(type == typeof(int) && int.TryParse(raw, out var i)){
				value = i;
				return true;
			}

			if(type == typeof(ulong) && ulong.TryParse(raw, out var u)){
				value = u;
				return true;
			}

			if(type == typeof(byte) && byte.TryParse(raw, out var y)){
				value = y;
				return true;
			}

			if(type == typeof(bool) && bool.TryParse(raw, out var b)){
				value = b;
				return true;
			}

			if(type.IsEnum && Enum.TryParse(type, raw, true, out value))
				return true;

			return false;
		}
	}
}