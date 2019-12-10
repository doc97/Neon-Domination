using UnityEngine;

public class GotoSceneCallback : BehaviourCallback {

    #region Fields
    [SerializeField]
    private string sceneName;
    [SerializeField]
    private bool QuitGame = false;
    #endregion

    public override void Call() {
        if (QuitGame == true)
        {
            Application.Quit();
        }
        else
        {
            G.Instance.Round.Reset();
            G.Instance.Scene.Load(sceneName);  
        }  
    }
}