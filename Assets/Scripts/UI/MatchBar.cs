using System;
using System.Collections;
using UnityEngine;

public class MatchBar : MonoBehaviour {

    #region Constants
    private const float MAX_SCORE = 100;
    private const int SIDE_WIDTH_PX = 430;
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

    private RectTransform indicator;
    private Animator indicatorAnim;
    #endregion

    private void Awake() {
        indicator = transform.Find("Indicator").GetComponent<RectTransform>();
        indicatorAnim = indicator.GetComponent<Animator>();
    }

    private void Start() {
        Score = 0;
        //StartCoroutine(UpdateScore());
    }

    private IEnumerator UpdateScore() {
        System.Random rand = new System.Random();
        while(true) {
            bool add = rand.Next(100) < 50;
            int count = rand.Next(10);
            int counter = 0;
            while (counter++ < count) {
                Score += add ? 1 : -1;
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}
