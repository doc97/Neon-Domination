using UnityEngine;

public class UnityLogger : ILogger {
    public void Log(object message) {
        Debug.Log(message);
    }

    public void Logf(string format, params object[] args) {
        Debug.LogFormat(format, args);
    }

    public void Warning(object message) {
        Debug.LogWarning(message);
    }

    public void Warningf(string format, params object[] args) {
        Debug.LogWarningFormat(format, args);
    }

    public void Error(object message) {
        Debug.LogError(message);
    }

    public void Errorf(string format, params object[] args) {
        Debug.LogErrorFormat(format, args);
    }
}