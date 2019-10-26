using UnityEngine;

public class DashAbility : Ability
{
    private const float COOLDOWN = 4; // seconds
    private const float FORCE = 50;

    #region Fields
    private Player player;
    private GameObject playerObj;
    #endregion

    public DashAbility(Player player, GameObject playerObj, string inputName) : base("Dash", COOLDOWN, inputName)
    {
        this.player = player;
        this.playerObj = playerObj;
    }

    protected override void ActivateImpl()
    {
        Logger.Logf("Ability ({0}): activated", Name);
        playerObj.GetComponent<Rigidbody>().AddForce(playerObj.transform.forward * FORCE, ForceMode.Impulse);
    }

    protected override bool CanActivateImpl()
    {
        return player.State.Value == 0;
    }
}