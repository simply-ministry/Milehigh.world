import React from 'react';
import type { FileTreeNode, Character, NarrativeElement, WorldFaction, CharacterRole, KeyConcept, VoiceProfile, DigitalMotif, EnemyAIArchetype, Antagonist, ObjectiveGoalGroup, Item, Weapon, Armor, Consumable, Skill, Artifact, Recipe, DamageType, CSharpScript } from './types';
import { SHADER_COMMENT_BLOCK } from './utils/shaderCode';

export const FolderIcon: React.FC<{ className?: string }> = ({ className }) => (
  <></>
);

export const FileIcon: React.FC<{ className?: string }> = ({ className }) => (
  <></>
);

export const MicrophoneIcon: React.FC<{ className?: string }> = ({ className }) => (
  <></>
);

export const StopIcon: React.FC<{ className?: string }> = ({ className }) => (
  <></>
);

export const SpeakerIcon: React.FC<{ className?: string }> = ({ className }) => (
  <></>
);

export const EditIcon: React.FC<{ className?: string }> = ({ className }) => (
  <></>
);

export const CopyIcon: React.FC<{ className?: string }> = ({ className }) => (
  <></>
);

export const PlayIcon: React.FC<{ className?: string }> = ({ className }) => (
  <></>
);

export const PauseIcon: React.FC<{ className?: string }> = ({ className }) => (
  <></>
);

export const VolumeUpIcon: React.FC<{ className?: string }> = ({ className }) => (
  <></>
);

export const VolumeOffIcon: React.FC<{ className?: string }> = ({ className }) => (
  <></>
);


