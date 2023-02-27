using UnityEngine;

public class ClimbState : PlayerBaseState
{
    [Header("--- Animation Hash Codes ---")]
    private readonly int ClimbBlendTreeHash = Animator.StringToHash("ClimbingBlendTree");
    private readonly int ClimbSpeedHash = Animator.StringToHash("ClimbSpeed");

    private bool isClimbing = false;

    [Tooltip("Check ApplyForce function if it already add force we should stop it otherwise it can be endless force in some situtations")]
    private bool didApplyForce = false;

    [Tooltip("force value if player decide to jump while climbing")]
    private float force = 5;

  
    public ClimbState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.LedgeDetection.OnLedgeDetect += ClimbToLedge;
        stateMachine.InputReader.JumpEvent += OnJump;
        isClimbing = true;        
        
        //Reseting climb start target position so if we want to put it more place, they dont overlap with eachother
        stateMachine.SetTargetNull();
       
        stateMachine.Controller.enabled = true;
        
        //Play climb blend tree
        stateMachine.Animator.Play(ClimbBlendTreeHash);
    }
    public override void Tick(float deltaTime)
    {
 
        if (isClimbing)
        {                
            //If moving down 
            if (stateMachine.InputReader.MovementValue.y <= 0)
            {   
                //If player collide with target at climb start position
                if (stateMachine.Ladder != null)
                {
                    isClimbing = false;

                    stateMachine.SwitchState(new MovementState(stateMachine));
                }
            }
        }

        ClimbOnLadder(deltaTime);

    }
    public override void FixedTick(float fixedDeltaTime)
    {
    }
    public override void Exit()
    {

        stateMachine.LedgeDetection.OnLedgeDetect -= ClimbToLedge;
        stateMachine.InputReader.JumpEvent -= OnJump;
        isClimbing = false;
        stateMachine.SetTargetNull();
       
    }
    private void ClimbOnLadder(float deltaTime)
    {
        stateMachine.Animator.Play(ClimbBlendTreeHash);
        float upwardMovement = stateMachine.InputReader.MovementValue.y;
        Vector3 movementDirection = new Vector3(0, upwardMovement, 0);
        stateMachine.Controller.Move(movementDirection * deltaTime * 2f);
        stateMachine.Gravity.ResetGravity();
        UpdateAnimation();
    }
    private void OnJump()
    {
        if (isClimbing)
        {
            ApplyForce();
            stateMachine.SwitchState(new LandState(stateMachine));
        }
    }
    /// <summary>
    /// If player press jump button leave the ladder and apply force to it
    /// </summary>
    private void ApplyForce()
    {
        if (didApplyForce) return;
        stateMachine.Gravity.AddForce(-stateMachine.transform.forward * force);
        didApplyForce = true;
    }
    private void ClimbToLedge()
    {
        isClimbing = false;
        stateMachine.SwitchState(new ClimbTopState(stateMachine));
    }
    private void UpdateAnimation()
    {
        stateMachine.Animator.SetFloat(ClimbSpeedHash, stateMachine.InputReader.MovementValue.y);

    }
   
}
