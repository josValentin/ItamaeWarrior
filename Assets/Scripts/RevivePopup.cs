using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RevivePopup : MonoBehaviour
{
    Canvas thisCanvas;

    [SerializeField] private Button btnNo;
    [SerializeField] private Button btnYes;
    // Start is called before the first frame update
    void Start()
    {
        InitRewardAdsDelegates();

        gameObject.SetActive(false);

        btnNo.onClick.AddListener(() =>
        {
            GameManager.Instance.ReturnMenuCoverTransitor();
        });
        btnYes.onClick.AddListener(WatchAdAndRevive);
    }

    void WatchAdAndRevive()
    {
#if UNITY_EDITOR
        OnReward();
#else
        btnNo.interactable = false;

        Yodo1AdsManager.ShowRewardedAd();
#endif
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Close()
    {
        GetComponent<Animator>().SetTrigger("Close");
    }

    void Hide()
    {
        if (rewarded)
        {
            if (GameManager.Instance.sushiTempList.Count > 0)
            {
                for (int i = GameManager.Instance.sushiTempList.Count - 1; i >= 0; i--)
                {
                    GameManager.Instance.sushiTempList[i].Die(false);
                }
            }
        }

        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    void InitRewardAdsDelegates()
    {
        Yodo1AdsManager.SetRewardAdDelegates(OnReward, () =>
        {

            btnNo.interactable = true;

        }, () =>
        {
            btnNo.interactable = true;

        }, () =>
        {
            btnNo.interactable = true;

        });
    }

    bool rewarded = false;
    void OnReward()
    {

        rewarded = true;

        GameManager.Instance.AddLife();
        Close();
        btnNo.interactable = true;
    }
}
