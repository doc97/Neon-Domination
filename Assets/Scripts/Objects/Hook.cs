using UnityEngine;

public class Hook : MonoBehaviour {

    #region Field
    [SerializeField]
    private float speed = 1;

    private Rigidbody body;
    private Transform origin;
    private Transform rope;
    private bool reverse;
    private Player hooking;
    private Player hooked;
    private Pipeline cancel;
    #endregion

    private void Awake() {
        body = GetComponent<Rigidbody>();
        rope = transform.Find("Rope");
    }

    private void Update() {
        UpdateHookPosition();
        UpdateRopeLength();

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

    private void UpdateHookPosition() {
        if (reverse) {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, origin.position, speed);
            body.MovePosition(newPosition);
        } else {
            body.MovePosition(transform.position + transform.forward * speed);
        }
    }

    private void UpdateRopeLength() {
        float distance = Vector3.Distance(transform.position, origin.position);
        Vector3 s = rope.transform.localScale;
        rope.transform.localScale = new Vector3(s.x, s.y, distance);
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