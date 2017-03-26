using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadTargetSceneButton : MonoBehaviour {
	
	public void LoadSceneNum(string name) {
		/*if (num < 0 || num >= SceneManager.sceneCountInBuildSettings) {
			Debug.LogWarning("Can't load scene num " + num + ", SceneManager only has " + SceneManager.sceneCountInBuildSettings + " scenes in BuildSettings");
			return;
		}*/
		
		LoadingScreenManager.LoadScene(name);
	}
}