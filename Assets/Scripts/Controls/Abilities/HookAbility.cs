using UnityEngine;

public class HookAbility : Ability {
    #region Fields
    private Player player;
    private GameObject playerObj;
    private GameObject hookObj;
    #endregion

    public HookAbility(Player player, GameObject playerObj, GameObject hookObj, float cooldown, string inputName) : base("Hook", cooldown, inputName) {
        this.player = player;
        this.playerObj = playerObj;
        this.hookObj = hookObj;
    }

    public override void OnButtonDown() {
        Activate();
    }

    protected override void ActivateImpl() {
        Logger.Logf("Ability ({0}): activated", Name);

        playerObj.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        player.State.On(Player.States.Hooking);
        Pipeline pipe = G.Instance.Pipeline.New().Delay(0.6f).Func(() => {
            player.State.Off(Player.States.Hooking);
        });

        Quaternion rot = playerObj.transform.rotation * hookObj.transform.localRotation;
        Vector3 pos = playerObj.transform.position + hookObj.transform.localPosition + playerObj.transform.forward * 2;
        GameObject instance = GameObject.Instantiate(hookObj, pos, rot, GameObject.Find("_Dynamic").transform);
        instance.GetComponent<Rigidbody>().freezeRotation = true;
        instance.GetComponent<Hook>().SetOrigin(playerObj.transform);
        instance.GetComponent<Hook>().SetCancelPipe(pipe);
        instance.GetComponent<Hook>().SetIsBlue(player.IsBlue());
        instance.SetActive(true);
    }

    protected override bool CanActivateImpl() {
        return player.State.Value == 0;
    }
}