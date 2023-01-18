using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elixir{
    public class Rewards : BaseWS {
        [System.Serializable]
        public class GameSession {
            public string userType;
            public string sessionId;
            public string elixirId;
            public string externId;
            public string rewardUrl;
            public uint userBalance;
            public string username;
        }
        public GameSession session;
        [System.Serializable]
        public class NTF {
            public int units;
            public string assetId;
            public string[] skinIds;
        };
        public NTF[]        nfts;
        public UserData     storage;
        public uint         userBalance; // Se usa en las respuestas de Add y Substract.
        public uint         amount;
        public uint         reward;
        // TODO: A�adir la URL para vincular cuentas, en caso de ser necesario...
        // Pide a Elixir que a�ada <amount> Satoshis al usuario actual.
        public void Get(callback OnOk = null, callback OnError = null) {
            ElixirController.Instance.StartCoroutine(base.Get($"/rewards/{GameID}/session", () => {
                ElixirController.balance = session.userBalance;
                /*
                if (!string.IsNullOrEmpty(session.rewardUrl)) {
                    ElixirController.Instance.dialog.Show($"Unclaimed Session", $"Do you want to claim your session?", "No", () => { }, "Yes", () => {
                        ElixirController.Instance.Log($"Trying to open {session.rewardUrl}");
#if UNITY_WEBGL
                            Application.ExternalEval($"window.open('{session.rewardUrl}');");
#else
                        Application.OpenURL(session.rewardUrl);
#endif
                        Application.Quit();
                    });
                }
                */
                ElixirController.AnalyticsEvent?.Invoke("Session_Login");
                ElixirController.OnBalance?.Invoke(session.userBalance);
                OnOk?.Invoke();
            }, OnError));
        }

        // Pide a Elixir que a�ada <amount> Satoshis al usuario actual.
        public void Add(int amount = 0, callback OnOk = null, callback OnError = null) {
            ElixirController.Instance.StartCoroutine(base.Post($"/rewards/{GameID}/balance", $"{{\"amount\":{amount}}}", () => {
                ElixirController.balance = userBalance;
                ElixirController.AnalyticsEvent?.Invoke("Add", (float)amount);
                ElixirController.OnBalance?.Invoke(userBalance);
                OnOk?.Invoke();
            }, OnError, true));
        }

        // Pide a Elixir que quite <amount> Satoshis al usuario actual.
        public void Subtract(int amount, callback OnOk = null, callback OnError = null) {
            ElixirController.Instance.StartCoroutine(base.Post($"/rewards/{GameID}/subtract", $"{{\"amount\":{amount}}}", () => {
                ElixirController.balance = userBalance;
                ElixirController.AnalyticsEvent?.Invoke("Subtract", (float)amount);
                ElixirController.OnBalance?.Invoke(userBalance);
                OnOk?.Invoke();
            }, OnError, true));
        }

        float timeToCallStorage = 0;
        public void Save(bool silence = false, callback OnOk = null, callback OnError = null) {
            if (timeToCallStorage == 0) {
                timeToCallStorage = 0.5f;
            }
            else {
                timeToCallStorage = 0.5f;
                return; // We are waiting, don´t do anything.
            }
            ElixirController.Instance.StartCoroutine(_SaveCoroutine(silence, OnOk, OnError));
        }

        IEnumerator _SaveCoroutine(bool silence = false, callback OnOk = null, callback OnError = null) {
            while (timeToCallStorage > 0) {
                timeToCallStorage -= Time.deltaTime;
                yield return null;
            }
            timeToCallStorage = 0;
            var data = $"{{\"storage\":{JsonUtility.ToJson(storage)}}}";
            yield return base.Post($"/rewards/{GameID}/storage", data, () => {
                ElixirController.AnalyticsEvent?.Invoke("Session_Saved");
                OnOk?.Invoke();
            }, OnError, silence);
        }

        [System.Serializable]
        public class AssetsIDs {
            public string[] assetsIds;
            public AssetsIDs(string[] assetsIds) {
                this.assetsIds = assetsIds;
            }
        }
        public string[] rewards;
        public delegate void callbackReward(string[] rewards);
        public void Reward(string[] assetsIds, callbackReward OnOk = null, callback OnError = null) {
            var data = JsonUtility.ToJson(new AssetsIDs(assetsIds));
            ElixirController.Instance.StartCoroutine(base.Post($"/rewards/{GameID}/asset", data, () => {
                OnOk?.Invoke(rewards);
            }, OnError,true));
        }
        public void BurnNFT(string assetIds, callback OnOk = null, callback OnError = null) {
            ElixirController.Instance.StartCoroutine(base.Get($"/assets/{GameID}/burn-nft/{assetIds}", () => {
            }, OnError, true, true));

        }

    }
}