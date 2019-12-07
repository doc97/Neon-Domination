using UnityEngine;

public class NDMath {

    public static double SmoothStartN(double t, int n) {
        if (t == 0) {
            return 0;
        }

        double res = 1;
        for (int i = 0; i < n; i++) {
            res *= t;
        }
        return res;
    }

    public static double SmoothStopN(double t, int n) {
        if (t == 0) {
            return 0;
        }

        double res = 1;
        for (int i = 0; i < n; i++) {
            res *= (1 - t);
        }
        return 1 - res;
    }

    public static Vector3 SmoothStartVectorN(Vector3 a, Vector3 b, double t, int n) {
        return Vector3.Lerp(a, b, (float)SmoothStartN(t, n));
    }

    public static Vector3 SmoothStopVectorN(Vector3 a, Vector3 b, double t, int n) {
        return Vector3.Lerp(a, b, (float)SmoothStopN(t, n));
    }
}