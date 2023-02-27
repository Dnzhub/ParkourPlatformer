using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base State machine for all characters
/// </summary>
public abstract class StateMachine : MonoBehaviour
{
    private State currentState;
     
    private void Update()
    {
        currentState?.Tick(Time.deltaTime);
      
    }
    private void FixedUpdate()
    {
        currentState?.FixedTick(Time.fixedDeltaTime);
    }

    public void SwitchState(State newState)
    {       
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
}
