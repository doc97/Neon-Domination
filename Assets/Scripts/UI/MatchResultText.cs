using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class MatchResultText : MonoBehaviour {

    private void Awake() {
        string team = G.Instance.Round.LastWinner == RoundManager.RoundWinner.Blue ? "Blue" : "Red";
        GetComponent<TextMeshProUGUI>().text = team + " team wins!";
    }
}