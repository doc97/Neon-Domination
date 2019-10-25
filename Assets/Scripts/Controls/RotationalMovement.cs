using UnityEngine;

public class RotationalMovement : MovementScheme
{
    private const float TURN_SPEED = 90; // Degrees per second

    public RotationalMovement(MovementSettings settings) : base(settings) {}

    protected override void UpdateImpl(Transform t)
    {
        float dx = Input.GetAxis("Horizontal");
        float dz = Input.GetAxis("Vertical");

        Quaternion rot = Quaternion.Euler(0, dx * TURN_SPEED * Time.deltaTime, 0);
        t.rotation *= rot;

        Vector3 movement = t.forward * dz * settings.Speed * Time.deltaTime;
        t.position += movement;
    }
}