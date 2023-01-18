using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Security.Cryptography;

// Clase base para consumo de EndPoints de Elixir.
namespace Elixir
{
    public class BaseWS
    {
        internal static string baseURL;
        internal static string APIKEY;
        internal static string GameID;

        public delegate void callback();
        [System.Serializable]
        public class Error
        {
            public int status = 0;
            public int code = 0;
            public string message = "-1";
        }
        public Error error = new Error();
        // GET Call.
        protected virtual IEnumerator Get(string uri, callback OnOk = null, callback OnError = null, bool silence = false, bool showDialogOnError = true, string target = null) {
            if (!silence) ElixirController.Instance.progress.Show();
            ElixirController.Instance.Log($"Elixir.Get({uri})");
            yield return MakeRequest(uri, null, (data) => {
                if (!string.IsNullOrEmpty(target)) data = $"{{\"{target}\":{data}}}";
                JsonUtility.FromJsonOverwrite(data, this);
                if (!silence) ElixirController.Instance.progress.Hide();
                OnOk?.Invoke();
            }, (code, message) => {
                if (!silence) ElixirController.Instance.progress.Hide();
                if (showDialogOnError) ElixirController.Instance.dialog.Show($"Error [{code}]", $"There is a problem with your account.", "Accept", () => { Application.Quit(); });
                OnError?.Invoke();
            });
        }
        // POST Call.
        protected virtual IEnumerator Post(string uri, string body, callback OnOk = null, callback OnError = null, bool silence = false, bool showDialogOnError = true) {
            if (!silence) ElixirController.Instance.progress.Show();
            ElixirController.Instance.Log($"Elixir.Post({uri}) <- {body}");
            yield return MakeRequest(uri, body, (data) => {
                JsonUtility.FromJsonOverwrite(data, this);
                if (!silence) ElixirController.Instance.progress.Hide();
                OnOk?.Invoke();
            }, (code, message) => {
                if (!silence) ElixirController.Instance.progress.Hide();
                if (showDialogOnError) ElixirController.Instance.dialog.Show($"Error [{code}]", $"There is a problem with your account.", "Accept", () => { Application.Quit(); });
                OnError?.Invoke();
            });
        }

        public delegate void callbackResponse(string data);
        public delegate void callbackError(int code, string message);
        protected IEnumerator MakeRequest(string uri, string body, callbackResponse OnResponse, callbackError OnError) {
            error.status = 0;
            error.code = -1;
            ulong epoch = GetEpoch();
            UnityWebRequest www;
            string signature;
            if (string.IsNullOrEmpty(body)) {
                signature = ByteArrayToString(hmac.ComputeHash(Encoding.UTF8.GetBytes($"{epoch}.\"{uri}\"")));
                www = UnityWebRequest.Get($"{baseURL}{uri}");
            } else {
                signature = ByteArrayToString(hmac.ComputeHash(Encoding.UTF8.GetBytes($"{epoch}.{body}")));
                www = UnityWebRequest.Post($"{baseURL}{uri}", body);
                www.SetRequestHeader("content-type", "application/json");
                www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(body));
                www.uploadHandler.contentType = "application/json";
            }
            www.SetRequestHeader("x-api-key", APIKEY);
            www.SetRequestHeader("x-api-time", epoch.ToString());
            www.SetRequestHeader("x-api-signature", signature);
            if (!string.IsNullOrEmpty(ElixirController.Instance.auth.token))
                www.SetRequestHeader("Authorization", "Bearer " + ElixirController.Instance.auth.token);
            yield return www.SendWebRequest();
            try {
                if (www.isNetworkError || www.isHttpError) {
                    ElixirController.Instance.Log($"ERROR({baseURL}{uri}): {www.error} ->{www?.downloadHandler?.text}");
                    if (www.downloadHandler != null) {
                        string text = www.downloadHandler.text;
                        JsonUtility.FromJsonOverwrite(text, this);
                        if (error.status != 0) {
                            if (error.code != 0 && error.code != 5301 && error.code != 5302) {
                                OnError?.Invoke(error.code, error.message);
                                yield break;
                            } else {
                                if (error.code == 5301) ElixirController.Instance.toast.Show("Daily reward limit reached.");
                                else if (error.code == 5302) ElixirController.Instance.toast.Show("Monthly reward limit reached");
                            }
                        }
                    }
                } else {
                    ElixirController.Instance.Log($"Elixir.Response({uri}) {www.downloadHandler.text} ");
                    // Check if error.
                    string text = www.downloadHandler.text;
                    if (text[0] != '[') { // is not an array.
                        JsonUtility.FromJsonOverwrite(text, this);
                        if (error.status != 0) {
                            if (error.code != 0 && error.code != 5301 && error.code != 5302)
                                throw new System.Exception($"Error on request status: {error.status} errorCode: {error.code} msg: {error.message}");
                            else {
                                if (error.code == 5301) ElixirController.Instance.toast.Show("Daily reward limit reached.");
                                else if (error.code == 5302) ElixirController.Instance.toast.Show("Monthly reward limit reached");
                            }
                        }
                    }
                    OnResponse?.Invoke(text);
                    yield break;
                }
            } catch (System.Exception e) {
                ElixirController.Instance.Log($"ERROR: Elixir.Get({uri}) {e.Message}");                
            }
            OnError?.Invoke(error.code, error.message);
        }
        internal static string ByteArrayToString(byte[] ba) {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba) hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        internal static ulong GetEpoch() {
            return (ulong)(System.DateTime.UtcNow - new System.DateTime(1970, 1, 1)).TotalSeconds;
        }
        internal static HMACSHA256 hmac;
    }
}