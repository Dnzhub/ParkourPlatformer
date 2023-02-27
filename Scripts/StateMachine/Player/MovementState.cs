using UnityEngine;

public class MovementState : PlayerBaseState
{
    [Header("--- Animation Hash Codes ---")]
    private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    private readonly int LocomotionSpeedHash = Animator.StringToHash("LocomotionSpeed");
    private const float AnimatorSmoothTime = 0.1f;

    public MovementState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.InputReader.JumpEvent += OnJump;

        ApplyStateValues();
    }
    public override void Tick(float deltaTime)
    {
        
        if (stateMachine.Ladder != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                stateMachine.SwitchState(new LadderClimbStartState(stateMachine));                   
            }
        }

        Vector3 movement = MovementCalculation(); 
        //Move function will be center on PlayerBaseState so all states have same gravity and force values
        Move(movement * stateMachine.MovementSpeed, deltaTime);
        
        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(LocomotionSpeedHash, 0, AnimatorSmoothTime, deltaTime);
            return;
        }

        stateMachine.Animator.SetFloat(LocomotionSpeedHash, 1, AnimatorSmoothTime, deltaTime);
        DirectionCalculation(movement,deltaTime);
        
    }
    public override void FixedTick(float fixedDeltaTime)
    {
    }
    public override void Exit()
    {
        stateMachine.InputReader.JumpEvent -= OnJump;
    
    }
  
    private void ApplyStateValues()
    {
        stateMachine.Controller.enabled = true;
        stateMachine.Animator.Play(LocomotionHash);
        stateMachine.Controller.Move(Vector3.zero);
        stateMachine.SetTargetNull();
    }
    private void OnJump()
    {
        stateMachine.SwitchState(new JumpState(stateMachine));
    }
    private void DirectionCalculation(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, Quaternion.LookRotation(movement), deltaTime * stateMachine.RotationSmoothTime); 
    }
    private Vector3 MovementCalculation()
    {
        Vector3 cameraForward = stateMachine.MainCamera.forward;
        Vector3 cameraRight = stateMachine.MainCamera.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        return cameraForward * stateMachine.InputReader.MovementValue.y + cameraRight * stateMachine.InputReader.MovementValue.x;
      

    }
}
