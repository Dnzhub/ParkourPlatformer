using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// State base for all characters
/// </summary>
public abstract class State 
{
    public abstract void Enter();
    public abstract void Tick(float deltaTime);
    public abstract void FixedTick(float fixedDeltaTime);
    public abstract void Exit();

}
