using UnityEngine;

public class GotoSceneCallback : BehaviourCallback {

    #region Fields
    [SerializeField]
    private string sceneName;
    #endregion

    public override void Call() {
        G.Instance.Round.Reset(); // Move later into main menu
        G.Instance.Scene.Load(sceneName);
    }
}