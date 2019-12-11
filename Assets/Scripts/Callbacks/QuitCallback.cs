using UnityEditor;
using UnityEngine;

public class QuitCallback : BehaviourCallback {
    public override void Call() {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}