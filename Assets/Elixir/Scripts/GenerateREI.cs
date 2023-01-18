#if UNITY_EDITOR

namespace Elixir {
    internal class GenerateREI : BaseWS {
        public string reikey;
        public void Do(callback OnOk = null, callback OnError = null) {
            ElixirController.Instance.auth.token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJfaWQiOiI5YmM4MTgzOC00NTY0LTQ4M2UtYmE2MC1iY2JiZjE2YmM1MjQiLCJlbWFpbCI6ImZlcm5hbmRvdGVzdDFAc2F0b3NoaXMuZ2FtZXMiLCJpYXQiOjE2Mjc0ODUxODYsImV4cCI6MjE1NTQ4NTE4Nn0.SAkEr9-CWwnLSNup7X1d9E3vmSj3mlPQA_5Z2p54m__hPsL765xFxvUvjRpKwMhxrAE9QirAodP7868_yA0s9A";
            ElixirController.Instance.StartCoroutine(base.Get($"/dev/reikey/{GameID}", () => {
                ElixirController.Instance.Log($"ReiKey {reikey}");
                ElixirController.Instance.rei = reikey;
                OnOk?.Invoke();
            }, OnError));
        }
    }
}

#endif