using System.Collections;
using UnityEngine;

public class Shake : MonoBehaviour {

    #region Fields
    [SerializeField, Tooltip("Target transform to shake")]
    private Transform target;
    [SerializeField, Tooltip("Shake amount")]
    private float amount = 1;
    [SerializeField, Tooltip("Shake interval in seconds")]
    private float interval = 0.1f;
    private Vector3 original;
    #endregion

    private void Awake() {
        original = target.localPosition;
    }

    private void Start() {
        StartCoroutine(UpdateShake());
    }

    private IEnumerator UpdateShake() {
        while (true) {
            target.localPosition = original + Random.insideUnitSphere * amount;
            yield return new WaitForSeconds(interval);
        }
    }
}