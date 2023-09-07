using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private Player player;

    private float footStapTimer;
    private float footStapTimerMax = 0.15f;
   // private float volume = 1.1f;
    private void Awake()
    {
        player = GetComponent<Player>();    
    }
    private void Update()
    {
        footStapTimer -= Time.deltaTime;
        if (footStapTimer < 0f)
        {
            footStapTimer = footStapTimerMax;

            if (player.IsWalking())
            {
                float volume = 1.1f;
                SaundManeger.Instance.PlayFootSound(player.transform.position, volume);
            }
        }
    }
}
