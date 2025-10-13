using UnityEngine;

public class IdleState : AnimationState
{
    private readonly Animator _animator;
    private const string _idleAnimationName = "Idle";

    public IdleState(Animator animator)
    {
        _animator = animator;
    }

    public override void Enter()
    {
        Debug.Log("Entering Idle State");
        _animator.Play(_idleAnimationName);
    }

    public override void Update()
    {
        // The transition logic will be handled by the controller.
    }

    public override void Exit()
    {
        Debug.Log("Exiting Idle State");
    }
}
