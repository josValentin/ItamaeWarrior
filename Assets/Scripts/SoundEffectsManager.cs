using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    private static SoundEffectsManager _Instance;

    [SerializeField] private AudioSource source;

    [SerializeField] private AudioClip slashClip;
    [SerializeField] private AudioClip damageClip;

    [SerializeField] private AudioClip chidoriClip;

    private void Start()
    {
        _Instance = this;
    }

    public static void Play_Slash()
    {
        _Instance.source.PlayOneShot(_Instance.slashClip);
    }

    public static void Play_Damage()
    {
        _Instance.source.PlayOneShot(_Instance.damageClip);
    }

    public static void Play_Chidori()
    {
        _Instance.source.clip = _Instance.chidoriClip;
        _Instance.source.Play();

    }
}
