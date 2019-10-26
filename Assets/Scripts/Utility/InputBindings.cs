
public class InputBindings
{
    #region Fields
    public string Horizontal { get; private set; }
    public string Vertical { get; private set; }
    public string Hook { get; private set; }
    public string Dash { get; private set; }
    #endregion

    public InputBindings SetHorizontal(string horizontal)
    {
        Horizontal = horizontal;
        return this;
    }

    public InputBindings SetVertical(string vertical)
    {
        Vertical = vertical;
        return this;
    }

    public InputBindings SetHook(string hook)
    {
        Hook = hook;
        return this;
    }

    public InputBindings SetDash(string dash)
    {
        Dash = dash;
        return this;
    }
}