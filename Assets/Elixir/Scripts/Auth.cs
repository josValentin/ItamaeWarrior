using UnityEngine.Networking;
using System.Text;

namespace Elixir {
    public class Auth : BaseWS {
        public string refreshToken;
        public string token;
        public ulong serverTimeMS;
        public ulong tokenExpiry;
        public ulong tokenLifeMS;
        public void REIKey(string rei, callback OnOk = null, callback OnError = null, bool showDialogOnError = true) {
            ElixirController.Instance.Log($"REIKey Auth using {rei}");
            ElixirController.Instance.StartCoroutine(base.Get($"/auth/{GameID}/rei/{rei}", () => {
                timeToRefreshToken = (tokenLifeMS / 1000) - 5;
                OnOk?.Invoke();
            }, OnError, false, showDialogOnError));
        }
        public void Steam(string steamID, callback OnOk = null, callback OnError = null, bool showDialogOnError = true) {
            ElixirController.Instance.Log($"Steam Auth using {steamID}");
            ElixirController.Instance.StartCoroutine(base.Get($"/auth/{GameID}/steam/{steamID}", () => {
                timeToRefreshToken = (tokenLifeMS / 1000) - 5;
                OnOk?.Invoke();
            }, OnError, false, showDialogOnError));
        }
        public void Android(string androidID=null, callback OnOk = null, callback OnError = null, bool showDialogOnError = true) {
            if (string.IsNullOrEmpty(androidID)) {
                if (UnityEngine.PlayerPrefs.HasKey("androidID")) androidID = UnityEngine.PlayerPrefs.GetString("androidID");
                else { androidID = GetGuid(); UnityEngine.PlayerPrefs.SetString("androidID", androidID); }
            }
            ElixirController.Instance.Log($"Android Auth using {androidID}");
            ElixirController.Instance.StartCoroutine(base.Get($"/auth/{GameID}/android/{androidID}", () => {
                timeToRefreshToken = (tokenLifeMS / 1000) - 5;
                ElixirController.Instance.StartCoroutine(WaitToOk(0.5f, OnOk));
//                OnOk?.Invoke();
            }, OnError, false, showDialogOnError));
        }

        public void WebGL(string webglID, callback OnOk = null, callback OnError = null, bool showDialogOnError = true) {
            ElixirController.Instance.Log($"WebGL Auth using {webglID}");
            ElixirController.Instance.StartCoroutine(base.Get($"/auth/{GameID}/webgl/{webglID}", () => {
                timeToRefreshToken = (tokenLifeMS / 1000) - 5;
                ElixirController.Instance.StartCoroutine(WaitToOk(0.5f, OnOk));
//                OnOk?.Invoke();
            }, OnError, false, showDialogOnError));
        }

        public System.Collections.IEnumerator WaitToOk(float time=0.5f, callback OnOk = null) {
            yield return new UnityEngine.WaitForSeconds(time);
            OnOk?.Invoke();
        }
        public void Refresh(callback OnOk = null, callback OnError = null) {
            ElixirController.Instance.StartCoroutine(base.Get($"/auth/{GameID}/refresh/{refreshToken}", () => {
                timeToRefreshToken = (tokenLifeMS / 1000) - 5;
                OnOk?.Invoke();
            }, OnError, true));
        }

        public void Close() {
            var uri = $"/auth/{GameID}/closerei";
            ulong epoch = BaseWS.GetEpoch();
            string signature = BaseWS.ByteArrayToString(BaseWS.hmac.ComputeHash(Encoding.ASCII.GetBytes($"{epoch}.\"{uri}\"")));
            UnityWebRequest www = UnityWebRequest.Get($"{BaseWS.baseURL}{uri}");
            www.SetRequestHeader("x-api-key", APIKEY);
            www.SetRequestHeader("x-api-time", epoch.ToString());
            www.SetRequestHeader("x-api-signature", signature);
            if (!string.IsNullOrEmpty(token)) www.SetRequestHeader("Authorization", "Bearer " + token);
            www.SendWebRequest();
            ElixirController.Instance.Log($"Closing session.");
        }
        float timeToRefreshToken = 0;
        public void CheckToken(float deltaTime) {
            if (timeToRefreshToken > 0) {
                timeToRefreshToken -= deltaTime;
                if (timeToRefreshToken < 0) {
                    timeToRefreshToken = 0;
                    Refresh();
                }
            }
        }

        string GetGuid() {
        byte[] timestamp = System.BitConverter.GetBytes(System.DateTime.UtcNow.Ticks);
        byte[] random = System.BitConverter.GetBytes((long)(UnityEngine.Random.value * long.MaxValue));
        byte[] guid = new byte[16];
            System.Array.Copy(timestamp, 0, guid, 0, System.Math.Min(8, timestamp.Length));
            System.Array.Copy(random, 0, guid, 7, System.Math.Min(8, random.Length));
            return new System.Guid(guid).ToString();
        }
    }
}