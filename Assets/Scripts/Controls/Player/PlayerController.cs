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
    private float speed;
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
        movementSettings = new MovementSettings().SetSpeed(speed);
        bindings = new InputBindings()
                        .SetHorizontal(horizontalBinding)
                        .SetVertical(verticalBinding)
                        .SetHook(hookBinding)
                        .SetDash(dashBinding);
        strafeScheme = new StrafeMovement(movementSettings, bindings);
        rotateScheme = new RotationalMovement(movementSettings, bindings);

        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
    }

    private void Start()
    {
        GameObject hookPrefab = prefabs.transform.Find("Hook")?.gameObject;
        Assert.IsNotNull(hookPrefab, "No Hook prefab exists!");

        Player.InitializeAbilities(gameObject, hookPrefab, bindings, movementSettings);
    }

    private void Update()
    {
        CheckFalling();
        CheckRestart();
        UpdateMovement();
        Player.Update(Time.deltaTime);
    }

    private void OnCollisionEnter(Collision col)
    {
        bool isPlayer = col.gameObject.GetComponent<PlayerController>() != null;
        if (Player.State.IsOn(Player.States.Dashing) && isPlayer)
        {
            Logger.Logf("Dash collision!");
            Rigidbody body = GetComponent<Rigidbody>();
            DashAbility dash = Player.GetAbility<DashAbility>();
            col.gameObject.GetComponent<Rigidbody>().AddForce(body.velocity.normalized * dash.Force / 2, ForceMode.Impulse);
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

    private void UpdateMovement()
    {
        if (Player.State.Value == 0)
        {
            MovementScheme?.Update(transform);
        }
    }
}
