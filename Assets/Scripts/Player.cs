using UnityEngine;

public class Player
{
    public enum States
    {
        Dashing, Pushed,
        Hooking, Hooked,
        Falling,
        Stunned,
    }

    #region Fields
    public EnumBitField<States> State { get; } = new EnumBitField<States>();
    public Vector3 AimDirection { get; private set; } = new Vector3(0, 0, 1);
    private Ability[] abilities;
    private InputBindings bindings;
    #endregion

    public void Initialize(GameObject player, GameObject hookPrefab, InputBindings bindings, MovementSettings settings)
    {
        this.bindings = bindings;
        abilities = new Ability[] {
            new HookAbility(this, player, hookPrefab, bindings.Hook),
            new DashAbility(this, player, settings, bindings.Dash)
        };
    }

    public void Update(float deltaTime)
    {
        UpdateAim();
        UpdateAbilities(deltaTime);
    }

    private void UpdateAim()
    {
        float dx = NDInput.GetAxis(bindings.Horizontal);
        float dz = NDInput.GetAxis(bindings.Vertical);
        Vector3 dir = new Vector3(dx, 0, dz).normalized;

        if (dir.sqrMagnitude > 0)
        {
            AimDirection = dir;
        }
    }

    private void UpdateAbilities(float deltaTime)
    {
        foreach (Ability ability in abilities)
        {
            ability.Update(deltaTime);
            if (NDInput.GetButtonDown(ability.InputName))
            {
                if (ability.IsOnCooldown())
                {
                    Logger.Logf("Cooldown: {0}", ability.Timer);
                }
                ability.Activate();
            }
        }
    }

    public T GetAbility<T>() where T : Ability
    {
        foreach (Ability ability in abilities)
        {
            if (ability is T)
            {
                return ability as T;
            }
        }
        return null;
    }
}