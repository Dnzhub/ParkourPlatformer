using System;
using UnityEngine;

public class RopeDetector : MonoBehaviour
{
    public event Action<Transform,Rigidbody> OnRope;

    private const string ROPE = "Rope";

    public Transform RopeGrapPosition { get; private set; }

    /// <summary>
    /// Check If character collide with rope object
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag(ROPE))
        {
            RopeGrapPosition = other.transform.GetChild(0);
            OnRope?.Invoke(RopeGrapPosition,other.GetComponent<Rigidbody>());
        }
       
    }
}
