using UnityEngine;

public class Player
{
    public enum States
    {
        Dashing, Falling, Hooked, Stunned
    }

    #region Fields
    public EnumBitField<States> State { get; } = new EnumBitField<States>();
    private Ability[] abilities;
    #endregion

    public void InitializeAbilities(GameObject player, GameObject hookPrefab, InputBindings bindings, MovementSettings settings)
    {
        abilities = new Ability[] {
            new HookAbility(this, player, hookPrefab, bindings.Hook),
            new DashAbility(this, player, settings, bindings.Dash)
        };
    }

    public void Update(float deltaTime)
    {
        UpdateAbilities(deltaTime);
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