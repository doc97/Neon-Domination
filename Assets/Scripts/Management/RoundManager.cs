using System;

public class RoundManager {

    #region Constants
    public const int POINTS_TO_SCORE = 100;
    #endregion

    #region Fields
    private float _points;
    public float Points {
        get => _points;
        private set {
            _points = Math.Min(Math.Max(value, -POINTS_TO_SCORE), POINTS_TO_SCORE);
            bar.Points = _points;
        }
    }
    private int _redScore;
    public int RedScore {
        get => _redScore;
        private set {
            _redScore++;
            bar.RedScore = _redScore;
        }
    }
    private int _blueScore;
    public int BlueScore {
        get => _blueScore;
        private set {
            _blueScore++;
            bar.BlueScore = _blueScore;
        }
    }

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
        Points++;

        if (Points == POINTS_TO_SCORE) {
            BlueScore++;
            Restart();
        }
    }

    private void AddRedPoint() {
        Points--;

        if (Points == -POINTS_TO_SCORE) {
            RedScore++;
            Restart();
        }
    }

    private void Restart() {
        Points = 0;
        // Respawn player's and orb
    }

    public void SetMatchBar(MatchBar bar) {
        this.bar = bar;
    }
}