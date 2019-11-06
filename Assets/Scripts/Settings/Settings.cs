using UnityEngine;

public class Settings : MonoBehaviour {
    #region Fields
    [SerializeField, Tooltip("In seconds")]
    private float dashCooldown;
    [SerializeField]
    private float dashForce;
    [SerializeField, Tooltip("In seconds")]
    private float hookCooldown;
    [SerializeField, Tooltip("In seconds")]
    private float stunDuration;

    public GameplaySettings Gameplay { get; private set; }
    #endregion

    private void Awake() {
        Gameplay = new GameplaySettings()
                    .SetDashCooldown(dashCooldown)
                    .SetDashForce(dashForce)
                    .SetHookCooldown(hookCooldown)
                    .SetStunDuration(stunDuration);
    }
}