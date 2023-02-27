using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderDetector : MonoBehaviour
{
    public Transform TargetTrigger { get; private set; }
    public bool CanClimb = false;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out LadderGO ladder))
        {          
            TargetTrigger = ladder.transform;
            CanClimb = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent(out LadderGO ladder))
        {
            TargetTrigger = null;
            CanClimb = false;
        }
    }
}
