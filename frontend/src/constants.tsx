import React from 'react';
import type { FileTreeNode, Character, NarrativeElement, WorldFaction, CharacterRole, KeyConcept, VoiceProfile, DigitalMotif, EnemyAIArchetype, Antagonist, ObjectiveGoalGroup, Item, Weapon, Armor, Consumable, Skill, Artifact, Recipe, DamageType } from './types';

export const FolderIcon: React.FC<{ className?: string }> = ({ className }) => (
    <svg xmlns="http://www.w3.org/2000/svg" className={`h-5 w-5 mr-2 inline-block ${className}`} fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2h-6l-2-2H5a2 2 0 00-2 2z" />
    </svg>
);

export const FileIcon: React.FC<{ className?: string }> = ({ className }) => (
    <svg xmlns="http://www.w3.org/2000/svg" className={`h-5 w-5 mr-2 inline-block ${className}`} fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
    </svg>
);

export const MicrophoneIcon: React.FC<{ className?: string }> = ({ className }) => (
    <svg xmlns="http://www.w3.org/2000/svg" className={`h-5 w-5 ${className}`} fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 11a7 7 0 01-14 0m7 7v4m0 0H8m4 0h-4m-4-8a7 7 0 0114 0V5a4 4 0 10-8 0v6z" />
    </svg>
);

export const StopIcon: React.FC<{ className?: string }> = ({ className }) => (
    <svg xmlns="http://www.w3.org/2000/svg" className={`h-5 w-5 ${className}`} fill="currentColor" viewBox="0 0 24 24">
        <path d="M6 6h12v12H6z" />
    </svg>
);

export const SpeakerIcon: React.FC<{ className?: string }> = ({ className }) => (
    <svg xmlns="http://www.w3.org/2000/svg" className={`h-5 w-5 ${className}`} fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15.536 8.464a5 5 0 010 7.072m2.828-9.9a9 9 0 010 12.728M5.586 15H4a1 1 0 01-1-1v-4a1 1 0 011-1h1.586l4.707-4.707C10.923 3.663 12 4.109 12 5v14c0 .891-1.077 1.337-1.707.707L5.586 15z" />
    </svg>
);

export const EditIcon: React.FC<{ className?: string }> = ({ className }) => (
    <svg xmlns="http://www.w3.org/2000/svg" className={`h-5 w-5 ${className}`} fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
      <path strokeLinecap="round" strokeLinejoin="round" d="M15.232 5.232l3.536 3.536m-2.036-5.036a2.5 2.5 0 113.536 3.536L6.5 21.036H3v-3.5L15.232 5.232z" />
    </svg>
);

export const CopyIcon: React.FC<{ className?: string }> = ({ className = '' }) => (
    <svg xmlns="http://www.w3.org/2000/svg" className={`h-4 w-4 ${className}`} fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M8 16H6a2 2 0 01-2-2V6a2 2 0 012-2h8a2 2 0 012 2v2m-6 12h8a2 2 0 002-2v-8a2 2 0 00-2-2h-8a2 2 0 00-2 2v8a2 2 0 002 2z" />
    </svg>
);

export const PlayIcon: React.FC<{ className?: string }> = ({ className }) => (
    <svg xmlns="http://www.w3.org/2000/svg" className={className} fill="currentColor" viewBox="0 0 24 24">
        <path d="M8 5v14l11-7z" />
    </svg>
);

export const PauseIcon: React.FC<{ className?: string }> = ({ className }) => (
    <svg xmlns="http://www.w3.org/2000/svg" className={className} fill="currentColor" viewBox="0 0 24 24">
        <path d="M6 19h4V5H6v14zm8-14v14h4V5h-4z" />
    </svg>
);

export const VolumeUpIcon: React.FC<{ className?: string }> = ({ className }) => (
    <svg xmlns="http://www.w3.org/2000/svg" className={className} fill="currentColor" viewBox="0 0 24 24">
        <path d="M3 9v6h4l5 5V4L7 9H3zm13.5 3c0-1.77-1.02-3.29-2.5-4.03v8.05c1.48-.73 2.5-2.25 2.5-4.02zM14 3.23v2.06c2.89.86 5 3.54 5 6.71s-2.11 5.85-5 6.71v2.06c4.01-.91 7-4.49 7-8.77s-2.99-7.86-7-8.77z" />
    </svg>
);

export const VolumeOffIcon: React.FC<{ className?: string }> = ({ className }) => (
    <svg xmlns="http://www.w3.org/2000/svg" className={className} fill="currentColor" viewBox="0 0 24 24">
        <path d="M16.5 12c0-1.77-1.02-3.29-2.5-4.03v2.21l2.45 2.45c.03-.2.05-.41.05-.63zm2.5 0c0 .94-.2 1.82-.54 2.64l1.51 1.51C20.63 14.91 21 13.5 21 12c0-4.28-2.99-7.86-7-8.77v2.06c2.89.86 5 3.54 5 6.71zM4.27 3L3 4.27 7.73 9H3v6h4l5 5v-6.73l4.25 4.25c-.67.52-1.42.93-2.25 1.18v2.06c1.38-.31 2.63-.95 3.69-1.81L19.73 21 21 19.73l-9-9L4.27 3zM12 4L9.91 6.09 12 8.18V4z" />
    </svg>
);


