// #define USE_GOOGLE_PLAY_GAMES

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

#if USE_GOOGLE_PLAY_GAMES
using GooglePlayGames.BasicApi;
using GooglePlayGames;
#endif

public class InitSceneController : MonoBehaviour {
    public string NextSceneToLoad;
    IEnumerator Start() {
#if UNITY_STANDALONE || UNITY_EDITOR
        Elixir.ElixirController.StartRei(); // DEV
#elif UNITY_ANDROID
#if USE_GOOGLE_PLAY_GAMES
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(async (bool result, string data) => {
            if (result) {
                string userID = PlayGamesPlatform.Instance.GetUserId();
                Elixir.ElixirController.StartAndroid($"AG{userID}");                    
            } else {
#if UNITY_EDITOR
                Elixir.ElixirController.StartAndroid(null);
#else
                Elixir.ElixirController.Instance.dialog.Show($"Error", $"Elixir does not accept guest accounts to claim rewards. Please login in Google Play Games before playing.", "Accept", () => { Application.Quit(); });
#endif
                }
            }, false);
#else
        Elixir.ElixirController.StartAndroid(null);
#endif
#elif UNITY_WEBGL
        Elixir.ElixirController.StartWebGL("ID"); // DEV
#endif

        while (!Elixir.ElixirController.isReady) yield return null;
        SceneManager.LoadScene(NextSceneToLoad);
    }
}