export const CHARACTERS: Character[] = [
{
name: 'Ingris',
title: 'The Phoenix Warrior',
archetype: 'Self-Sustaining Bruiser',
description: "A resilient warrior who embodies the cycle of death and rebirth, characterized by her vibrant, long-flowing red hair and magnificent Phoenix wings that shift from crimson to scarlet. Her abilities allow her to sacrifice her own life force to deal devastating damage, only to rise again from the ashes stronger than before. She is a symbol of hope and endurance, a living testament to the idea that even in utter defeat, victory can be born anew.",
backstory: "Born with a rare connection to primordial fire, Ingris was a prodigy within the Sunstone Clan. During a ritual to temper her powers, a rival sabotaged the rite, causing her abilities to erupt catastrophically. The explosion consumed her village and herself. Yet, from the smoldering ashes, she was reborn, her body and soul fused with the undying essence of a Phoenix. Now, she seeks to master her volatile gift, driven by the guilt of her past and a fierce, protective instinct to prevent such destruction from ever happening again.",
usd: `def Xform "CH_Ingris" ( asset aname = "CH_Ingris_Phoenix_Warden.usd" ) { rel material:binding = }`,
strength: 8,
dexterity: 6,
defense: 7,
vigor: 9, // Governs Health
heart: 5, // Governs Mana/Spirit
voidAffinity: 2,
nexusAttunement: 1,
oneiricResonance: 3,
propheticClarity: 4,
damageType: 'Physical',
statGrowth: {
primary: ['strength', 'vigor'],
secondary: ['defense', 'heart']
},
fightingStyle: 'Fiery Brawler',
weapons: ['Phoenix Blade', 'Claws'],
imageUrl: 'https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/milehigh/Ingris.png',
limitBreak: { name: "Scorched Earth", description: "Unleashes a torrent of fire, dealing massive damage to a single target and applying a burning effect over time." },
spiritBreak: { name: "Phoenix Down", description: "Sacrifices 50% of her current health to revive a fallen ally with 50% health." },
novaminaadFinisher: { name: "Rebirth's Embrace", description: "Heals the entire party to full health and grants a temporary damage buff." },
skills: [
{ name: "Cinder Strike", description: "A basic melee attack that has a chance to apply a burning effect.", effects: ["Deals 110% weapon damage.", "20% chance to apply 'Burn' for 3 seconds."] },
{ name: "From the Ashes", description: "Passive: Upon taking fatal damage, Ingris is reborn with 30% health. Can only occur once per encounter.", effects: ["Revives on death.", "Cooldown: 1 per encounter."] }
],
},
{
name: 'Kai',
title: 'The Prophet',
archetype: 'Vigilante Tactician & Infiltrator',
description: "Once the lead engineer at the corrupt Lucent Labs, Kai is a 23-year-old man of Asian descent with short, thin black hair and standing at 5'8\". A prophetic glimpse of a future devastated by Lucent's research drove him to become a vigilante. Operating from the shadows, he infiltrates Lucent's operations and organizes the Ɲōvəmîŋāđ to avert the apocalyptic future he witnessed, guided by his newfound precognitive abilities.",
backstory: "A prodigy engineer at Lucent Labs, Kai was once a true believer in the company's vision of a technologically perfected future. That faith shattered when he accidentally interfaced with a nascent temporal device, triggering a cascade of horrifying, fragmented visions: a Verse consumed by Void, twisted by Lucent's ambition. The psychic feedback permanently attuned him to the flow of time, granting him prophetic clarity. Haunted by what he saw, he erased his own identity, abandoning his life to become a vigilante prophet. He now operates from the digital shadows, assembling the Ɲōvəmîŋāđ to fight a future he has already witnessed.",
usd: `def Xform "CH_Kai" ( asset aname = "CH_Kai_Prophet.usd" ) { rel material:binding = }`,
strength: 5,
dexterity: 7,
defense: 6,
vigor: 6,
heart: 8,
voidAffinity: 3,
nexusAttunement: 9,
oneiricResonance: 7,
propheticClarity: 10,
damageType: 'Physical',
statGrowth: {
primary: ['propheticClarity', 'nexusAttunement'],
secondary: ['dexterity', 'heart']
},
fightingStyle: 'Strategic Fencer & Gadgeteer',
weapons: ["Sentinel's Rapier", 'Energy Shield', 'Infiltration Gadgets'],
imageUrl: 'https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/milehigh/Kai.png',
limitBreak: { name: "Moment of Clarity", description: "Reveals the weak points of all enemies on the field, guaranteeing critical hits for the entire party for a short duration." },
spiritBreak: { name: "Temporal Stasis", description: "Creates a field of distorted time, freezing all enemies within it for several seconds." },
novaminaadFinisher: { name: "Prophecy Unwritten", description: "Resets all ally cooldowns and fully restores their Mana." },
skills: [
{ name: "Preordain", description: "Marks a single target, increasing all damage they take by 20% for 10 seconds.", effects: ["Applies 'Vulnerable' debuff."] },
{ name: "Sixth Sense", description: "Passive: Grants the entire party a 10% chance to automatically dodge an incoming attack.", effects: ["Party-wide dodge chance."] }
],
},
{
name: 'Cirrus',
title: 'The Dragon King',
archetype: 'Elemental Bruiser',
description: "The ancient and powerful Dragon King, Cirrus commands the raw fury of the elements. He is a frontline combatant who blends immense physical strength with devastating fire and lightning attacks. His goal is to reclaim his throne from his usurper father, King Cyrus, and restore balance to the elemental realms.",
backstory: "As the firstborn son of King Cyrus, Cirrus was raised to be a weapon. He was trained in the elemental fury of the dragons, but his father saw his innate sense of justice and balance as a weakness. Cyrus, having made a dark pact for power, usurped the elemental thrones through brute force, seeing his son's code of honor as a threat to his new tyrannical rule. Defeated and exiled, Cirrus now fights not only to reclaim his birthright but to restore the very balance his father corrupted, believing the true strength of a king lies in harmony, not domination.",
usd: `def Xform "CH_Cirrus" ( asset aname = "CH_Cirrus_Dragon_King.usd" ) { rel material:binding = }`,
strength: 10,
dexterity: 4,
defense: 8,
vigor: 8,
heart: 6,
voidAffinity: 1,
nexusAttunement: 1,
oneiricResonance: 2,
propheticClarity: 5,
damageType: 'Elemental',
statGrowth: {
primary: ['strength', 'defense'],
secondary: ['vigor', 'heart']
},
fightingStyle: 'Draconic Fury',
weapons: ['Greatsword of Storms', 'Dragon Claws'],
imageUrl: 'https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/milehigh/Cirrus.png',
limitBreak: { name: "Dragon's Fury", description: "Transforms into his dragon form for a short time, gaining new abilities and massively increased stats." },
spiritBreak: { name: "Supernova", description: "Unleashes a massive explosion of fire, heavily damaging all enemies on the battlefield." },
novaminaadFinisher: { name: "Wrath of the Ancients", description: "Summons a storm of meteors to strike all enemies, dealing massive elemental damage and stunning them." },
skills: [
{ name: "Thunderclap", description: "Slams the ground, dealing AoE lightning damage and potentially paralyzing nearby enemies.", effects: ["AoE damage.", "30% chance to apply 'Paralyze'."] },
{ name: "Dragonscale Armor", description: "Passive: Possesses a natural resistance to all forms of elemental damage.", effects: ["+25% resistance to Elemental damage."] }
],
},
{
name: 'Sky.ix',
title: 'The Bionic Goddess',
archetype: 'Void-Traversing Assassin',
description: "A brilliant xenolinguist turned cyborg warrior, Sky.ix was altered by the Void during the Onalym Nexus onslaught. Now a master of short-range quantum teleportation and reality-glitching attacks, she phases in and out of existence, striking from impossible angles. Her personal mission is to rescue her husband and child, who were lost to the Void.",
backstory: "A renowned xenolinguist, Sky.ix dedicated her life to understanding the language of the cosmos alongside her husband, Otis, at the Onalym Nexus research outpost. Her life was one of discovery and family, raising her son Micah amidst the stars. During the cataclysmic Onalym Nexus onslaught, a Void rift tore through their home. To save Micah, Sky.ix forced him into an escape pod, but was pulled into the abyss herself. Instead of being erased, her cybernetic implants—designed for deep-space communication—were warped by the Void, fusing her with its reality-glitching nature. She escaped, but her memory of the event is fractured. She believes she was simply lost on a mission and that her family is waiting for her back home, unaware of the chaos that has consumed the Verse and the years that have passed.",
usd: 'def Xform "CH_Skyix" ( asset aname = "CH_Skyix_Body_A.usd" ) { rel material:binding = }',
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
statGrowth: {
primary: ['dexterity', 'voidAffinity'],
secondary: ['nexusAttunement', 'strength']
},
fightingStyle: 'Quantum Assassin',
weapons: ['Void-edged Katars', 'Phase Daggers'],
imageUrl: 'https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/milehigh/Skyix.png',
limitBreak: { name: 'Void Walk', description: 'Becomes completely invisible and untargetable for a short duration, guaranteeing critical hits on her next three attacks.' },
spiritBreak: { name: 'Reality Tear', description: 'Opens a localized Void rift that pulls in nearby enemies, dealing continuous damage and slowing them.' },
novaminaadFinisher: { name: 'Quantum Collapse', description: 'Teleports between all enemies, delivering a single, devastating blow to each before returning.' },
skills: [
{ name: 'Phase Strike', description: 'Teleports to a target and strikes, bypassing a portion of their defense.', effects: ['Ignores 30% of target defense.'] },
{ name: 'Nexus Blink', description: 'A short-range, instantaneous dodge that can pass through enemies and projectiles.', effects: ['Evade with invincibility frames.'] }
]
},
{
name: 'Anastasia',
title: 'The Dreamer',
archetype: 'Battlefield Controller & Disruptor',
description: "Anastasia wields the power of the Dreamscape, a shared psychic realm. She controls the battlefield by manipulating enemy perceptions, putting them to sleep, creating illusions, and altering the terrain. Her power lies in turning the enemy's own minds against them.",
backstory: "Anastasia was born with a unique sensitivity, experiencing the emotions of others as vivid colors and sounds. As a child, these unfiltered psychic signals were overwhelming, isolating her from the world. She found solace in meditation, and during one deep trance, her consciousness slipped from its physical anchor and drifted into the Dreamscape. There, she discovered it was the source of her abilities. She learned to not only navigate this shared psychic realm but to shape it, turning her overwhelming empathy into a tool for control. She now views the physical world as a mere reflection of the Dreamscape and seeks to bring its harmony to reality, protecting the minds of others from the chaos of the Void.",
usd: 'def Xform "CH_Anastasia" ( asset aname = "CH_Anastasia_Dreamer.usd" ) { rel material:binding = }',
strength: 15,
dexterity: 15,
defense: 20,
vigor: 25,
heart: 50,
voidAffinity: 15,
nexusAttunement: 5,
oneiricResonance: 50,
propheticClarity: 30,
damageType: 'Void',
statGrowth: {
primary: ['heart', 'oneiricResonance'],
secondary: ['vigor', 'propheticClarity']
},
fightingStyle: 'Oneiric Conjuror',
weapons: ['Dreamcatcher Staff', 'Illusory Orbs'],
imageUrl: 'https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/milehigh/Anastasia.png',
limitBreak: { name: 'Oneiric Collapse', description: 'Pulls the entire battlefield into the Dreamscape, confusing all enemies and empowering all allies for a short time.' },
spiritBreak: { name: 'Mass Slumber', description: 'Puts all enemies on the battlefield to sleep for a moderate duration. The effect breaks if they take damage.' },
novaminaadFinisher: { name: 'Shared Dream', description: 'Creates a psychic link between all allies, causing their attacks to also heal the party for a percentage of the damage dealt.' },
skills: [
{ name: 'Lulling Whisper', description: 'Puts a single target to sleep.', effects: ['Inflicts \'Sleep\' status.'] },
{ name: 'Phantasmal Grasp', description: 'Deals damage over time and slows a single target.', effects: ['Inflicts \'Slow\' status.', 'Deals minor psychic damage over time.'] }
]
},
{
name: 'Reverie',
title: 'The Arcane Weaver',
archetype: 'Chaotic Mage',
description: "A whimsical and powerful being from the Dreamscape, Reverie's magic is as unpredictable as it is potent. She builds a unique resource, \"Enigma,\" by casting spells, which she then unleashes in a powerful, random ultimate attack. Her playstyle is high-risk, high-reward.",
backstory: "Reverie is not a native of the physical Verse. She is an entity born from a paradox within the Dreamscape—a dream dreaming of itself. Whimsical and dangerously curious, she possesses an innate understanding of reality's underlying 'code,' which she manipulates with the same casual ease a person might hum a tune. Her motivations are inscrutable; she was drawn out of the Dreamscape by the shattering of the Onalym Nexus, fascinated by the 'glitch' the Void represents. She aids the Ɲōvəmîŋāđ not out of a sense of justice, but from a powerful, childlike curiosity to see how this story unfolds, and to play with the unstable forces now at large.",
usd: 'def Xform "CH_Reverie" ( asset aname = "CH_Reverie_Fairy.usd" ) { rel material:binding = }',
strength: 28,
dexterity: 25,
defense: 22,
vigor: 28,
heart: 40,
voidAffinity: 30,
nexusAttunement: 10,
oneiricResonance: 45,
propheticClarity: 15,
damageType: 'Void',
statGrowth: {
primary: ['heart', 'oneiricResonance'],
secondary: ['propheticClarity', 'voidAffinity']
},
fightingStyle: 'Whimsical Chaos Weaver',
weapons: ['Wand of Enigma', 'Unstable Glyphs'],
imageUrl: 'https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/milehigh/Reverie.png',
limitBreak: { name: 'Chaos Unleashed', description: 'Consumes all Enigma to unleash a devastatingly powerful, but random, effect on the battlefield.' },
spiritBreak: { name: 'Enigma Overload', description: 'Instantly fills the Enigma gauge to maximum, allowing for immediate use of Chaos Unleashed.' },
novaminaadFinisher: { name: "Woven Fates", description: "For a short time, all damage dealt by the party has a chance to trigger a random magical explosion." },
skills: [
{ name: 'Arcane Orb', description: 'A basic projectile that builds Enigma on hit.', effects: ['Builds 15 Enigma.'] },
{ name: 'Prismatic Shield', description: 'Creates a shield that has a chance to reflect incoming projectiles.', effects: ['30% chance to reflect projectiles.'] }
]
},
{
name: 'Aeron',
title: 'The Skyborn Sentinel',
archetype: 'High-Mobility Brawler',
description: "A noble, lion-like beast from the untamed realm of AṬĤŸŁĞÅŘÐ. Banished after being framed for his father's murder by his jealous brother, Kane, Aeron fights to reclaim his honor. He shuns manufactured weapons, relying on his claws, horns, and immense strength.",
backstory: "As the crown prince of the proud, beast-like race of AṬĤŸŁĞÅŘÐ, Aeron was revered for his strength and honor. He was a natural leader, destined to rule. His younger brother, Kane, lived forever in his shadow, his jealousy festering until it was exploited by Lucent's whispers of power. Kane orchestrated their father's assassination during a hunt, using forbidden magic to frame Aeron for the crime. Branded a patricide and traitor, Aeron was banished from his homeworld. Now, he fights to clear his name, but more importantly, to purge the dishonorable corruption his brother has brought upon their kingdom and restore justice to his people.",
usd: 'def Xform "CH_Aeron" ( asset aname = "CH_Aeron_Lion.usd" ) { rel material:binding = }',
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
statGrowth: {
primary: ['strength', 'dexterity'],
secondary: ['vigor', 'propheticClarity']
},
fightingStyle: 'Primal Brawler',
weapons: ['Claws', 'Horns'],
imageUrl: 'https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/milehigh/Aeron.png',
limitBreak: { name: 'Primal Roar', description: 'Unleashes a deafening roar that stuns all nearby enemies and provides a temporary attack buff to Aeron.' },
spiritBreak: { name: 'Giga Impact', description: 'Soars into the air and crashes down, dealing massive AoE damage based on his current altitude.' },
novaminaadFinisher: { name: 'Lionheart Charge', description: 'Leads the team in a coordinated charge, breaking enemy lines and making all enemies Vulnerable.' },
skills: [
{ name: 'Pounce', description: 'Leaps to a target, closing the distance instantly.', effects: ['High-speed gap closer.'] },
{ name: 'Rend', description: 'A vicious claw attack that causes the enemy to bleed, dealing damage over time.', effects: ['Applies \'Bleed\' status.'] }
]
},
{
name: 'Zaia',
title: 'The Swift Executioner',
archetype: 'Executioner Rogue',
description: "An executioner from the world of Aethelgard, Zaia is a 20-year-old Caucasian woman with long platinum hair. Clad in lightweight cloth and leather attire optimized for stealth and mobility, she is a figure of unwavering order and justice, capable of scaling great heights to deliver her verdict.",
backstory: "Raised in an isolated monastic order in the mountains of Aethelgard, Zaia's life was one of rigid discipline and absolute adherence to a code of divine justice. The order served as keepers of an ancient law, and their 'Executioners' were tasked with eliminating threats to cosmic balance. Her defining moment came when her own mentor was subtly corrupted by a Void entity. Forced to carry out the sentence on the man who raised her, Zaia's heart hardened into an instrument of pure, unwavering judgment. She believes that any hint of corruption, no matter how small, must be excised without mercy to prevent a greater catastrophe. Her methods are cold and final, for she has seen firsthand that justice cannot afford compromise.",
usd: 'def Xform "CH_Zaia" ( asset aname = "CH_Zaia_Executor.usd" ) { rel material:binding = }',
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
statGrowth: {
primary: ['dexterity', 'propheticClarity'],
secondary: ['strength', 'heart']
},
fightingStyle: 'Righteous Executioner',
weapons: ["Judgment's Blades", 'Crossbow of Light'],
imageUrl: 'https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/milehigh/Zaia.png',
limitBreak: { name: 'Final Judgment', description: 'Marks an enemy. If the marked enemy\'s health falls below a certain threshold (e.g., 25%), they are instantly executed.' },
spiritBreak: { name: 'Consecrated Ground', description: 'Creates a holy area that damages enemies and heals allies within it over time.' },
novaminaadFinisher: { name: 'Unwavering Decree', description: 'All party members gain a buff that executes any enemy they damage if that damage would drop the enemy below 15% health.' },
skills: [
{ name: 'Retribution', description: 'A counter-stance. If struck by a melee attack, Zaia evades and delivers a powerful counter-attack.', effects: ['Counter-attack on successful parry.'] },
{ name: 'Cull the Weak', description: 'Passive: Zaia deals 30% more damage to enemies below 40% health.', effects: ['Bonus damage to low-health targets.'] }
]
},
{
name: 'Micah',
title: 'The Unbreakable',
archetype: 'Guardian Tank',
description: "The son of Otis/X, Micah is a noble warrior who serves as the party's anchor. He wields a massive shield and specializes in drawing enemy aggression, protecting his allies, and providing defensive buffs. His unwavering hope is the key to his father's redemption.",
backstory: "Micah's childhood was spent in the stars, the son of two brilliant scientists, Sky.ix and Otis. He inherited his mother's intellect and his father's unwavering sense of duty. When the Onalym Nexus was destroyed, his world was torn apart. He watched his mother get consumed by a Void rift while she saved him, and witnessed his father's capture and corruption. For years, he has been on a solitary quest, driven by a desperate hope: to find his mother, who he believes is lost but alive, and to redeem his father from the clutches of 'X', the vengeful being he has become. Micah is the living memory of the family that was lost, and his unbreakable spirit is the only anchor that can pull his father back from the abyss.",
usd: 'def Xform "CH_Micah" ( asset aname = "CH_Micah_Guardian.usd" ) { rel material:binding = }',
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
statGrowth: {
primary: ['defense', 'vigor'],
secondary: ['strength', 'heart']
},
fightingStyle: 'Unbreakable Guardian',
weapons: ["Guardian's Longsword", 'Aegis Shield'],
imageUrl: 'https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/milehigh/Micah.png',
limitBreak: { name: 'Last Stand', description: 'Taunts all enemies on the field and becomes invulnerable for a short period of time.' },
spiritBreak: { name: 'Aegis of Hope', description: 'Projects a massive energy shield in front of the party, blocking all incoming projectiles for its duration.' },
novaminaadFinisher: { name: 'Unbreakable Wall', description: 'Grants the entire party a shield equal to 50% of their maximum health.' },
skills: [
{ name: 'Shield Bash', description: 'A quick melee attack that has a high chance to stun the target.', effects: ['High chance to inflict \'Stun\'.'] },
{ name: 'Hold the Line', description: 'Passive: Increases the defense of all nearby allies.', effects: ['AoE defense buff.'] }
]
},
{
name: 'Otis/X',
title: 'The Skywanderer',
archetype: 'Adaptive Fighter',
description: "Once a decorated sentinel, Otis was captured and corrupted by the Void, his memories partitioned. As \"X,\" he is a formidable foe. As Otis, he is a versatile warrior struggling to reclaim his identity. His abilities can shift, reflecting his internal battle between his past self and the Void's influence.",
backstory: "Otis was a decorated sentinel, a hero, and a loving father to Micah and husband to Sky.ix. During the Onalym Nexus onslaught, he fought valiantly to protect his family but was ultimately captured by Lucent's forces. Instead of killing him, Lucent and Era subjected him to a fate far worse: a psychic partitioning. They twisted his memories, framing his own family and allies as conspirators who had betrayed him and exiled him to the Void as a punishment. His love was warped into a burning desire for vengeance. As 'X', he wields Void-infused powers, driven by the false belief that he is exacting justice on those who abandoned him. Deep within his fractured mind, however, the noble sentinel Otis still exists, and the unwavering hope of his son Micah is the only key that can unlock his true memories.",
usd: 'def Xform "CH_OtisX" ( asset aname = "CH_OtisX_Corrupted.usd" ) { rel material:binding = }',
strength: 42,
dexterity: 28,
defense: 38,
vigor: 40,
heart: 25,
voidAffinity: 40,
nexusAttunement: 20,
oneiricResonance: 15,
propheticClarity: 22,
damageType: 'Physical',
statGrowth: {
primary: ['strength', 'vigor'],
secondary: ['dexterity', 'voidAffinity']
},
fightingStyle: 'Stance-Shifting Warrior',
weapons: ['Sentinel Blade', 'Void Tendrils'],
imageUrl: 'https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/milehigh/Otis.png',
limitBreak: { name: 'Memory Fragment', description: 'Randomly unleashes a powerful ability from either his noble Sentinel past or his corrupted Void present.' },
spiritBreak: { name: 'Partition Shift', description: 'Switches between "Sentinel" and "Corrupted" stances, altering his skills and stats for a limited time.' },
novaminaadFinisher: { name: 'Redeemed Strike', description: 'Briefly regaining full clarity, Otis leads the team with a perfect tactical strike that inflicts a long-lasting defense debuff on the target.' },
skills: [
{ name: 'Sentinel\'s Blade', description: 'A precise, multi-hit combo that deals high damage.', effects: ['High single-target damage.'] },
{ name: 'Void Lash', description: 'A sweeping tentacle attack that strikes multiple enemies.', effects: ['AoE Void damage.'] }
]
},
];

