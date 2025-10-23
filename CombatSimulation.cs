
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilehighWorld.CombatSimulation
{
    // ==========================================================================
    // STATIC COMBAT LOGGER
    // Fulfills rule: "Provide clear descriptions of fight events."
    // ==========================================================================
    /// <summary>
    /// Provides static methods for logging combat events to the console.
    /// </summary>
    public static class CombatLog
    {
        /// <summary>
        /// Logs a generic message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Log(string message)
        {
            Console.WriteLine($"[LOG] {message}");
        }

        /// <summary>
        /// Logs the current elapsed time in the simulation.
        /// </summary>
        /// <param name="time">The elapsed time in seconds.</param>
        public static void LogTime(float time)
        {
            Console.WriteLine($"\n----- TIME {time:F1}s -----");
        }

        /// <summary>
        /// Logs an action performed by a character.
        /// </summary>
        /// <param name="characterName">The name of the character performing the action.</param>
        /// <param name="actionDescription">A description of the action.</param>
        public static void LogAction(string characterName, string actionDescription)
        {
            Console.WriteLine($" > {characterName} {actionDescription}.");
        }

        /// <summary>
        /// Logs damage taken by a character.
        /// </summary>
        /// <param name="targetName">The name of the character taking damage.</param>
        /// <param name="damage">The amount of damage taken.</param>
        /// <param name="type">The type of damage.</param>
        public static void LogDamage(string targetName, int damage, DamageType type)
        {
            Console.WriteLine($" ! {targetName} takes {damage} {type} damage.");
        }

        /// <summary>
        /// Logs healing received by a character.
        /// </summary>
        /// <param name="targetName">The name of the character being healed.</param>
        /// <param name="amount">The amount of health restored.</param>
        public static void LogHeal(string targetName, int amount)
        {
            Console.WriteLine($" + {targetName} heals for {amount} health.");
        }

        /// <summary>
        /// Logs a status effect change for a character.
        /// </summary>
        /// <param name="targetName">The name of the character affected.</param>
        /// <param name="status">The description of the status change.</param>
        public static void LogStatus(string targetName, string status)
        {
            Console.WriteLine($" * {targetName} {status}.");
        }

        /// <summary>
        /// Logs the defeat of a character.
        /// </summary>
        /// <param name="characterName">The name of the defeated character.</param>
        public static void LogDeath(string characterName)
        {
            Console.WriteLine($" X {characterName} has been defeated!");
        }

        /// <summary>
        /// Logs the final outcome of the battle.
        /// </summary>
        /// <param name="message">The outcome message.</param>
        public static void LogOutcome(string message)
        {
            Console.WriteLine("\n=============================");
            Console.WriteLine($"OUTCOME: {message}");
            Console.WriteLine("=============================");
        }

        /// <summary>
        /// Logs the current health status of all combatants.
        /// </summary>
        /// <param name="combatants">The list of combatants in the fight.</param>
        public static void LogHealthStatus(List<ICombatant> combatants)
        {
            Console.WriteLine("--- Health Status ---");
            foreach (var c in combatants)
            {
                Console.WriteLine($" {c.Name} ({c.Affiliation}): {c.Health} / {c.MaxHealth} HP");
            }
        }
    }

    // --- Aeron's Abilities ---
    /// <summary>
    /// Aeron's signature ability that damages a target and has a chance to stun.
    /// </summary>
    public class ChronoShift : IAbility
    {
        /// <inheritdoc/>
        public string Name => "Chrono Shift";
        /// <inheritdoc/>
        public string Description => "Damages a target and has a 30% chance to stun them for 1 second.";
        /// <inheritdoc/>
        public float Cooldown => 4.0f;
        /// <inheritdoc/>
        public float CurrentCooldown { get; set; }

        /// <inheritdoc/>
        public void Execute(ICombatant caster, List<ICombatant> allCombatants)
        {
            CombatLog.LogAction(caster.Name, "uses Chrono Shift");
            var target = allCombatants
                .Where(c => c.Affiliation != caster.Affiliation && c.IsAlive)
                .OrderBy(c => Guid.NewGuid()) // Random target
                .FirstOrDefault();

            if (target != null)
            {
                target.TakeDamage(50, DamageType.Void);
                if (BaseCharacter.Rng.NextDouble() < 0.3)
                {
                    if (target is BaseCharacter baseCharacter)
                    {
                        baseCharacter.ApplyStun(1.0f);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Represents Aeron, the Timeless. A versatile hero for the Novaminad faction.
    /// </summary>
    public class AeronTheTimeless : BaseCharacter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AeronTheTimeless"/> class.
        /// </summary>
        public AeronTheTimeless() : base("Aeron the Timeless", 250, CharacterAffiliation.Novaminad)
        {
            Abilities.Add(new ChronoShift());
        }

        /// <inheritdoc/>
        public override void PerformAction(List<ICombatant> allCombatants)
        {
            if (ActionTimer > 0) return; // Busy

            var chronoShift = Abilities.First(a => a.Name == "Chrono Shift");
            const float GlobalCooldown = 1.8f;

            // AI: Use Chrono Shift whenever it's available.
            if (chronoShift.CurrentCooldown == 0)
            {
                UseAbility(chronoShift, allCombatants);
                ActionTimer = GlobalCooldown;
            }
            // AI: If the ability is on cooldown, do nothing and wait for the next opportunity.
        }
    }

    // ==========================================================================
    // ENUMS
    // ==========================================================================
    /// <summary>
    /// Defines the factions a character can belong to.
    /// </summary>
    public enum CharacterAffiliation
    {
        /// <summary>The player-aligned faction.</summary>
        Novaminad,
        /// <summary>The enemy faction.</summary>
        ShadowDominion
    }

    /// <summary>
    /// Defines the types of damage that can be dealt.
    /// </summary>
    public enum DamageType
    {
        /// <summary>Standard physical damage.</summary>
        Physical,
        /// <summary>Damage from the void.</summary>
        Void,
        /// <summary>Damage from shadow magic.</summary>
        Shadow,
        /// <summary>Damage over time or corrosive effects.</summary>
        Decay,
        /// <summary>Pure energy damage.</summary>
        Energy,
        /// <summary>Holy or radiant damage.</summary>
        Holy
    }

    // ==========================================================================
    // INTERFACES
    // ==========================================================================

    /// <summary>
    /// Represents a single ability a character can use.
    /// </summary>
    public interface IAbility
    {
        /// <summary>Gets the name of the ability.</summary>
        string Name { get; }
        /// <summary>Gets the description of what the ability does.</summary>
        string Description { get; }
        /// <summary>Gets the base cooldown time in seconds.</summary>
        float Cooldown { get; }
        /// <summary>Gets or sets the remaining cooldown time in seconds.</summary>
        float CurrentCooldown { get; set; }

        /// <summary>
        /// Executes the ability's logic.
        /// </summary>
        /// <param name="caster">The character using the ability.</param>
        /// <param name="allCombatants">The list of all combatants in the fight for targeting purposes.</param>
        void Execute(ICombatant caster, List<ICombatant> allCombatants);
    }

    /// <summary>
    /// Represents any entity that can participate in combat.
    /// </summary>
    public interface ICombatant
    {
        /// <summary>Gets the name of the combatant.</summary>
        string Name { get; }
        /// <summary>Gets the current health of the combatant.</summary>
        int Health { get; }
        /// <summary>Gets the maximum health of the combatant.</summary>
        int MaxHealth { get; }
        /// <summary>Gets the faction affiliation of the combatant.</summary>
        CharacterAffiliation Affiliation { get; }
        /// <summary>Gets a value indicating whether the combatant is still alive.</summary>
        bool IsAlive { get; }
        /// <summary>Gets the list of abilities the combatant possesses.</summary>
        List<IAbility> Abilities { get; }
        /// <summary>Gets the remaining time in seconds before the combatant can perform another action.</summary>
        float ActionTimer { get; }

        /// <summary>
        /// Executes the combatant's AI logic for a single action phase.
        /// </summary>
        /// <param name="allCombatants">The list of all combatants in the fight.</param>
        void PerformAction(List<ICombatant> allCombatants);

        /// <summary>
        /// Updates the combatant's state over time, ticking down cooldowns and effects.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last update, in seconds.</param>
        void Update(float deltaTime);

        /// <summary>
        /// Applies damage to the combatant.
        /// </summary>
        /// <param name="amount">The base amount of damage to apply.</param>
        /// <param name="type">The type of damage.</param>
        void TakeDamage(int amount, DamageType type);

        /// <summary>
        /// Heals the combatant for a specified amount.
        /// </summary>
        /// <param name="amount">The amount of health to restore.</param>
        void Heal(int amount);
    }

    // ==========================================================================
    // BASE CHARACTER IMPLEMENTATION
    // ==========================================================================
    /// <summary>
    /// An abstract base class providing common functionality for all combatants.
    /// </summary>
    public abstract class BaseCharacter : ICombatant
    {
        /// <inheritdoc/>
        public string Name { get; protected set; }
        /// <inheritdoc/>
        public int Health { get; protected set; }
        /// <inheritdoc/>
        public int MaxHealth { get; protected set; }
        /// <inheritdoc/>
        public CharacterAffiliation Affiliation { get; protected set; }
        /// <inheritdoc/>
        public bool IsAlive => Health > 0;
        /// <inheritdoc/>
        public List<IAbility> Abilities { get; protected set; }
        /// <inheritdoc/>
        public float ActionTimer { get; protected set; }

        // Buffs/Debuffs
        /// <summary>Amount of damage to reduce from incoming attacks.</summary>
        protected int DamageReduction { get; set; }
        /// <summary>Remaining duration for the damage reduction buff.</summary>
        protected float DamageReductionDuration { get; set; }

        /// <summary>A shared random number generator for all characters.</summary>
        public static Random Rng = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCharacter"/> class.
        /// </summary>
        /// <param name="name">The name of the character.</param>
        /// <param name="health">The maximum health of the character.</param>
        /// <param name="affiliation">The faction the character belongs to.</param>
        protected BaseCharacter(string name, int health, CharacterAffiliation affiliation)
        {
            Name = name;
            MaxHealth = health;
            Health = health;
            Affiliation = affiliation;
            Abilities = new List<IAbility>();
        }

        /// <inheritdoc/>
        public void TakeDamage(int amount, DamageType type)
        {
            if (!IsAlive) return;

            int actualDamage = amount - DamageReduction;
            if (actualDamage < 0) actualDamage = 0;

            if (DamageReduction > 0)
            {
                CombatLog.LogStatus(Name, $"blocks {DamageReduction} damage");
            }

            Health -= actualDamage;
            CombatLog.LogDamage(Name, actualDamage, type);

            if (Health <= 0)
            {
                Health = 0;
                CombatLog.LogDeath(Name);
            }
        }

        /// <inheritdoc/>
        public void Heal(int amount)
        {
            if (!IsAlive) return;

            Health += amount;
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
            CombatLog.LogHeal(Name, amount);
        }

        /// <summary>
        /// Applies a stun effect by increasing the action timer.
        /// </summary>
        /// <param name="duration">The duration of the stun in seconds.</param>
        public void ApplyStun(float duration)
        {
            if (IsAlive)
            {
                ActionTimer += duration;
                CombatLog.LogStatus(Name, $"is stunned for {duration:F1}s!");
            }
        }

        /// <inheritdoc/>
        public virtual void Update(float deltaTime)
        {
            // Tick Action Timer
            if (ActionTimer > 0)
            {
                ActionTimer -= deltaTime;
                if (ActionTimer < 0) ActionTimer = 0;
            }

            // Tick ability cooldowns
            foreach (var ability in Abilities)
            {
                if (ability.CurrentCooldown > 0)
                {
                    ability.CurrentCooldown -= deltaTime;
                    if (ability.CurrentCooldown < 0) ability.CurrentCooldown = 0;
                }
            }

            // Tick buffs
            if (DamageReductionDuration > 0)
            {
                DamageReductionDuration -= deltaTime;
                if (DamageReductionDuration <= 0)
                {
                    DamageReductionDuration = 0;
                    DamageReduction = 0;
                    CombatLog.LogStatus(Name, "'s damage reduction wears off");
                }
            }
        }

        /// <inheritdoc/>
        public abstract void PerformAction(List<ICombatant> allCombatants);

        // --- Helper Methods for AI ---

        /// <summary>
        /// Gets a random, living target from a specific faction.
        /// </summary>
        /// <param name="allCombatants">The list of all combatants.</param>
        /// <param name="targetAffiliation">The faction to target.</param>
        /// <returns>A random combatant, or null if no valid targets exist.</returns>
        protected ICombatant GetRandomTarget(List<ICombatant> allCombatants, CharacterAffiliation targetAffiliation)
        {
            var possibleTargets = allCombatants
                .Where(c => c.Affiliation == targetAffiliation && c.IsAlive)
                .ToList();

            if (!possibleTargets.Any()) return null;

            return possibleTargets[Rng.Next(possibleTargets.Count)];
        }

        /// <summary>
        /// Gets the allied combatant with the lowest health percentage.
        /// </summary>
        /// <param name="allCombatants">The list of all combatants.</param>
        /// <returns>The most wounded ally, or null if none exist.</returns>
        protected ICombatant GetLowestHealthAlly(List<ICombatant> allCombatants)
        {
            return allCombatants
                .Where(c => c.Affiliation == this.Affiliation && c.IsAlive)
                .OrderBy(c => (double)c.Health / c.MaxHealth)
                .FirstOrDefault();
        }

        /// <summary>
        /// Executes an ability and puts it on cooldown.
        /// </summary>
        /// <param name="ability">The ability to use.</param>
        /// <param name="allCombatants">The list of all combatants for targeting.</param>
        protected void UseAbility(IAbility ability, List<ICombatant> allCombatants)
        {
            if (ability.CurrentCooldown > 0)
            {
                // Should not happen if AI is correct, but as a fallback
                CombatLog.LogAction(Name, $"tries to use {ability.Name}, but it's on cooldown");
                return;
            }

            ability.Execute(this, allCombatants);
            ability.CurrentCooldown = ability.Cooldown;
        }
    }

    // ==========================================================================
    // PROTAGONIST (NOVAMINAD) CLASSES
    // ==========================================================================

    /// <summary>
    /// Represents Micah, the Unbreakable. A tank character for the Novaminad faction.
    /// </summary>
    public class MicahTheUnbreakable : BaseCharacter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MicahTheUnbreakable"/> class.
        /// </summary>
        public MicahTheUnbreakable() : base("Micah the Unbreakable", 350, CharacterAffiliation.Novaminad)
        {
            Abilities.Add(new SeismicSlam());
            Abilities.Add(new AdamantineSkin());
        }

        /// <inheritdoc/>
        public override void PerformAction(List<ICombatant> allCombatants)
        {
            if (ActionTimer > 0) return; // Busy

            var skin = Abilities.First(a => a.Name == "Adamantine Skin");
            var slam = Abilities.First(a => a.Name == "Seismic Slam");
            const float GlobalCooldown = 1.5f;

            // AI: If buff is down and ability is ready, use it.
            if (DamageReductionDuration <= 0 && skin.CurrentCooldown == 0)
            {
                UseAbility(skin, allCombatants);
                ActionTimer = GlobalCooldown;
            }
            // AI: Otherwise, if slam is ready, use it.
            else if (slam.CurrentCooldown == 0)
            {
                UseAbility(slam, allCombatants);
                ActionTimer = GlobalCooldown;
            }
            // AI: Basic attack fallback
            else
            {
                // CombatLog.LogAction(Name, "braces for impact and prepares his next move"); // Too spammy for real-time
            }
        }

        /// <summary>
        /// Applies the Adamantine Skin buff, providing damage reduction.
        /// </summary>
        /// <param name="reduction">The amount of damage to reduce.</param>
        /// <param name="duration">The duration of the buff in seconds.</param>
        public void ApplyAdamantineSkin(int reduction, float duration)
        {
            this.DamageReduction = reduction;
            this.DamageReductionDuration = duration;
            CombatLog.LogStatus(Name, $"activates Adamantine Skin, gaining {reduction} damage reduction");
        }
    }

    /// <summary>
    /// Represents Anastasia, the Dreamer. A healer for the Novaminad faction.
    /// </summary>
    public class AnastasiaTheDreamer : BaseCharacter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnastasiaTheDreamer"/> class.
        /// </summary>
        public AnastasiaTheDreamer() : base("Anastasia the Dreamer", 200, CharacterAffiliation.Novaminad)
        {
            Abilities.Add(new EtherealEmbrace());
        }

        /// <inheritdoc/>
        public override void PerformAction(List<ICombatant> allCombatants)
        {
            if (ActionTimer > 0) return; // Busy

            var heal = Abilities.First(a => a.Name == "Ethereal Embrace");
            const float GlobalCooldown = 1.5f;

            // AI: Find the most wounded ally.
            var target = GetLowestHealthAlly(allCombatants);

            // AI: Only heal if they are below 80% health and heal is ready.
            if (target != null && ((double)target.Health / target.MaxHealth < 0.8) && heal.CurrentCooldown == 0)
            {
                UseAbility(heal, allCombatants);
                ActionTimer = GlobalCooldown;
            }
            else
            {
                // CombatLog.LogAction(Name, "weaves protective wards, waiting for an opening"); // Too spammy for real-time
            }
        }
    }

    /// <summary>
    /// Represents Sky.ix, the Bionic Goddess. A damage dealer for the Novaminad faction.
    /// </summary>
    public class SkyixTheBionicGoddess : BaseCharacter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkyixTheBionicGoddess"/> class.
        /// </summary>
        public SkyixTheBionicGoddess() : base("Sky.ix", 220, CharacterAffiliation.Novaminad)
        {
            Abilities.Add(new EnergyBlast());
        }

        /// <inheritdoc/>
        public override void PerformAction(List<ICombatant> allCombatants)
        {
            if (ActionTimer > 0) return; // Busy

            var blast = Abilities.First(a => a.Name == "Energy Blast");
            const float AttackSpeed = 2.0f; // Acts every 2 seconds

            // AI: Always attack if ability is ready.
            if (blast.CurrentCooldown == 0)
            {
                UseAbility(blast, allCombatants);
                ActionTimer = AttackSpeed;
            }
            else
            {
                // CombatLog.LogAction(Name, "channels Void Tech energy"); // Too spammy for real-time
            }
        }
    }

    // ==========================================================================
    // ANTAGONIST (SHADOW DOMINION) CLASSES
    // ==========================================================================

    /// <summary>
    /// Represents Delilah, the Desolate. A damage dealer for the Shadow Dominion faction.
    /// </summary>
    public class DelilahTheDesolate : BaseCharacter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelilahTheDesolate"/> class.
        /// </summary>
        public DelilahTheDesolate() : base("Delilah the Desolate", 240, CharacterAffiliation.ShadowDominion)
        {
            Abilities.Add(new CorruptedPhoenixFire());
        }

        /// <inheritdoc/>
        public override void PerformAction(List<ICombatant> allCombatants)
        {
            if (ActionTimer > 0) return; // Busy

            var fire = Abilities.First(a => a.Name == "Corrupted Phoenix Fire");
            const float AttackSpeed = 2.2f; // Acts every 2.2 seconds

            // AI: Always attack if ability is ready.
            if (fire.CurrentCooldown == 0)
            {
                UseAbility(fire, allCombatants);
                ActionTimer = AttackSpeed;
            }
            else
            {
                // CombatLog.LogAction(Name, "gathers decaying energy"); // Too spammy for real-time
            }
        }
    }

    /// <summary>
    /// Represents Nefarious, the Sovereign. A damage dealer for the Shadow Dominion faction.
    /// </summary>
    public class NefariousTheSovereign : BaseCharacter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NefariousTheSovereign"/> class.
        /// </summary>
        public NefariousTheSovereign() : base("Nefarious the Sovereign", 280, CharacterAffiliation.ShadowDominion)
        {
            Abilities.Add(new ShadowDominionGrasp());
        }

        /// <inheritdoc/>
        public override void PerformAction(List<ICombatant> allCombatants)
        {
            if (ActionTimer > 0) return; // Busy

            var grasp = Abilities.First(a => a.Name == "Shadow Dominion Grasp");
            const float AttackSpeed = 2.5f; // Acts every 2.5 seconds

            // AI: Always attack if ability is ready.
            if (grasp.CurrentCooldown == 0)
            {
                UseAbility(grasp, allCombatants);
                ActionTimer = AttackSpeed;
            }
            else
            {
                // CombatLog.LogAction(Name, "draws shadows from the Dominion"); // Too spammy for real-time
            }
        }
    }

    // ==========================================================================
    // ABILITY IMPLEMENTATIONS
    // ==========================================================================

    // --- Micah's Abilities ---
    /// <summary>
    /// Micah's area-of-effect damage ability.
    /// </summary>
    public class SeismicSlam : IAbility
    {
        /// <inheritdoc/>
        public string Name => "Seismic Slam";
        /// <inheritdoc/>
        public string Description => "Slams the ground, dealing Physical damage to all enemies.";
        /// <inheritdoc/>
        public float Cooldown => 3.0f;
        /// <inheritdoc/>
        public float CurrentCooldown { get; set; }

        /// <inheritdoc/>
        public void Execute(ICombatant caster, List<ICombatant> allCombatants)
        {
            CombatLog.LogAction(caster.Name, "uses Seismic Slam");
            var targets = allCombatants.Where(c => c.Affiliation != caster.Affiliation && c.IsAlive);
            foreach (var target in targets)
            {
                target.TakeDamage(40, DamageType.Physical);
            }
        }
    }

    /// <summary>
    /// Micah's defensive buff ability.
    /// </summary>
    public class AdamantineSkin : IAbility
    {
        /// <inheritdoc/>
        public string Name => "Adamantine Skin";
        /// <inheritdoc/>
        public string Description => "Harden's skin, granting 25 damage reduction for 2 turns.";
        /// <inheritdoc/>
        public float Cooldown => 4.0f;
        /// <inheritdoc/>
        public float CurrentCooldown { get; set; }

        /// <inheritdoc/>
        public void Execute(ICombatant caster, List<ICombatant> allCombatants)
        {
            CombatLog.LogAction(caster.Name, "uses Adamantine Skin");
            if (caster is MicahTheUnbreakable micah)
            {
                micah.ApplyAdamantineSkin(25, 3.0f); // 3 seconds
            }
        }
    }

    // --- Anastasia's Abilities ---
    /// <summary>
    /// Anastasia's single-target healing ability.
    /// </summary>
    public class EtherealEmbrace : IAbility
    {
        /// <inheritdoc/>
        public string Name => "Ethereal Embrace";
        /// <inheritdoc/>
        public string Description => "A support spell that heals the most wounded ally.";
        /// <inheritdoc/>
        public float Cooldown => 2.0f;
        /// <inheritdoc/>
        public float CurrentCooldown { get; set; }

        /// <inheritdoc/>
        public void Execute(ICombatant caster, List<ICombatant> allCombatants)
        {
            CombatLog.LogAction(caster.Name, "casts Ethereal Embrace");
            var target = allCombatants
                .Where(c => c.Affiliation == caster.Affiliation && c.IsAlive)
                .OrderBy(c => (double)c.Health / c.MaxHealth)
                .FirstOrDefault();

            if (target != null)
            {
                target.Heal(75);
            }
        }
    }

    // --- Sky.ix's Abilities ---
    /// <summary>
    /// Sky.ix's primary single-target damage ability.
    /// </summary>
    public class EnergyBlast : IAbility
    {
        /// <inheritdoc/>
        public string Name => "Energy Blast";
        /// <inheritdoc/>
        public string Description => "Fires a blast of pure energy at an enemy.";
        /// <inheritdoc/>
        public float Cooldown => 0.0f; // No cooldown, limited by ActionTimer
        /// <inheritdoc/>
        public float CurrentCooldown { get; set; }

        /// <inheritdoc/>
        public void Execute(ICombatant caster, List<ICombatant> allCombatants)
        {
            CombatLog.LogAction(caster.Name, "unleashes an Energy Blast");
            var target = allCombatants
                .Where(c => c.Affiliation != caster.Affiliation && c.IsAlive)
                .OrderBy(c => c.Health) // Target lowest health
                .FirstOrDefault();

            if (target != null)
            {
                target.TakeDamage(60, DamageType.Energy);
            }
        }
    }

    // --- Delilah's Abilities ---
    /// <summary>
    /// Delilah's primary single-target damage ability.
    /// </summary>
    public class CorruptedPhoenixFire : IAbility
    {
        /// <inheritdoc/>
        public string Name => "Corrupted Phoenix Fire";
        /// <inheritdoc/>
        public string Description => "Lashes out with green-black flames of decay.";
        /// <inheritdoc/>
        public float Cooldown => 0.0f; // No cooldown, limited by ActionTimer
        /// <inheritdoc/>
        public float CurrentCooldown { get; set; }

        /// <inheritdoc/>
        public void Execute(ICombatant caster, List<ICombatant> allCombatants)
        {
            CombatLog.LogAction(caster.Name, "unleashes Corrupted Phoenix Fire");
            var target = allCombatants
                .Where(c => c.Affiliation != caster.Affiliation && c.IsAlive)
                .OrderBy(c => Guid.NewGuid()) // Random target
                .FirstOrDefault();

            if (target != null)
            {
                target.TakeDamage(65, DamageType.Decay);
            }
        }
    }

    // --- Nefarious's Abilities ---
    /// <summary>
    /// Nefarious's primary single-target damage ability.
    /// </summary>
    public class ShadowDominionGrasp : IAbility
    {
        /// <inheritdoc/>
        public string Name => "Shadow Dominion Grasp";
        /// <inheritdoc/>
        public string Description => "Crushes a target with pure shadow energy.";
        /// <inheritdoc/>
        public float Cooldown => 0.0f; // No cooldown, limited by ActionTimer
        /// <inheritdoc/>
        public float CurrentCooldown { get; set; }

        /// <inheritdoc/>
        public void Execute(ICombatant caster, List<ICombatant> allCombatants)
        {
            CombatLog.LogAction(caster.Name, "uses Shadow Dominion Grasp");
            var target = allCombatants
                .Where(c => c.Affiliation != caster.Affiliation && c.IsAlive)
                .OrderBy(c => Guid.NewGuid()) // Random target
                .FirstOrDefault();

            if (target != null)
            {
                // Target the tank if available
                var tank = allCombatants.FirstOrDefault(c => c.Name == "Micah the Unbreakable" && c.IsAlive);
                if (tank != null)
                {
                    target = tank;
                }

                target.TakeDamage(55, DamageType.Shadow);
            }
        }
    }


    // ==========================================================================
    // SIMULATION ENGINE
    // Fulfills rules: "Implement real time combat logic," "Determine the outcome"
    // ==========================================================================
    /// <summary>
    /// Manages and executes the real-time combat simulation.
    /// </summary>
    public class FightSimulator
    {
        private List<ICombatant> _combatants;
        private float _elapsedTime;
        private float _timeSinceLastLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="FightSimulator"/> class.
        /// </summary>
        public FightSimulator()
        {
            _combatants = new List<ICombatant>();
            _elapsedTime = 0.0f;
            _timeSinceLastLog = 0.0f;
        }

        /// <summary>
        /// Adds a combatant to the simulation.
        /// </summary>
        /// <param name="combatant">The combatant to add.</param>
        public void AddCombatant(ICombatant combatant)
        {
            _combatants.Add(combatant);
            CombatLog.Log($"{combatant.Name} has joined the fight for the {combatant.Affiliation}!");
        }

        /// <summary>
        /// Runs the entire combat simulation asynchronously until a victor is decided.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task RunSimulation()
        {
            CombatLog.Log("\n--- The Battle for Mîlēhîgh.wørld begins! ---");
            CombatLog.LogHealthStatus(_combatants);

            const float TickRate = 0.1f; // 100ms ticks (10 updates per second)
            _elapsedTime = 0.0f;
            _timeSinceLastLog = 0.0f;

            while (!IsFightOver())
            {
                _elapsedTime += TickRate;
                bool logThisTick = false;

                // Check if it's time to log
                _timeSinceLastLog += TickRate;
                if (_timeSinceLastLog >= 1.0f) // Log every 1 second
                {
                    CombatLog.LogTime(_elapsedTime);
                    logThisTick = true;
                    _timeSinceLastLog = 0.0f;
                }

                // 1. Update all combatants (tick down cooldowns)
                foreach (var combatant in _combatants)
                {
                    if (combatant.IsAlive)
                    {
                        combatant.Update(TickRate);
                    }
                }

                // 2. All combatants perform actions (if ready)
                foreach (var combatant in _combatants)
                {
                    if (combatant.IsAlive)
                    {
                        combatant.PerformAction(_combatants);
                    }
                }

                // 3. Check if fight ended mid-tick
                if (IsFightOver()) break;

                // 4. Log health if it's time
                if (logThisTick && !IsFightOver())
                {
                    CombatLog.LogHealthStatus(_combatants);
                }

                // 5. Wait for next tick
                await Task.Delay((int)(TickRate * 1000));
            }

            // Fight is over, log final health and determine winner
            CombatLog.LogHealthStatus(_combatants);
            bool novaminadAlive = _combatants.Any(c => c.Affiliation == CharacterAffiliation.Novaminad && c.IsAlive);
            bool shadowDominionAlive = _combatants.Any(c => c.Affiliation == CharacterAffiliation.ShadowDominion && c.IsAlive);

            if (novaminadAlive && !shadowDominionAlive)
            {
                CombatLog.LogOutcome("The Ɲōvəmîŋāđ are victorious!");
            }
            else if (!novaminadAlive && shadowDominionAlive)
            {
                CombatLog.LogOutcome("The Shadow Dominion has triumphed!");
            }
            else
            {
                CombatLog.LogOutcome("The battle is a draw... all combatants have fallen.");
            }
        }

        /// <summary>
        /// Checks if the fight has concluded.
        /// </summary>
        /// <returns>True if one or both teams have been completely defeated, otherwise false.</returns>
        private bool IsFightOver()
        {
            bool novaminadAlive = _combatants.Any(c => c.Affiliation == CharacterAffiliation.Novaminad && c.IsAlive);
            bool shadowDominionAlive = _combatants.Any(c => c.Affiliation == CharacterAffiliation.ShadowDominion && c.IsAlive);

            // If one team is wiped out, the fight is over
            return !novaminadAlive || !shadowDominionAlive;
        }
    }

    // ==========================================================================
    // PROGRAM ENTRY POINT
    // ==========================================================================
    /// <summary>
    /// The main entry point for the combat simulation application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Initializes and runs the combat simulation.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Initializing Combat Simulation...");

            // 1. Create the simulator
            var simulator = new FightSimulator();

            // 2. Define and add combatants
            simulator.AddCombatant(new MicahTheUnbreakable());
            simulator.AddCombatant(new AnastasiaTheDreamer());
            simulator.AddCombatant(new SkyixTheBionicGoddess());
            simulator.AddCombatant(new AeronTheTimeless());

            simulator.AddCombatant(new DelilahTheDesolate());
            simulator.AddCombatant(new NefariousTheSovereign());

            // 3. Run the fight
            await simulator.RunSimulation();
        }
    }
}
