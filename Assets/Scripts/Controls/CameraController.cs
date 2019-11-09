using UnityEngine;
using UnityEngine.Assertions;

public class CameraController : MonoBehaviour {

    #region Fields
    [SerializeField]
    private Transform players;
    [SerializeField, Range(10, 20)]
    private float zoom = 15;

    private Vector3 targetPos;
    private float startY;
    #endregion

    private void Awake() {
        Assert.IsNotNull(players, "target must not be null");
        startY = transform.position.y;
    }

    private void FixedUpdate() {
        Recalculate();
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.fixedDeltaTime);
    }

    private void Recalculate() {
        if ((players?.childCount ?? 0) == 0) {
            targetPos = transform.position;
            return;
        }

        targetPos = new Vector3();
        foreach (Transform player in players) {
            targetPos += player.transform.position;
        }
        targetPos /= players.childCount;

        float maxDistance = 0;
        foreach (Transform player in players) {
            Vector3 distance = targetPos - player.position;
            distance.y = 0;
            if (distance.magnitude > maxDistance) {
                maxDistance = distance.magnitude;
            }
        }

        float deltaY = maxDistance - zoom;
        targetPos.y = startY + deltaY;
    }
}
