using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const int RESPAWN_Y_THRESHOLD = -80;

    #region Fields
    [SerializeField, Tooltip("Units per second")]
    private float speed;

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    #endregion

    private void Awake() {
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
    }

    private void Update()
    {
        UpdateMovement();
        CheckRestart();
    }

    private void UpdateMovement() {
        float dx = Input.GetAxis("Horizontal");
        float dz = Input.GetAxis("Vertical");
        
        Vector3 movement = new Vector3(dx, 0, dz).normalized * speed * Time.deltaTime;
        transform.position += movement;

        if (movement.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(movement);
        }
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
