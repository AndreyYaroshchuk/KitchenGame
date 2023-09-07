using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnFlashingBarUI : MonoBehaviour
{
    private const string IS_FLASHING = "IsFlashing";
    [SerializeField] StoveCounter stoveCounter;

    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burningShowProgress = 0.5f;
        bool show = stoveCounter.IsFried() && e.progressNirmalized >= burningShowProgress;
        animator.SetBool(IS_FLASHING, show);
    }
    
}
