public class MovementSettings
{
    #region Fields
    public float Speed { get; private set; }
    #endregion

    public static MovementSettings Defaults()
    {
        MovementSettings settings = new MovementSettings();
        settings.Speed = 1;
        return settings;
    }

    public MovementSettings SetSpeed(float speed)
    {
        this.Speed = speed;
        return this;
    }
}