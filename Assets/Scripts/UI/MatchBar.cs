using UnityEngine;
using TMPro;

public class MatchBar : MonoBehaviour {

    #region Constants
    private const int SIDE_WIDTH_PX = 520;
    #endregion

    #region Fields
    private float _points;
    public float Points {
        get => _points;
        set {
            _points = value;
            float percentage = Mathf.Abs(_points / RoundManager.POINTS_TO_SCORE);
            float xPosition = (float) NDMath.SmoothStopN(percentage, 2) * Mathf.Sign(_points);
            float indicatorX = xPosition * SIDE_WIDTH_PX;
            indicator.localPosition = new Vector3(indicatorX, indicator.localPosition.y, indicator.localPosition.z);
            indicatorAnim.SetInteger("score", (int)_points);
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
        G.Instance.Round.SetMatchBar(this);
    }

    private void Start() {
        Points = 0;
        redScoreText.text = "" + G.Instance.Round.RedScore;
        blueScoreText.text = "" + G.Instance.Round.BlueScore;
    }
}
