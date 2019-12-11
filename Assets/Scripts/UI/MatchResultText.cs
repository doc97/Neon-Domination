using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class MatchResultText : MonoBehaviour {

    #region Fields
    [SerializeField]
    private Material redMaterial;
    [SerializeField]
    private Material blueMaterial;
    #endregion

    private void Awake() {
        bool blueWon = G.Instance.Round.LastWinner == RoundManager.RoundWinner.Blue;
        string team = blueWon ? "Blue" : "Red";
        GetComponent<TextMeshProUGUI>().text = team + " team wins!";
        GetComponent<TextMeshProUGUI>().fontMaterial = blueWon ? blueMaterial : redMaterial;
    }
}