using UnityEngine;

public class StrafeMovement : MovementScheme
{
    private Player player;
    private EnumBitField<Player.States> allowedForMovement;

    public StrafeMovement(Player player, MovementSettings settings, InputBindings bindings) : base(settings, bindings)
    {
        this.player = player;
        allowedForMovement = new EnumBitField<Player.States>(Player.States.Hooking);
    }

    protected override void UpdateImpl(Transform t)
    {
        Rigidbody body = t.GetComponent<Rigidbody>();
        UpdateMovement(body);
        UpdateRotation(body);
    }

    private void UpdateMovement(Rigidbody body)
    {
        if (!(player.State.Value == 0 || player.State == allowedForMovement))
        {
            return;
        }

        float dx = NDInput.GetAxis(bindings.Horizontal);
        float dz = NDInput.GetAxis(bindings.Vertical);
        Vector3 force = player.AimDirection * settings.Acceleration;

        if (player.State.IsOn(Player.States.Hooking))
        {
            force *= 0.8f;
        }

        if (dx != 0 || dz != 0)
        {
            body.AddForce(force, ForceMode.Acceleration);
            body.velocity = Vector3.ClampMagnitude(body.velocity, settings.MaxSpeed);
        }
    }

    private void UpdateRotation(Rigidbody body)
    {
        if (player.State.AreOff(Player.States.Stunned, Player.States.Hooked, Player.States.Hooking))
        {
            body.MoveRotation(Quaternion.LookRotation(player.AimDirection));
        }
    }
}