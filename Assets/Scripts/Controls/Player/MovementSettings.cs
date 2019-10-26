public class MovementSettings
{
    #region Fields
    public float MaxSpeed { get; private set; }
    public float MaxSpeedSqrd { get => MaxSpeed * MaxSpeed; }
    public float Acceleration { get; private set; }
    #endregion

    public static MovementSettings Defaults()
    {
        MovementSettings settings = new MovementSettings();
        settings.MaxSpeed = 1;
        return settings;
    }

    public MovementSettings SetMaxSpeed(float maxSpeed)
    {
        MaxSpeed = maxSpeed;
        return this;
    }

    public MovementSettings SetAcceleration(float acceleration)
    {
        Acceleration = acceleration;
        return this;
    }
}