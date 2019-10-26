using UnityEngine;

public class RotationalMovement : MovementScheme
{
    private const float TURN_SPEED = 90; // Degrees per second

    public RotationalMovement(MovementSettings settings, InputBindings bindings) : base(settings, bindings) {}

    protected override void UpdateImpl(Transform t)
    {
        float dx = Input.GetAxis(bindings.Horizontal);
        float dz = Input.GetAxis(bindings.Vertical);

        Quaternion rot = Quaternion.Euler(0, dx * TURN_SPEED * Time.deltaTime, 0);
        t.GetComponent<Rigidbody>().MoveRotation(t.rotation * rot);

        Vector3 movement = t.forward * dz * settings.Speed * Time.deltaTime;
        t.GetComponent<Rigidbody>().MovePosition(t.position + movement);
    }
}