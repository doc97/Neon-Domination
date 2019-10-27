using UnityEngine;

public class DashAbility : Ability
{
    #region Fields
    public float Force { get; }
    private Player player;
    private GameObject playerObj;
    private MovementSettings movementSettings;
    #endregion

    public DashAbility(Player player, GameObject playerObj, MovementSettings movementSettings, float force, float cooldown, string inputName)
        : base("Dash", cooldown, inputName)
    {
        this.player = player;
        this.playerObj = playerObj;
        this.movementSettings = movementSettings;
        Force = force;
    }

    protected override void ActivateImpl()
    {
        Logger.Logf("Ability ({0}): activated", Name);
        playerObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        playerObj.GetComponent<Rigidbody>().AddForce(player.AimDirection * Force, ForceMode.Impulse);
        player.State.On(Player.States.Dashing);
        G.Instance.Pipeline.New().Delay(1).Func(() => {
            player.State.Off(Player.States.Dashing);
        });
    }

    protected override bool CanActivateImpl()
    {
        return player.State.Value == 0;
    }
}