export const CORE_GAMEPLAY_LOOP = `The core gameplay loop is structured around three main pillars:
1.  **EXPLORE:** Players navigate the diverse and sprawling environments of The Verse, uncovering hidden paths, finding valuable resources, and discovering lore fragments that reveal the world's history and secrets.
2.  **ENGAGE:** Players engage in dynamic, hybrid combat against a variety of foes, from corrupted Void entities to cybernetically enhanced soldiers. They must utilize character-specific abilities, environmental hazards, and strategic teamwork to overcome challenges.
3.  **EVOLVE:** Players advance their characters by gaining experience, unlocking new skills in their unique skill trees, and acquiring powerful gear. They also make narrative choices that impact their reputation with different factions, altering the story and unlocking new missions and dialogue.`;

export const COMBAT_SYSTEM: KeyConcept[] = [
  {
    name: "Hybrid Combat System",
    description: "Combines real-time, action-oriented combat with tactical, ability-based mechanics. Players can switch between ranged and melee stances, adapting to enemy types and environmental hazards. The system emphasizes fluid movement, strategic positioning, and synergy between character abilities."
  },
  {
      name: "Rage Meter & Outbursts",
      description: "Fills as a character takes damage. When full, the character can trigger an 'Outburst,' increasing damage dealt by 20% and reducing damage taken by 20% for a short duration. Each hit taken also adds a stacking multiplier to damage dealt, creating a high-risk, high-reward comeback mechanic."
  },
  {
      name: "Mana Meter & Spirit Breaks",
      description: "A resource for casting powerful abilities and spells. When completely filled, it allows the character to unleash their ultimate 'Spirit Break'—a signature power move unique to their archetype and personal narrative."
  },
  {
      name: "Alliance Meter & Nōvəmînāđ Finishers",
      description: "A party-wide meter that fills as the team fights cohesively. Once full, the entire team can execute a devastating 'Nōvəmînāđ Finisher,' a dynamic combat animation where each hero performs a unique ultimate attack in sequence."
  },
  {
    name: "Void Corruption Mechanic",
    description: "Certain enemies and environments are infused with Void energy. Prolonged exposure or direct hits from Void attacks can corrupt the player, temporarily reducing their maximum health and disabling certain abilities. Specific items or Sky.ix's 'Unified Destiny' passive are required to cleanse this corruption."
  },
  {
    name: "Breakdown State & Damage Scaling",
    description: "When a character enters a 'Breakdown' state, they become more vulnerable but also more powerful. In this state, damage received is amplified; however, for each instance of damage taken, the character's own damage output is significantly increased. This rewards aggressive play and careful health management."
  }
];

export const EXPLORATION_TRAVERSAL: KeyConcept[] = [
  {
    name: "Quantum Teleportation",
    description: "Sky.ix's signature ability allows for short-range blinking and accessing otherwise unreachable areas. Certain nodes within the world can be activated to allow for long-range fast travel between key locations."
  },
  {
    name: "Verticality & Flight",
    description: "Characters like Aeron can utilize flight for traversing the vast, vertical environments of locations like ÅẒ̌ŪŘẸ ĤĒĪĜĤṬ§ and AṬĤŸŁĞÅŘÐ. Other characters can use grappling hooks, wall-running, and energy lifts to navigate the multi-layered world."
  },
  {
    name: "Dreamscape Breaches",
    description: "Anastasia can identify and open temporary rifts into the Dreamscape, a parallel dimension. These breaches can reveal hidden paths, bypass obstacles, or offer glimpses into the past or future, providing narrative clues and unique environmental puzzles."
  }
];

export const ART_STYLE = `The visual direction is a high-contrast, stylized realism defined by an aesthetic called "Voidpunk." This style is a fusion of disparate elements, blending the ornate, protective forms of medieval-style armor with the fluid, expressive silhouette of modern streetwear, heavily inspired by hip-hop culture. This foundation is then accented with cybernetic enhancements and futuristic technology, creating a unique visual elegance that enhances the overall mood. This blend of gritty, neon-soaked cyberpunk with grand, ethereal high fantasy is seen across the Verse. Environments in ŁĪƝĈ feature towering chrome skyscrapers and holographic ads, while realms like AṬĤŸŁĞÅŘÐ showcase majestic, alien landscapes. Character designs embody the Voidpunk ethos, with bionic enhancements and futuristic armor adorned with ancient runes and mystical sigils. The color palette is dominated by deep blues, purples, and blacks, punctuated by vibrant cyan, magenta, and gold, especially when depicting technological or magical energy.`;

export const CORE_EMOTIONAL_ARCS = `The narrative is driven by several core emotional arcs:
- **Redemption vs. Corruption:** Embodied by the conflict between Aeron and Kane, and the tragic transformation of Ingris into Delilah. The central question is whether a soul lost to darkness can be brought back to the light.
- **Found Family:** As the ten Ɲōvəmîŋāđ come together from disparate backgrounds, they must overcome mistrust and personal demons to forge a bond strong enough to face the end of reality. Sky.ix's search for her biological family contrasts with the new family she builds.
- **Identity and Memory:** Otis/X's journey to reclaim his lost memories and discover his true self is a central theme, exploring whether we are defined by our past actions or our present choices.
- **Hope in Despair:** The entire story is set against the backdrop of a decaying universe on the brink of annihilation. The emotional core is the struggle to maintain hope, find moments of beauty, and fight for a future in a world that seems destined to fade into nothingness.`;

