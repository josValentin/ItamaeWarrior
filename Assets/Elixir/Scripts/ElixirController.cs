using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Elixir
{
    public class ElixirController : MonoBehaviour
    {
        static ElixirController _Instance;
        public static ElixirController Instance {
            get {
                if (_Instance == null) {
                    _Instance = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Elixir")).GetComponent<ElixirController>();

                    if (Screen.orientation == ScreenOrientation.Portrait) _Instance.GetComponent<UnityEngine.UI.CanvasScaler>().matchWidthOrHeight = 0.5f; // Originally was 1
                    DontDestroyOnLoad(_Instance.gameObject);
                }
                return _Instance;
            }
        }

        protected enum AuthMode
        {
            REIKey,
            Android,
            Steam,
            WebGL
        };

        // Informa si el API de Elixir esta inicializado.
        public static bool isReady { get; private set; }
        public static bool useconsole { get; private set; }
        // Balance actual del usuario dentro de Elixir
        public static uint balance { get; internal set; }
        public static callbackBalance OnBalance;
        public static int safetyBox { get; set; }
        public static void EarnSafetyBox() {
            if (safetyBox > 0) BalanceAdd(safetyBox);
            safetyBox = 0;
        }
        public static UserData storage { get { return Instance.rewards.storage; } }
        public static void Save(bool silence = true) { Instance.rewards.Save(silence); }

        public static void StartRei(BaseWS.callback OnInit = null, BaseWS.callback OnError = null) {
#if UNITY_EDITOR
            // Read previous REIKey
            if (string.IsNullOrEmpty(Instance.rei))
                Instance.rei = PlayerPrefs.GetString("REI");
#endif

#if UNITY_STANDALONE && !UNITY_EDITOR
            // Check for REIKey parameter (-rei)
            string[] args = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
                if (args[i] == "-rei")
                    Instance.rei = args[i + 1];
            if (string.IsNullOrEmpty(Instance.rei)) {
                Instance.dialog.Show("Error(-1)", "This game can only be run from the Elixir Launcher.", "Accept", () => { Application.Quit(); });
                return;
            }
#endif
            if (Instance.PrepareElixir()) {
                Instance.auth.REIKey(Instance.rei, () => { Instance.GetRewards(OnInit); }, () => {
#if UNITY_EDITOR
                    // If is invalid, try to generate a valid REIKey.
                    new GenerateREI().Do(() => {
                        // Save generated REIKey for further use.
                        PlayerPrefs.SetString("REI", Instance.rei);
                        Instance.auth.REIKey(Instance.rei, () => { Instance.GetRewards(OnInit); }, OnError);
                    });
#else
                OnError?.Invoke();
#endif
                }, !Instance.isDevelop);

            }
        }
        public static void StartAndroid(string androidID, BaseWS.callback OnInit = null, BaseWS.callback OnError = null) {
            if (Instance.PrepareElixir()) {
                Instance.auth.Android(androidID, () => {
                    Instance.GetRewards(OnInit);
                }, () => {
                    OnError?.Invoke();
                }, !Instance.isDevelop);
            }
        }

        public static void StartSteam(string steamID, BaseWS.callback OnInit = null, BaseWS.callback OnError = null) {
            if (Instance.PrepareElixir()) {
                Instance.auth.Steam(steamID, () => {
                    Instance.GetRewards(OnInit);
                }, () => {
                    OnError?.Invoke();
                }, !Instance.isDevelop);
            }
        }

        public static void StartWebGL(string webGLID, BaseWS.callback OnInit = null, BaseWS.callback OnError = null) {
            if (Instance.PrepareElixir()) {
                Instance.auth.WebGL(webGLID, () => {
                    Instance.GetRewards(OnInit);
                }, () => {
                    OnError?.Invoke();
                }, !Instance.isDevelop);
            }
        }

        // Llamada de ayuda para a�adir Satoshis.
        public delegate void callbackBalance(uint balance);
        public static void BalanceAdd(int amount = 0, callbackBalance OnOKCallback = null, BaseWS.callback OnErrorCallback = null) {
            Instance.rewards.Add(amount, () => { OnOKCallback?.Invoke(Instance.rewards.reward); }, OnErrorCallback);
        }

        // Llamada de ayuda para substraer Satoshis.
        public static void BalanceSubtract(int amount = 0, callbackBalance OnOKCallback = null, BaseWS.callback OnErrorCallback = null) {
            Instance.rewards.Subtract(amount, () => { OnOKCallback?.Invoke(Instance.rewards.amount); }, OnErrorCallback);
        }

        internal string rei;

        public delegate void analyticsEvent(string eventName, float value = 0);
        public static analyticsEvent AnalyticsEvent;
        public static BaseWS.callback OnOpenDialog;
        public static BaseWS.callback OnCloseDialog;

        bool isDevelop = false;
        public DialogWindow dialog;
        public CircularProgressBar progress;
        public Toast toast;
        public Auth auth { get; private set; }
        public Rewards rewards { get; private set; }
        public Assets assets { get; private set; }

        protected bool PrepareElixir() {
            if (isReady) return false;
            var elixirDescriptor = Resources.Load<ElixirDescriptor>("ElixirDescriptor");
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Instance.isDevelop = true;
#else
            Instance.isDevelop = false;
#endif
#if UNITY_EDITOR
            BaseWS.APIKEY = elixirDescriptor.Sanbox.APIKey;
            BaseWS.GameID = elixirDescriptor.Sanbox.GameID;
            BaseWS.baseURL = "https://sandbox.elixir.app";
            BaseWS.hmac = new HMACSHA256(Encoding.ASCII.GetBytes("nIhnQDqV6NYN5bYxhFOh4mpOU43fIj6f"));
            useconsole = elixirDescriptor.useconsole;
#else
            switch (elixirDescriptor.InBuild) {
                case ElixirDescriptor.Environments.Sanbox:
                    BaseWS.APIKEY = elixirDescriptor.Sanbox.APIKey;
                    BaseWS.GameID = elixirDescriptor.Sanbox.GameID;
                    BaseWS.baseURL = "https://sandbox.elixir.app";
                    BaseWS.hmac = new HMACSHA256(Encoding.ASCII.GetBytes("nIhnQDqV6NYN5bYxhFOh4mpOU43fIj6f"));
                    useconsole = elixirDescriptor.useconsole;

                    break;
                case ElixirDescriptor.Environments.Production:
#if UNITY_ANDROID
                    BaseWS.APIKEY = elixirDescriptor.AndroidProduction.APIKey;
                    BaseWS.GameID = elixirDescriptor.AndroidProduction.GameID;
#else
                    BaseWS.APIKEY = elixirDescriptor.PCProduction.APIKey;
                    BaseWS.GameID = elixirDescriptor.PCProduction.GameID;
#endif
                    BaseWS.baseURL = "https://kend.elixir.app";
                    BaseWS.hmac = new HMACSHA256(Encoding.ASCII.GetBytes("kiu84SHMmIKGjDnIWxH7ICySrcDLB06b"));
                    useconsole = elixirDescriptor.useconsole;
                    break;
            }
#endif
            rewards = new Rewards();
            assets = new Assets();
            auth = new Auth();
            return true;
        }

        void GetRewards(BaseWS.callback OnInit) {
            rewards.Get(() => { isReady = true; OnInit?.Invoke(); });
        }

        void Update() {
            auth?.CheckToken(Time.deltaTime);
#if !ENABLE_INPUT_SYSTEM
            if (useconsole && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Escape))
                showedConsole = !showedConsole;
#endif
        }

        void OnDestroy() {
            auth.Close();
        }
        string consoleText = "";
        bool showedConsole = false;
        private void OnGUI() {
            if (showedConsole)
                GUI.Label(new Rect(0, 0, Screen.width, Screen.height), consoleText);
        }
        public void Log(string log) {
            if (useconsole) {
                Debug.Log($"<color=#a0a000>[Elixir] {log}</color>");
                consoleText += log + "\n";
            }

        }
    }
}