using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class RoundText : MonoBehaviour {

    private void Start() {
        GetComponent<TextMeshProUGUI>().text = "Round " + G.Instance.Round.RoundNumber + " Complete";
    }
}
