using UnityEngine;

public class StrafeMovement : MovementScheme
{
    private Player player;

    public StrafeMovement(Player player, MovementSettings settings, InputBindings bindings) : base(settings, bindings)
    {
        this.player = player;
    }

    protected override void UpdateImpl(Transform t)
    {
        Rigidbody body = t.GetComponent<Rigidbody>();
        UpdateMovement(body);
        UpdateRotation(body);
    }

    private void UpdateMovement(Rigidbody body)
    {
        if (CanMove())
        {
            float dx = NDInput.GetAxis(bindings.Horizontal);
            float dz = NDInput.GetAxis(bindings.Vertical);
            if (dx != 0 || dz != 0)
            {
                body.AddForce(player.AimDirection * settings.Acceleration, ForceMode.Acceleration);// * Time.deltaTime);
                body.velocity = Vector3.ClampMagnitude(body.velocity, settings.MaxSpeed);
            }
        }
    }

    private void UpdateRotation(Rigidbody body)
    {
        // body.angularVelocity = Vector3.zero;
        // if (!MathUtil.EqualsFloat(body.velocity.sqrMagnitude, 0, 0.1f))
        // {
            body.MoveRotation(Quaternion.LookRotation(player.AimDirection));
        // }
    }

    private bool CanMove()
    {
        return player.State.Value == 0;
    }
}