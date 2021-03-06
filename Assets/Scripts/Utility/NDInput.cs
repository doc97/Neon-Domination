using System;
using UnityEngine;

public class NDInput {
    public static float GetAxis(string inputName, float def = 0f) {
        try { return Input.GetAxis(inputName); }
        catch (ArgumentException) { return def; }
    }

    public static float GetAxisRaw(string inputName, float def = 0f) {
        try { return Input.GetAxisRaw(inputName); }
        catch (ArgumentException) { return def; }
    }

    public static bool GetButtonDown(string inputName) {
        try { return Input.GetButtonDown(inputName); }
        catch (ArgumentException) { return false; }
    }

    public static bool GetButtonUp(string inputName) {
        try { return Input.GetButtonUp(inputName); }
        catch (ArgumentException) { return false; }
    }

    public static bool IsButtonDown(string inputName) {
        try { return Input.GetButton(inputName); }
        catch (ArgumentException) { return false; }
    }
}