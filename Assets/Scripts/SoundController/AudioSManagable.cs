using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSManagable : MonoBehaviour
{
    float defaultVolume;

    AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        defaultVolume = source.volume;

        GameSettings.audioSManagableList.Add(this);

        SetSoundOn(GameSettings.SoundOn);
    }

    public void SetSoundOn(bool value)
    {
        if (value)
            source.volume = defaultVolume;
        else
            source.volume = 0;
    }


    private void OnDestroy()
    {
        GameSettings.audioSManagableList.Remove(this);
    }

}
