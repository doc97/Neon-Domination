using UnityEngine;

public class DashAbility : Ability
{
    private const float COOLDOWN = 4; // seconds

    #region Fields
    public float Force { get; } = 15;
    private Player player;
    private GameObject playerObj;
    private MovementSettings movementSettings;
    #endregion

    public DashAbility(Player player, GameObject playerObj, MovementSettings movementSettings, string inputName) : base("Dash", COOLDOWN, inputName)
    {
        this.player = player;
        this.playerObj = playerObj;
        this.movementSettings = movementSettings;
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        // Deactivate dashing status when no longer moving fast
        Vector3 velocity = playerObj.GetComponent<Rigidbody>().velocity;
        if (velocity.sqrMagnitude <= movementSettings.MaxSpeedSqrd)
           player.State.Off(Player.States.Dashing);
    }

    protected override void ActivateImpl()
    {
        Logger.Logf("Ability ({0}): activated", Name);
        playerObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        playerObj.GetComponent<Rigidbody>().AddForce(player.AimDirection * Force, ForceMode.Impulse);
        player.State.On(Player.States.Dashing);
    }

    protected override bool CanActivateImpl()
    {
        return player.State.Value == 0;
    }
}