using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NDSceneManager : MonoBehaviour {

    private AsyncOperation async;
    public bool IsLoading { get => (!async?.isDone) ?? false; }
    public float Progress { get => async?.progress ?? 0; }
    public string LoadingScene { get; private set; }

    public void Load(string sceneName) {
        if (!IsLoading) {
            StartCoroutine(LoadNextScene(sceneName));
        }
    }

    private IEnumerator LoadNextScene(string sceneName) {
        LoadingScene = sceneName;
        async = SceneManager.LoadSceneAsync(sceneName);
        while (!async.isDone) {
            yield return null;
        }
        LoadingScene = "";
    }
}