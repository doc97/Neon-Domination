using UnityEngine;
using UnityEngine.Assertions;

public class CameraController : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float damping = 5;

    private Vector3 offset;
    #endregion

    private void Awake() {
        Assert.IsNotNull(target, "target must not be null");
        offset = target.transform.position - transform.position;
    }

    private void LateUpdate()
    {
        Vector3 newPosition = target.transform.position - offset;
        newPosition.y = transform.position.y;

        transform.position = newPosition;
        transform.LookAt(target.transform);
    }
}
