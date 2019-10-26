using UnityEngine;

public class Init : MonoBehaviour
{
    private void Awake()
    {
        Logger.SetLogger(new UnityLogger());
    }
}