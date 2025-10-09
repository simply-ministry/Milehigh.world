using UnityEngine;

[RequireComponent(typeof(Character))]
public class AIController : MonoBehaviour
{
    private Character self;
    private Character playerTarget;

    private enum AIState { Idle, Chasing, Attacking }
    private AIState currentState = AIState.Idle;

    public float attackRange = 2.0f;
    public float chaseRange = 15.0f;
    private AbilitySystem abilitySystem;

    void Awake()
    {
        self = GetComponent<Character>();
        abilitySystem = GetComponent<AbilitySystem>();
        // Find the player target once at the start
        // In a real game, this would be more dynamic
        playerTarget = FindObjectOfType<PlayerController>()?.GetComponent<Character>();
    }

    void Update()
    {
        if (playerTarget == null) return;
        if (!self.isAlive || !playerTarget.isAlive) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.transform.position);

        // --- State Machine Logic ---
        switch (currentState)
        {
            case AIState.Idle:
                if (distanceToPlayer <= chaseRange)
                {
                    currentState = AIState.Chasing;
                }
                break;

            case AIState.Chasing:
                if (distanceToPlayer <= attackRange)
                {
                    currentState = AIState.Attacking;
                }
                else if (distanceToPlayer > chaseRange)
                {
                    currentState = AIState.Idle;
                }
                else
                {
                    // Move towards the player
                    // self.MoveTowards(playerTarget.transform.position);
                }
                break;

            case AIState.Attacking:
                if (distanceToPlayer > attackRange)
                {
                    currentState = AIState.Chasing;
                }
                else
                {
                    // Use an ability if available
                    abilitySystem.UseAbility(0, playerTarget);
                }
                break;
        }
    }
}