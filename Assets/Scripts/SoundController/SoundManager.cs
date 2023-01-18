using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public List<AudioSManagable> audioSManagableList;

    // Start is called before the first frame update
    void Start()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void OnSoundValueChanged(bool value)
    {
        for (int i = 0; i < audioSManagableList.Count; i++)
        {
            audioSManagableList[i].SetSoundOn(value);
        }
    }
}
