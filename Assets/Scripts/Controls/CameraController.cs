using UnityEngine;
using UnityEngine.Assertions;

public class CameraController : MonoBehaviour {

    #region Fields
    [SerializeField]
    private Transform players;
    [SerializeField, Tooltip("Camera center offset in world space")]
    private Vector3 centerOffset;
    [SerializeField, Range(0, 50)]
    private float zoom = 15;
    [SerializeField, Range(1, 50), Tooltip("Larger value => snappier movement")]
    private float lerpSpeed = 1;

    private Vector3 targetPos;
    private float startY;
    #endregion

    private void Awake() {
        Assert.IsNotNull(players, "target must not be null");
        startY = transform.position.y;
    }

    private void FixedUpdate() {
        Recalculate();
        transform.position = NDMath.SmoothStartVectorN(transform.position, targetPos, Time.fixedDeltaTime * lerpSpeed, 3);
    }

    private void Recalculate() {
        if ((players?.childCount ?? 0) == 0) {
            targetPos = transform.position;
            return;
        }

        targetPos = GetPlayersCenterPosition() + centerOffset;
        float maxDistance = GetMaxDistanceFromPlayersToPoint(targetPos, players);

        float deltaY = maxDistance - zoom;
        targetPos.y = startY + deltaY;
    }

    private Vector3 GetPlayersCenterPosition() {
        Vector3 center = new Vector3();
        foreach (Transform player in players) {
            center += player.transform.position;
        }
        center /= players.childCount;
        return center;
    }

    private float GetMaxDistanceFromPlayersToPoint(Vector3 point, Transform players) {
        float maxDistance = 0;
        foreach (Transform player in players) {
            Vector3 distance = point - player.position;
            distance.y = 0;
            if (distance.magnitude > maxDistance) {
                maxDistance = distance.magnitude;
            }
        }
        return maxDistance;
    }

    private Vector3 SmoothStop(Vector3 start, Vector3 end, float t) {
        float y = Mathf.Lerp(start.y, end.y, t);
        return new Vector3(end.x, y, end.z);
    }
}