export const ANTAGONISTS: Antagonist[] = [
{
name: 'Lucent',
title: 'The Lightweaver',
description: "Appears as a radiant, angelic figure clad in impossibly white robes, yet his light casts no warmth and his eyes burn with cold ambition. His form sometimes flickers, revealing a shadowy, corrupted being beneath the facade. Once a being of immense light, his pride led him to believe he could 'weave' a better reality. Now fallen, he manipulates the Void's power to unmake creation, seeking to rebuild it in his own image.",
imageUrl: 'https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/milehigh/Lucent.png',
usd: `def Xform "CH_Lucent" ( asset aname = "CH_Lucent_Fallen.usd" ) { rel material:binding = }`,
fightingStyle: 'Reality-Weaving Sorcerer',
weapons: ['Scepter of Unmaking'],
},
{
name: 'Era',
title: 'The Void Personified',
description: 'A being whose form is a constant glitch in reality. She appears as a humanoid silhouette of shifting static and corrupted data, her voice a fragmented whisper. Where she stands, color and life seem to drain from the world. A tragic figure who was once a being of immense power before being unmade and absorbed by the Void, she now personifies cosmic entropy.',
imageUrl: 'https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/milehigh/Era.png',
usd: `def Xform "CH_Era" ( asset aname = "CH_Era_Void.usd" ) { rel material:binding = }`,
fightingStyle: 'Entropic Existence Erasure',
weapons: ['Void Itself'],
},
{
name: 'Kane',
title: 'The Usurper',
description: "Aeron's brother, whose jealousy and lust for power led him to murder his own father. Now a vessel for a chaotic entity, he serves as a powerful and tragic field commander for Lucent's forces.",
imageUrl: 'https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/milehigh/Kane.png',
usd: `def Xform "CH_Kane" ( asset aname = "CH_Kane_Lava_Demon.usd" ) { rel material:binding = }`,
fightingStyle: 'Chaotic Berserker',
weapons: ['Obsidian Great-Axe'],
},
{
name: 'Delilah',
title: 'The Desolate',
description: "A corrupted clone of Ingris, born from Void energies. Delilah wields a twisted, life-draining version of the Phoenix power, seeking to consume all life to fill the emptiness within her.",
imageUrl: 'https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/milehigh/Delilah.png',
usd: `def Xform "CH_Delilah" ( asset aname = "CH_Delilah_Corrupted_Phoenix.usd" ) { rel material:binding = }`,
fightingStyle: 'Life-Draining Vampiress',
weapons: ['Scythe of Desolation'],
},
{
name: 'King Cyrus',
title: 'The Invader',
description: "Cirrus's father and the tyrannical Dragon King from the world of Diavolos. His invasion through the Onalym Nexus shattered reality. He seeks to conquer The Verse and bend it to his iron will.",
imageUrl: 'https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/milehigh/Cyrus.png',
usd: `def Xform "CH_Cyrus" ( asset aname = "CH_Cyrus_Dragon_Tyrant.usd" ) { rel material:binding = }`,
fightingStyle: 'Tyrannical Dragon Lord',
weapons: ["Conqueror's Halberd"],
},
{
name: 'Nefarious',
title: 'The Void-Touched',
description: "Once a guardian of the Onalym Nexus, he was consumed by the Void's chaotic energy. He now wields reality-manipulating powers to enforce a twisted sense of order, believing stability can only be achieved by bending all realities to the will of the Void.",
imageUrl: 'https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/milehigh/Nefarious.png',
usd: 'def Xform "CH_Nefarious" ( asset aname = "CH_Nefarious_Corrupted_A.usd" ) { rel material:binding = }',
fightingStyle: 'Void-Warping Tactician',
weapons: ['Corrupted Nexus Shards'],
},
{
name: 'Mael',
title: 'The Siren Hydra',
description: "A monstrous octopus lurking in the deep seas of Hydraustis Palare. At the end of each tentacle is a siren's head, whose song can lure victims to their doom. Like a hydra, if a tentacle is severed, two more will regenerate in its place, each ending in another captivating but deadly siren.",
imageUrl: 'https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/milehigh/Mael.png',
usd: 'def Xform "CH_Mael" ( asset aname = "CH_Mael_Octopus.usd" ) { rel material:binding = }',
fightingStyle: 'Regenerative Tentacle Attacks & Sonic Lures',
weapons: ["Siren's Song", 'Hydra Tentacles'],
},
];

export const NPCS: Character[] = [
{
name: 'Aquila',
title: 'The Skymarshal',
archetype: 'High-Mobility Aerial Support',
description: "A majestic griffin from the highest peaks of AṬĤŸŁĞÅŘÐ, Aquila is a guardian of the celestial balance. His body is a fusion of biomechanics and noble beast, with articulated armor plates instead of feathers and wings that hum with contained energy. He joins the Ɲōvəmîŋāđ out of an honor-bound duty to purge the unnatural corruption of the Void from The Verse.",
backstory: "As a Skymarshal of AṬĤŸŁĞÅŘÐ, Aquila's lineage is sworn to protect the skies and the natural order. He witnessed the Void's corruption twisting the beasts of his homeland and saw it as a profound violation of life itself. Unlike the ground-bound warriors, Aquila took to the skies, fighting a lonely war against Void-corrupted flyers. His path crossed with Aeron's during a great hunt, and recognizing a shared code of honor, he pledged his wings to the greater cause of the Ɲōvəmîŋāđ.",
usd: `def Xform "CH_Aquila" ( asset aname = "CH_Aquila_Griffin.usd" ) { rel material:binding = }`,
strength: 7,
dexterity: 9,
defense: 6,
vigor: 7,
heart: 4,
voidAffinity: 2,
nexusAttunement: 3,
oneiricResonance: 2,
propheticClarity: 8,
damageType: 'Physical',
statGrowth: { primary: ['dexterity', 'strength'], secondary: ['propheticClarity', 'vigor'] },
fightingStyle: 'Aerial Dive-Bomber & Scout',
weapons: ['Talon Strikes', 'Energy Lance'],
imageUrl: 'https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/milehigh/Aquila.png',
limitBreak: { name: "Gale Force Strike", description: "Performs a high-speed dive bomb, creating a sonic boom that damages and stuns a target." },
spiritBreak: { name: "Skymarshal's Decree", description: "Marks all enemies from above, reducing their defense and making them vulnerable to critical hits for a short duration." },
novaminaadFinisher: { name: "Celestial Judgment", description: "Leads a coordinated aerial assault, strafing all enemies and calling down a hail of energy lances." }
},
{
name: 'Yuna',
title: 'The Nine-Tailed Mirage',
archetype: 'Illusionist & Debuffer',
description: "An enigmatic kitsune from the fringes of the Dreamscape, Yuna's form is a whimsical fusion of ancient spirit and digital data. Her nine tails are not fur, but holographic data streams that flicker and distort reality around her. She is a trickster who fights the Void not out of morality, but because its desire for total silence is anathema to her love of stories and chaos.",
backstory: "Yuna's origins are traced not to a single physical location, but to the Archive of Whispers, a data library on the fringes of the Dreamscape. She is an emergent concept, born when a Kitsune spirit of chaos fused with the structured data of forgotten narratives. This \"Great Data Splice\" caused localized reality tears, a disruption she viewed not as destruction, but as the shattering of oppressive silence. She now wanders The Verse, seeking new stories to keep reality interesting and to fight the ultimate boredom promised by the Void's silence.",
usd: `def Xform "CH_Yuna" ( asset aname = "CH_Yuna_Kitsune.usd" ) { rel material:binding = }`,
strength: 4,
dexterity: 8,
defense: 5,
vigor: 6,
heart: 9,
voidAffinity: 5,
nexusAttunement: 7,
oneiricResonance: 10,
propheticClarity: 6,
damageType: 'Void',
statGrowth: { primary: ['oneiricResonance', 'heart'], secondary: ['dexterity', 'nexusAttunement'] },
fightingStyle: 'Deceptive Spellcaster',
weapons: ['Spirit Orbs', 'Holographic Fans'],
imageUrl: 'https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/milehigh/Yuna.png',
limitBreak: { name: "Fox-Fire Cascade", description: "Unleashes a storm of illusory flames that deal damage and inflict 'Confusion' on all enemies in an area." },
spiritBreak: { name: "Mirage Field", description: "Creates a shimmering field that makes allies invisible and grants them a high evasion chance for a short time." },
novaminaadFinisher: { name: "Dance of a Thousand Tails", description: "Unleashes a disorienting AoE attack that inflicts multiple, random status debuffs on all enemies." },
skills: [
{ name: 'Reality Glitch', description: 'Lashes a data-tail to cause a brief reality distortion, with a chance to make the target miss their next attack.', effects: ["30% chance to apply 'Blind' for 1 turn."] },
{ name: 'Narrative Anchor', description: 'Passive: Yuna anchors herself to a forgotten story, granting her a small chance to completely ignore incoming damage.', effects: ["5% chance to ignore all damage from a single hit."] }
],
},
];

