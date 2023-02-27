using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetector : MonoBehaviour
{

    public event Action OnLedgeDetect;

    private void OnTriggerEnter(Collider other)
    {
        OnLedgeDetect?.Invoke();
    }
}
