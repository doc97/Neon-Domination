using UnityEngine;
using TMPro;

public class MatchBar : MonoBehaviour {

    #region Constants
    private const float MAX_SCORE = 100;
    private const int SIDE_WIDTH_PX = 270;
    #endregion

    #region Fields
    private float _score;
    public float Score {
        get => _score;
        set {
            _score = Mathf.Max(Mathf.Min(value, MAX_SCORE), -MAX_SCORE);
            float indicatorX = _score / MAX_SCORE * SIDE_WIDTH_PX;
            indicator.localPosition = new Vector3(indicatorX, indicator.localPosition.y, indicator.localPosition.z);
            indicatorAnim.SetInteger("score", (int)_score);
        }
    }

    private int _redScore;
    public int RedScore {
        get => _redScore;
        set {
            _redScore = value;
            blueScoreText.text = "" + _redScore;
        }
    }

    private int _blueScore;
    public int BlueScore {
        get => _blueScore;
        set {
            _blueScore = value;
            blueScoreText.text = "" + _blueScore;
        }
    }

    private RectTransform indicator;
    private Animator indicatorAnim;
    private TextMeshProUGUI redScoreText;
    private TextMeshProUGUI blueScoreText;
    #endregion

    private void Awake() {
        indicator = transform.Find("Indicator").GetComponent<RectTransform>();
        indicatorAnim = indicator.GetComponent<Animator>();
        redScoreText = transform.Find("RedScore").GetComponent<TextMeshProUGUI>();
        blueScoreText = transform.Find("BlueScore").GetComponent<TextMeshProUGUI>();
    }

    private void Start() {
        Score = 0;
        redScoreText.text = "0";
        blueScoreText.text = "0";
    }
}