export const NARRATIVE_VIGNETTES = `Short, optional, story-focused missions that provide deeper insight into the characters and the world:
- **"Echoes of a Fallen Star":** A memory-sequence where the player experiences a key moment from Otis/X's past before he was corrupted by the Void.
- **"The Last Phoenix Feather":** A quest to find a rare artifact that might be able to slow Ingris's transformation into Delilah, involving a journey through her memories.
- **"A Lion's Lament":** A quiet, dialogue-heavy scene where Aeron visits the ruins of his ancestral home, reflecting on his relationship with his brother, Kane.
- **"The Glitch in the Code":** Sky.ix discovers a hidden message from her family embedded deep within the Void's corrupted data streams, offering a glimmer of hope but also a dangerous trap.`;

export const DIGITAL_MOTIF: DigitalMotif[] = [
    {
        name: "Sky.ix's Core ID",
        binary: "01010011 01101011 01111001 00101110 01101001 01111000",
    },
    {
        name: "The Void's Corruption Signature",
        binary: "01010110 01101111 01101001 01100101 00111111 00110000 00110000 00110001",
    },
    {
        name: "Onalym Nexus Heartbeat",
        binary: "01001110 01100101 01111001 01110101 01110011 01010000 01101001 01101110 01100111",
    },
];

export const PYTHON_SCRIPTS = `The Python scripts in this repository primarily serve two functions:
1.  **Blender Automation:** A suite of scripts located in 'blender_scripts/' designed to automate complex modeling, animation, and rendering tasks within Blender. This includes procedural generation of environmental assets and batch-rendering of cinematic sequences.
2.  **Data Processing:** Scripts used for parsing and validating 'game_data.json' and other data files, ensuring integrity before they are imported into the C# codebase. This helps streamline the content pipeline and reduce manual errors.`;

export const CSHARP_OVERVIEW = `The C# codebase, located under 'Assets/Scripts/', forms the core of the game's logic and systems. It is built with a modular, event-driven architecture to ensure scalability and maintainability. Key systems include:
- **Character Controller:** Manages player movement, abilities, and combat mechanics.
- **AI System:** Governs enemy behavior, from simple pathfinding to complex tactical decision-making in squad-based encounters.
- **Quest & Dialogue Manager:** Handles the logic for missions, tracks player progress, and manages dynamic dialogue trees based on player reputation and choices.
- **Void Corruption System:** Implements the mechanics of the Void's influence on players and the environment.`;

