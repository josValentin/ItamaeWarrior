using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField]
    private GameObject returnMenuButton;

    bool isPaused = false;

    public void pauseGame()
    {
        if (isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
            returnMenuButton.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            isPaused = true;
            returnMenuButton.SetActive(true);

        }
    }

    public void ReturnMenu()
    {
        GameManager.Instance.ReturnMenuCoverTransitor();
    }

}
