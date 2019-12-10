using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
[RequireComponent(typeof(Animator))]
public class ScoreText : MonoBehaviour {

    #region Fields
    [SerializeField]
    private bool isBlue;

    private int score;
    #endregion

    private void Start() {
        score = isBlue ? G.Instance.Round.BlueScore : G.Instance.Round.RedScore;
        if (WasWinner()) {
            StartCoroutine(UpdateScore());
            GetComponent<TextMeshProUGUI>().text = "" + (score - 1);
        } else {
            GetComponent<TextMeshProUGUI>().text = "" + score;
        }
    }

    private bool WasWinner() {
        return (isBlue && G.Instance.Round.LastWinner == RoundManager.RoundWinner.Blue) ||
               (!isBlue && G.Instance.Round.LastWinner == RoundManager.RoundWinner.Red);
    }

    private IEnumerator UpdateScore() {
        yield return new WaitForSeconds(2);
        GetComponent<Animator>().enabled = true;
        GetComponent<TextMeshProUGUI>().text = "" + score;
    }
}