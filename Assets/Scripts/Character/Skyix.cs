using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents Sky.ix, the Bionic Goddess. Transformed by the Void, she is a key
/// figure in the prophecy, destined to either save or irrevocably change the Verse.
/// </summary>
public class Skyix : Novamina
{
    // --- Core Character & Narrative Properties ---

    /// <summary>
    /// Her original name before being trapped and transformed by the Void.
    /// </summary>
    public string OriginalName { get; private set; } = "Skye";

    /// <summary>
    /// Represents her current state of being, which can change at key narrative points
    /// and may affect which abilities are available.
    /// </summary>
    public CharacterState CurrentState { get; private set; }

    /// <summary>
    /// A list of entities she has created through her unique powers.
    /// In a live game, these would be GameObjects. For now, they are data objects.
    /// </summary>
    public List<Character> CreatedBeings { get; private set; }

    /// <summary>
    /// A collection tracking her significant relationships with other characters.
    /// The Key is the character's unique ID, and the Value is a string describing the relationship (e.g., "Son", "Rival").
    /// </summary>
    public Dictionary<Guid, string> Relationships { get; private set; }

    // --- Resource and Combat Properties ---

    /// <summary>
    /// The current amount of Void Energy Sky.ix has. This resource is used for her special abilities.
    /// </summary>
    public float VoidEnergy { get; set; }

    /// <summary>
    /// The maximum amount of Void Energy Sky.ix can hold.
    /// </summary>
    public float MaxVoidEnergy { get; set; }

    /// <summary>
    /// The number of active drones currently deployed by Sky.ix.
    /// </summary>
    public int ActiveDrones { get; private set; }


    // --- Initialization ---

    protected override void Awake()
    {
        base.Awake(); // Call Novamina -> Character Awake()

        // Set base character properties from the new logic
        characterName = "Sky.ix the Bionic Goddess";
        maxHealth = 100;
        currentHealth = maxHealth;

        // Set Novamina-specific properties
        Archetype = "Ranged DPS / Support Caster";

        // Set Sky.ix-specific properties
        MaxVoidEnergy = 150f;
        VoidEnergy = MaxVoidEnergy;

        // In the context of the game's story, she begins as having already been transformed.
        CurrentState = CharacterState.VoidTransformed;

        // Initialize collections
        CreatedBeings = new List<Character>();
        Relationships = new Dictionary<Guid, string>();

        // Represent her creation of Omega.one upon initialization
        // Note: This creates the C# object, but doesn't spawn it in the scene.
        CreatedBeings.Add(this.CreateOmegaOne());
    }

    // --- Narrative and Relational Methods ---

    /// <summary>
    /// A narrative method representing the creation of Omega.one. In a real game,
    /// this would instantiate the character as a GameObject and add it to the world.
    /// </summary>
    private OmegaOne CreateOmegaOne()
    {
        Debug.Log($"[NARRATIVE] {characterName} channels her power and brings Omega.one into existence!");
        // In a real implementation, we would instantiate a prefab.
        // For now, we create the component class and set its creator.
        var omegaGO = new GameObject("OmegaOne_Instance");
        omegaGO.transform.SetParent(this.transform); // Child the new object to Sky.ix
        var omega = omegaGO.AddComponent<OmegaOne>();
        omega.CreatorId = this.CharacterId;
        return omega;
    }

    /// <summary>
    /// A method to dynamically establish a relationship during gameplay.
    /// </summary>
    /// <param name="character">The character to form a relationship with.</param>
    /// <param name="description">A description of the relationship (e.g., "Son", "Initial Antagonist").</param>
    public void AddRelationship(Character character, string description)
    {
        if (!Relationships.ContainsKey(character.CharacterId))
        {
            Relationships.Add(character.CharacterId, description);
            Debug.Log($"[RELATIONSHIP] {characterName} now recognizes {character.characterName} as: {description}.");
        }
    }


    // --- Combat Abilities ---

    /// <summary>
    /// Uses her unique connection to the Void, blending it with technology for a powerful attack.
    /// This ability is only available in her VoidTransformed state.
    /// </summary>
    /// <param name="target">The Character to target with the ability.</param>
    public void UseVoidTech(Character target)
    {
        if (CurrentState == CharacterState.VoidTransformed && VoidEnergy >= 40)
        {
            VoidEnergy -= 40;
            Debug.Log($"{characterName} harnesses the abyss, striking {target.characterName} with raw Void Tech!");
            // ... damage calculation logic would go here, likely calling TakeDamage() on the target.
        }
        else
        {
            Debug.Log($"{characterName} cannot currently access the power of Void Tech.");
        }
    }

    /// <summary>
    /// Deploys a combat or utility drone to the battlefield.
    /// </summary>
    /// <param name="droneType">The type of drone to deploy (e.g., "Attack", "Shield").</param>
    public void DeployDrone(string droneType)
    {
        Debug.Log($"{characterName} deploys a {droneType} drone to the battlefield!");
        ActiveDrones++;
        // ... logic to create a drone GameObject ...
    }

    // New property for ability cost
    private const int BlinkVoidEnergyCost = 30;

    /// <summary>
    /// Skyix's signature mobility and evasion tool.
    /// Instantly teleports Skyix up to a maximum distance or to a targeted enemy.
    /// Grants a brief period of Invisibility and increased Evasion after the blink.
    /// </summary>
    /// <param name="targetPosition">The desired position to blink to.</param>
    public void VoidBlink(Vector3 targetPosition)
    {
        bool isVoidTransformed = CurrentState == CharacterState.VoidTransformed;
        int finalEnergyCost = isVoidTransformed ? (int)(BlinkVoidEnergyCost * 0.75f) : BlinkVoidEnergyCost;
        float evasionDuration = isVoidTransformed ? 2.5f : 1.5f;

        if (VoidEnergy < finalEnergyCost)
        {
            Debug.Log($"[Skyix] Fails to cast Void Blink. Not enough Void Energy (Cost: {finalEnergyCost}).");
            return;
        }

        // Subtract Void Energy
        VoidEnergy -= finalEnergyCost;

        // Placeholder logic for ability effect
        Debug.Log(
            $"[Skyix] **Void Blink** executed! Teleporting to target location. Grants {evasionDuration}s of Evasion. " +
            (isVoidTransformed ? "(Void-Enhanced: Lower cost, longer duration!)" : "") +
            $" Void Energy remaining: {VoidEnergy}."
        );

        // In a full implementation:
        // 1. Calculate the actual destination based on targetPosition and max range.
        // 2. Set the character's transform.position to the destination.
        // 3. Apply a temporary 'EvasionBuff' component.
    }


    // --- Ultimate/Destiny Ability ---

    /// <summary>
    /// A placeholder for the ultimate narrative ability where Sky.ix merges with Era.
    /// This represents the fulfillment of her destiny to restore the broken verse and achieve Millenia.
    /// </summary>
    /// <param name="era">The personification of the Void to unify with.</param>
    public void UnifyWithVoid(Era era)
    {
        Debug.Log($"[DESTINY] {characterName} and {era.characterName} begin to merge, a blinding light of order and chaos... The verse is being restored!");

        // This action fundamentally changes the character.
        this.CurrentState = CharacterState.UnifiedEntity;
        this.characterName = "Sky.ix-Era, The Unifying Force";

        // This could unlock a whole new set of abilities, change stats, and complete her character arc.
        Debug.Log($"Her form has changed. She has transcended and is now {this.characterName}.");
    }
}