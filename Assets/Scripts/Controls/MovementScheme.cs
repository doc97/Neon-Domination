using UnityEngine;

public abstract class MovementScheme
{
    #region Fields
    protected MovementSettings settings;
    #endregion

    public MovementScheme(MovementSettings settings) {
        this.settings = settings;
    }

    public void Update(Transform t)
    {
        UpdateImpl(t);
    }

    protected abstract void UpdateImpl(Transform t);
}