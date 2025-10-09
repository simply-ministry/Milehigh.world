using UnityEngine;

/// <summary>
/// A simple AI that attacks a random player character.
/// </summary>
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

    public void TakeTurn()
    {
        // A real AI would have more complex logic. For now, we'll just attack.
        PlayerCharacter target = FindObjectOfType<PlayerCharacter>(); // Assuming a PlayerCharacter script/tag
        if (target != null && target.isAlive)
        {
            Debug.Log($"{self.characterName} decides to attack {target.characterName}!");
            // In a full implementation, the AI would use its own abilities.
            // For now, a basic attack will suffice.
            // self.Attack(target);
        }
    }
}