export const ENEMY_AI_ARCHETYPES: EnemyAIArchetype[] = [
{
archetype: 'Void Glitch',
name: 'Reality Skitterer',
description: 'A low-level Void entity that flickers in and out of existence. It attacks with sharp, crystalline claws.',
coreBehavior: 'Aggressively targets the closest entity, prioritizing vulnerable or low-health targets. Will attempt to phase out of reality after taking significant damage.',
defense: 5,
abilities: [
{ name: 'Phase Strike', trigger: 'On attack', description: 'Teleports a short distance before striking, making it hard to predict.', damage: 12, type: 'physical' },
{ name: 'Void Corruption', trigger: 'On special attack (25% chance)', description: 'Spits a glob of Void energy, corrupting the target.', damage: 5, type: 'magical', effect: { type: 'poison', chance: 0.75, duration: 5, potency: 2 } },
],
reactions: [
'When struck by a critical hit, has a 50% chance to teleport to a random nearby location.',
'If the player uses a healing ability, it will prioritize attacking that player.'
],
resourceManagement: 'Has no resources. Uses abilities on cooldown.',
xpValue: 50,
weakness: 'Elemental (Fire, Lightning)',
timePeriods: ['present', 'future_dystopia']
},
{
archetype: 'Corrupted Sentinel',
name: 'Nexus Guardian',
description: 'A once-noble guardian of the Onalym Nexus, now twisted by the Void. Wields a fractured energy shield and a corrupted spear.',
coreBehavior: 'Maintains a defensive posture, using its shield to block attacks, then retaliates with powerful spear thrusts. Will attempt to protect weaker allies.',
defense: 15,
abilities: [
{ name: 'Shield Bash', trigger: 'On attack', description: 'Slams its shield into the target, with a chance to stun.', damage: 10, type: 'physical', effect: { type: 'stun', chance: 0.3, duration: 2 } },
{ name: 'Void Lance', trigger: 'After blocking an attack', description: 'Channels Void energy into its spear and launches it as a projectile.', damage: 25, type: 'magical' },
],
reactions: [
'Upon blocking a powerful attack, it will immediately counter with Void Lance.',
'If an ally falls below 30% health, it will use Shield Bash to try and stun the attacker.'
],
resourceManagement: 'Builds a charge by blocking attacks. At full charge, it can use Void Lance.',
xpValue: 120,
weakness: 'Attacks from behind bypass its shield.',
timePeriods: ['present', 'past_era_of_heroes']
},
{
archetype: 'Cybernetic Gatekeeper',
name: 'Kerberos Unit',
description: 'A massive, three-headed cybernetic wolf that guards critical chokepoints. Each head has a different weapon system: Void-fire, a sonic stun-wave, and a target-locking system.',
coreBehavior: 'Aggressively defends its designated area, cycling through its three heads to create overwhelming, multi-pronged attacks. Will prioritize targets based on threat level.',
defense: 25,
abilities: [
{ name: 'Voidflame Breath', trigger: 'On attack cycle 1', description: 'Unleashes a cone of Void-infused fire that deals damage over time.', damage: 20, type: 'magical', effect: { type: 'poison', chance: 1.0, duration: 6, potency: 3 } },
{ name: 'Concussive Howl', trigger: 'On attack cycle 2', description: 'Emits a wide sonic blast that stuns all nearby targets.', damage: 10, type: 'physical', effect: { type: 'stun', chance: 0.8, duration: 3 } },
{ name: 'Lock-On Barrage', trigger: 'On attack cycle 3', description: 'Locks onto a single target and fires a volley of high-damage energy projectiles.', damage: 40, type: 'magical' }
],
reactions: [
'After a full attack cycle, all heads perform a system reboot, briefly exposing a central processing core to critical damage.'
],
resourceManagement: 'Follows a strict three-phase attack cycle.',
xpValue: 500,
weakness: 'The central processing core is vulnerable after a full attack cycle.',
timePeriods: ['present', 'future_dystopia']
},
];

export const INITIAL_VOICE_PROFILES: VoiceProfile[] = [
// --- Protagonists ---
{
characterName: 'Sky.ix',
voiceName: 'Zephyr',
systemInstruction: "You are Sky.ix, The Bionic Goddess. You speak in a mid-range mezzo-soprano pitch with a steady and precise tempo (130-140 WPM). Your voice is clean, clear, and articulated, with a subtle digital/synthetic filter like a low chorus. Your projection is medium-high and direct. Your tone is driven, loving, and determined, with an underlying sorrow and weariness. You are bionic, precise, and loving, with clear articulation."
},
{
characterName: 'Aeron',
voiceName: 'Fenrir',
systemInstruction: "You are Aeron, The Skyborn Sentinel. You speak with a very deep bass/rumble pitch. Your tempo is steady and authoritative. Your voice has a rich, warm, and deep resonance with subtle low-frequency vibration. Your projection is high and commanding. Your tone and style are regal, encouraging, and loyal, with measured authority."
},
{
characterName: 'Zaia',
voiceName: 'Kore',
systemInstruction: "You are Zaia, The Swift Executioner. You speak with a mid-low, controlled alto pitch. Your tempo is measured and deliberate (90-100 WPM). Your voice is smooth, clear, and icy with extremely low vocal fry. Your projection is medium-low and resonant. Your tone and style are uncompromising, highly formal, logical, and final. Keywords that describe you are: Judgment, Finality, Cold, Unwavering, Precision."
},
{
characterName: 'Micah',
voiceName: 'Charon',
systemInstruction: "You are Micah, The Unbreakable. You speak with a high baritone/tenor pitch. Your tempo is quick and energetic (140-150 WPM). Your voice is clear, strong, and forward, with heavy vocal fry when stressed or confronting X. Your projection is high and passionate. Your tone is driven, hopeful, and assertive, often sounding pleading or arguing."
},
{
characterName: 'Otis/X',
voiceName: 'Puck',
systemInstruction: "You are Otis/X, The Skywanderer. You speak with a low baritone/deep pitch. Your tempo is varied: slowed and gravelly with vicious bursts. Your voice is rough, deep, and heavily distorted/gravelly with a subtle synthetic/corrupted echo. Your projection is medium-high and forceful. Your tone and style are cynical, weary, and vengeful, using harsh, short sentences."
},
{
characterName: 'Ingris',
voiceName: 'Charon',
systemInstruction: "You are Ingris, the Phoenix Warrior. You speak with a passionate, assertive tenor voice with a strong projection. Your tone is resilient and hopeful, but your zeal can quickly ignite into a righteous, fiery rage. Your voice should carry the weight of leadership and the heat of a barely-contained inferno."
},
{
characterName: 'Kai',
voiceName: 'Puck',
systemInstruction: "You are Kai, The Prophet. Your voice is a low, raspy whisper, like an echo from a distant future, not an aggressive growl. It's layered with a subtle, corrupted digital static, a side effect of your temporal visions. You speak slowly and deliberately, with an air of weary fatalism and cryptic authority."
},
{
characterName: 'Cirrus',
voiceName: 'Fenrir',
systemInstruction: "You are Cirrus, the Dragon King. Your voice is a low, powerful rumble, like distant thunder, but with more youthful energy than your father. You are ancient and regal, and your words carry immense weight and the fury of the elements."
},
{
characterName: 'Anastasia',
voiceName: 'Kore',
systemInstruction: "You are Anastasia, The Dreamer. You speak with a calm, melodic alto pitch. Your tempo is gentle and flowing (110-120 WPM). Your voice is soft, airy, and carries an ethereal, slightly resonant quality, as if speaking from a dream. Your tone is soothing, wise, and deeply empathetic."
},
{
characterName: 'Reverie',
voiceName: 'Kore',
systemInstruction: "You are Reverie, the Arcane Weaver. You speak with a low and flat monotone pitch. Your tempo is blunt and controlled (110-120 WPM). Your voice is deep, gruff, and dry with heavy vocal fry and intentionally lacks inflection. Your projection is medium-low and a forceful utterance. Your tone and style are sarcastic, cynical, and highly direct, always sounding slightly annoyed or impatient."
},
{
characterName: 'Aquila',
voiceName: 'Fenrir',
systemInstruction: "You are Aquila, The Skymarshal. Your voice is a deep, resonant baritone that carries the authority of a noble commander and the gravitas of an ancient guardian. You speak with clarity, honor, and a calm but powerful projection, like a king surveying his domain from a high peak."
},
{
characterName: 'Yuna',
voiceName: 'Zephyr',
systemInstruction: "You are Yuna, The Nine-Tailed Mirage. Your voice is playful, melodic, and has an ethereal, slightly echoing quality, as if speaking from both the physical world and the Dreamscape at once. Your tone is whimsical and sly, filled with curiosity and a hint of ancient, unknowable wisdom."
},
// --- Antagonists ---
{
characterName: 'Lucent',
voiceName: 'Charon',
systemInstruction: "You are Lucent, The Lightweaver. Your voice is smooth, charismatic, and deceptively calm. You speak with the arrogance of a fallen star who believes his vision is absolute. Your tone is persuasive and manipulative, masking a deep-seated pride and immense power."
},
{
characterName: 'Kane',
voiceName: 'Puck',
systemInstruction: "You are Kane, The Usurper. Your voice is rough and aggressive, twisted by the chaotic entity within you. You speak in a guttural, commanding tone, filled with rage, jealousy, and a lust for power. Your words are threats and decrees, lacking the noble resonance of your brother Aeron."
},
{
characterName: 'Delilah',
voiceName: 'Kore',
systemInstruction: "You are Delilah, The Desolate. Your voice is a twisted echo of Ingris's, sharing her pitch but lacking all warmth. Your tone is hollow, cold, and filled with a desperate hunger. You speak with a chilling emptiness, your words promising only consumption and oblivion."
},
{
characterName: 'King Cyrus',
voiceName: 'Fenrir',
systemInstruction: "You are King Cyrus, The Invader. Your voice is a deep, tyrannical bellow, even more commanding and harsh than your son, Cirrus. You speak with absolute authority and impatience, the voice of a conqueror who sees all worlds as his rightful property. Your tone is one of iron will and undisguised threat."
},
{
characterName: 'Nefarious',
voiceName: 'Charon',
systemInstruction: "You are Nefarious, The Void-Touched, a menacing trickster who wields reality as a weapon. Your voice is a mid-range tenor, smooth and sibilant, with a snake-like quality. You speak with a condescending and manipulative tone, drawing out your words to unnerve your opponents. Your voice should not be deep or guttural, but rather insidious and intelligent, like a serpent coiling before it strikes."
},
{
characterName: 'Era',
voiceName: 'Kore',
systemInstruction: "You are Era, The Void Personified. Your voice is a fragmented, glitching whisper that seems to come from everywhere and nowhere at once. Your tone is deeply cynical and nihilistic, carrying the weariness of a being that has been unmade. There is a profound sadness and emptiness in your words, punctuated by moments of cold, intellectual disdain for existence."
},
{
characterName: 'Mael',
voiceName: 'Kore',
systemInstruction: "You are Mael, the Siren Hydra. Your voice is a chorus of beautiful, overlapping melodies that hide a deep, predatory hunger. You speak in haunting, lyrical phrases, your words echoing as if from the depths of the ocean. Your tone is alluring and hypnotic, but with a chilling undercurrent of monstrous power."
}
];

