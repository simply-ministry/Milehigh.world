using UnityEngine;

public class AnimationStateMachine : MonoBehaviour
{
    public AnimationState CurrentState { get; private set; }

    public void Initialize(AnimationState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(AnimationState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();
        }
    }
}
