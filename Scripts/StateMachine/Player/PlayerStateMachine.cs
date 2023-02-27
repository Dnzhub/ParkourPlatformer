using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerStateMachine : StateMachine
{
    #region Character References

    public InputReader InputReader { get; private set; }
    public CharacterController Controller { get; private set; }
    public Rigidbody PlayerRigidBody { get; private set; }
    public GravityHandler Gravity { get; private set; }
    public Animator Animator { get; private set; }
    public Transform MainCamera { get; private set; }
    public RopeDetector RopeDetector { get; private set; }

    #endregion

    #region Climb and Ladder system
    [field: SerializeField] public LedgeDetector LedgeDetection { get; private set; }
    public Transform Ladder { get; private set; }
    public Transform LadderTransition { get; private set; }

    #endregion

    #region Movement
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float RotationSmoothTime { get; private set; }
    [field: SerializeField] public float JumpForce { get; private set; }
    #endregion

    #region Rigging
    [field: SerializeField] public Rig ForwardLegRig { get; private set; }
    #endregion


    private void Awake()
    {
        
        InputReader = GetComponent<InputReader>();
        Controller = GetComponent<CharacterController>();
        PlayerRigidBody = GetComponent<Rigidbody>();
        RopeDetector = GetComponent<RopeDetector>();
        Gravity = GetComponent<GravityHandler>();
        Animator = GetComponent<Animator>();
        MainCamera = Camera.main.transform;
      
    }
    private void Start()
    {
        SwitchState(new MovementState(this));
    }

    private void OnTriggerEnter(Collider other)
    {
        //Ladder Trigger
        if (other.gameObject.TryGetComponent(out LadderGO ladderGO))
        {
            Ladder = ladderGO.transform;
            LadderTransition = ladderGO.TransitionPosition;          
        }
    }
    public void SetTargetNull()
    {
        Ladder = null;
        LadderTransition = null;
    }

}
