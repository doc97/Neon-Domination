using System;

public class RoundManager {

    public enum RoundWinner { None, Blue, Red }

    #region Constants
    public const int POINTS_TO_SCORE = 100;
    #endregion

    #region Fields
    private float _points;
    public float Points {
        get => _points;
        private set {
            _points = Math.Min(Math.Max(value, -POINTS_TO_SCORE), POINTS_TO_SCORE);
            if (bar != null) {
                bar.Points = _points;
            }
        }
    }
    private int _redScore;
    public int RedScore {
        get => _redScore;
        private set {
            _redScore = Math.Max(value, 0);
            if (bar != null) {
                bar.RedScore = _redScore;
            }
        }
    }
    private int _blueScore;
    public int BlueScore {
        get => _blueScore;
        private set {
            _blueScore = Math.Max(value, 0);
            if (bar != null) {
                bar.BlueScore = _blueScore;
            }
        }
    }

    public int RoundNumber { get => BlueScore + RedScore; }
    public RoundWinner LastWinner { get; private set; } = RoundWinner.None;

    private MatchBar bar;
    #endregion

    public void AddPoint(bool isBlue) {
        if (isBlue) {
            AddBluePoint();
        } else {
            AddRedPoint();
        }
    }

    private void AddBluePoint() {
        LastWinner = RoundWinner.Blue;
        Points++;

        if (Points == POINTS_TO_SCORE) {
            BlueScore++;
            EndRound();
        }
    }

    private void AddRedPoint() {
        LastWinner = RoundWinner.Red;
        Points--;

        if (Points == -POINTS_TO_SCORE) {
            RedScore++;
            EndRound();
        }
    }

    private void EndRound() {
        Points = 0;
        
        Logger.Logf("Red: {0} | Blue: {1}", RedScore, BlueScore);
        if (RedScore == 2 || BlueScore == 2) {
            G.Instance.Scene.Load("MatchResult");
        } else {
            G.Instance.Scene.Load("RoundResult");
        }
    }

    public void Reset() {
        RedScore = 0;
        BlueScore = 0;
        LastWinner = RoundWinner.None;
    }
    
    public void SetMatchBar(MatchBar bar) {
        this.bar = bar;
    }
}