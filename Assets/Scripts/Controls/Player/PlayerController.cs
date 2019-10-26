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
    private MovementSettings movementSettings;
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

    private Ability[] abilities;
    #endregion

    private void Awake() {
        Assert.IsNotNull(prefabs, "No prefab game object given!");
        movementSettings = new MovementSettings().SetSpeed(speed);
        strafeScheme = new StrafeMovement(movementSettings);
        rotateScheme = new RotationalMovement(movementSettings);
        abilities = new Ability[0];

        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
    }

    private void Start()
    {
        GameObject hookPrefab = prefabs.transform.Find("Hook")?.gameObject;
        Assert.IsNotNull(hookPrefab, "No Hook prefab exists!");

        abilities = new Ability[] {
            new HookAbility(gameObject, hookPrefab),
        };
    }

    private void Update()
    {
        MovementScheme?.Update(transform);
        CheckRestart();
        UpdateAbilties();
    }

    private void CheckRestart()
    {
        if (transform.position.y < RESPAWN_Y_THRESHOLD)
        {
            transform.rotation = spawnRotation;
            transform.position = spawnPosition;
        }
    }

    private void UpdateAbilties()
    {
        foreach (Ability ability in abilities)
        {
            ability.Update(Time.deltaTime);

            if (Input.GetButtonDown(ability.InputName))
            {
                if (ability.IsOnCooldown())
                {
                    Logger.Logf("Cooldown: {0}", ability.Timer);
                }
                ability.Activate();
            }
        }
    }
}
