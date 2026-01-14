using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

[Preserve]
public class SkipUnityLogo{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
	private static void BeforeSplashScreen(){
		System.Threading.Tasks.Task.Run(AsyncSkip);
	}

	private static void AsyncSkip(){
		SplashScreen.Stop(SplashScreen.StopBehavior.StopImmediate);
	}
}

/*
 * 战前选择武器，进入游戏之后由CB直接持有
 *
 */