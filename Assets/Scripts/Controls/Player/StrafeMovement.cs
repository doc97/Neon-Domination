using UnityEngine;

public class StrafeMovement : MovementScheme
{
    public StrafeMovement(MovementSettings settings, InputBindings bindings) : base(settings, bindings) {}

    protected override void UpdateImpl(Transform t)
    {
        float dx = NDInput.GetAxis(bindings.Horizontal);
        float dz = NDInput.GetAxis(bindings.Vertical);
        
        Vector3 movement = new Vector3(dx, 0, dz).normalized * settings.Speed * Time.deltaTime;
        if (movement.sqrMagnitude > 0)
        {
            t.GetComponent<Rigidbody>().MoveRotation(Quaternion.LookRotation(movement));
        }

        t.GetComponent<Rigidbody>().MovePosition(t.position + movement);
    }
}