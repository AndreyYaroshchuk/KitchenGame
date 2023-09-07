using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class SaundManeger : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";
    public static SaundManeger Instance { get; private set; }

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    private float volume = 1f;
    
    private void Awake()
    {
        Instance = this;
       volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }
    private void Start()
    {
        DeliveryManeger.Instance.OnRecipeFailed += DeliveryManeger_OnRecipeFailed;
        DeliveryManeger.Instance.OnRecipeSuccess += DeliveryManeger_OnRecipeSuccess;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.OnAnyPlayerPicked += Player_OnPickedSomething;
        BaseCounter.OnObjectDrops += BaseCounter_OnObjectDrops;
        TrashCounter.OnObjectTrash += TrashCounter_OnObjectTrash;
    }
    private void TrashCounter_OnObjectTrash(object sender, EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnObjectDrops(object sender, EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnPickedSomething(object sender, EventArgs e)
    {
        Player player = sender as Player;
        PlaySound(audioClipRefsSO.objectPickup, player.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    

    private void DeliveryManeger_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliverySuccess, deliveryCounter.transform.position); // Camera.main.transform.position это ложит звуки на камеру
    }

    private void DeliveryManeger_OnRecipeFailed(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliveryFail, deliveryCounter.transform.position);
    }
    private void PlaySound(AudioClip[] audioClipArray, Vector3 vector3Position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], vector3Position, volume);
    }

    private void PlaySound(AudioClip audioClip, Vector3 vector3Position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, vector3Position, volumeMultiplier * volume);
    }

    public void PlayFootSound(Vector3 vector3Position, float volume)
    {
        PlaySound(audioClipRefsSO.footstep, vector3Position, volume);
    }
    public void PlayCounDownSound()
    {
        PlaySound(audioClipRefsSO.warning, Vector3.zero);
    }
    public void PlayWarningSound( Vector3 position)
    {
        PlaySound(audioClipRefsSO.warning, position);
    }
    public void ChangeVolume()
    {
        volume += 0.1f;
        if(volume > 1f)
        {
            volume = 0f;
          
        }
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }
    public float GetVolume()
    {
        return volume;
    }
}
