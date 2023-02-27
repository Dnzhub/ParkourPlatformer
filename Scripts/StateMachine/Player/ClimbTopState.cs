using UnityEngine;

public class ClimbTopState : PlayerBaseState
{
    [Header("--- Animation Hash Codes ---")]
    private readonly int ClimbOnTopHash = Animator.StringToHash("ClimbToTop");
    private float animationSmoothTime = 0.1f;
    private float functionDelayValue;
    private float animationPlayTime = 4f;
    public ClimbTopState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        functionDelayValue = 0;
        stateMachine.Controller.enabled = false;
        stateMachine.Animator.applyRootMotion = true;
        stateMachine.Animator.CrossFadeInFixedTime(ClimbOnTopHash, animationSmoothTime);
      
     
    }
    public override void Tick(float deltaTime)
    {
        functionDelayValue += deltaTime;
        //stateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime sometimes cause bug between animation thats why we give here animation time manually
        if (functionDelayValue> animationPlayTime)
        {
            stateMachine.SwitchState(new MovementState(stateMachine));
        }
    }
    public override void FixedTick(float fixedDeltaTime)
    {
    }
    public override void Exit()
    {
        stateMachine.Animator.applyRootMotion = false;
        stateMachine.Controller.Move(Vector3.zero);
        stateMachine.Gravity.ResetGravity();    
    }   
}
