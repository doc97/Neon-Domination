using UnityEngine;

public class Orb : MonoBehaviour {
    private Player player;

    private void OnTriggerEnter(Collider col) {
        PlayerController controller = col.gameObject.GetComponent<PlayerController>();
        if (controller != null) {
            player = controller.Player;
        }
        Logger.Logf("Orb picked up!");
    }
}