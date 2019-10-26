using UnityEngine;

public abstract class MovementScheme
{
    #region Fields
    protected MovementSettings settings;
    protected InputBindings bindings;
    #endregion

    public MovementScheme(MovementSettings settings, InputBindings bindings) {
        this.settings = settings;
        this.bindings = bindings;
    }

    public void Update(Transform t)
    {
        UpdateImpl(t);
    }

    protected abstract void UpdateImpl(Transform t);
}