using UnityEngine;

public class Hook : MonoBehaviour {
    [SerializeField]
    private float speed = 0.2f;

    private Rigidbody body;
    private Transform origin;
    private bool reverse;
    private Player hooking;
    private Player hooked;
    private Pipeline cancel;

    private void Awake() {
        body = GetComponent<Rigidbody>();
    }

    private void Update() {
        if (reverse) {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, origin.position, speed);
            body.MovePosition(newPosition);
        } else {
            body.MovePosition(transform.position + transform.forward * speed);
        }

        if (hooking.State.IsOff(Player.States.Hooking)) {
            Destroy();
        }
    }

    private void OnCollisionEnter(Collision col) {
        if (IsTarget(col.gameObject)) {
            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = col.rigidbody;
            hooked = col.gameObject.GetComponent<PlayerController>().Player;
            hooked.State.On(Player.States.Hooked);
            cancel.Abort();
            reverse = true;
        } else {
            Destroy();
        }
    }

    private void Destroy() {
        hooked?.State.Off(Player.States.Hooked);
        hooking.State.Off(Player.States.Hooking);
        Destroy(gameObject);
    }

    private bool IsTarget(GameObject gameObject) {
        return gameObject != origin.gameObject &&gameObject.GetComponent<PlayerController>() != null;
    }

    public void SetOrigin(Transform origin) {
        this.origin = origin;
        this.hooking = origin.GetComponent<PlayerController>().Player;
    }

    public void SetCancelPipe(Pipeline cancel) {
        this.cancel = cancel;
    }
}