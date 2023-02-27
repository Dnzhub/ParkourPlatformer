using UnityEngine;

public class JumpState : PlayerBaseState
{
    [Header("--- Animation Hash Codes ---")]
    private readonly int JumpHash = Animator.StringToHash("Jumping");
    private float animationSmoothTime = 0.1f;

    private Vector3 jumpDirection;
    private float groundDistance = 0.5f;
    
    public JumpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.RopeDetector.OnRope += HandleOnRope;
        stateMachine.LedgeDetection.OnLedgeDetect += ClimbToLedge;

        ApplyJumpValues();
        stateMachine.Animator.CrossFadeInFixedTime(JumpHash, animationSmoothTime);
      
        
    }
    public override void Tick(float deltaTime)
    {
        Move(jumpDirection, deltaTime);

        if(stateMachine.Controller.velocity.y <= groundDistance)
        {
            stateMachine.SwitchState(new LandState(stateMachine));
        }
    }
    public override void FixedTick(float fixedDeltaTime)
    {
    }
    public override void Exit()
    {
        stateMachine.RopeDetector.OnRope -= HandleOnRope;
        stateMachine.LedgeDetection.OnLedgeDetect -= ClimbToLedge;

    }
    private void ApplyJumpValues()
    {
        stateMachine.Gravity.Jump(stateMachine.JumpForce);
        jumpDirection = stateMachine.Controller.velocity;
        jumpDirection.y = 0;
    }
    private void ClimbToLedge()
    {      
        stateMachine.SwitchState(new ClimbTopState(stateMachine));
    }
    private void HandleOnRope(Transform activeRopePosition,Rigidbody ropeRigidBody)
    {
        
        stateMachine.SwitchState(new SwingState(stateMachine,activeRopePosition,ropeRigidBody));
    }
}
