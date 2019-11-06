public class Logger {
    private static ILogger logger;

    public static void SetLogger(ILogger logger) {
        Logger.logger = logger;
    }

    public static void Log(object message) {
        logger?.Log(message);
    }

    public static void Logf(string format, params object[] args) {
        logger?.Logf(format, args);
    }

    public static void Warning(object message) {
        logger?.Warning(message);
    }

    public static void Warningf(string format, params object[] args) {
        logger?.Warningf(format, args);
    }

    public static void Error(object message) {
        logger?.Error(message);
    }

    public static void Errorf(string format, params object[] args) {
        logger?.Errorf(format, args);
    }
}