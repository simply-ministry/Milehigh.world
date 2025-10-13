using UnityEngine;
using UnityEngine.Events; // Required for UnityEvent

// We'll assume the base class looks something like this for context.
// You do not need to add this file, it's just for explanation.
/*
public abstract class ShadowSyndicateVillain : MonoBehaviour
{
    public string villainName;
    public float maxHealth;
    public float currentHealth;
    protected AudioSource audioSource;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public virtual void UsePrimaryAbility(GameObject target) { }
    public virtual void TakeDamage(float amount) { }
    protected virtual void Die() { }
}
*/


public class Cyrus : ShadowSyndicateVillain
{
    [Header("Primary Ability: Chaotic Torrent")]
    public GameObject chaoticBeamVFX; // Prefab for the beam/projectile
    public AudioClip chaoticBeamSFX;
    public float primaryAbilityDamage = 25f;
    [SerializeField] private float primaryAbilityCooldown = 5f;
    private float _primaryCooldownTimer;

    [Header("Secondary Ability: Void Shield")]
    public GameObject voidShieldVFX; // Prefab for the shield effect
    public AudioClip voidShieldSFX;
    public float shieldDuration = 8f;
    [SerializeField] private float secondaryAbilityCooldown = 20f;
    private float _secondaryCooldownTimer;
    private bool _isShielded = false;

    [Header("Ultimate Ability: Dimensional Rift")]
    public GameObject dimensionalRiftVFX; // Prefab for the damaging zone
    public AudioClip dimensionalRiftSFX;
    public float ultimateDamagePerSecond = 15f;
    public float ultimateDuration = 10f;
    [SerializeField] private float ultimateAbilityCooldown = 45f;
    private float _ultimateCooldownTimer;

    [Header("Health & State")]
    public GameObject hitVFX;
    public GameObject deathVFX;
    public AudioClip hurtSound;
    public AudioClip deathSound;
    public UnityEvent OnDeath; // Event to trigger on death
    private bool _isEnraged = false;
    private bool _isDead = false;

    [Header("AI & Targeting")]
    [SerializeField] private float attackRange = 20f;
    [SerializeField] private float abilityDecisionRate = 1.0f; // How often to check for ability usage
    private Transform _playerTarget;
    private float _decisionTimer;

