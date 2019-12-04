using UnityEngine;

public class Player {
    public enum States {
        Dashing, Pushed,
        Hooking, Hooked,
        Falling,
        Stunned,
    }

    #region Fields
    public EnumBitField<States> State { get; } = new EnumBitField<States>();
    public Vector3 AimDirection { get; private set; } = new Vector3(0, 0, 1);
    public bool HasOrb { get => orb != null; }

    private Ability[] abilities;
    private InputBindings bindings;

    private PlayerController controller;
    private GameObject effect;
    private Orb orb;
    #endregion

    public void Initialize(GameObject player, GameObject hookPrefab, InputBindings bindings, MovementSettings movementSettings, GameplaySettings gameplaySettings) {
        this.bindings = bindings;
        abilities = new Ability[] {
            new HookAbility(this, player, hookPrefab, gameplaySettings.HookCooldown, bindings.Hook),
            new DashAbility(this, player, movementSettings, gameplaySettings.DashForce, gameplaySettings.DashCooldown, bindings.Dash)
        };
        effect = player.transform.Find("Effect").gameObject;
        effect.SetActive(false);
        controller = player.GetComponent<PlayerController>();
    }

    public void Update(float deltaTime) {
        UpdateAim();
        UpdateAbilities(deltaTime);
    }

    private void UpdateAim() {
        float dx = NDInput.GetAxisRaw(bindings.Horizontal);
        float dz = NDInput.GetAxisRaw(bindings.Vertical);
        Vector3 dir = new Vector3(dx, 0, dz).normalized;

        if (dir.sqrMagnitude > 0) {
            AimDirection = dir;
        }
    }

    private void UpdateAbilities(float deltaTime) {
        foreach (Ability ability in abilities) {
            ability.Update(deltaTime);
            if (NDInput.GetButtonDown(ability.InputName)) {
                if (ability.IsOnCooldown()) {
                    Logger.Logf("Cooldown: {0}", ability.Timer);
                }
                ability.Activate();
            }
        }
    }

    public void PickupOrb(Orb orb) {
        effect.SetActive(true);
        this.orb = orb;
        controller.OnOrbPickup();
    }

    public void DropOrb(Vector3? position) {
        effect.SetActive(false);
        orb?.Spawn(position);
        orb = null;
        controller.OnOrbDrop();
    }

    public T GetAbility<T>() where T : Ability {
        foreach (Ability ability in abilities) {
            if (ability is T) {
                return ability as T;
            }
        }
        return null;
    }
}