public class HookAbility : Ability
{
    private const float COOLDOWN = 10; // seconds

    public HookAbility() : base("Hook", COOLDOWN, "Hook") {}

    protected override void ActivateImpl()
    {
        Logger.Logf("Ability ({0}): activated", Name);
    }

    protected override bool CanActivateImpl()
    {
        return true;
    }
}