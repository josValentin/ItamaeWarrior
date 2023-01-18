//#define GOOGLEPLAYGAMES
//#define GAME_ANALYTICS

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


#if GAME_ANALYTICS
using GameAnalyticsSDK;
#endif



public class SplashScreen : MonoBehaviour
{

    public string nextScene;
    // Start is called before the first frame update
    IEnumerator Start() {
        float time = Time.realtimeSinceStartup;
#if GAME_ANALYTICS
        GameAnalytics.Initialize();
        Elixir.ElixirController.AnalyticsEvent += (string eventName, float value) => {
            GameAnalytics.NewDesignEvent(eventName, value);
        };
#endif
        yield return StarElixir();
        float diff = Time.realtimeSinceStartup - time;
        if (diff < 2) yield return new WaitForSeconds(5 - diff);
        yield return LoadAsynchronously(nextScene);
    }

    IEnumerator StarElixir() {
#if UNITY_WEBGL && !UNITY_EDITOR
        // MiniGames.
        //LeChuckAPI lechuck = LeChuckAPI.getInstance();
        //lechuck.api.onReady((bool simulationMode, LeChuckAPI.Api.Info apiInfo, LeChuckAPI.Api.User currentUser) => {
        //    if (currentUser.isGuest)
        //        Elixir.ElixirController.Instance.dialog.Show($"Error", $"Elixir can not use Guest accounts.\nPlease login and reload.", "Accept", () => { Application.Quit(); });
        //    else
        //        Elixir.ElixirController.StartWebGL($"MG{currentUser.id}");

        //});

#elif UNITY_ANDROID && GOOGLEPLAYGAMES
        GooglePlayGames.BasicApi.PlayGamesClientConfiguration config = new GooglePlayGames.BasicApi.PlayGamesClientConfiguration.Builder().Build();
        //PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().RequestServerAuthCode(false).RequestIdToken().Build();
        //PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().RequestIdToken().Build();

        GooglePlayGames.PlayGamesPlatform.InitializeInstance(config);
        GooglePlayGames.PlayGamesPlatform.DebugLogEnabled = true;
        GooglePlayGames.PlayGamesPlatform.Activate();
        GooglePlayGames.PlayGamesPlatform.Instance.Authenticate(async (bool result, string data) => {
            if (result) {
                string userID = GooglePlayGames.PlayGamesPlatform.Instance.GetUserId();
                //string IDToken = PlayGamesPlatform.Instance.GetIdToken();
                //string serverAuthCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                Elixir.ElixirController.StartAndroid($"AG_{userID}");
            } else
                Elixir.ElixirController.StartAndroid(null);
        }, false);

#else
        Elixir.ElixirController.StartRei();
#endif
        
        while (!Elixir.ElixirController.isReady) yield return null;
    }
    IEnumerator LoadAsynchronously(string name) {
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        while (!operation.isDone) {
            yield return null;
        }
    }
}
