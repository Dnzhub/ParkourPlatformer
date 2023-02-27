using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;
	 
	public PlayerBaseState(PlayerStateMachine stateMachine)
	{
		this.stateMachine = stateMachine;
	}
	 
	/// <summary>
	/// Move function will manage from base so all state have same gravity values
	/// </summary>
	/// <param name="motion : direction of movement"></param>
	/// <param name="deltaTime"></param>
	protected void Move(Vector3 motion, float deltaTime)
	{
		stateMachine.Controller.Move((motion + stateMachine.Gravity.Movement) * deltaTime);
	}	
	
}
