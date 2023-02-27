using UnityEngine;

public class LandState : PlayerBaseState
{
    [Header("--- Animation Hash Codes ---")]
    private readonly int LandHash = Animator.StringToHash("Landing");
    private float animationSmoothTime = 0.1f;

    private Vector3 jumpDirection;
    public LandState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.RopeDetector.OnRope += HandleOnRope;
        stateMachine.LedgeDetection.OnLedgeDetect += ClimbToLedge;

        jumpDirection = stateMachine.Controller.velocity;
        jumpDirection.y = 0;

        stateMachine.Animator.CrossFadeInFixedTime(LandHash, animationSmoothTime);
    }
    public override void Tick(float deltaTime)
    {
        Move(jumpDirection, deltaTime);
        
        if (stateMachine.Controller.isGrounded)
        {
            stateMachine.SwitchState(new MovementState(stateMachine));
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
    private void HandleOnRope(Transform activeRopePosition,Rigidbody ropeRigidBody)
    {
       
        stateMachine.SwitchState(new SwingState(stateMachine,activeRopePosition,ropeRigidBody));
    }
    private void ClimbToLedge()
    {
        stateMachine.SwitchState(new ClimbTopState(stateMachine));
    }


}
