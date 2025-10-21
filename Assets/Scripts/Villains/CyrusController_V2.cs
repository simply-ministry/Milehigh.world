using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// [Feedback 1] Assuming an IHealth interface exists and is implemented by the player.
// Note from Jules: IHealth is now in Assets/Scripts/Core/Interfaces/IHealth.cs
// public interface IHealth
// {
//     void TakeDamage(float amount);
//     float CurrentHealth { get; }
//     float MaxHealth { get; }
// }

public class CyrusController_V2 : MonoBehaviour, IHealth
{
    [Header("Core Stats")]
    [SerializeField] private float maxHealth = 2000f;
    [SerializeField] private float currentHealth;

    [Header("Targeting")]
    [SerializeField] private Transform target; // Typically the player

    [Header("Primary Ability: Void Bolt")]
    [SerializeField] private float primaryAbilityDamage = 50f;
    [SerializeField] private float primaryAbilityCooldown = 5f;
    [SerializeField] private GameObject voidBoltVFX;
    private float _primaryCooldownTimer;

    [Header("Secondary Ability: Defensive Shield")]
    [SerializeField] private float shieldDuration = 8f;
    [SerializeField] private float secondaryAbilityCooldown = 20f;
    [SerializeField] private GameObject shieldVFX;
    private float _secondaryCooldownTimer;
    private bool _isShielded = false;
    private GameObject _activeShieldInstance;

    [Header("Ultimate Ability: Dimensional Rift")]
    [SerializeField] private float ultimateAbilityDamage = 150f;
    [SerializeField] private GameObject dimensionalRiftVFX; // Should have its own damage script
    private bool _hasUsedUltimate = false;

    [Header("Enrage Phase")]
    [SerializeField] private float enrageHealthThreshold = 0.4f; // Enrage at 40% health
    [SerializeField] private Color enrageColor = Color.red;
    private bool _isEnraged = false;

    [Header("Events")]
    public UnityEvent OnTakeDamage;
    public UnityEvent OnEnrage;
    public UnityEvent OnDeath;

    void Start()
    {
        currentHealth = maxHealth;
        _primaryCooldownTimer = primaryAbilityCooldown / 2f; // Start ready to attack
        _secondaryCooldownTimer = 0f;
    }

    void Update()
    {
        if (currentHealth <= 0 || target == null) return;

        HandleCooldowns();
        DecideNextAction();
    }

    private void HandleCooldowns()
    {
        if (_primaryCooldownTimer > 0) _primaryCooldownTimer -= Time.deltaTime;
        if (_secondaryCooldownTimer > 0) _secondaryCooldownTimer -= Time.deltaTime;
    }

    private void DecideNextAction()
    {
        // Ultimate is a one-time, high-priority move when health is critical
        if (!_hasUsedUltimate && currentHealth / maxHealth <= 0.2f)
        {
            UseUltimateAbility();
            return;
        }

        // Defensive shield is a high priority if available and not already active
        if (_secondaryCooldownTimer <= 0 && !_isShielded)
        {
            StartCoroutine(UseSecondaryAbility());
            return;
        }

        // Default to primary attack if it's off cooldown
        if (_primaryCooldownTimer <= 0)
        {
            // [Feedback 9] Use a coroutine to telegraph the attack
            StartCoroutine(TelegraphedPrimaryAttack(0.5f));
        }
    }

    // --- Abilities ---

    private IEnumerator TelegraphedPrimaryAttack(float windUpTime)
    {
        _primaryCooldownTimer = primaryAbilityCooldown;
        Debug.Log("Cyrus is charging Void Bolt...");

        // Optional: Play a "charging" animation or sound here.

        yield return new WaitForSeconds(windUpTime);

        Debug.Log("Cyrus fires Void Bolt!");
        if (voidBoltVFX != null)
        {
            Instantiate(voidBoltVFX, target.position, Quaternion.identity);
        }
        target.GetComponent<IHealth>()?.TakeDamage(primaryAbilityDamage);
    }

    private IEnumerator UseSecondaryAbility()
    {
        _secondaryCooldownTimer = secondaryAbilityCooldown;
        _isShielded = true;
        Debug.Log("Cyrus activates his defensive shield!");

        if (shieldVFX != null)
        {
            // [Feedback 4] Parent the shield to maintain position
            _activeShieldInstance = Instantiate(shieldVFX, transform.position, transform.rotation, transform);
        }

        yield return new WaitForSeconds(shieldDuration);

        _isShielded = false;
        if (_activeShieldInstance != null)
        {
            Destroy(_activeShieldInstance);
        }
        Debug.Log("Cyrus's shield fades.");
    }



    private void UseUltimateAbility()
    {
        _hasUsedUltimate = true;
        Debug.Log("Cyrus unleashes a Dimensional Rift!");

        // [Feedback 6] Spawn the rift. The rift's own script will handle damage over time.
        if (dimensionalRiftVFX != null)
        {
            Instantiate(dimensionalRiftVFX, target.position, Quaternion.identity);
        }
    }

    // --- Health and State Management ---

    public void TakeDamage(float amount)
    {
        if (_isShielded)
        {
            Debug.Log("Cyrus's shield absorbed the damage!");
            return;
        }
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        OnTakeDamage?.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
        else if (!_isEnraged && (currentHealth / maxHealth) <= enrageHealthThreshold)
        {
            EnterEnragePhase();
        }
    }

    private void EnterEnragePhase()
    {
        _isEnraged = true;
        Debug.Log("Cyrus has become ENRAGED!");

        // --- IMPROVEMENTS from feedback ---
        // [Feedback 2] Iterate through all child renderers for complex models.
        foreach (var rend in GetComponentsInChildren<Renderer>())
        {
            foreach (var mat in rend.materials)
            {
                mat.color = enrageColor; // Or use mat.SetColor("_EmissionColor", enrageColor); for emissive shaders
            }
        }

        // Boost stats
        primaryAbilityCooldown /= 1.5f;
        secondaryAbilityCooldown /= 1.2f;

        // [Feedback 3] Crucial fix: Clamp existing timers to the new, shorter cooldowns.
        _primaryCooldownTimer = Mathf.Min(_primaryCooldownTimer, primaryAbilityCooldown);
        _secondaryCooldownTimer = Mathf.Min(_secondaryCooldownTimer, secondaryAbilityCooldown);
        // --- End of Improvements ---

        OnEnrage?.Invoke();
    }

    private void Die()
    {
        Debug.Log("Cyrus has been defeated.");
        OnDeath?.Invoke();
        // [Feedback 5] Delay destruction to allow death animations/VFX to play.
        Destroy(gameObject, 3f);
    }

    // IHealth properties for compatibility
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
}