    protected override void Awake()
    {
        base.Awake();
        villainName = "Cyrus";
        maxHealth = 400;
        currentHealth = maxHealth;

        // Initialize timers so abilities aren't used immediately
        _primaryCooldownTimer = primaryAbilityCooldown / 2;
        _secondaryCooldownTimer = secondaryAbilityCooldown;
        _ultimateCooldownTimer = ultimateAbilityCooldown;

        // Find the player target (in a real game, you might use a more robust system)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTarget = player.transform;
        }
    }

    void Update()
    {
        if (_isDead || _playerTarget == null) return;

        // Tick down all cooldowns
        _primaryCooldownTimer -= Time.deltaTime;
        _secondaryCooldownTimer -= Time.deltaTime;
        _ultimateCooldownTimer -= Time.deltaTime;
        _decisionTimer -= Time.deltaTime;

        // Simple AI Logic: Periodically decide what to do
        if (_decisionTimer <= 0)
        {
            DecideNextAction();
            _decisionTimer = abilityDecisionRate;
        }
    }

    private void DecideNextAction()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _playerTarget.position);

        if (distanceToPlayer <= attackRange)
        {
            // AI Priority: Use ultimate if available, then shield, then primary.
            if (_isEnraged && _ultimateCooldownTimer <= 0)
            {
                UseUltimateAbility(_playerTarget.gameObject);
            }
            else if (currentHealth < maxHealth * 0.75f && !_isShielded && _secondaryCooldownTimer <= 0)
            {
                UseSecondaryAbility();
            }
            else if (_primaryCooldownTimer <= 0)
            {
                UsePrimaryAbility(_playerTarget.gameObject);
            }
        }
    }

    /// <summary>
    /// Unleashes a torrent of chaotic energy.
    /// </summary>
    public override void UsePrimaryAbility(GameObject target)
    {
        base.UsePrimaryAbility(target);
        Debug.Log($"{villainName} unleashes a torrent of chaotic energy at {target.name}!");

        transform.LookAt(target.transform);

        // Instantiate VFX and play sound
        if (chaoticBeamVFX) Instantiate(chaoticBeamVFX, transform.position + transform.forward * 2, transform.rotation);
        if (chaoticBeamSFX) audioSource.PlayOneShot(chaoticBeamSFX);

        // Deal damage to the target
        // Assumes target has a component like 'PlayerHealth' with a TakeDamage method.
        target.GetComponent<IHealth>()?.TakeDamage(primaryAbilityDamage);

        // Reset cooldown
        _primaryCooldownTimer = primaryAbilityCooldown;
    }

    /// <summary>
    /// Creates a temporary shield to absorb damage.
    /// </summary>
    public void UseSecondaryAbility()
    {
        Debug.Log($"{villainName} summons a Void Shield!");
        _isShielded = true;

        if (voidShieldVFX)
        {
            GameObject shieldInstance = Instantiate(voidShieldVFX, transform.position, transform.rotation, transform);
            Destroy(shieldInstance, shieldDuration); // The shield VFX disappears after its duration
        }
        if (voidShieldSFX) audioSource.PlayOneShot(voidShieldSFX);

        _secondaryCooldownTimer = secondaryAbilityCooldown;
        Invoke(nameof(DeactivateShield), shieldDuration);
    }

    private void DeactivateShield()
    {
        _isShielded = false;
        Debug.Log($"{villainName}'s Void Shield fades.");
    }

    /// <summary>
    /// Tears open a damaging rift in reality at the target's location.
    /// Unlocked when enraged.
    /// </summary>
    public void UseUltimateAbility(GameObject target)
    {
        Debug.Log($"{villainName} tears open a Dimensional Rift!");

        if (dimensionalRiftVFX)
        {
            // Create the rift at the player's position
            GameObject riftInstance = Instantiate(dimensionalRiftVFX, target.transform.position, Quaternion.identity);
            // TODO: The rift's own script would handle dealing damage over time to players inside it.
            Destroy(riftInstance, ultimateDuration);
        }
        if (dimensionalRiftSFX) audioSource.PlayOneShot(dimensionalRiftSFX);

        _ultimateCooldownTimer = ultimateAbilityCooldown;
    }

    public override void TakeDamage(float amount)
    {
        if (_isDead) return;

        // Reduce damage if shielded
        if (_isShielded)
        {
            amount *= 0.5f; // Shield absorbs 50% of damage
            Debug.Log("Void Shield absorbs part of the attack!");
        }

        currentHealth -= amount;

        if (hitVFX) Instantiate(hitVFX, transform.position, Quaternion.identity);
        if (hurtSound) audioSource.PlayOneShot(hurtSound);

        Debug.Log($"{villainName} takes {amount} damage. Health: {currentHealth}/{maxHealth}");

        // Check for enrage state transition
        if (!_isEnraged && currentHealth <= maxHealth * 0.5f)
        {
            EnterEnrageState();
        }

        // Check for death
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void EnterEnrageState()
    {
        _isEnraged = true;
        Debug.Log($"{villainName} becomes ENRAGED! Abilities are faster and he can now use his ultimate!");

        // Example: Make him glow red
        GetComponent<Renderer>().material.color = Color.red;

        // Reduce cooldowns
        primaryAbilityCooldown /= 1.5f;
        secondaryAbilityCooldown /= 1.2f;
    }

    protected override void Die()
    {
        if (_isDead) return;
        _isDead = true;

        Debug.Log($"{villainName} has been defeated!");

        if (deathVFX) Instantiate(deathVFX, transform.position, Quaternion.identity);
        if (deathSound) audioSource.PlayOneShot(deathSound);

        // Trigger the death event for other systems to listen to
        OnDeath?.Invoke();

        // Destroy the GameObject after a delay to allow effects to play
        Destroy(gameObject, 3f);
    }
}