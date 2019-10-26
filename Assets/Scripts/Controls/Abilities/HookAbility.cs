using UnityEngine;

public class HookAbility : Ability
{
    private const float COOLDOWN = 5; // seconds

    #region Fields
    private GameObject player;
    private GameObject hook;
    #endregion

    public HookAbility(GameObject player, GameObject hook, string inputName) : base("Hook", COOLDOWN, inputName)
    {
        this.player = player;
        this.hook = hook;
    }

    protected override void ActivateImpl()
    {
        Logger.Logf("Ability ({0}): activated", Name);
        Quaternion rot = player.transform.rotation * hook.transform.localRotation;
        Vector3 pos = player.transform.position + hook.transform.localPosition + player.transform.forward * 2;
        GameObject instance = GameObject.Instantiate(hook, pos, rot, GameObject.Find("_Dynamic").transform);
        instance.GetComponent<Hook>().SetOrigin(player.transform);
        instance.SetActive(true);
    }

    protected override bool CanActivateImpl()
    {
        return true;
    }
}