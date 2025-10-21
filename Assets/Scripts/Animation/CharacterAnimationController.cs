using UnityEngine;

/// <summary>
/// Controls character animations by managing a state machine.
/// It transitions between idle and walking states based on player input.
/// </summary>
public class CharacterAnimationController : MonoBehaviour
{
    private Animator _animator;
    private AnimationStateMachine _stateMachine;

    // States
    private IdleState _idleState;
    private WalkingState _walkingState;

    // Input tracking
    private bool _isMoving = false;

    /// <summary>
    /// Initializes the component, getting references and setting up the state machine.
    /// </summary>
    void Awake()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator component not found on this GameObject!");
            enabled = false;
            return;
        }

        _stateMachine = GetComponent<AnimationStateMachine>();
        if (_stateMachine == null)
        {
            _stateMachine = gameObject.AddComponent<AnimationStateMachine>();
        }

        _idleState = new IdleState(_animator);
        _walkingState = new WalkingState(_animator);

        _stateMachine.Initialize(_idleState);
    }

    /// <summary>
    /// Called every frame. Checks for movement input and handles state transitions.
    /// </summary>
    void Update()
    {
        CheckForMovementInput();

        if (_isMoving && _stateMachine.CurrentState != _walkingState)
        {
            _stateMachine.ChangeState(_walkingState);
        }
        else if (!_isMoving && _stateMachine.CurrentState != _idleState)
        {
            _stateMachine.ChangeState(_idleState);
        }
    }

    /// <summary>
    /// Checks for player input to determine if the character is moving.
    /// </summary>
    private void CheckForMovementInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        _isMoving = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;
    }
}
