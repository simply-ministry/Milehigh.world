using UnityEngine;

public class WalkingState : AnimationState
{
    private readonly Animator _animator;
    private const string _walkingAnimationName = "Walking";

    public WalkingState(Animator animator)
    {
        _animator = animator;
    }

    public override void Enter()
    {
        Debug.Log("Entering Walking State");
        _animator.Play(_walkingAnimationName);
    }

    public override void Update()
    {
        // In this simple version, the state doesn't need to do anything every frame.
        // The transition logic will be handled by the controller.
    }

    public override void Exit()
    {
        Debug.Log("Exiting Walking State");
        // No specific exit behavior needed for this state.
    }
}
