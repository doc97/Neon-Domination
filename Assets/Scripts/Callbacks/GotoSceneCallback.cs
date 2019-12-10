using UnityEngine;

public class GotoSceneCallback : BehaviourCallback {

    #region Fields
    [SerializeField]
    private string sceneName;
    #endregion

    public override void Call() {
        G.Instance.Scene.Load(sceneName);
    }
}