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

    [Header("AI Settings")]
    public float moveSpeed = 3.0f;
    public float turnSpeed = 5.0f;
    public float attackRange = 2.0f;
    public float chaseRange = 15.0f;
    public float obstacleAvoidanceDistance = 3.0f;
    public LayerMask obstacleLayers;

    private AbilitySystem abilitySystem;
    private Vector3 velocity = Vector3.zero;

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
                // Smoothly stop the character when idle
                velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime * turnSpeed);
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
                    // Move towards the player with obstacle avoidance
                    Vector3 targetDirection = (playerTarget.transform.position - transform.position).normalized;
                    Vector3 moveDirection = GetSteeringDirection(targetDirection);

                    Vector3 desiredVelocity = moveDirection * moveSpeed;
                    velocity = Vector3.Lerp(velocity, desiredVelocity, Time.deltaTime * turnSpeed);

                    transform.position += velocity * Time.deltaTime;

                    if (velocity.sqrMagnitude > 0.01f)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(velocity.normalized);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                    }
                }
                break;

            case AIState.Attacking:
                // Stop moving when attacking
                velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime * turnSpeed);
                transform.position += velocity * Time.deltaTime; // Apply deceleration

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

    /// <summary>
    /// Calculates a steering direction to avoid obstacles.
    /// </summary>
    /// <param name="targetDirection">The initial desired direction.</param>
    /// <returns>A new direction vector that avoids obstacles.</returns>
    private Vector3 GetSteeringDirection(Vector3 targetDirection)
    {
        // Check for obstacles directly ahead
        if (Physics.Raycast(transform.position, targetDirection, obstacleAvoidanceDistance, obstacleLayers))
        {
            // If the path is blocked, try to find a clear path by checking left and right "whiskers"
            float whiskerAngle = 30f; // Angle of the whisker rays

            // Check right whisker
            Vector3 rightWhiskerDir = Quaternion.Euler(0, whiskerAngle, 0) * targetDirection;
            if (!Physics.Raycast(transform.position, rightWhiskerDir, obstacleAvoidanceDistance, obstacleLayers))
            {
                return rightWhiskerDir;
            }

            // Check left whisker
            Vector3 leftWhiskerDir = Quaternion.Euler(0, -whiskerAngle, 0) * targetDirection;
            if (!Physics.Raycast(transform.position, leftWhiskerDir, obstacleAvoidanceDistance, obstacleLayers))
            {
                return leftWhiskerDir;
            }

            // If all paths are blocked, the AI can stop or reverse, here we just stop moving forward in this direction
            return Vector3.zero;
        }

        // If the path is clear, continue in the target direction
        return targetDirection;
    }
}