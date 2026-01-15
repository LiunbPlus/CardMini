using UnityEngine;

namespace Util.Extension{
	public static class Vector3Extension{
		public static Vector3 WithX(this Vector3 v, float x) => new Vector3(x, v.y, v.z);

		public static Vector3 WithY(this Vector3 v, float y) => new Vector3(v.x, y, v.z);

		public static Vector3 WithZ(this Vector3 v, float z) => new Vector3(v.x, v.y, z);

		public static Vector2 WithX(this Vector2 v, float x) => new Vector2(x, v.y);

		public static Vector2 WithY(this Vector2 v, float y) => new Vector2(v.x, y);

		public static Vector3 WithZ(this Vector2 v, float z) => new Vector3(v.x, v.y, z);

		public static Vector3 WithAddX(this Vector3 v, float x) => new Vector3(v.x + x, v.y, v.z);

		public static Vector3 WithAddY(this Vector3 v, float y) => new Vector3(v.x, v.y + y, v.z);

		public static Vector3 WithAddZ(this Vector3 v, float z) => new Vector3(v.x, v.y, v.z + z);

		public static Vector2 WithAddX(this Vector2 v, float x) => new Vector2(v.x + x, v.y);

		public static Vector2 WithAddY(this Vector2 v, float y) => new Vector2(v.x, v.y + y);
	}
}