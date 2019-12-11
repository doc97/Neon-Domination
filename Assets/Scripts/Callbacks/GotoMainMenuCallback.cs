public class GotoMainMenuCallback : GotoSceneCallback {
    public override void Call() {
        G.Instance.Round.Reset();
        base.Call();
    }
}