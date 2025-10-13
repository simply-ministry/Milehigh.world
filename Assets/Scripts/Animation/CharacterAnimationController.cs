using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    private Animator _animator;
    private AnimationStateMachine _stateMachine;

    // States
    private IdleState _idleState;
    private WalkingState _walkingState; // Add a variable for the walking state

    // Input tracking
    private bool _isMoving = false;

    void Awake()
    {
        // Get the Animator component attached to this GameObject
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator component not found on this GameObject!");
            enabled = false;
            return;
        }

        // Get or add the AnimationStateMachine component
        _stateMachine = GetComponent<AnimationStateMachine>();
        if (_stateMachine == null)
        {
            _stateMachine = gameObject.AddComponent<AnimationStateMachine>();
        }

        // Create instances of the states, passing in the Animator
        _idleState = new IdleState(_animator);
        _walkingState = new WalkingState(_animator); // Create an instance of the WalkingState

        // Initialize the state machine with the IdleState as the starting state
        _stateMachine.Initialize(_idleState);
    }

    void Update()
    {
        // Check for player input to determine movement
        CheckForMovementInput();

        // State transition logic
        if (_isMoving && _stateMachine.CurrentState != _walkingState)
        {
            _stateMachine.ChangeState(_walkingState);
        }
        else if (!_isMoving && _stateMachine.CurrentState != _idleState)
        {
            _stateMachine.ChangeState(_idleState);
        }
    }

    private void CheckForMovementInput()
    {
        // A simple check for horizontal or vertical input (e.g., WASD or arrow keys)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            _isMoving = true;
        }
        else
        {
            _isMoving = false;
        }
    }
}