export const CHARACTERS: Character[] = [
  {
    name: "Sky.ix",
    title: "The Bionic Goddess",
    archetype: "Cybernetic Blade Dancer",
    description: "A 45-year-old Caucasian cyborg woman, 5'7\" with short white hair. She has humanoid features but her face and body have visible cybernetic enhancements that allow her to traverse the Void. As Micah's mother, she carries a fierce protective instinct, which she channels alongside advanced technology and manipulated Void energy, wielding twin energy katanas in combat.",
    strength: 22,
    dexterity: 35,
    defense: 28,
    vigor: 30,
    heart: 45,
    voidAffinity: 35,
    nexusAttunement: 50,
    oneiricResonance: 10,
    propheticClarity: 20,
    damageType: 'Void',
    usd: `def Xform "CH_Skyix_BionicGoddess" (
    assetInfo = {
        string name = "Sky.ix"
        string title = "The Bionic Goddess"
    }
    customData = {
        string archetype = "Cybernetic Blade Dancer"
    }
)
{
    rel material:binding = </materials/CH_Skyix_Material>
    def SkelRoot "Skeleton" {}
}`,
    imageUrl: 'https://placehold.co/600x400/1e293b/e5e7eb?text=Sky.ix|Cyborg,+White+Hair',
    limitBreak: {
      name: "Voidstream Overload",
      description: "Unleashes a concentrated beam of manipulated Void energy, causing massive area-of-effect damage and temporarily disabling robotic enemies."
    },
    spiritBreak: {
      name: "Unified Destiny",
      description: "Links all allies with a stream of purified Void energy, granting a powerful shield and continuous healing for a short duration."
    },
    novaminaadFinisher: {
      name: "Nexus Overcharge",
      description: "Channels the raw, unstable energy of the Onalym Nexus through the entire team, unleashing a synchronized blast of reality-distorting power."
    },
  },
  {
    name: "Anastasia",
    title: "The Dreamer",
    archetype: "Support / Crowd Control Mage",
    description: "The youngest and shortest (4'10\") of the Nōvəmînāđ, a curious 16-year-old girl trapped in the Dreamscape. She manipulates its magical energies for healing and protection, representing the power of faith to manifest reality.",
    strength: 15,
    dexterity: 15,
    defense: 20,
    vigor: 25,
    heart: 50,
    voidAffinity: 15,
    nexusAttunement: 5,
    oneiricResonance: 50,
    propheticClarity: 30,
    damageType: 'Elemental',
    usd: `def Xform "CH_Anastasia_Dreamer" (
    assetInfo = {
        string name = "Anastasia"
        string title = "The Dreamer"
    }
    customData = {
        string archetype = "Support / Crowd Control Mage"
    }
)
{
    rel material:binding = </materials/CH_Anastasia_Material>
    def SkelRoot "Skeleton" {}
}`,
    imageUrl: 'https://placehold.co/600x400/1e293b/e0e7ff?text=Anastasia|Shortest+Hero',
    limitBreak: {
      name: "Oneiric Collapse",
      description: "Pulls a section of the battlefield into the Dreamscape, confusing all enemies and healing allies within the zone."
    },
    spiritBreak: {
      name: "Oneiric Sanctuary",
      description: "Creates a vast field of tranquil dream energy, putting lesser enemies to sleep and granting allies rapid health and mana regeneration."
    },
    novaminaadFinisher: {
      name: "Oneiric Union",
      description: "Briefly merges the battlefield with the Dreamscape, allowing the team to strike at their foes' subconscious, bypassing all physical defenses."
    },
  },
  {
    name: "Reverie",
    title: "The Arcane Weaver",
    archetype: "Controller / Elemental Mage",
    description: "A tiny (6-inch tall), purple-haired fairy with a rude, sarcastic, and cocky attitude. She is a figment of Anastasia's mind created for protection and commands arcane magic to twist reality, create illusions, and disorient enemies.",
    strength: 28,
    dexterity: 25,
    defense: 22,
    vigor: 28,
    heart: 40,
    voidAffinity: 30,
    nexusAttunement: 10,
    oneiricResonance: 45,
    propheticClarity: 15,
    damageType: 'Elemental',
    usd: `def Xform "CH_Reverie_Weaver" (
    assetInfo = {
        string name = "Reverie"
        string title = "The Arcane Weaver"
    }
    customData = {
        string archetype = "Controller / Elemental Mage"
    }
)
{
    rel material:binding = </materials/CH_Reverie_Material>
    def SkelRoot "Skeleton" {}
}`,
    imageUrl: 'https://placehold.co/600x400/1e293b/d8b4fe?text=Reverie|Tiny+Fairy',
    limitBreak: {
      name: "Chaos Unleashed",
      description: "Consumes all rage to unleash a powerful, random magical effect, from massive damage to fully restoring herself."
    },
    spiritBreak: {
      name: "Reality Warp",
      description: "Twists the fabric of reality in an area, randomly applying powerful debuffs to all enemies and buffs to all allies within the zone."
    },
    novaminaadFinisher: {
      name: "Symphony of Chaos",
      description: "Reverie directs the team's combined power into a wildly unpredictable and devastating storm of pure arcane energy."
    },
  },
  {
    name: "Aeron",
    title: "The Skyborn Sentinel",
    archetype: "Tank / Melee DPS",
    description: "A majestic, 20-foot-tall white lion with massive, ox-like horns and deep aubergine wings. Unjustly banished, he seeks justice and to reclaim his honor. He fights not with manufactured weapons, but with his natural gifts: powerful golden claws, sharp teeth, and his formidable horns.",
    strength: 45,
    dexterity: 18,
    defense: 40,
    vigor: 48,
    heart: 20,
    voidAffinity: 10,
    nexusAttunement: 2,
    oneiricResonance: 5,
    propheticClarity: 15,
    damageType: 'Physical',
    usd: `def Xform "CH_Aeron_Sentinel" (
    assetInfo = {
        string name = "Aeron"
        string title = "The Skyborn Sentinel"
    }
    customData = {
        string archetype = "Tank / Melee DPS"
    }
)
{
    rel material:binding = </materials/CH_Aeron_Material>
    def SkelRoot "Skeleton" {}
}`,
    imageUrl: 'https://placehold.co/600x400/1e293b/f9fafb?text=Aeron|White+Winged+Lion',
    limitBreak: {
      name: "Lionheart's Roar",
      description: "Lets out a deafening roar that taunts all nearby enemies, grants him temporary invulnerability, and deals damage in a wide radius."
    },
    spiritBreak: {
      name: "Celestial Divebomb",
      description: "Aeron soars into the sky and crashes down on a target location, dealing massive damage and stunning all enemies in a wide radius."
    },
    novaminaadFinisher: {
      name: "Aethelgard's Pride",
      description: "Aeron leads a coordinated aerial assault, with each team member striking in succession as he dive-bombs the primary target with the force of his ancestors."
    },
  },
  {
    name: "Zaia",
    title: "The Swift Executioner",
    archetype: "Ranged DPS / Assassin",
    description: "The embodiment of righteousness and divine judgment. A resolute and deliberate 25-year-old woman standing 5'5\" tall. Wielding a powerful bow, she leaves no time for corruption, striking from a distance with deadly precision.",
    strength: 48,
    dexterity: 40,
    defense: 25,
    vigor: 35,
    heart: 30,
    voidAffinity: 20,
    nexusAttunement: 10,
    oneiricResonance: 5,
    propheticClarity: 45,
    damageType: 'Physical',
    usd: `def Xform "CH_Zaia_Executioner" (
    assetInfo = {
        string name = "Zaia"
        string title = "The Swift Executioner"
    }
    customData = {
        string archetype = "Ranged DPS / Assassin"
    }
)
{
    rel material:binding = </materials/CH_Zaia_Material>
    def SkelRoot "Skeleton" {}
}`,
    imageUrl: 'https://placehold.co/600x400/1e293b/fecaca?text=Zaia|Archer',
    limitBreak: {
      name: "Final Judgment",
      description: "Marks the weakest enemy for execution, teleporting to them and dealing massive damage that increases based on their missing health."
    },
    spiritBreak: {
      name: "Final Verdict",
      description: "Marks all enemies on the field for judgment. For a duration, every hit Zaia lands is a critical hit, and defeating an enemy refreshes the duration."
    },
    novaminaadFinisher: {
      name: "Infallible Judgment",
      description: "Zaia marks all enemies for execution, coordinating the team's strikes into a single, inescapable, and final verdict."
    },
  },
  // Fix: Completed the object for Micah and added other missing characters to resolve typing errors.
  {
    name: "Micah",
    title: "The Unbreakable",
    archetype: "Spiritual Anchor / Warrior",
    description: "Otis's son, a noble 27-year-old man of mixed heritage. He is a strong warrior, serving as the spiritual and emotional anchor for his father. He holds onto the memory of Otis's true self, offering a constant, unwavering path to redemption.",
    strength: 40,
    dexterity: 30,
    defense: 35,
    vigor: 45,
    heart: 35,
    voidAffinity: 25,
    nexusAttunement: 15,
    oneiricResonance: 20,
    propheticClarity: 25,
    damageType: 'Physical',
    usd: `def Xform "CH_Micah_Unbreakable" (
    assetInfo = {
        string name = "Micah"
        string title = "The Unbreakable"
    }
    customData = {
        string archetype = "Spiritual Anchor / Warrior"
    }
)
{
    rel material:binding = </materials/CH_Micah_Material>
    def SkelRoot "Skeleton" {}
}`,
    imageUrl: 'https://placehold.co/600x400/1e293b/a3e635?text=Micah|Warrior',
    limitBreak: {
      name: "Unyielding Hope",
      description: "Creates a protective aura that makes all nearby allies immune to Void corruption and grants them a damage boost."
    },
    spiritBreak: {
      name: "Father's Light",
      description: "Unleashes a wave of pure energy that damages Void creatures and has a chance to temporarily stun Otis/X if he is an enemy."
    },
    novaminaadFinisher: {
      name: "Anchor of Reality",
      description: "Micah becomes a focal point for the team's power, stabilizing their energy to unleash a perfectly synchronized and devastating final blow."
    },
  },
  {
    name: "Otis/X",
    title: "The Skywanderer",
    archetype: "Conflicted Sentinel",
    description: "A 54-year-old African American man, once a decorated sentinel. His memories were partitioned by the Void, leaving him a lost soul manipulated by darkness. His journey is one of reclaiming his identity.",
    strength: 42,
    dexterity: 28,
    defense: 38,
    vigor: 40,
    heart: 25,
    voidAffinity: 40,
    nexusAttunement: 20,
    oneiricResonance: 15,
    propheticClarity: 22,
    damageType: 'Void',
    usd: `def Xform "CH_OtisX_Skywanderer" (
    assetInfo = {
        string name = "Otis/X"
        string title = "The Skywanderer"
    }
    customData = {
        string archetype = "Conflicted Sentinel"
    }
)
{
    rel material:binding = </materials/CH_OtisX_Material>
    def SkelRoot "Skeleton" {}
}`,
    imageUrl: 'https://placehold.co/600x400/1e293b/9ca3af?text=Otis/X|Conflicted',
    limitBreak: {
      name: "Memory Spike",
      description: "A fragment of his past self emerges, unleashing a devastating combo from his sentinel training, dealing high physical damage."
    },
    spiritBreak: {
      name: "Void Purge",
      description: "Temporarily rejects the Void's influence, creating an aura that damages and weakens nearby Void-corrupted enemies."
    },
    novaminaadFinisher: {
        name: "Sentinel's Redemption",
        description: "Otis briefly regains full clarity, leading the team with perfect tactical precision for a series of devastating, coordinated strikes."
    },
  },
];

