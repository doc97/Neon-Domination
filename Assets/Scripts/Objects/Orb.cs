using UnityEngine;

public class Orb : MonoBehaviour {

    private const float DROP_COOLDOWN_SEC = 0.0f;

    #region Fields
    private Vector3 spawnPosition;
    private bool active;
    #endregion

    public void Spawn(Vector3? position, float cooldown = DROP_COOLDOWN_SEC) {
        Deactivate();
        transform.position = position ?? spawnPosition;
        G.Instance.Pipeline.New().Delay(cooldown).Func(Activate);
    }

    private void Awake() {
        spawnPosition = transform.position;
    }

    private void Start() {
        Spawn(null, 0);
    }

    private void OnTriggerEnter(Collider col) {
        Player player = col.gameObject.GetComponentInParent<PlayerController>()?.Player;
        if (active && player != null) {
            player.PickupOrb(this);
            // To avoid multiple players picking up the orb, it is deactivated
            active = false;
            transform.position = new Vector3(0, 100, 0);
        }
    }

    private void Deactivate() {
        active = false;
        Material mat = GetComponent<Renderer>().material;
        mat.DisableKeyword("_EMISSION");
    }

    private void Activate() {
        active = true;
        Material mat = GetComponent<Renderer>().material;
        mat.EnableKeyword("_EMISSION");
    }
}