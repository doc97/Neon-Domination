using UnityEngine;

public class Player
{
    public enum States
    {
        Hooked, Stunned
    }

    #region Fields
    public EnumBitField<States> State { get; } = new EnumBitField<States>();
    private Ability[] abilities;
    #endregion

    public void InitializeAbilities(GameObject player, GameObject hookPrefab)
    {
        abilities = new Ability[] {
            new HookAbility(player, hookPrefab),
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

            if (Input.GetButtonDown(ability.InputName))
            {
                if (ability.IsOnCooldown())
                {
                    Logger.Logf("Cooldown: {0}", ability.Timer);
                }
                ability.Activate();
            }
        }
    }
}