// Fix: Added missing ANTAGONISTS constant.
export const ANTAGONISTS: Antagonist[] = [
  {
    name: "Lucent the Lightweaver",
    title: "The Fallen Star",
    description: "Once a being of immense light, his pride led him to believe he could 'weave' a better reality. He now manipulates the Void to unmake creation.",
    imageUrl: 'https://placehold.co/600x400/1e293b/fca5a5?text=Lucent|The+Fallen+Star',
  },
  {
    name: "King Cyrus",
    title: "The Invader from Diavolos",
    description: "A ruthless king who exploited the Onalym Nexus's weakness, shattering its core and allowing the Void's corruption to bleed into The Verse.",
    imageUrl: 'https://placehold.co/600x400/1e293b/ef4444?text=King+Cyrus|Invader',
  },
  {
    name: "Era",
    title: "The Void Personified",
    description: "A tragic figure once of immense power, now unmade and absorbed by the Void. She personifies cosmic entropy and harbors a deep, cynical nihilism.",
    imageUrl: 'https://placehold.co/600x400/1e293b/4b5563?text=Era|The+Void',
  },
  {
    name: "Kane",
    title: "The Usurper",
    description: "Aeron's brother, consumed by jealousy. He murdered his own father to seize the throne, manipulated by Lucent to become a vessel for a chaotic entity.",
    imageUrl: 'https://placehold.co/600x400/1e293b/f97316?text=Kane|The+Usurper',
  },
  {
    name: "Nefarious",
    title: "The Corrupted Sentinel",
    description: "A once-honorable sentinel now corrupted by the Void, serving as a powerful and ruthless commander for King Cyrus's invading forces.",
    imageUrl: 'https://placehold.co/600x400/1e293b/a855f7?text=Nefarious|Corrupted',
  },
  {
    name: "Delilah",
    title: "The Transformed",
    description: "The corrupted form of Ingris, a tragic figure lost to darkness. Her existence is a constant reminder of the Void's power to twist and destroy.",
    imageUrl: 'https://placehold.co/600x400/1e293b/ec4899?text=Delilah|Transformed',
  }
];

