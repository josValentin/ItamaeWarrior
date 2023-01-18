using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Elixir
{

    public class ClaimRewards : MonoBehaviour
    {
        public CanvasGroup canvasGroup;
        public static ClaimRewards Instance { get; private set; }

        private void Awake() {
            ClaimRewards.Instance = this;
            HideClaimRewards();
        }

        // Start is called before the first frame update
        public void ShowClaimRewards() {
            if (!string.IsNullOrEmpty(ElixirController.Instance.rewards.session.rewardUrl)) {
                canvasGroup.interactable = true;
                canvasGroup.alpha = 1;
                canvasGroup.blocksRaycasts = true;
            }
        }
        public void HideClaimRewards() {
            canvasGroup.interactable = false;
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }

        public void OnClaimRewards() {
            if (!string.IsNullOrEmpty(ElixirController.Instance.rewards.session.rewardUrl)) {
                ElixirController.Instance.dialog.Show($"Unclaimed Session", $"Do you want to claim your session?", "No", () => { }, "Yes", () => {
                    ElixirController.Instance.Log($"Trying to open {ElixirController.Instance.rewards.session.rewardUrl}");
#if UNITY_WEBGL
                            //Application.ExternalEval($"window.open('{session.rewardUrl}');");
#else
                    Application.OpenURL(ElixirController.Instance.rewards.session.rewardUrl);
#endif
                    Application.Quit();
                });
            }
        }
    }
}