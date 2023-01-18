using System.Collections;
using UnityEngine;
using Elixir;

public class ElixirGameController : MonoBehaviour
{
    private static ElixirGameController _Instance;

    [SerializeField]
    private bool enableElixir = true;

    [Header("Identifiers")]

    [SerializeField] string apiKey = "be17c3ec-7739-4d1a-8470-aaef746f21fb"; // "afae1b30-65b4-49c5-a8a4-d9c98ea4cadb" // Sandbox api key?
    [SerializeField] string gameId = "b9721f6d-9473-4dea-8589-2493e57df03c";

    public static bool _EnableElixir;

    private System.Action OnReadyCallback;

    // Start is called before the first frame update
    void Awake()
    {
        _EnableElixir = enableElixir;

        if (!_EnableElixir)
        {
            Destroy(this.gameObject);
            return;
        }


        if (_Instance != null)
            Destroy(this.gameObject);
        else
        {
            _Instance = this;
            DontDestroyOnLoad(this.gameObject);

            Initialize_Elixir();
        }

        OnReadyCallback = () => { };
    }

    void Initialize_Elixir() // Satoshi games SDK
    {
        Debug.Log("Api Key: " + apiKey);
        Debug.Log("Game ID: " + gameId);

        //Elixir.ElixirController.StartElixir(apiKey, gameId);

        Debug.Log("Init Elixir...");
        //Elixir.ElixirController.StartElixir("be17c3ec-7739-4d1a-8470-aaef746f21fb", "b9721f6d-9473-4dea-8589-2493e57df03c");
#if UNITY_ANDROID

        Debug.Log("userGoogleId : " + InitializeCloudOnce.GooglePlayUserID);
        if (!string.IsNullOrEmpty(InitializeCloudOnce.GooglePlayUserID))
            ElixirController.StartAndroid("AG" + InitializeCloudOnce.GooglePlayUserID);
        else
            StartCoroutine(I_WaitAndroidGooglePlayID());

#else
        Elixir.ElixirController.StartRei();
#endif

        StartCoroutine(WaitElixirReady());
    }


    IEnumerator I_WaitAndroidGooglePlayID()
    {
        int TotalCheckers = 6;

        int counter = 0;
        while (string.IsNullOrEmpty(InitializeCloudOnce.GooglePlayUserID))
        {
            yield return new WaitForSeconds(0.7f);

            counter++;

            if(counter == TotalCheckers)
            {
                Debug.Log("Was not possible to initialize Elixir Android");

                StopAllCoroutines();
                _Instance = null;
                Destroy(this.gameObject);

                yield break;
            }
        }

        ElixirController.StartAndroid("AG" + InitializeCloudOnce.GooglePlayUserID);

        //yield return new WaitForSeconds(0.5f);

        //if (!string.IsNullOrEmpty(InitializeCloudOnce.GooglePlayUserID))
        //{
        //    ElixirController.StartAndroid(InitializeCloudOnce.GooglePlayUserID + "IW");
        //    yield break;
        //}

        //yield return new WaitForSeconds(1f);

        //if (!string.IsNullOrEmpty(InitializeCloudOnce.GooglePlayUserID))
        //{
        //    ElixirController.StartAndroid(InitializeCloudOnce.GooglePlayUserID + "IW");
        //    yield break;
        //}

        //yield return new WaitForSeconds(1f);

        //if (!string.IsNullOrEmpty(InitializeCloudOnce.GooglePlayUserID))
        //{
        //    ElixirController.StartAndroid(InitializeCloudOnce.GooglePlayUserID + "IW");
        //    yield break;

        //}

        //StopAllCoroutines();
        //_Instance = null;
        //Destroy(this.gameObject);

    }

    IEnumerator WaitElixirReady()
    {
        Debug.Log("Waiting Elixir to be ready...");

        while (!ElixirController.isReady)
            yield return null;


        Debug.Log("Elixir is ready");

        ElixirController.OnBalance += OnBalance;

        OnReadyCallback?.Invoke();

        if(OnReadyCallback == null)
        {
            Debug.Log("callback was null");

        }

        OnReadyCallback = null;



        //#if UNITY_ANDROID

        //        //...
        //        Elixir.ClaimRewards.Instance.ShowClaimRewards();
        //#endif

    }

    private void OnBalance(uint balance)
    {
        Debug.Log("Current balance: " + balance.ToString());
    }

    public static void AddBalance(int balance)
    {
        if (!_EnableElixir)
            return;

        if (ElixirController.isReady)
            ElixirController.BalanceAdd(balance);
        else
            _Instance.OnReadyCallback += () => { AddBalance(balance); };
    }

    public static void SubstractBalance(int balance)
    {
        if (!_EnableElixir)
            return;

        if (ElixirController.isReady)
            ElixirController.BalanceSubtract(balance);
        else
            _Instance.OnReadyCallback += () => { SubstractBalance(balance); };
    }

    public static void Save()
    {
        if (!_EnableElixir)
            return;

        if (ElixirController.isReady)
            ElixirController.Save();
        else
            _Instance.OnReadyCallback += Save;
    }

    public static uint GetBalance()
    {
        if (!_EnableElixir)
            return 0;

        if (ElixirController.isReady)
            return ElixirController.balance;
        else
            return 0;
    }

    public static void ShowClaimRewards_Android()
    {
        Debug.Log("1");

        if (!_EnableElixir)
            return;

        Debug.Log("2");

        if (ElixirController.isReady)
        {
            Elixir.ClaimRewards.Instance.ShowClaimRewards();
            Debug.Log("Fue llamado en ready");

        }
        else
        {
            _Instance.OnReadyCallback += ShowClaimRewards_Android;
            Debug.Log("almacenado");


        }
    }

    public static void HideClaimRewards_Android()
    {
        if (!_EnableElixir)
            return;

        Elixir.ClaimRewards.Instance.HideClaimRewards();
    }
}
