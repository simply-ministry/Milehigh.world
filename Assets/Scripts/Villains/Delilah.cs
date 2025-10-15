using UnityEngine;

public class Delilah : ShadowSyndicateVillain
{
    [TextArea(2, 4)]
    public string dialogue = "You cannot stop the inevitable.";

    protected override void Awake()
    {
        base.Awake();
        villainName = "Delilah the Desolate";
        maxHealth = 600;
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Casts a spell that fills the heroes with despair.
    /// </summary>
    public void CastDespair()
    {
        currentState = VillainAIState.Casting;
        Debug.Log($"{villainName}: '{dialogue}'");
        Debug.Log($"{villainName} casts a wave of despair!");
        // TODO: Apply a debuff (e.g., attack power down) to all heroes in an area.
    }
}