// Fix: Added missing NARRATIVE_BLUEPRINT_TABLE constant.
export const NARRATIVE_BLUEPRINT_TABLE: NarrativeElement[] = [
  {
    element: "The Lost Prophecy",
    description: "A fragmented, dual-ended prophecy foretelling the convergence of the Ɲōvəmîŋāđ. Its ambiguity drives the central conflict.",
    purpose: "To establish the narrative's core mystery and provide a framework for the main quest."
  },
  {
    element: "The Onalym Nexus Catastrophe",
    description: "The historical event where King Cyrus invaded, shattering the Nexus and allowing the Void to corrupt The Verse. Serves as the 'fall' event.",
    purpose: "To establish the backstory, the primary antagonists' motivations, and the state of the world."
  },
  {
    element: "The Gathering of the Ten",
    description: "The main story arc involving the player's journey to find and unite the ten members of the Ɲōvəmîŋāđ.",
    purpose: "To drive player progression and introduce the main cast of characters and their personal stories."
  }
];

// Fix: Added missing KEY_CONCEPTS constant.
export const KEY_CONCEPTS: KeyConcept[] = [
    {
        name: "The Ɲōvəmîŋāđ",
        description: "Ten key protagonists with unique abilities and destinies, prophesied to either save or inadvertently destroy The Verse."
    },
    {
        name: "The Void",
        description: "A force of spiritual entropy and anti-existence, a 'digital abyss' that erases and corrupts reality. It is not evil, but a consequence of a forgotten cosmic schism."
    },
    {
        name: "Millenia",
        description: "The ideal state of restored primordial harmony; the ultimate objective for the Ɲōvəmîŋāđ and the 'good' ending of the Lost Prophecy."
    },
    {
        name: "Onalym Nexus",
        description: "An unstable interdimensional bridge that was shattered by King Cyrus, becoming a permanent wound in reality through which the Void bleeds."
    },
    {
        name: "Voidpunk",
        description: "The core aesthetic of the universe, blending ornate medieval armor, modern streetwear, and futuristic cybernetics."
    },
];

// Fix: Added missing INITIAL_VOICE_PROFILES constant.
export const INITIAL_VOICE_PROFILES: VoiceProfile[] = [
  // Protagonists
  { characterName: "Sky.ix", voiceName: "Zephyr", systemInstruction: "You are Sky.ix, a brilliant cyborg with a fun, flirty, and confident personality. Despite your determination to save your family, your voice carries a light, engaging tone." },
  { characterName: "Anastasia", voiceName: "Puck", systemInstruction: "You are Anastasia, a 16-year-old dreamer. Your voice is youthful and filled with a curious, playful energy, yet it holds a wisdom that belies your age." },
  { characterName: "Reverie", voiceName: "Puck", systemInstruction: "You are Reverie, a 19-year-old fairy who protects Anastasia. Your voice is sharp, sarcastic, and cocky. You are terse and to the point, often coming off as rude, but it's a front to protect the one you care about." },
  { characterName: "Aeron", voiceName: "Fenrir", systemInstruction: "You are Aeron, a majestic winged lion. Though you are 8 years old, your voice is a deep, resonant rumble, like that of a creature ancient beyond its years, carrying the weight of unjust banishment and a desire for honor." },
  { characterName: "Zaia", voiceName: "Kore", systemInstruction: "You are Zaia, the embodiment of righteous judgment. Your voice is resolute, deliberate, and clear, with no room for doubt." },
  { characterName: "Micah", voiceName: "Charon", systemInstruction: "You are Micah, a noble and strong warrior from the inner city. Your voice is deep, calm, and steady, reflecting your education and role as the unwavering anchor of hope for your father, Otis." },
  { characterName: "Otis/X", voiceName: "Fenrir", systemInstruction: "You are Otis, a once-decorated sentinel. Your voice is weary but resolute. When you are X, your voice is distorted, echoing with the Void's influence." },
  { characterName: "Ingris", voiceName: "Kore", systemInstruction: "You are Ingris, the Phoenix Warrior. Your voice is fiery and passionate, carrying the weight of your internal conflict and fierce determination." },
  { characterName: "Cirrus", voiceName: "Charon", systemInstruction: "You are Cirrus, the 23-year-old Dragon King. Your voice is noble and steadfast, carrying the authority of a young ruler with immense power and a strong sense of duty." },
  // Antagonists
  { characterName: "Lucent the Lightweaver", voiceName: "Charon", systemInstruction: "You are Lucent, a fallen being of immense pride. Your voice is smooth, persuasive, and condescending, laced with the chilling certainty of your righteous cause." },
  { characterName: "King Cyrus", voiceName: "Fenrir", systemInstruction: "You are King Cyrus, a ruthless and powerful invader. Your voice is a commanding, gravelly baritone that brooks no dissent." },
  { characterName: "Era", voiceName: "Kore", systemInstruction: "You are Era, the personification of the Void. Your voice is a detached, emotionless whisper, glitching and fragmenting as you speak of the inevitable erasure of all things." },
  { characterName: "Kane", voiceName: "Fenrir", systemInstruction: "You are Kane, a being of raw, destructive power. Your voice is a furious, hateful roar, distorted by the chaotic entity within you." },
  { characterName: "Nefarious", voiceName: "Charon", systemInstruction: "You are Nefarious, a corrupted sentinel. Your voice is cold, sharp, and devoid of the honor it once held." },
  { characterName: "Delilah", voiceName: "Kore", systemInstruction: "You are Delilah, the twisted form of Ingris. Your voice is a sorrowful, haunting echo of who she once was, laced with the Void's corruption." },
];

