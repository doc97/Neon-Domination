using System.Collections;
using UnityEngine;

public class NextScene : MonoBehaviour {

    [SerializeField, Tooltip("Time before scene transition")]
    private float transitionTime;
    [SerializeField, Tooltip("Name of scene to load")]
    private string scene;

    public void Start() {
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer() {
        yield return new WaitForSeconds(transitionTime); 
        G.Instance.Scene.Load(scene);
    }
}