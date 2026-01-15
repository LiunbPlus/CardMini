using Core.Data;

namespace Controller{
	public class DataManager{
		private static DataManager instance;
		public static DataManager Instance => instance ??= new ();

		public DataLoader<ImageData> ImageData{get; private set;}
		public DataLoader<PlayerData> PlayerData{get; private set;}
		public DataLoader<EnemyData> EnemyData{get; private set;}
		public DataLoader<EnemyPoolData> EnemyPoolData{get; private set;}
		public DataLoader<BuffData> BuffData{get; private set;}

		public void Init(){
			ImageData = new DataLoader<ImageData>();
			PlayerData = new DataLoader<PlayerData>();
			EnemyData = new DataLoader<EnemyData>();
			EnemyPoolData = new DataLoader<EnemyPoolData>();
			BuffData = new DataLoader<BuffData>();
		}
	}
}