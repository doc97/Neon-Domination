using UnityEngine;

[RequireComponent(typeof(NDSceneManager))]
public class GObject : MonoBehaviour {

    private void Awake() {
        G.Instance.Scene = GetComponent<NDSceneManager>();
    }

    private void Update() {
        G.Instance.Update(Time.deltaTime);
    }
}