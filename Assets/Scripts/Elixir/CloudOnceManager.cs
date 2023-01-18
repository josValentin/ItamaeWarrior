using UnityEngine;
using CloudOnce;
using System;
[AddComponentMenu("CloudOnce/CloudOnce Initializer", 0)]
public class CloudOnceManager : MonoBehaviour
{
    public static CloudOnceManager Instance;

    public static string PlayerId { get => Cloud.PlayerID; } // (Google play / App Store) ID
    public static bool IsSignedIn { get => Cloud.IsSignedIn; }
    private Action OnInitialize = null;

    void Awake()
    {
#if UNITY_STANDALONE

        Destroy(gameObject);
        //return;
#endif
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        Cloud.OnSignedInChanged += OnSignInChanged;
        Cloud.OnSignInFailed += OnSignInFailed;
        Cloud.OnInitializeComplete += () =>
        {
            OnInitialize?.Invoke();
            OnInitialize = null;
        };
    }

    public static void Initialize(Action OnInitialize)
    {
        Cloud.Initialize(false, true, false);
        Instance.OnInitialize = OnInitialize;
    }


    public void OnSignInChanged(bool signed)
    {
        if (signed)
        {
            // On sign in success
            Debug.Log("CLOUD ONCE SIGNED IN - Player Id : " + Cloud.PlayerID);
        }
        else
        {
            // Do something if the user has signed out
        }
    }

    public void OnSignInFailed()
    {
        // Do something if the sign in failed
    }
}
