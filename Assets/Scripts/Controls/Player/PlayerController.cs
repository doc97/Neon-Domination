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
        MovementSettings settings = new MovementSettings().SetSpeed(speed);
        bindings = new InputBindings()
                        .SetHorizontal(horizontalBinding)
                        .SetVertical(verticalBinding)
                        .SetHook(hookBinding);
        strafeScheme = new StrafeMovement(settings, bindings);
        rotateScheme = new RotationalMovement(settings, bindings);

        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
    }

    private void Start()
    {
        GameObject hookPrefab = prefabs.transform.Find("Hook")?.gameObject;
        Assert.IsNotNull(hookPrefab, "No Hook prefab exists!");

        Player.InitializeAbilities(gameObject, hookPrefab, bindings);
    }

    private void Update()
    {
        if (Player.State.Value == 0)
        {
            MovementScheme?.Update(transform);
        }
        CheckRestart();
        Player.Update(Time.deltaTime);
    }

    private void CheckRestart()
    {
        if (transform.position.y < RESPAWN_Y_THRESHOLD)
        {
            transform.rotation = spawnRotation;
            transform.position = spawnPosition;
        }
    }
}
