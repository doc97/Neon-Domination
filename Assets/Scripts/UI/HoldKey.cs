using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HoldKey : MonoBehaviour {

    #region Fields
    [SerializeField, Tooltip("Key to hold")]
    private KeyCode key;
    [SerializeField, Tooltip("Duration to hold down key, in seconds")]
    private float duration;
    [SerializeField, Tooltip("Function to call when key has been held down for the duration")]
    private BehaviourCallback callback;
    [SerializeField, Tooltip("Slider text")]
    private TextMeshProUGUI text;
    [SerializeField, Tooltip("Text to display once done")]
    private string doneText;

    private Slider slider;
    private bool done;
    #endregion

    private void Awake() {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener((v) => {
            if (v == 1) {
                if (text != null) {
                    text.text = doneText;
                }
                done = true;
                callback?.Call();
            }
        });
        text.text = "[Hold " + key + "]";
    }

    private void Update() {
        if (done) {
            return;
        }

        if (Input.GetKey(key)) {
            slider.value += Time.deltaTime / duration;
        } else {
            slider.value = 0;
        }
    }
}