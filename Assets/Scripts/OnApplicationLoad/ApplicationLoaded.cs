using UnityEngine;

public static class ApplicationLoaded
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void OnApplicationLoad()
    {
        Application.targetFrameRate = 60;

        GameSettings.InitializeSettings();

        Debug.Log("On app Loaded settings ");
    }
}
