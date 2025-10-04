using UnityEngine;

public class Ingris : Character
{
    public int combo = 0;
    private float phoenixFireCooldown = 1f;
    private float lastPhoenixFireTime;
    private float shadowStrikeCooldown = 4f;
    private float lastShadowStrikeTime;
    private float rebirthCooldown = 30f;
    private float lastRebirthTime;
    private float comboDecayRate = 1f; // Combo lost per second
    private float lastComboUpdateTime;
    private bool isAlive = true;

    public bool IsAlive { get => isAlive; private set => isAlive = value; }

    protected override void Awake()
    {
        base.Awake();
        characterName = "Ingris";
        maxHealth = 100;
        currentHealth = maxHealth;
        attack = 75;
        defense = 50;
        speed = 80f;

        lastPhoenixFireTime = -phoenixFireCooldown;
        lastShadowStrikeTime = -shadowStrikeCooldown;
        lastRebirthTime = -rebirthCooldown;
        lastComboUpdateTime = Time.time;
    }

    private void Update()
    {
        DecayCombo();
    }

    private void DecayCombo()
    {
        if (combo > 0 && Time.time >= lastComboUpdateTime + 1f)
        {
            float timeSinceUpdate = Time.time - lastComboUpdateTime;
            int comboLoss = Mathf.FloorToInt(comboDecayRate * timeSinceUpdate);
            if (comboLoss > 0)
            {
                combo = Mathf.Max(0, combo - comboLoss);
                lastComboUpdateTime = Time.time;
            }
        }
    }

    public void PhoenixFire(Character target)
    {
        if (Time.time >= lastPhoenixFireTime + phoenixFireCooldown)
        {
            int damage = attack + combo * 5; // Combo bonus
            target.TakeDamage(damage);
            Debug.Log($"{characterName} strikes with Phoenix Fire for {damage} damage!");
            combo = Mathf.Min(combo + 1, 5);
            Debug.Log($"{characterName}'s combo increases!");
            lastPhoenixFireTime = Time.time;
            lastComboUpdateTime = Time.time; // Reset combo decay timer
        }
    }

    public void ShadowStrike(Character target)
    {
        if (Time.time >= lastShadowStrikeTime + shadowStrikeCooldown)
        {
            if (combo >= 3)
            {
                int damage = attack * 2; // High damage
                target.TakeDamage(damage);
                Debug.Log($"{characterName} teleports and uses Shadow Strike for {damage} damage!");
                combo = 0; // Reset combo
                lastShadowStrikeTime = Time.time;
                lastComboUpdateTime = Time.time; // Reset combo decay timer
            }
            else
            {
                Debug.Log($"{characterName} needs more combo to use this!");
            }
        }
    }

    public void Rebirth()
    {
        if (Time.time >= lastRebirthTime + rebirthCooldown)
        {
            if (!IsAlive)
            {
                IsAlive = true;
                gameObject.SetActive(true); // Reactivate the GameObject
                currentHealth = maxHealth / 2; // Revive with half health
                Debug.Log($"{characterName} is reborn!");
                lastRebirthTime = Time.time;
            }
            else
            {
                Debug.Log($"{characterName} cannot use this while alive!");
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        lastComboUpdateTime = Time.time; // Taking damage resets combo decay timer
    }

    protected override void Die()
    {
        Debug.Log($"{characterName} has fallen!");
        IsAlive = false;
        // Deactivates the GameObject, allowing for potential revival.
        gameObject.SetActive(false);
    }
}