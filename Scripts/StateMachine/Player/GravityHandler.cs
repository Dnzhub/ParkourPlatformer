using UnityEngine;

/// <summary>
/// This script responsible for gravity and force apply on character controller
/// </summary>
public class GravityHandler : MonoBehaviour
{
    [Header("--- Gravity attributes ---")]
    private CharacterController controller;
    private float verticalVeloctiy;

    [Header("--- Force attributes ---")]
    private float smoothTime = 0.3f;
    private Vector3 dampingVelocity;
    private Vector3 impact;

    public Vector3 Movement => impact + Vector3.up * verticalVeloctiy;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        if (verticalVeloctiy < 0 && controller.isGrounded)
        {
            verticalVeloctiy = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVeloctiy += Physics.gravity.y * Time.deltaTime;
        }
        
        //Consume impact force
        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, smoothTime);
    }

    public void ResetGravity()
    {
        verticalVeloctiy = 0;
    }

    public void Jump(float jumpForce)
    {
        verticalVeloctiy += jumpForce;
    }

    public void AddForce(Vector3 force)
    {
        impact += force;
    }
}
