using UnityEngine;

public class LadderClimbStartState : PlayerBaseState
{
    [Header("--- Animation Hash Codes ---")]
    private readonly int ClimbStartSpeedHash = Animator.StringToHash("ClimbStart");
    private float animationTransitionTime = 0.1f;

    [Tooltip("Timer for prevent from race condition.For better result you shouldnt give this value less then 0.1 sec")]
    private float functionDelayValue;
    private float functionDelayMaxTime;

    [Tooltip("Player rotation will be lock on ladder forward direction when climb start")]
    private Vector3 ladderForward;
    private bool climbStart;
    public LadderClimbStartState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        functionDelayMaxTime = 1;
        functionDelayValue = 0;
        climbStart = true;
        LockRotationToTargetDirection();
        stateMachine.Animator.CrossFadeInFixedTime(ClimbStartSpeedHash, animationTransitionTime);
        stateMachine.Controller.enabled = false;       
    }
    public override void Tick(float deltaTime)
    {
        functionDelayValue += deltaTime;

        if (climbStart)
        {
            stateMachine.transform.position = Vector3.Lerp(stateMachine.transform.position, stateMachine.LadderTransition.position, 5f * deltaTime);
        }
        //Move player a bit upward so animation looks smoother
        
        
        //Player will take position on ladder before passing to climb state
        if (functionDelayValue > functionDelayMaxTime)
        {
            climbStart = false;
            stateMachine.SwitchState(new ClimbState(stateMachine));
        }
        
    }
    public override void FixedTick(float fixedDeltaTime)
    {
    }
    public override void Exit()
    {
        stateMachine.Controller.enabled = true;
    }

   
    private void LockRotationToTargetDirection()
    {
        ladderForward = stateMachine.LadderTransition.forward;
        stateMachine.transform.rotation = Quaternion.LookRotation(ladderForward, Vector3.up);
    }
}
