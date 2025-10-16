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
        // No update logic needed for idle state
    }

    public override void Exit()
    {
        Debug.Log("Exiting Idle State");
    }
}
        // No specific exit behavior needed for this state.
    }
}
