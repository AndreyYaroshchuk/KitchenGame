using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicManeger : MonoBehaviour
{
    private const string PLAYER_PREFS_VOLUME = "MusicVolume";
    public static MusicManeger Instance { get; private set; }

    [SerializeField] AudioSource audioSource;
    private float volume = 0.3f;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();  
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_VOLUME, 1f);
        audioSource.volume = volume;    
    }

    public void ChangeVolume()
    {
        volume += 0.1f;
        if (volume > 1f)
        {
            volume = 0f;     
        }
        audioSource.volume = volume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_VOLUME, volume);
        PlayerPrefs.Save();
    }
    public float GetVolume()
    {
        return volume;
    }

}
