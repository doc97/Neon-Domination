using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TimeLeft : MonoBehaviour {

    #region Fields
    [SerializeField, Tooltip("Seconds to count down from")]
    private float timer;
    [SerializeField, Tooltip("The line above the number")]
    private string startText;
    private TextMeshProUGUI text;
    #endregion

    private void Awake() {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Start() {
        StartCoroutine(UpdateText());
    }

    private IEnumerator UpdateText() {
        while (timer > 0) {
            text.text = startText + "\n" + Mathf.CeilToInt(timer);
            timer = Mathf.Max(timer - Time.deltaTime, 0);
            yield return null;
        }
        text.text = startText + "\n0";
    }
}