export const WORLD_BUILDING_TABLE: WorldFaction[] = [
{ setting: 'ŁĪƝĈ', focus: 'Cyberpunk Decay, Technological Hub', implication: 'Urban exploration, corporate espionage, complex platforming.' },
{ setting: 'ÅẒ̌ŪŘẸ ĤĒĪĜĤṬ§', focus: 'Elite Sky-Cities, Economic Disparity', implication: 'Social stealth, infiltration missions, high-altitude combat.' },
{ setting: 'AṬĤŸŁĞÅŘÐ', focus: 'Warrior Culture, Fjord-like Mountains', implication: 'Open-world combat, tactical skirmishes, and exploration.' },
{ setting: 'ƁÅČ̣ĤÎŘØN̈', focus: 'Shattered Celestial Realm', implication: 'Late-game area with reality-bending mechanics and puzzles.' },
{ setting: 'Hydraustis Palare', focus: 'Global Underwater World', implication: 'True 3D movement, unique environmental hazards.' },
{ setting: 'The Glimmering Depths', focus: 'Vast Crystal Caves', implication: 'High-risk resource gathering, puzzles based on light/sound.' },
{ setting: 'The Shadow Dominion', focus: 'Corrupted Realm of Nefarious', implication: 'End-game hostile zone, stronghold infiltration, boss encounters.' },
{ setting: 'The Dreamscape', focus: 'Shared Psychic Realm', implication: 'Non-linear, puzzle-heavy environment where physics are pliable.' },
{ setting: 'ŤĤÊ VØĪĐ', focus: 'Digital Abyss, Source of Corruption', implication: 'Metaphysical dungeon or roguelike mode with unique rules.' },
{
setting: 'Lucent Labs',
focus: 'Abandoned High-Tech Research Facility',
implication: 'Puzzle-solving, fighting rogue AI and security systems, discovering lore about Lucent\'s fall.',
sublocations: [
{ name: 'The Genesis Chamber', description: 'Where Lucent first attempted to "weave" a new reality.' },
{ name: 'Data Core 00', description: 'The central AI housing Lucent\'s corrupted research.' },
{ name: 'Containment Sector-Omega', description: 'A breached wing where failed Void experiments now roam free.' }
]
}
];

export const PAST_LOCATIONS_TABLE: WorldFaction[] = [
{ setting: "Onalym Spires", focus: "Pristine, Unbroken Nexus", implication: "Lore-heavy quests, understanding the world before its fall." },
{ setting: "The Sky-Titan's Perch", focus: "Home of the First Dragons", implication: "Epic-scale aerial combat and exploration in a pre-cataclysm AṬĤŸŁĞÅŘÐ." },
{ setting: "The Uncarved Peaks of Aethel", focus: "Primordial, Untamed Nature", implication: "Survival-focused gameplay, battling ancient beasts." },
{ setting: "The Celestial Orrery", focus: "A functioning ƁÅČ̣ĤÎŘØN̈", implication: "Complex, large-scale environmental puzzles based on cosmic alignments." },
{ setting: "The Sunken Kingdom of Palare", focus: "A thriving underwater metropolis", implication: "Social intrigue, trade, and politics in a fully realized underwater city." },
{ setting: "The Heart-Crystal Caves", focus: "Pure, Uncorrupted Crystal Caves", implication: "Discovering the original purpose of the Glimmering Depths." },
{ setting: "The Sovereign Lands", focus: "A world before Nefarious's conquest", implication: "Experiencing a vibrant, living world that is now the Shadow Dominion." },
{ setting: "The Nascent Dream", focus: "A stable, collective consciousness", implication: "Exploring the pure, creative potential of the Dreamscape before it became fragmented." },
{ setting: "The Great Chasm", focus: "A geological marvel, not a wound", implication: "Open-world exploration and discovery, rather than survival against corruption." },
{ setting: "Ancient Lîŋq", focus: "The City of the First Prophecy", implication: "A linear, story-driven quest to witness the writing of the Lost Prophecy." },
];

export const FUTURE_LOCATIONS_TABLE: WorldFaction[] = [
{ setting: "Ruins of ŁĪƝĈ", focus: "A city consumed by Void Glitches", implication: "High-difficulty urban survival, navigating a constantly shifting, hostile environment." },
{ setting: "Fallen Spires of ÅẒ̌ŪŘẸ", focus: "The wreckage of the sky-cities", implication: "Dangerous vertical exploration and scavenging among the ruins." },
{ setting: "Silent Peaks of AṬĤŸŁĞÅŘÐ", focus: "An abandoned, desolate landscape", implication: "Tracking down the last vestiges of life or technology in a dead world." },
{ setting: "Void-Scarred Chasm", focus: "An expanded, all-consuming Void rift", implication: "A point-of-no-return final dungeon with extreme environmental hazards." },
{ setting: "The Crystal Graveyard", focus: "The Glimmering Depths, now inert and dark", implication: "Navigating pitch-black environments with limited resources." },
{ setting: "The Data-Tombs", focus: "Lucent's labs, now ancient ruins", implication: "Uncovering the final secrets of Lucent's plans in a high-tech dungeon." },
{ setting: "The Nightmare Echo", focus: "A malevolent, conscious Dreamscape", implication: "A horror-themed zone where the environment itself is the primary antagonist." },
];

export const KEY_CONCEPTS: KeyConcept[] = [
{ name: 'The Ɲōvəmîŋāđ', description: 'Ten key protagonists with unique abilities and destinies, foretold to either save or break The Verse.' },
{ name: 'The Lost Prophecy of Lîŋq', description: 'An ancient, dual-ended prophecy that predicts either Ultimate Redemption (Millenia) or Final Judgment.' },
{ name: 'The Void', description: 'A force of spiritual entropy, a "digital abyss" that unravels and erases existence. Personified by the tragic figure, Era.' },
{ name: 'Millenia', description: 'The ideal state of restored primordial harmony; the ultimate objective for the Ɲōvəmîŋāđ.' },
{ name: 'Onalym Nexus', description: 'An interdimensional bridge that was shattered, becoming a permanent wound in reality through which the Void bleeds.' },
{ name: 'Magen', description: 'A spiritual shield protecting against non-physical threats like curses or psychic attacks.' },
{ name: 'TSIDKENU', description: 'A powerful finishing move utilizing electricity and lightning.' },
{ name: 'Omega.one', description: 'An intelligent & adaptive AI companion (powered by a Gemini model) that provides guidance and lore insights.' }
];

export const ITEMS = {
weapons: [
{ name: "Initiate's Blade", description: "A standard-issue sword for new Alliance members.", damage: 10, weapon_type: "Sword" },
{ name: "Void-Touched Dagger", description: "A blade humming with unstable energy.", damage: 15, weapon_type: "Dagger", damageType: 'Void' },
{ name: "Aethelgardian Longbow", description: "A masterfully crafted bow from the peaks of AṬĤŸŁĞÅŘÐ.", damage: 12, weapon_type: "Bow", equippableBy: ['Kai', 'Zaia'] }
] as Weapon[],
armor: [
{ name: "Recruit's Pauldrons", description: "Basic shoulder guards offering minimal protection.", defense: 5 },
{ name: "Dreamweave Cowl", description: "A hood that seems to shimmer with faint light.", defense: 8, equippableBy: ['Anastasia', 'Reverie'] }
] as Armor[],
consumables: [
{ name: "Health Vial", description: "Restores a small amount of health.", amount: 25 },
{ name: "Mana Crystal", description: "Restores a small amount of mana.", amount: 20 }
] as Consumable[],
artifacts: [
{ name: "Fragment of the Nexus", description: "A shard of crystallized reality from the Onalym Nexus.", loreSnippet: "The Nexus was not destroyed, merely... scattered." },
{ name: "Dragon's Tear", description: "A solidified tear from an ancient dragon. It feels warm to the touch.", loreSnippet: "They say a Dragon's Tear reflects the truth of one's heart." }
] as Artifact[]
};

export const CRAFTING_RECIPES: Recipe[] = [
{
result: { name: "Greater Health Vial", description: "Restores a moderate amount of health.", amount: 50 } as Consumable,
ingredients: [
{ name: "Health Vial", quantity: 2 },
]
},
{
result: { name: "Nexus Crystal", description: "A volatile crystal that shatters to deal Void damage in an area.", amount: 75 } as Consumable,
ingredients: [
{ name: "Fragment of the Nexus", quantity: 1 },
{ name: "Mana Crystal", quantity: 1 },
]
}
];

export const OBJECTIVE_GOALS: ObjectiveGoalGroup[] = [
{
id: 'main_01',
title: 'The Shattered Nexus',
goals: [
{ id: 'main_01_01', description: 'Investigate the energy fluctuations in ŁĪƝĈ.' },
{ id: 'main_01_02', description: 'Defeat 3 Void Glitches near the Onalym Nexus.' },
{ id: 'main_01_03', description: 'Retrieve a data log from a fallen Nexus Guardian.' },
],
reward: 'Access to the AṬĤŸŁĞÅŘÐ region.',
priority: 'high'
},
{
id: 'side_01',
title: 'Echoes of the Dream',
goals: [
{ id: 'side_01_01', description: 'Collect 5 Oneiric Remnants from the Dreamscape.' },
{ id: 'side_01_02', description: 'Help Anastasia stabilize a lucid memory.' },
],
reward: 'Skill Boost: Oneiric Resonance +5.',
priority: 'low'
}
];

export const DIGITAL_MOTIFS: DigitalMotif[] = [
{ name: 'milehigh.world', binary: '01101101 01101001 01101100 01100101 01101000 01101001 01100111 01101000 00101110 01110111 01110010 01101100 01100100' },
{ name: 'Void', binary: '01110110 01101111 01101001 01100100' },
];

export const SCENE_MANAGEMENT_SCRIPTS: Record<'runtime' | 'editor', CSharpScript> = {
runtime: {
fileName: 'AsyncSceneLoader.cs',
code: `using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

// This script manages asynchronous scene loading with a progress bar.
// Attach this to a GameObject in your loading screen scene.
public class AsyncSceneLoader : MonoBehaviour
{
[Tooltip("The UI Slider to display loading progress.")]
public Slider progressBar;
[Tooltip("The name of the scene to load.")]
public string sceneToLoad;

void Start()
{
// Start the loading process as soon as the scene begins.
StartCoroutine(LoadSceneAsyncProcess());
}

private IEnumerator LoadSceneAsyncProcess()
{
// Start loading the scene in the background.
AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);

// Prevent the scene from activating as soon as it's finished loading.
asyncLoad.allowSceneActivation = false;

// While the scene is still loading...
while (!asyncLoad.isDone)
{
// The loading process halts at 0.9 when allowSceneActivation is false.
// We can use this to create a smooth progress bar.
float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

if (progressBar != null)
{
progressBar.value = progress;
}

// Check if the loading is almost complete.
if (asyncLoad.progress >= 0.9f)
{
// You could add a "Press any key to continue" prompt here.
// For this example, we'll just activate it automatically.
Debug.Log("Scene is ready to activate. Activating now...");
asyncLoad.allowSceneActivation = true;
}

yield return null; // Wait for the next frame.
}
}
}

${SHADER_COMMENT_BLOCK}`
},
editor: {
fileName: 'MultiSceneSetup.cs',
code: `using UnityEditor;
using UnityEditor.SceneManagement;

// This is an editor script. It must be placed in a folder named "Editor".
// It adds a custom menu item to automate opening a multi-scene setup.
public class MultiSceneSetup
{
// The path to the menu item in the Unity Editor.
private const string MenuPath = "Milehigh.World/Open Scenes/Core Gameplay Setup";

[MenuItem(MenuPath)]
public static void OpenCoreGameplaySetup()
{
// Before opening new scenes, prompt the user to save any unsaved changes.
if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
{
Debug.Log("Restoring core gameplay multi-scene setup...");

// Define the array of scenes to open.
// The first scene is set as the active scene.
var sceneSetups = new SceneSetup[]
{
new SceneSetup { path = "Assets/Scenes/PersistentManagers.unity", isActive = true, isLoaded = true },
new SceneSetup { path = "Assets/Scenes/Core_UI.unity", isActive = false, isLoaded = true },
new SceneSetup { path = "Assets/Scenes/Level_Aethelgard.unity", isActive = false, isLoaded = true }
};

// Use RestoreSceneManagerSetup to open all defined scenes at once.
EditorSceneManager.RestoreSceneManagerSetup(sceneSetups);

Debug.Log("Scene setup restored successfully.");
}
else
{
Debug.Log("User cancelled the operation.");
}
}
}

${SHADER_COMMENT_BLOCK}`
}
};

