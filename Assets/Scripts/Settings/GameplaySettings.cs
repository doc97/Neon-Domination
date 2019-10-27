public class GameplaySettings
{
    #region Fields
    public float DashCooldown { get; private set; } = 3;
    public float DashForce { get; private set; } = 30;
    public float HookCooldown { get; private set; } = 3;
    public float StunDuration { get; private set; } = 2;
    #endregion

    public GameplaySettings SetDashCooldown(float dashCooldown)
    {
        DashCooldown = dashCooldown;
        return this;
    }

    public GameplaySettings SetDashForce(float dashForce)
    {
        DashForce = dashForce;
        return this;
    }

    public GameplaySettings SetHookCooldown(float hookCooldown)
    {
        HookCooldown = hookCooldown;
        return this;
    }

    public GameplaySettings SetStunDuration(float stunDuration)
    {
        StunDuration = stunDuration;
        return this;
    }
}