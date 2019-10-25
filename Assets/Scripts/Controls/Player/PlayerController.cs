using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum MovementType
    {
        Strafe, Rotational
    }

    private const int RESPAWN_Y_THRESHOLD = -80;

    #region Fields
    [SerializeField, Tooltip("Units per second")]
    private float speed;
    [SerializeField]
    private MovementType movementType = MovementType.Strafe;

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

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

    private void Awake() {
        movementSettings = new MovementSettings().SetSpeed(speed);
        strafeScheme = new StrafeMovement(movementSettings);
        rotateScheme = new RotationalMovement(movementSettings);
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
    }

    private void Update()
    {
        MovementScheme?.Update(transform);
        CheckRestart();
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
