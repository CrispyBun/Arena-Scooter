using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sound
{
    ButtonSelect,
    ButtonClick
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioClick;
    [SerializeField] private AudioSource audioSelect;

    private static Dictionary<Sound, AudioSource> sounds;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        sounds = new();

        sounds[Sound.ButtonClick] = audioClick;
        sounds[Sound.ButtonSelect] = audioSelect;

        Cursor.visible = false;
    }

    public static void Play(Sound sound)
    {
        sounds[sound].Play();
    }
}
