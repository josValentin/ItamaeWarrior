using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elixir;

public class ElixirManager : MonoBehaviour
{
    public static ElixirManager Instance;
    [Header("Eixir Identifiers")]
    [SerializeField] private string apiKey;
    [SerializeField] private string gameId;

    private System.Action OnReadyCallback;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            Initialize_Elixir();
        }
    }

    private void Initialize_Elixir()
    {
        OnReadyCallback = () => { }; // Set empty

        Debug.Log("Api Key: " + apiKey);
        Debug.Log("Game ID: " + gameId);
        Debug.Log("Initializing Elixir...");
#if UNITY_ANDROID

        CloudOnceManager.Initialize(() =>
        {
            if (CloudOnceManager.IsSignedIn)
            {
                Debug.Log("userGoogleId : " + CloudOnceManager.PlayerId);
                ElixirController.StartAndroid("AG" + CloudOnceManager.PlayerId, OnElixirInitialized, OnError);
            }
            else
            {
                Debug.LogError("It was not possible to sign in to play services");
                OnError();
            }

        });

#else
        Elixir.ElixirController.StartRei(OnElixirInitialized, OnError);
#endif

    }

    void OnElixirInitialized()
    {
        Debug.Log("Elixir initialized");
        ElixirController.OnBalance += OnBalance;
        OnReadyCallback?.Invoke();

        OnReadyCallback = null;
    }

    void OnError()
    {
        Debug.LogError("Error trying to initialize Elixir");
        //

    }
    //---------------------------------------------------------------
    private void OnBalance(uint balance)
    {
        Debug.Log("Current balance: " + balance.ToString());
    }

    public static void AddBalance(int balance)
    {
        if (!Instance)
            return;

        if (ElixirController.isReady)
            ElixirController.BalanceAdd(balance);
        else
            Instance.OnReadyCallback += () => { AddBalance(balance); };
    }

    public static void SubstractBalance(int balance)
    {
        if (!Instance)
            return;

        if (ElixirController.isReady)
            ElixirController.BalanceSubtract(balance);
        else
            Instance.OnReadyCallback += () => { SubstractBalance(balance); };
    }

    public static void Save()
    {
        if (!Instance)
            return;

        if (ElixirController.isReady)
            ElixirController.Save();
        else
            Instance.OnReadyCallback += Save;
    }

    public static uint GetBalance()
    {
        if (!Instance)
            return 0;

        if (ElixirController.isReady)
            return ElixirController.balance;
        else
            return 0;
    }

    public static void ShowClaimRewards_Android()
    {
        if (!Instance)
            return;

        if (ElixirController.isReady)
            Elixir.ClaimRewards.Instance.ShowClaimRewards();
        else
            Instance.OnReadyCallback += ShowClaimRewards_Android;
    }

    public static void HideClaimRewards_Android()
    {
        if (!Instance)
            return;

        Elixir.ClaimRewards.Instance.HideClaimRewards();
    }

}
