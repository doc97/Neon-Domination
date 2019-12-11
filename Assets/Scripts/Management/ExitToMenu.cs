using UnityEngine;

public class ExitToMenu : MonoBehaviour {

    [SerializeField]
    private KeyCode key;

    public void Update() {
        if (Input.GetKeyDown(key)) {
            G.Instance.Scene.Load("MainMenu");
        }
    }
}