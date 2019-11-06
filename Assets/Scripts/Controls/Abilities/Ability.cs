using System;

public abstract class Ability {
    #region Fields
    public string Name { get; }
    public float Cooldown { get; protected set; }
    public float Timer { get; private set; }
    public string InputName { get; }
    #endregion

    public Ability(string name, float cooldown, string inputName) {
        Name = name;
        Cooldown = cooldown;
        InputName = inputName;
    }

    public virtual void Update(float deltaTime) {
        Timer = Math.Max(0, Timer - deltaTime);
    }

    public void Activate() {
        if (CanActivate()) {
            ActivateImpl();
            ResetCooldown();
        }
    }

    public void ResetCooldown() {
        Timer = Cooldown;
    }

    public bool IsOnCooldown() {
        return Timer > 0;
    }

    public bool CanActivate() {
        return !IsOnCooldown() && CanActivateImpl();
    }

    protected abstract bool CanActivateImpl();
    protected abstract void ActivateImpl();
}