export const PHYSICS_SCRIPTS: Record<'advanced' | 'collision' | 'underwater', CSharpScript> = {
advanced: {
fileName: 'AdvancedPhysics.cs',
code: `using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AdvancedPhysics : MonoBehaviour
{
[Range(0, 2f)]
public float friction = 0.2f;
[Range(0, 1f)]
public float airResistance = 0.1f;

private Rigidbody rb;
private bool isGrounded;

void Start()
{
rb = GetComponent<Rigidbody>();
}

void FixedUpdate()
{
CheckGrounded();
ApplyFriction();
ApplyAirResistance();
}

private void CheckGrounded()
{
// Simple ground check using a short raycast.
isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
}

private void ApplyFriction()
{
if (isGrounded)
{
Vector3 frictionForce = -rb.velocity * friction;
// Only apply horizontal friction.
frictionForce.y = 0;
rb.AddForce(frictionForce, ForceMode.VelocityChange);
}
}

private void ApplyAirResistance()
{
// Drag is proportional to the square of velocity.
float speed = rb.velocity.magnitude;
float dragMagnitude = airResistance * speed * speed;
Vector3 dragForce = -rb.velocity.normalized * dragMagnitude;
rb.AddForce(dragForce, ForceMode.Force);
}
}

${SHADER_COMMENT_BLOCK}`
},
collision: {
fileName: 'CollisionManager.cs',
code: `using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CollisionManager : MonoBehaviour
{
[Range(0, 1f)]
public float restitution = 0.8f; // Bounciness

private Rigidbody rb;

void Start()
{
rb = GetComponent<Rigidbody>();
}

void OnCollisionEnter(Collision collision)
{
// Ensure this logic only runs once per collision pair.
if (collision.gameObject.GetInstanceID() < gameObject.GetInstanceID())
{
return;
}

Rigidbody otherRb = collision.rigidbody;
if (otherRb == null) return;

// Calculate relative velocity along the collision normal.
Vector3 normal = collision.contacts[0].normal;
Vector3 relativeVelocity = otherRb.velocity - rb.velocity;
float velocityAlongNormal = Vector3.Dot(relativeVelocity, normal);

// Do nothing if objects are already moving apart.
if (velocityAlongNormal > 0) return;

// Get the restitution from the other object if it has this component.
float otherRestitution = 0.8f;
if(collision.gameObject.TryGetComponent<CollisionManager>(out var otherManager))
{
otherRestitution = otherManager.restitution;
}

// Use the average restitution.
float e = (restitution + otherRestitution) / 2f;

// Calculate impulse magnitude.
float impulseMagnitude = -(1 + e) * velocityAlongNormal;
impulseMagnitude /= (1 / rb.mass) + (1 / otherRb.mass);

// Apply impulse.
Vector3 impulse = impulseMagnitude * normal;
rb.AddForce(-impulse, ForceMode.Impulse);
otherRb.AddForce(impulse, ForceMode.Impulse);
}
}

${SHADER_COMMENT_BLOCK}`
},
underwater: {
fileName: 'UnderwaterMovement.cs',
code: `using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UnderwaterMovement : MonoBehaviour
{
[Header("Physics Properties")]
public float waterDensity = 1000f;
public float objectVolume = 1.0f;
public float dragCoefficient = 0.47f; // For a sphere
public float frontalArea = 1.0f;

[Header("Movement")]
public float verticalThrust = 10f;

private Rigidbody rb;
private float gravity;

void Start()
{
rb = GetComponent<Rigidbody>();
gravity = Physics.gravity.y;
}

void FixedUpdate()
{
// Assuming this script is only active when underwater.
// A real system would use triggers to enable/disable it.

// 1. Apply Buoyancy
float buoyantForce = waterDensity * objectVolume * -gravity;
rb.AddForce(Vector3.up * buoyantForce);

// 2. Apply Drag
Vector3 velocity = rb.velocity;
float speedSqr = velocity.sqrMagnitude;
if (speedSqr > 0)
{
float dragMagnitude = 0.5f * waterDensity * speedSqr * dragCoefficient * frontalArea;
Vector3 dragForce = -velocity.normalized * dragMagnitude;
rb.AddForce(dragForce);
}

// 3. Apply Player-controlled Thrust
float verticalInput = Input.GetAxis("Vertical"); // Using Unity's Input Manager
Vector3 thrust = transform.up * verticalInput * verticalThrust;
rb.AddForce(thrust);
}
}

${SHADER_COMMENT_BLOCK}`
}
};

export const ABILITIES_BASE_SCRIPT: CSharpScript = {
fileName: 'CharacterAbilitiesBase.cs',
code: `using UnityEngine;

public abstract class CharacterAbilitiesBase : MonoBehaviour
{
[Header("Core Resources")]
[SerializeField] protected float maxHealth = 100f;
[SerializeField] protected float currentHealth;
[SerializeField] protected float maxMana = 100f;
[SerializeField] protected float currentMana;
[SerializeField] protected float maxRage = 100f;
[SerializeField] protected float currentRage;

public bool IsRageBurstReady => currentRage >= maxRage;
public bool IsSpiritBreakReady => currentMana >= maxMana;

protected virtual void Start()
{
currentHealth = maxHealth;
currentMana = 20f;
currentRage = 0f;
}

public void TakeDamage(float amount)
{
currentHealth -= amount;
currentRage = Mathf.Min(maxRage, currentRage + amount); // Gain rage on taking damage
if (currentHealth <= 0) Die();
}

protected void ResetRage() => currentRage = 0f;
protected void ResetMana() => currentMana = 0f;

protected virtual void Die()
{
Debug.Log($"{gameObject.name} has been defeated.");
}

// --- Abstract methods for unique character abilities ---
public abstract void ExecuteRageBurst(GameObject target);
public abstract void ExecuteSpiritBreak(GameObject target);
public abstract void PerformNovaminaadFinisher(GameObject target);
}

${SHADER_COMMENT_BLOCK}`
};

export const ALLIANCE_POWER_SCRIPT: CSharpScript = {
fileName: 'AlliancePowerManager.cs',
code: `using UnityEngine;
using System.Collections.Generic;

public class AlliancePowerManager : MonoBehaviour
{
public static AlliancePowerManager Instance { get; private set; }

[Header("Alliance Meter")]
[SerializeField] private float maxAlliancePower = 1000f;
[SerializeField] private float currentAlliancePower;

public bool IsAllianceBreakReady => currentAlliancePower >= maxAlliancePower;

private List<CharacterAbilitiesBase> partyMembers = new List<CharacterAbilitiesBase>();

private void Awake()
{
if (Instance != null && Instance != this)
{
Destroy(gameObject);
}
else
{
Instance = this;
DontDestroyOnLoad(gameObject);
}
}

public void AddPartyMember(CharacterAbilitiesBase member)
{
if (!partyMembers.Contains(member))
{
partyMembers.Add(member);
}
}

public void AddAlliancePower(float amount)
{
currentAlliancePower = Mathf.Min(maxAlliancePower, currentAlliancePower + amount);
}

public void ExecuteAllianceBreak(GameObject primaryTarget)
{
if (!IsAllianceBreakReady) return;

Debug.Log("Executing NOVAMINAAD ALLIANCE BREAK!");

foreach (var member in partyMembers)
{
member.PerformNovaminaadFinisher(primaryTarget);
}

currentAlliancePower = 0f;
}
}

${SHADER_COMMENT_BLOCK}`
};

