using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button btnSwitchSound;
    private Sprite sprSoundButtonOn;

    [SerializeField] private Sprite sprSoundButtonOff;

    private void Start()
    {
        if (Time.timeScale != 1)
            Time.timeScale = 1;

        btnSwitchSound.onClick.AddListener(SwitchSoundOn);
        sprSoundButtonOn = btnSwitchSound.image.sprite;

        btnSwitchSound.image.sprite = GameSettings.SoundOn ? sprSoundButtonOn : sprSoundButtonOff;

#if UNITY_ANDROID
        ElixirGameController.ShowClaimRewards_Android();
#endif
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");

#if UNITY_ANDROID
        ElixirGameController.HideClaimRewards_Android();
#endif
    }

    private void SwitchSoundOn()
    {

        GameSettings.SoundOn = !GameSettings.SoundOn;

        btnSwitchSound.image.sprite = GameSettings.SoundOn ? sprSoundButtonOn : sprSoundButtonOff;

    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
