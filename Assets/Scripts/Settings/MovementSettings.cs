public class MovementSettings {
    #region Fields
    public float MaxSpeed { get; private set; }
    public float MaxSpeedSqrd { get => MaxSpeed * MaxSpeed; }
    public float Acceleration { get; private set; }
    #endregion

    public MovementSettings SetMaxSpeed(float maxSpeed) {
        MaxSpeed = maxSpeed;
        return this;
    }

    public MovementSettings SetAcceleration(float acceleration) {
        Acceleration = acceleration;
        return this;
    }
}