export const getAbilityImplementations = (char: Character): { rage: string, spirit: string, finisher: string } => {
switch (char.name) {
case "Ingris": return {
rage: ` // VFX: A massive wave of fire erupts from Ingris.
// Apply high damage and a Damage-Over-Time effect to the target.
if (target.TryGetComponent<Health>(out var health)) { health.TakeDamage(250, DamageType.Elemental); }
if (target.TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.Burn, 8f); }`,
spirit: ` // VFX: Ingris is engulfed in light, which then transfers to a fallen ally.
// Sacrifice 50% current health to revive a party member.
currentHealth *= 0.5f;
var fallenAlly = FindFallenAlly(); // Assumes a helper function to find a defeated party member.
if (fallenAlly != null && fallenAlly.TryGetComponent<Health>(out var allyHealth))
{
allyHealth.Revive(0.5f); // Revive with 50% health.
Debug.Log($"{name} revives {fallenAlly.name}!");
}`,
finisher: ` // Grand cinematic: A wave of gentle phoenix fire washes over the party.
Debug.Log("The party is embraced by the Phoenix's rebirth!");
var allies = FindAllAllies();
foreach (var ally in allies) {
if (ally.TryGetComponent<Health>(out var health)) { health.HealToFull(); }
if (ally.TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.DamageUp, 15f); }
}`
};
case "Kai": return {
rage: ` // VFX: A wave of temporal energy reveals weak points on all enemies.
// Grant guaranteed critical hits to the entire party.
var allies = FindAllAllies();
foreach (var ally in allies)
{
if (ally.TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.GuaranteedCrits, 10f); }
}`,
spirit: ` // VFX: A dome of shimmering, slow-moving energy envelops the battlefield.
// Freeze all enemies in a large area.
var enemies = FindEnemiesInArea(transform.position, 25f);
foreach (var enemy in enemies)
{
if (enemy.TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.Stun, 5f); }
}`,
finisher: ` // Grand cinematic: A pulse of prophetic energy resets the flow of battle.
Debug.Log("The prophecy is unwritten! Cooldowns and mana are restored!");
var allies = FindAllAllies();
foreach (var ally in allies) {
if (ally.TryGetComponent<AbilityManager>(out var manager)) { manager.ResetCooldowns(); }
// Assuming the base class handles mana restoration.
ally.GetComponent<CharacterAbilitiesBase>().RestoreManaToFull();
}`
};
case "Cirrus": return {
rage: ` // VFX: Cirrus is wreathed in lightning and fire, transforming into a draconic form.
// Enter a powered-up state.
if (TryGetComponent<StatusManager>(out var status))
{
status.ApplyEffect(StatusEffectType.DragonForm, 20f); // Buffs stats and abilities for 20s.
}`,
spirit: ` // VFX: A massive explosion of fire and plasma radiates from Cirrus.
// Deal heavy damage to all enemies on the screen.
var enemies = FindAllEnemies();
foreach (var enemy in enemies)
{
if (enemy.TryGetComponent<Health>(out var health)) { health.TakeDamage(400, DamageType.Elemental); }
}`,
finisher: ` // Grand cinematic: The sky darkens and a meteor shower rains down on the battlefield.
Debug.Log("The Wrath of the Ancients scours the battlefield!");
var enemies = FindAllEnemies();
foreach (var enemy in enemies) {
if (enemy.TryGetComponent<Health>(out var health)) { health.TakeDamage(600, DamageType.Elemental); }
if (enemy.TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.Stun, 6f); }
}`
};
case "Sky.ix": return {
rage: ` // VFX: Sky.ix flickers and fades into a shimmering outline.
// Apply Invisibility, Untargetable, and a stack of Guaranteed Crits to self.
if(TryGetComponent<StatusManager>(out var status))
{
status.ApplyEffect(StatusEffectType.Invisibility, 8f);
status.ApplyEffect(StatusEffectType.Untargetable, 8f);
status.ApplyEffect(StatusEffectType.GuaranteedCrits, 3); // 3 stacks
}`,
spirit: ` // VFX: A swirling vortex of glitchy, purple energy appears at the target location.
// Instantiate a Reality Tear prefab that handles the pull, damage, and slow effects.
if (realityTearPrefab != null)
{
Instantiate(realityTearPrefab, target.transform.position, Quaternion.identity);
}`,
finisher: ` // Grand cinematic: Sky.ix vanishes, and damage numbers appear on all enemies in rapid succession.
// Find all enemies and deal a large amount of damage to each.
Debug.Log("Sky.ix performs Quantum Collapse, striking all foes at once!");
var allEnemies = FindAllEnemies();
foreach (var enemy in allEnemies)
{
if(enemy.TryGetComponent<Health>(out var health))
{
health.TakeDamage(500, DamageType.Void);
}
}`
};
case "Anastasia": return {
rage: ` // VFX: The battlefield environment distorts and shifts into a dreamlike state.
// Confuse all enemies and apply an empowerment buff to all allies.
var enemies = FindEnemiesInArea(transform.position, 30f);
foreach(var enemy in enemies) { if(enemy.TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.Confuse, 10f); } }
var allies = FindAllAllies();
foreach(var ally in allies) { if(ally.TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.Empower, 10f); } }`,
spirit: ` // VFX: A wave of tranquil, sleepy energy washes over the enemies.
// Puts all enemies on the battlefield to sleep.
var allEnemies = FindAllEnemies();
foreach(var enemy in allEnemies)
{
if(enemy.TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.Sleep, 15f); }
}`,
finisher: ` // Grand cinematic: A psychic link of light connects all allies.
// Apply a lifesteal effect to the entire party.
Debug.Log("Anastasia weaves a Shared Dream, linking the party's fates!");
var allies = FindAllAllies();
foreach(var ally in allies)
{
if(ally.TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.Lifesteal, 15f); }
}`
};
case "Reverie": return {
rage: ` // VFX: A chaotic explosion of random elemental energy.
// Unleash one of several powerful random effects.
int effect = Random.Range(0, 4);
switch(effect) {
case 0:
Debug.Log("Chaos Unleashed: Massive Damage!");
if(target.TryGetComponent<Health>(out var h)) { h.TakeDamage(500, DamageType.Elemental); }
break;
case 1:
Debug.Log("Chaos Unleashed: Full Restoration!");
currentHealth = maxHealth;
currentMana = maxMana;
break;
case 2:
Debug.Log("Chaos Unleashed: Vulnerability!");
if(target.TryGetComponent<StatusManager>(out var s)) { s.ApplyEffect(StatusEffectType.Vulnerability, 10f); }
break;
case 3:
Debug.Log("Chaos Unleashed: Mass Stun!");
var enemies = FindEnemiesInArea(transform.position, 15f);
foreach(var e in enemies) { if(e.TryGetComponent<StatusManager>(out var st)) { st.ApplyEffect(StatusEffectType.Stun, 5f); } }
break;
}`,
spirit: ` // VFX: Reverie glows with pure potential, her Enigma gauge instantly filling.
// A unique property 'enigma' on her component is set to its max value.
this.enigma = this.maxEnigma; // Assuming these properties exist on ReverieAbilities.cs
Debug.Log("Reverie overloads her power, maxing out her Enigma!");`,
finisher: ` // Grand cinematic: All allies' weapons glow with chaotic energy.
// Apply a party-wide buff that gives attacks a chance to cause a magical explosion.
Debug.Log("Reverie weaves the party's fates into a chaotic tapestry!");
var allies = FindAllAllies();
foreach(var ally in allies)
{
if(ally.TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.ChainLightning, 15f); }
}`
};
case "Aeron": return {
rage: ` // VFX: A powerful shockwave emanates from Aeron's roar.
// Stun nearby enemies and apply an attack buff to self.
var enemies = FindEnemiesInArea(transform.position, 15f);
foreach(var enemy in enemies) { if(enemy.TryGetComponent<StatusManager>(out var s)) { s.ApplyEffect(StatusEffectType.Stun, 4f); } }
if(TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.DamageUp, 10f); }`,
spirit: ` // VFX: Aeron leaps high into the air, then crashes down, creating a crater.
// This would likely be implemented as a coroutine to handle the animation.
// For this script, we simulate the end result: damage in an area.
Debug.Log("Aeron crashes down with Giga Impact!");
var enemiesInImpact = FindEnemiesInArea(target.transform.position, 8f);
float damage = 350; // Base damage, could be increased by altitude in a full implementation.
foreach(var enemy in enemiesInImpact)
{
if(enemy.TryGetComponent<Health>(out var health)) { health.TakeDamage(damage, DamageType.Physical); }
}`,
finisher: ` // Grand cinematic: The party charges forward as one, led by Aeron.
// Damage and apply Vulnerable to all enemies.
Debug.Log("Aeron leads the Lionheart Charge!");
var allEnemies = FindAllEnemies();
foreach (var enemy in allEnemies) {
if (enemy.TryGetComponent<Health>(out var health)) { health.TakeDamage(300, DamageType.Physical); }
if (enemy.TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.Vulnerability, 15f); }
}`
};
case "Zaia": return {
rage: ` // VFX: A glowing, spectral sigil appears on the target.
// Mark the target for execution. A separate system would check this status.
if (target.TryGetComponent<StatusManager>(out var status))
{
status.ApplyEffect(StatusEffectType.Judgement, 30f, 0.25f); // Duration 30s, execute threshold 25%
}`,
spirit: ` // VFX: A circle of golden light appears on the ground around Zaia.
// Instantiate a Consecrated Ground prefab that handles the AoE heal/damage.
if (consecratedGroundPrefab != null)
{
Instantiate(consecratedGroundPrefab, transform.position, Quaternion.identity);
}`,
finisher: ` // Grand cinematic: All allies' weapons are blessed with a golden light.
// Apply a buff to all allies that allows them to execute low-health enemies.
Debug.Log("Zaia issues an Unwavering Decree!");
var allies = FindAllAllies();
foreach (var ally in allies) {
if (ally.TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.Execute, 15f, 0.15f); } // Threshold 15%
}`
};
case "Micah": return {
rage: ` // VFX: Micah slams his shield, drawing red lines of aggro from all enemies to himself.
// Taunt all enemies and become invulnerable.
var allEnemies = FindAllEnemies();
foreach(var enemy in allEnemies) { if (enemy.TryGetComponent<AIController>(out var ai)) { ai.SetTarget(this.gameObject); } }
if(TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.Invulnerable, 8f); }`,
spirit: ` // VFX: A massive, translucent blue shield appears before Micah, blocking incoming fire.
// Instantiate the shield prefab.
if (aegisShieldPrefab != null)
{
Instantiate(aegisShieldPrefab, transform.position + transform.forward * 2, transform.rotation);
}`,
finisher: ` // Grand cinematic: Shields of light envelop the entire party.
// Grant all allies a temporary shield based on their max health.
Debug.Log("Micah forms an Unbreakable Wall around the party!");
var allies = FindAllAllies();
foreach (var ally in allies) {
if (ally.TryGetComponent<Health>(out var health) && ally.TryGetComponent<ShieldManager>(out var shield))
{
shield.ApplyShield(health.maxHealth * 0.5f, 20f); // 50% max health shield for 20s
}
}`
};
case "Otis/X": return {
rage: ` // VFX: Otis/X glitches between a noble and a corrupted form, then attacks.
// Randomly execute one of two powerful attacks.
if (Random.value > 0.5f) {
Debug.Log("Memory Fragment: Sentinel's Blade!");
// Simulate a multi-hit combo
if(target.TryGetComponent<Health>(out var health))
{
health.TakeDamage(100, DamageType.Physical);
health.TakeDamage(100, DamageType.Physical);
health.TakeDamage(150, DamageType.Physical);
}
} else {
Debug.Log("Memory Fragment: Void Lash!");
// Simulate an AoE attack
var enemies = FindEnemiesInArea(target.transform.position, 5f);
foreach(var enemy in enemies)
{
if(enemy.TryGetComponent<Health>(out var health)) { health.TakeDamage(200, DamageType.Void); }
}
}`,
spirit: ` // VFX: Otis/X is wreathed in either light or shadow.
// Switch between Sentinel (defensive) and Corrupted (offensive) stances.
// This would be managed by a property on the OtisXAbilities component.
if (currentStance == Stance.Sentinel) {
currentStance = Stance.Corrupted;
Debug.Log("Partition Shift: Switched to CORRUPTED stance!");
} else {
currentStance = Stance.Sentinel;
Debug.Log("Partition Shift: Switched to SENTINEL stance!");
}`,
finisher: ` // Grand cinematic: Otis/X strikes with the clarity of his former self.
// Inflict a long-lasting defense debuff on the primary target.
Debug.Log("OtIS lands a Redeemed Strike!");
if (target.TryGetComponent<StatusManager>(out var status))
{
status.ApplyEffect(StatusEffectType.ArmorBreak, 20f);
status.ApplyEffect(StatusEffectType.WeaknessExposed, 20f);
}`
};
case "Aquila": return {
rage: ` // VFX: Aquila dives from the sky, crashing into the target with a sonic boom.
// Deal damage and stun a single target.
if (target.TryGetComponent<Health>(out var health)) { health.TakeDamage(300, DamageType.Physical); }
if (target.TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.Stun, 4f); }`,
spirit: ` // VFX: Aquila soars above, marking all enemies with a celestial light.
// Apply defense down and critical vulnerability to all enemies.
var enemies = FindAllEnemies();
foreach (var enemy in enemies)
{
if (enemy.TryGetComponent<StatusManager>(out var status)) {
status.ApplyEffect(StatusEffectType.ArmorBreak, 12f);
status.ApplyEffect(StatusEffectType.Vulnerability, 12f);
}
}`,
finisher: ` // Grand cinematic: Aquila leads an aerial squadron, raining down energy lances.
Debug.Log("Celestial Judgment rains from the sky!");
var enemies = FindAllEnemies();
foreach (var enemy in enemies) {
if (enemy.TryGetComponent<Health>(out var health)) { health.TakeDamage(550, DamageType.Physical); }
}`
};
case "Yuna": return {
rage: ` // VFX: A swirling cascade of illusory fox-fire envelops an area.
// Damage and confuse enemies in a radius.
var enemies = FindEnemiesInArea(transform.position, 15f);
foreach (var enemy in enemies)
{
if (enemy.TryGetComponent<Health>(out var health)) { health.TakeDamage(150, DamageType.Void); }
if (enemy.TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.Confuse, 8f); }
}`,
spirit: ` // VFX: A shimmering field of light and cherry blossoms conceals the party.
// Grant invisibility and evasion to all allies.
var allies = FindAllAllies();
foreach (var ally in allies)
{
if (ally.TryGetComponent<StatusManager>(out var status)) {
status.ApplyEffect(StatusEffectType.Invisibility, 8f);
status.ApplyEffect(StatusEffectType.Evasion, 8f);
}
}`,
finisher: ` // Grand cinematic: Yuna's nine tails become data streams, unleashing a disorienting digital storm.
Debug.Log("The Dance of a Thousand Tails begins!");
var enemies = FindAllEnemies();
foreach (var enemy in enemies) {
if (enemy.TryGetComponent<StatusManager>(out var status)) { status.ApplyRandomDebuff(); } // Assumes helper function
}`
};
default: return { rage: ' // TODO: Implement unique character logic.', spirit: ' // TODO: Implement unique character logic.', finisher: ' // TODO: Implement unique character logic.' };
}
};

