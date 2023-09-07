using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCuttingVisual : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    private Animator animator;
    private const string CUT = "Cut";

    private void Awake()
    {
        animator = GetComponent<Animator>(); 
    }
    private void Start()
    {
        cuttingCounter.OnCutt += CuttingCounter_OnCutt;
    }

    private void CuttingCounter_OnCutt(object sender, System.EventArgs e)
    {
        animator.SetTrigger(CUT);
    }
}
