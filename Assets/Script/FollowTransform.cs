using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    private Transform targetTransform;

    private void LateUpdate() // срабатывае после того как все апдейты сработают
    {
        if (targetTransform == null)
        {
            return;
        }
        transform.position = targetTransform.position;
        transform.rotation = targetTransform.rotation;
    }
    public void SetTargetTransform(Transform targetTransform)
    {
        this.targetTransform = targetTransform;
    }
}