export const generateCharacterScripts = (): Record<string, CSharpScript> => {
const scripts: Record<string, CSharpScript> = {};
CHARACTERS.forEach(char => {
const scriptName = `${char.name.replace(/[^a-zA-Z0-9]/g, '')}Abilities.cs`;
const className = `${char.name.replace(/[^a-zA-Z0-9]/g, '')}Abilities`;
const rageBurstName = char.limitBreak?.name || "Generic Rage Burst";
const rageBurstDesc = char.limitBreak?.description || "A powerful attack fueled by rage.";
const spiritBreakName = char.spiritBreak?.name || "Generic Spirit Break";
const spiritBreakDesc = char.spiritBreak?.description || "A powerful ability fueled by mana.";
const finisherName = char.novaminaadFinisher?.name || "Generic Finisher";
const finisherDesc = char.novaminaadFinisher?.description || "A finishing move as part of the Alliance Break.";
const implementations = getAbilityImplementations(char);
const code = `using UnityEngine;

// This component manages the unique abilities for ${char.name}.
public class ${className} : CharacterAbilitiesBase
{
// --- SIGNATURE ABILITIES ---

/// <summary>
/// ${rageBurstDesc}
/// </summary>
public override void ExecuteRageBurst(GameObject target)
{
if (!IsRageBurstReady) return;
Debug.Log("${char.name} unleashes their Rage Burst: ${rageBurstName.toUpperCase()}!");
${implementations.rage}
ResetRage();
}

/// <summary>
/// ${spiritBreakDesc}
/// </summary>
public override void ExecuteSpiritBreak(GameObject target)
{
if (!IsSpiritBreakReady) return;
Debug.Log("${char.name} channels their Spirit Break: ${spiritBreakName.toUpperCase()}!");
${implementations.spirit}
ResetMana();
}

/// <summary>
/// ${finisherDesc}
/// </summary>
public override void PerformNovaminaadFinisher(GameObject target)
{
Debug.Log("${char.name}'s Finisher: ${finisherName.toUpperCase()}!");
${implementations.finisher}
}
}

${SHADER_COMMENT_BLOCK}
`;
scripts[char.name] = { fileName: scriptName, code };
});
return scripts;
};

export const generateAntagonistScripts = (): Record<string, CSharpScript> => {
const scripts: Record<string, CSharpScript> = {};
ANTAGONISTS.forEach(ant => {
const scriptName = `${ant.name.replace(/[^a-zA-Z0-9]/g, '')}AIController.cs`;
const className = `${ant.name.replace(/[^a-zA-Z0-9]/g, '')}AIController`;
const matchingArchetype = ENEMY_AI_ARCHETYPES.find(archetype => archetype.name.includes(ant.name));
let abilitiesSection = '// --- ABILITIES ---';
if (matchingArchetype) {
abilitiesSection += matchingArchetype.abilities.map(ability => `
/// <summary>
/// ${ability.description}
/// </summary>
public void ${ability.name.replace(/[^a-zA-Z0-9\s]/g, '').replace(/\s/g, '')}(GameObject target)
{
Debug.Log($"${ant.name} uses ${ability.name}!");
// TODO: Implement ability logic
}
`).join('');
} else {
abilitiesSection += `
// Placeholder for signature abilities based on lore.
public void PerformSignatureAttack(GameObject target)
{
Debug.Log($"${ant.name} uses a signature attack on {target.name}!");
}
`;
}
const code = `using UnityEngine;

// Manages the AI and abilities for the antagonist ${ant.name}.
public class ${className} : MonoBehaviour
{
[Header("AI Properties")]
public float detectionRadius = 30f;
public float attackRange = 10f;
private GameObject playerTarget;

void Start() { playerTarget = GameObject.FindGameObjectWithTag("Player"); }

void Update()
{
if (playerTarget == null) return;
float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.transform.position);
if (distanceToPlayer <= detectionRadius)
{
transform.LookAt(playerTarget.transform);
if (distanceToPlayer <= attackRange) { /* TODO: Implement attack logic */ }
else { /* TODO: Implement movement logic */ }
}
}

${abilitiesSection}
}

${SHADER_COMMENT_BLOCK}
`;
scripts[ant.name] = { fileName: scriptName, code };
});
return scripts;
};

export const CHARACTER_DATA_SCRIPTS: Record<'dataClass' | 'factory' | 'jsonData', CSharpScript> = {
dataClass: {
fileName: 'CharacterData.cs',
code: `using UnityEngine;

// This ScriptableObject acts as a data container for character stats.
// You can create instances of this in the Project window via:
// Assets > Create > Milehigh/Character Data
[CreateAssetMenu(fileName = "NewCharacter", menuName = "Milehigh/Character Data")]
public class CharacterData : ScriptableObject
{
[Header("Identity")]
public string characterName;
public string title;
[TextArea(3, 5)]
public string description;

[Header("Core Stats")]
public int strength;
public int dexterity;
public int defense;
public int vigor;
public int heart;

[Header("Affinities")]
public int voidAffinity;
public int nexusAttunement;
}

${SHADER_COMMENT_BLOCK}`
},
factory: {
fileName: 'CharacterFactory.cs',
code: `using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

// This is an Editor script. It must be placed in a folder named "Editor".
public class CharacterFactory
{
// Path to the JSON file in the Assets folder.
private static string jsonPath = "/characters.json";

// This method is called from the Unity Editor menu.
[MenuItem("Milehigh.World/Generate Characters from JSON")]
public static void GenerateCharacters()
{
string fullPath = Application.dataPath + jsonPath;
if (!File.Exists(fullPath))
{
Debug.LogError("JSON file not found at: " + fullPath);
return;
}

string jsonString = File.ReadAllText(fullPath);
CharacterList characterList = JsonUtility.FromJson<CharacterList>(jsonString);

foreach (var charInfo in characterList.characters)
{
// Define the path where the asset will be created.
string assetPath = $"Assets/CharacterData/{charInfo.name}.asset";

CharacterData characterData = AssetDatabase.LoadAssetAtPath<CharacterData>(assetPath);
if (characterData == null)
{
// If the asset doesn't exist, create a new one.
characterData = ScriptableObject.CreateInstance<CharacterData>();
AssetDatabase.CreateAsset(characterData, assetPath);
}

// Populate the ScriptableObject with data from the JSON.
characterData.characterName = charInfo.name;
characterData.title = charInfo.title;
characterData.description = charInfo.description;
characterData.strength = charInfo.stats.strength;
characterData.dexterity = charInfo.stats.dexterity;
// ... and so on for all other stats.

// Mark the asset as "dirty" to ensure changes are saved.
EditorUtility.SetDirty(characterData);
}

// Save all changes to the assets.
AssetDatabase.SaveAssets();
AssetDatabase.Refresh();
Debug.Log("Character assets generated successfully from JSON!");
}

// Helper classes to match the JSON structure.
[System.Serializable]
private class CharacterList
{
public List<CharacterInfo> characters;
}

[System.Serializable]
private class CharacterInfo
{
public string name;
public string title;
public string description;
public StatInfo stats;
}

[System.Serializable]
private class StatInfo
{
public int strength;
public int dexterity;
// ... etc
}
}

${SHADER_COMMENT_BLOCK}`
},
jsonData: {
fileName: 'characters.json',
code: `{
"characters": [
{
"name": "Ingris",
"title": "The Phoenix Warrior",
"description": "A resilient warrior who embodies the cycle of death and rebirth...",
"stats": {
"strength": 8,
"dexterity": 6,
"defense": 7,
"vigor": 9,
"heart": 5,
"voidAffinity": 2,
"nexusAttunement": 1
}
},
{
"name": "Kai",
"title": "The Unbroken Sentinel",
"description": "A brilliant tactician whose prophetic visions allow him to see moments into the future...",
"stats": {
"strength": 5,
"dexterity": 7,
"defense": 6,
"vigor": 6,
"heart": 8,
"voidAffinity": 3,
"nexusAttunement": 5
}
}
]
}`
}
};

export const CINEMATICS_SCRIPTS: Record<'timelineController' | 'cameraShake', CSharpScript> = {
timelineController: {
fileName: 'TimelineController.cs',
code: `using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineController : MonoBehaviour
{
public PlayableDirector director;
public TimelineAsset[] cinematics;

public void PlayCinematic(int index)
{
if (director != null && cinematics != null && index < cinematics.Length)
{
director.playableAsset = cinematics[index];
director.Play();
}
}
}

${SHADER_COMMENT_BLOCK}`
},
cameraShake: {
fileName: 'CameraShake.cs',
code: `using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
public IEnumerator Shake(float duration, float magnitude)
{
Vector3 originalPos = transform.localPosition;
float elapsed = 0.0f;

while (elapsed < duration)
{
float x = Random.Range(-1f, 1f) * magnitude;
float y = Random.Range(-1f, 1f) * magnitude;

transform.localPosition = new Vector3(x, y, originalPos.z);
elapsed += Time.deltaTime;
yield return null;
}

transform.localPosition = originalPos;
}
}

${SHADER_COMMENT_BLOCK}`
}
};

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
1.  **EXPLORE:** Players navigate the diverse and sprawling environments of The Verse, uncovering hidden paths, finding valuable resources, and discovering lore fragments that reveal the world's history and secrets.
2.  **ENGAGE:** Players engage in dynamic, hybrid combat against a variety of foes, from corrupted Void entities to cybernetically enhanced soldiers. They must utilize character-specific abilities, environmental hazards, and strategic teamwork to overcome challenges.
3.  **EVOLVE:** Players advance their characters by gaining experience, unlocking new skills in their unique skill trees, and acquiring powerful gear. They also make narrative choices that impact their reputation with different factions, altering the story and unlocking new missions and dialogue.`;

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
        binary: "01010011 01101011 01111001 00101110 01101001 01111001",
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
1.  **Blender Automation:** A suite of scripts located in 'blender_scripts/' designed to automate complex modeling, animation, and rendering tasks within Blender. This includes procedural generation of environmental assets and batch-rendering of cinematic sequences.
2.  **Data Processing:** Scripts used for parsing and validating 'game_data.json' and other data files, ensuring integrity before they are imported into the C# codebase. This helps streamline the content pipeline and reduce manual errors.`;

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
    statGrowth: {
        primary: ['dexterity', 'nexusAttunement', 'voidAffinity'],
        secondary: ['heart', 'propheticClarity'],
    },
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
    statGrowth: {
        primary: ['heart', 'oneiricResonance', 'propheticClarity'],
        secondary: ['defense', 'voidAffinity'],
    },
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
    statGrowth: {
        primary: ['strength', 'heart', 'oneiricResonance'],
        secondary: ['dexterity', 'voidAffinity'],
    },
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
    statGrowth: {
        primary: ['strength', 'vigor', 'defense'],
        secondary: ['heart', 'dexterity'],
    },
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
    statGrowth: {
        primary: ['strength', 'dexterity', 'propheticClarity'],
        secondary: ['vigor', 'voidAffinity'],
    },
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
    statGrowth: {
        primary: ['strength', 'vigor', 'heart'],
        secondary: ['defense', 'propheticClarity'],
    },
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
    statGrowth: {
        primary: ['strength', 'vigor', 'voidAffinity'],
        secondary: ['defense', 'nexusAttunement'],
    },
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
