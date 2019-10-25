using UnityEngine;

public class StrafeMovement : MovementScheme
{
    public StrafeMovement(MovementSettings settings) : base(settings) {}

    protected override void UpdateImpl(Transform t)
    {
        float dx = Input.GetAxis("Horizontal");
        float dz = Input.GetAxis("Vertical");
        
        Vector3 movement = new Vector3(dx, 0, dz).normalized * settings.Speed * Time.deltaTime;
        t.position += movement;

        if (movement.sqrMagnitude > 0)
        {
            t.rotation = Quaternion.LookRotation(movement);
        }
    }
}