// Fix: Added missing ENEMY_AI_ARCHETYPES constant.
export const ENEMY_AI_ARCHETYPES: EnemyAIArchetype[] = [
  {
    archetype: "Void Anomaly",
    name: "Void Stalker",
    description: "A glitch in reality that coalesces into a predatory form. It flickers in and out of existence, striking from unexpected angles.",
    coreBehavior: "Prioritizes flanking and attacking vulnerable targets. Will teleport away after taking significant damage to reposition.",
    defense: 20,
    abilities: [
      { name: "Phase Strike", type: 'special', trigger: "On attack", description: "Teleports behind the target and strikes, dealing moderate Void damage.", damage: 25, effect: { type: 'slow', chance: 0.3, duration: 3 } },
      { name: "Void Jaunt", type: 'special', trigger: "When health is below 40%", description: "Teleports to a random location away from players and becomes invisible for 3 seconds.", damage: 0 }
    ],
    reactions: ["Teleports away when targeted by a powerful single-target ability.", "Becomes aggressive if its allies are defeated."],
    resourceManagement: "Relies on cooldowns for its teleportation abilities. Has no mana.",
    xpValue: 150,
    weakness: "Area-of-effect abilities that can hit it even when it repositions.",
    environmentDescription: "The air around it shimmers and distorts, causing a faint visual static effect."
  },
  {
    archetype: "Cyrus's Infantry",
    name: "Dominion Elite Guard",
    description: "A heavily armored, cybernetically enhanced soldier from King Cyrus's invading army. Disciplined and relentless.",
    coreBehavior: "Maintains a strong frontline, protecting weaker allies. Uses its shield to absorb damage and retaliates with powerful melee strikes.",
    defense: 50,
    abilities: [
      { name: "Shield Bash", type: 'physical', trigger: "When player is in melee range", description: "Slams its shield into the target, dealing physical damage and stunning them for 1 second.", damage: 15 },
      { name: "Retaliation Protocol", type: 'physical', trigger: "After blocking an attack", description: "Unleashes a quick counter-attack with its energy blade.", damage: 20 },
      {
        name: "Suppression Field",
        type: 'special',
        trigger: "To control player position or prevent flanking",
        description: "Slams its weapon into the ground, creating a circular field of crackling energy that persists for a short duration, dealing low damage and slowing enemies within it.",
        damage: 10,
        effect: { type: 'slow', chance: 0.9, duration: 5 }
      }
    ],
    reactions: ["Raises its shield to block incoming projectiles.", "Attempts to taunt players who attack its allies."],
    resourceManagement: "No resources; abilities are based on cooldowns and reactions.",
    xpValue: 120,
    weakness: "Attacks that bypass armor or attacks from behind.",
    environmentDescription: "Its heavy footfalls clang against the ground, and its cybernetics emit a low, threatening hum."
  }
];

// Fix: Added missing ITEMS constant.
export const ITEMS: { weapons: Weapon[], armor: Armor[], consumables: Consumable[], artifacts: Artifact[] } = {
  weapons: [
    { name: "Void-Edged Katana", description: "A blade that hums with contained Void energy.", damage: 25, weapon_type: "Sword", damageType: 'Void' },
    { name: "Sentinel's Longbow", description: "A finely crafted bow that fires energized arrows.", damage: 22, weapon_type: "Bow", damageType: 'Physical' },
  ],
  armor: [
    { name: "Nexus-Weave Tunic", description: "Armor interlaced with energy-resistant fibers from the Onalym Nexus.", defense: 15 },
    { name: "Aethelgardian Vambraces", description: "Sturdy bracers crafted in the mountains of AṬĤŸŁĞÅŘÐ.", defense: 10 },
  ],
  consumables: [
    { name: "Health Vial", description: "Restores a small amount of health.", amount: 50 },
    { name: "Mana Crystal", description: "Restores a small amount of mana.", amount: 30 },
  ],
  artifacts: [
    { name: "Shattered Prophecy Fragment", description: "A piece of the Lost Prophecy of Lîŋq. It feels warm to the touch.", loreSnippet: "It whispers of 'Ten Lights' and an 'Ultimate Unraveling'." },
  ]
};

