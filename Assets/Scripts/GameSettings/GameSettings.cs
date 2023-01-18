using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings
{
    public static bool SoundOn { set { SetValue(value); } get => _SoundOn; }

    private static bool _SoundOn = true;

    public static List<AudioSManagable> audioSManagableList = new List<AudioSManagable>();

    private static void SetValue(bool value)
    {
        _SoundOn = value;
        int target = value ? 1 : 0;
        PlayerPrefs.SetInt("SoundOn", target);
        for (int i = 0; i < audioSManagableList.Count; i++)
        {
            audioSManagableList[i].SetSoundOn(value);
        }
    }

    public static void InitializeSettings()
    {
        if (PlayerPrefs.HasKey("SoundOn"))
        {
            int value = PlayerPrefs.GetInt("SoundOn");
            _SoundOn = value == 1 ? true : false;
        }


    }


}
