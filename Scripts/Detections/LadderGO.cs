using UnityEngine;

/// <summary>
/// This script attached to ladder trigger object(inside ladder)
/// </summary>
public class LadderGO : MonoBehaviour
{
    [Tooltip("If player interact with ladder it will take its child object as target")]
    public Transform TransitionPosition { get; private set; }
    [SerializeField] private bool isBottomTrigger;
    private void Start()
    {
        TransitionPosition = transform.GetChild(0);
    } 
   
}