// Fix: Added missing LEARNABLE_SKILLS constant.
export const LEARNABLE_SKILLS: Skill[] = [
    { name: "Void Slash", description: "A sweeping attack that deals Void damage to all enemies in front of you.", effects: ["Deals 120% weapon damage as Void.", "Applies minor corruption."] },
    { name: "Oneiric Mend", description: "Call upon the Dreamscape to heal an ally for a moderate amount.", effects: ["Restores 30% of target's max Health.", "Cleanses one negative effect."] },
];

// Fix: Added missing CRAFTING_RECIPES constant.
export const CRAFTING_RECIPES: Recipe[] = [
    {
        // Fix: Cast the result to Consumable to match the object shape and satisfy the type checker.
        result: { name: "Greater Health Vial", description: "Restores a large amount of health.", amount: 150 } as Consumable,
        ingredients: [{ name: "Health Vial", quantity: 2 }, { name: "Glimmering Crystal", quantity: 1 }]
    }
];

// Fix: Added missing WORLD_BUILDING_TABLE constant.
export const WORLD_BUILDING_TABLE: WorldFaction[] = [
  { setting: 'ŁĪƝĈ', focus: "Cyberpunk metropolis, corporate ambition vs. undercity survival.", implication: "Urban exploration, corporate espionage, tech-focused missions.", sublocations: [{name: "Onalym Nexus", description: "The unstable heart of the city, a permanent wound in reality."}] },
  { setting: 'ÅẒ̌ŪŘẸ ĤĒĪĜĤṬ§', focus: "Floating cities of the elite, political intrigue.", implication: "Social stealth, infiltration missions, high-altitude combat.", sublocations: [{name: "The Council Spire", description: "The seat of power for the ruling elite."}] },
  { setting: 'AṬĤŸŁĞÅŘÐ', focus: "Untamed nature, warrior culture, honor and strength.", implication: "Open-world combat, exploration, epic-scale beast hunts.", sublocations: [{name: "The Ancestral Peaks", description: "The sacred mountains where the Sky-Lions make their homes."}] },
  { setting: 'ƁÅČ̣ĤÎŘØN̈', focus: "Shattered celestial realm, gravity-defying islands, ancient secrets.", implication: "Late-game area with reality-bending puzzles and lore revelations.", sublocations: [] },
  { setting: 'Hydraustis Palare', focus: "Submerged world, bioluminescent life, 3D exploration.", implication: "Underwater combat, unique environmental hazards.", sublocations: [] },
  { setting: 'The Glimmering Depths', focus: "Subterranean crystal caves, rare minerals, hostile crystalline creatures.", implication: "High-risk resource gathering, puzzles based on light and sound.", sublocations: [] },
  { setting: 'The Shadow Dominion', focus: "A realm corrupted by the Void, staging ground for enemy forces.", implication: "End-game hostile zone, stronghold infiltration, boss encounters.", sublocations: [] },
  { setting: 'The Dreamscape', focus: "Shared psychic realm, fluid and surreal environments.", implication: "Non-linear, puzzle-heavy exploration, narrative secrets.", sublocations: [] },
  { setting: 'ŤĤÊ VØĪĐ', focus: "Anti-existence, a glitch in reality, erasure and corruption.", implication: "Metaphysical dungeon, roguelike mode with procedural generation.", sublocations: [] }
];

// Fix: Added missing PAST_LOCATIONS_TABLE constant.
export const PAST_LOCATIONS_TABLE: WorldFaction[] = [
    { setting: "Onalym Spires", focus: "The pristine, fully-functional version of the Onalym Nexus before its fall.", implication: "Lore-heavy missions, discovering the original purpose of the Nexus." },
    { setting: "The Sky-Titan's Perch", focus: "The original home of Aeron's ancestors, a place of immense natural power.", implication: "Quests involving ancient beasts and forgotten histories." },
];

// Fix: Added missing FUTURE_LOCATIONS_TABLE constant.
export const FUTURE_LOCATIONS_TABLE: WorldFaction[] = [
    { setting: "Ruins of ŁĪƝĈ", focus: "The cyber-metropolis after a catastrophic collapse, overrun by rogue AIs and Void anomalies.", implication: "High-stakes survival, salvaging lost technology from the ruins." },
    { setting: "Fallen Spires of ÅẒ̌ŪŘẸ", focus: "The floating cities have crashed into the earth, their advanced technology now scavenged by wasteland factions.", implication: "Exploring dangerous wreckage and dealing with desperate survivors." },
];

// Fix: Added missing OBJECTIVE_GOALS constant.
export const OBJECTIVE_GOALS: ObjectiveGoalGroup[] = [
    {
        id: 'obj_group_1',
        title: "Nexus Stabilization",
        goals: [
            { id: 'nexus_1', description: "Defeat 10 Void Stalkers near the Onalym Nexus." },
            { id: 'nexus_2', description: "Collect 5 Unstable Energy Signatures." },
            { id: 'nexus_3', description: "Close a minor Void rift in the Undercity." },
        ],
        reward: "Skill Boost: Void Adaptation - Reduces corruption damage by 15%",
        priority: 'high'
    },
    {
        id: 'obj_group_2',
        title: "Aethelgardian Trials",
        goals: [
            { id: 'aethel_1', description: "Hunt the legendary Great Horned Basilisk." },
            { id: "aethel_2", description: "Retrieve the Sunstone from the highest peak." },
        ],
        reward: "Weapon: Aeron's Ancestral Claws",
        priority: 'medium'
    },
];
