using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : MonoBehaviour
{
    public enum MovementType
    {
        Strafe, Rotational
    }

    private const int RESPAWN_Y_THRESHOLD = -80;

    #region Fields
    #region Serialized
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
    private MovementSettings movementSettings;
    private StrafeMovement strafeScheme;
    private RotationalMovement rotateScheme;
    private MovementScheme MovementScheme {
        get
        {
            switch (movementType)
            {
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

    private InputBindings bindings;
    public Player Player { get; } = new Player();
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

    private void Start()
    {
        GameObject hookPrefab = prefabs.transform.Find("Hook")?.gameObject;
        Assert.IsNotNull(hookPrefab, "No Hook prefab exists!");

        Player.Initialize(gameObject, hookPrefab, bindings, movementSettings);
    }

    private void Update()
    {
        CheckFalling();
        CheckRestart();
        Player.Update(Time.deltaTime);
        MovementScheme?.Update(transform);
    }

    private void OnCollisionEnter(Collision col)
    {
        bool isPlayer = col.gameObject.GetComponent<PlayerController>() != null;
        if (Player.State.IsOn(Player.States.Dashing) && isPlayer)
        {
            Rigidbody body = GetComponent<Rigidbody>();
            DashAbility dash = Player.GetAbility<DashAbility>();
            Rigidbody otherBody = col.gameObject.GetComponent<Rigidbody>();
            Player otherPlayer = col.gameObject.GetComponent<PlayerController>().Player;

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
        }
    }

    private void CheckFalling()
    {
        Rigidbody body = GetComponent<Rigidbody>();
        float horizontalSpeedSqrd = body.velocity.x * body.velocity.x + body.velocity.z * body.velocity.z;
        float verticalSpeedSqrd = body.velocity.y * body.velocity.y;
        bool isFalling = body.velocity.y < 0;
        if (isFalling && verticalSpeedSqrd > horizontalSpeedSqrd)
        {
            Player.State.On(Player.States.Falling);
        }
        else
        {
            Player.State.Off(Player.States.Falling);
        }
    }

    private void CheckRestart()
    {
        if (transform.position.y < RESPAWN_Y_THRESHOLD)
        {
            Rigidbody body = GetComponent<Rigidbody>();
            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
            transform.rotation = spawnRotation;
            transform.position = spawnPosition;
        }
    }
}
