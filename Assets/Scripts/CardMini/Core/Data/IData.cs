using System;

namespace Core.Data{
	[Serializable]
	public abstract class Data{
		public int id;

		protected Data(int i) => id = i;
	}
}