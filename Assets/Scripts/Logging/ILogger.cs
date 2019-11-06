public interface ILogger {
    void Log(object message);
    void Logf(string format, params object[] args);

    void Warning(object message);
    void Warningf(string format, params object[] args);

    void Error(object message);
    void Errorf(string format, params object[] args);
}