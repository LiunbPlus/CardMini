using Core.Data;

namespace Controller{
	public static class DataManager{
		public static DataLoader<ImageData> ImageData{get; private set;}
		public static JsonLoader<PlayerData> PlayerData{get; private set;}
		public static JsonLoader<CardData> CardData{get; private set;}
		public static JsonLoader<EnemyData> EnemyData{get; private set;}
		public static JsonLoader<IntentData> IntentData{get; private set;}
		public static JsonLoader<BuffData> BuffData{get; private set;}
		public static ImageLoader<BuffData> BuffImageData{get; private set;}
		public static ImageLoader<CardData> CardImageData{get; private set;}
		public static ImageLoader<PlayerData> PlayerImageData{get; private set;}
		public static ImageLoader<EnemyData> EnemyImageData{get; private set;}

		public static void Init(){
			ImageData = new DataLoader<ImageData>();
			CardData = new JsonLoader<CardData>();
			IntentData = new JsonLoader<IntentData>();
			EnemyData = new JsonLoader<EnemyData>();
			PlayerData = new JsonLoader<PlayerData>();
			BuffData = new JsonLoader<BuffData>();
			BuffImageData = new ImageLoader<BuffData>();
			CardImageData = new ImageLoader<CardData>();
			PlayerImageData = new ImageLoader<PlayerData>();
			EnemyImageData = new ImageLoader<EnemyData>();
		}
	}
}