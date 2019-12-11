using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour {

    private Color cooldownColor = new Color(0.25f, 0.25f, 0.25f, 1);

    #region Fields
    [SerializeField]
    private PlayerController player;

    private Transform hook;
    private Transform dash;

    private HookAbility hookAbility;
    private DashAbility dashAbility;
    #endregion

    private void Awake() {
        hook = transform.Find("HookAbility");
        dash = transform.Find("DashAbility");
    }

    private void Update() {
        if (hookAbility == null || dashAbility == null) {
            Initialize();
        } else {
            UpdateAbility(dashAbility, dash);
            UpdateAbility(hookAbility, hook);
        }
    }

    private void Initialize() {
        if (player.Player.IsInitialized) {
            hookAbility = player.Player.GetAbility<HookAbility>();
            dashAbility = player.Player.GetAbility<DashAbility>();
        }
    }

    private void UpdateAbility(Ability ability, Transform icon) {
        if (ability.IsOnCooldown()) {
            icon.GetComponent<Image>().color = cooldownColor;
            icon.Find("Cooldown").gameObject.SetActive(true);
            icon.Find("Cooldown").GetComponent<TextMeshProUGUI>().text = ability.Timer.ToString("0.0");
        } else {
            icon.GetComponent<Image>().color = Color.white;
            icon.Find("Cooldown").gameObject.SetActive(false);
        }
    }
}