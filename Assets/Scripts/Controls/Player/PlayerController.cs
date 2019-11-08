using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : MonoBehaviour {
    public enum MovementType {
        Strafe, Rotational
    }

    private const int RESPAWN_Y_THRESHOLD = -80;

    #region Fields
    #region Serialized
    [Header("Configuration")]
    [SerializeField]
    private GameObject settings;

    [Header("Controls")]
    [SerializeField]
    private string horizontalBinding;
    [SerializeField]
    private string verticalBinding;
    [SerializeField]
    private string hookBinding;
    [SerializeField]
    private string dashBinding;
    
    [Header("Other")]
    [SerializeField, Tooltip("Units per second")]
    private float maxSpeed;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private MovementType movementType = MovementType.Strafe;
    [SerializeField, Tooltip("World prefab storage")]
    private GameObject prefabs;
    #endregion

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    #region Movement
    private StrafeMovement strafeScheme;
    private RotationalMovement rotateScheme;
    private MovementScheme MovementScheme {
        get {
            switch (movementType) {
                case MovementType.Strafe:
                    return strafeScheme;
                case MovementType.Rotational:
                    return rotateScheme;
                default:
                    return null;
            }
        }
    }
    #endregion

    #region Settings
    private InputBindings bindings;
    private MovementSettings movementSettings;
    private GameplaySettings gameplaySettings;
    #endregion

    public Player Player { get; } = new Player();

    private bool isOnFloor;
    #endregion

    private void Awake() {
        Assert.IsNotNull(prefabs, "No prefab game object given!");
        movementSettings = new MovementSettings()
                        .SetMaxSpeed(maxSpeed)
                        .SetAcceleration(acceleration);
        bindings = new InputBindings()
                        .SetHorizontal(horizontalBinding)
                        .SetVertical(verticalBinding)
                        .SetHook(hookBinding)
                        .SetDash(dashBinding);
        strafeScheme = new StrafeMovement(Player, movementSettings, bindings);
        rotateScheme = new RotationalMovement(movementSettings, bindings);

        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
    }

    private void Start() {
        GameObject hookPrefab = prefabs.transform.Find("Hook")?.gameObject;
        Assert.IsNotNull(hookPrefab, "No Hook prefab exists!");

        gameplaySettings = settings.GetComponent<Settings>()?.Gameplay ?? new GameplaySettings();
        Player.Initialize(gameObject, hookPrefab, bindings, movementSettings, gameplaySettings);
    }

    private void Update() {
        CheckFalling();
        CheckRestart();
        Player.Update(Time.deltaTime);
    }

    private void FixedUpdate() {
        if (isOnFloor) {
            MovementScheme?.Update(transform);
        }

        float friction = 0.9f;
        Vector3 vel = GetComponent<Rigidbody>().velocity;
        vel.Set(vel.x * friction, vel.y, vel.z * friction);
        GetComponent<Rigidbody>().velocity = vel;
    }

    private void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == "Floor") {
            isOnFloor = true;
        }

        bool isPlayer = col.gameObject.GetComponent<PlayerController>() != null;
        if (Player.State.IsOn(Player.States.Dashing) && isPlayer) {
            Push(col.gameObject);
        }

        if (Player.State.IsOn(Player.States.Pushed)) {
            GetStunned();
        }
    }

    private void OnCollisionExit(Collision col) {
        if (col.gameObject.name == "Floor") {
            isOnFloor = false;
        }
    }

    private void CheckFalling() {
        if (isOnFloor)  { Player.State.Off(Player.States.Falling); }
        else            { Player.State.On(Player.States.Falling); }

        if (Player.HasOrb && Player.State == Player.States.Falling) {
            Player.DropOrb(null);
        }
    }

    private void CheckRestart() {
        if (transform.position.y < RESPAWN_Y_THRESHOLD) {
            Rigidbody body = GetComponent<Rigidbody>();
            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
            transform.rotation = spawnRotation;
            transform.position = spawnPosition;
        }
    }

    private void Push(GameObject target) {
        Rigidbody body = GetComponent<Rigidbody>();
        DashAbility dash = Player.GetAbility<DashAbility>();
        Rigidbody otherBody = target.GetComponent<Rigidbody>();
        Player otherPlayer = target.GetComponent<PlayerController>().Player;

        otherPlayer.State.On(Player.States.Pushed);
        G.Instance.Pipeline.New().
            Func(() => {
                Vector3 force = otherBody.velocity.normalized * 0.5f * dash.Force;
                otherBody.AddForce(force, ForceMode.Impulse);
            })
            .Delay(0.8f)
            .Func(() => {
                otherPlayer.State.Off(Player.States.Pushed);
            });

        body.velocity = Vector3.zero;
        Player.State.Off(Player.States.Dashing);

        if (otherPlayer.HasOrb) {
            otherPlayer.DropOrb(otherBody.transform.position);
        }
    }

    private void GetStunned() {
        Player.State.Off(Player.States.Pushed);
        Player.State.On(Player.States.Stunned);
        G.Instance.Pipeline.New().Delay(gameplaySettings.StunDuration).Func(() => Player.State.Off(Player.States.Stunned));
    }
}
