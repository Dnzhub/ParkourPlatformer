using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SwingState : PlayerBaseState
{
    [Header("--- Animation Hash Codes ---")]
    private readonly int HangingHash = Animator.StringToHash("Hanging");
    private readonly int LandHash = Animator.StringToHash("Landing");
    private float animationTransitionTime = 0.1f;

    [Header("--- Rope Attributes ---")]
    private Transform activeRopePosition;
    private Transform ropeParentObject;

    [Header("--- State Attributes ---")]
    private float lastInputValue;
    private Rigidbody ropeRigidBody;
    private bool isSwinging;

    [Tooltip("Timer for prevent from race condition.For better result you shouldnt give this value less then 0.1 sec")]
    private float functionDelayValue;
    private float functionDelayMaxTime;
    
    public SwingState(PlayerStateMachine stateMachine, Transform ropePosition, Rigidbody ropeRigidBody) : base(stateMachine)
    {
        this.ropeRigidBody = ropeRigidBody;
        ropeParentObject = ropeRigidBody.gameObject.transform.parent;
        activeRopePosition = ropePosition;
    }

    public override void Enter()
    {
        functionDelayMaxTime = .1f;
        functionDelayValue = 0;
        if (IsLookingRight())
        {
            lastInputValue = 90;
        }
        else
        {
            lastInputValue = -90;
        }

        SetStateBooleans();
        stateMachine.Animator.CrossFadeInFixedTime(HangingHash, animationTransitionTime);
     

    }
   
    public override void Tick(float deltaTime)
    {
        
        functionDelayValue += deltaTime;
      
        if (functionDelayValue > .1f)
        {
            if (IsLanded())
            {
                stateMachine.SwitchState(new MovementState(stateMachine));
            }
            //If character on rope
            if (isSwinging)
            {          
                stateMachine.transform.position = activeRopePosition.position;
                stateMachine.transform.rotation = ropeRigidBody.transform.rotation;

                PlayAnimationRig(deltaTime);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    DropRope();
                }
            }                       
           
            //Reset gravity so character dont fly on rope accidently
            stateMachine.Gravity.ResetGravity();
        }

    
    }
    public override void FixedTick(float fixedDeltaTime)
    {
        if (isSwinging)
        {
            CalculateSwingDirection(fixedDeltaTime);
        }

    }
    /// <summary>
    /// If character drop the rope it will launch itself to forward and play fall animation
    /// </summary>
    private void DropRope()
    {
        isSwinging = false;
        stateMachine.PlayerRigidBody.velocity = new Vector3(ropeRigidBody.velocity.x, ropeRigidBody.velocity.y + 5, ropeRigidBody.velocity.z);
        stateMachine.PlayerRigidBody.useGravity = true;
        stateMachine.Animator.CrossFadeInFixedTime(LandHash, animationTransitionTime);
        stateMachine.transform.rotation = Quaternion.Euler(0, lastInputValue, 0);
        stateMachine.ForwardLegRig.weight = 0;
    }
    /// <summary>
    /// Check isgrounded manually since character controller disabled
    /// </summary>
    /// <returns></returns>
    private bool IsLanded()
    {
        return Physics.Raycast(stateMachine.transform.position, Vector3.down, out RaycastHit floorHit, 0.2f);
    }

    /// <summary>
    /// Character leg rig will be play according to movement point
    /// </summary>
    /// <param name="deltaTime"></param>
    private void PlayAnimationRig(float deltaTime)
    {
        //Facing Right
        if (IsLookingRight())
        {   //Ýf going right
            if (stateMachine.InputReader.MovementValue.x > 0)
            {
                EnableIKConstraints(stateMachine.ForwardLegRig, 1f, deltaTime);
            }
            //if going left or idle
            else if (stateMachine.InputReader.MovementValue.x <= 0)
            {
                DisableIKConstraints(stateMachine.ForwardLegRig, 1f, deltaTime);
            }
        }
        //Facing Left
        else
        {
            //Ýf going left
            if (stateMachine.InputReader.MovementValue.x < 0)
            {
                EnableIKConstraints(stateMachine.ForwardLegRig, 1f, deltaTime);
            }

            else if (stateMachine.InputReader.MovementValue.x >= 0)
            {
                DisableIKConstraints(stateMachine.ForwardLegRig, 1f, deltaTime);
            }
        }
    }


    /// <summary>
    /// Enable to IK bones smoothly
    /// </summary>
    /// <param name="targetRigToEnable : Those legs will move forward"></param>
    /// <param name="SmoothTime"></param>
    /// <param name="deltaTime"></param>
    private void EnableIKConstraints(Rig targetRigToEnable,float SmoothTime,float deltaTime)
    {
        targetRigToEnable.weight = Mathf.MoveTowards(targetRigToEnable.weight,
                         1, SmoothTime * deltaTime);
        
    }


    /// <summary>
    /// Disable to IK bones smoothly
    /// </summary>
    /// <param name="targetRigToDisable : Those legs will move orginal position smoothly"></param>
    /// <param name="SmoothTime"></param>
    /// <param name="deltaTime"></param>
    private void DisableIKConstraints(Rig targetRigToDisable, float SmoothTime, float deltaTime)
    {
        targetRigToDisable.weight = Mathf.MoveTowards(targetRigToDisable.weight,
                       0, SmoothTime * deltaTime);
    }


    
    private void SetStateBooleans()
    {
        isSwinging = true;
        stateMachine.Controller.enabled = false;
        stateMachine.PlayerRigidBody.isKinematic = false;
        stateMachine.PlayerRigidBody.useGravity = false;
    }

    /// <summary>
    /// This function will help to determine IK movement direction
    /// </summary>
    /// <returns></returns>
    public bool IsLookingRight()
    {
        if (stateMachine.transform.rotation.y > 0) return true;
        return false;
    }

    /// <summary>
    /// Add force on movement direction while swinging
    /// </summary>
    /// <param name="deltaTime"></param>
    public void CalculateSwingDirection(float deltaTime)
    {
        if(stateMachine.InputReader.MovementValue.x != 0 && isSwinging)
        {            
            ropeRigidBody.AddForce(new Vector3(stateMachine.InputReader.MovementValue.x, 0,0) * 25 * deltaTime);           
        }  
    }
    public override void Exit()
    {
        stateMachine.Controller.enabled = true;
        stateMachine.PlayerRigidBody.isKinematic = true;
        isSwinging = false;
        stateMachine.PlayerRigidBody.useGravity = false;



    }
 
}
