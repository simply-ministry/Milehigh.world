// frontend/src/types.ts

export type DamageType = 'Physical' | 'Void' | 'Elemental';

export interface FileTreeNode {
  name: string;
  type: 'folder' | 'file';
  children?: FileTreeNode[];
  description?: string;
  path?: string;
}

export interface Skill {
  name: string;
  description: string;
  effects: string[];
}

export interface Character {
  name: string;
  title: string;
  archetype: string;
  description: string;
  backstory: string;
  usd: string;
  strength: number;
  dexterity: number;
  defense: number;
  vigor: number;
  heart: number;
  voidAffinity: number;
  nexusAttunement: number;
  oneiricResonance: number;
  propheticClarity: number;
  damageType: DamageType;
  statGrowth: {
    primary: string[];
    secondary: string[];
  };
  fightingStyle: string;
  weapons: string[];
  imageUrl: string;
  limitBreak?: { name: string; description: string };
  spiritBreak?: { name: string; description: string };
  novaminaadFinisher?: { name: string; description: string };
  skills?: Skill[];
}

export interface Antagonist {
  name: string;
  title: string;
  description: string;
  imageUrl: string;
  usd: string;
  fightingStyle: string;
  weapons: string[];
}

export interface NarrativeElement {
  element: string;
  description: string;
  purpose: string;
}

export interface WorldFaction {
  setting: string;
  focus: string;
  implication: string;
  sublocations?: { name: string; description: string }[];
}

export interface CharacterRole {
  character: string;
  archetype: string;
  role: string;

// A single, unified source of truth for all data structures in the application.

// Core Types
export type DamageType = 'Physical' | 'Void' | 'Elemental' | 'True';
export type EffectType = 'poison' | 'slow' | 'stun' | 'corruption' | 'armor_break' | 'outburst' | 'weakness_exposed';

// File and Code Structure
export interface FileTreeNode {
  name: string;
  type: 'folder' | 'file';
  children?: FileTreeNode[];
  description?: string;
  path?: string;
}

export interface CSharpScript {
  fileName: string;
  code: string;
}

// Game Content and Lore
export interface KeyConcept {
  name: string;
  description: string;
}

export interface VoiceProfile {
  characterName: string;
  voiceName: string;
  systemInstruction: string;
}

export interface DigitalMotif {
  name: string;
  binary: string;
  name: string;
  description: string;
}

export interface DigitalMotif {
  name: string;
  binary: string;
}

export interface NarrativeElement {
  element: string;
  description: string;
  purpose: string;
}

export interface SubLocation {
  name: string;
  description: string;
}

export interface WorldFaction {
  setting: string;
  focus: string;
  implication: string;
  sublocations?: SubLocation[];
}

// Items, Skills, and Crafting
export interface Item {
  name: string;
  description: string;
}

export interface Weapon extends Item {
  damage: number;
  weapon_type: string;
  damageType: DamageType;
}

export interface Armor extends Item {
  defense: number;
}

export interface Consumable extends Item {
  amount: number;
}

export interface Artifact extends Item {
  loreSnippet: string;
}

export interface Skill {
  name: string;
  description: string;
  effects: string[];
}

export interface Ingredient {
    name: string;
    quantity: number;
}

export interface Recipe {
  result: Item;
  ingredients: Ingredient[];
}

// Character and AI
export interface CharacterAbility {
  name: string;
  description: string;
}

export interface CharacterStatGrowth {
    primary: string[];
    secondary: string[];
}

export interface Character {
  name: string;
  title: string;
  archetype: string;
  description: string;
  strength: number;
  dexterity: number;
  defense: number;
  vigor: number;
  heart: number;
  voidAffinity: number;
  nexusAttunement: number;
  oneiricResonance: number;
  propheticClarity: number;
  damageType: DamageType;
  statGrowth: CharacterStatGrowth;
  skills?: Skill[];
  usd: string;
  imageUrl: string;
  limitBreak: CharacterAbility;
  spiritBreak: CharacterAbility;
  novaminaadFinisher: CharacterAbility;
}

export interface CharacterRole {
  character: string;
  archetype: string;
  role: string;
}

export interface Antagonist {
  name: string;
  title: string;
  description: string;
  imageUrl: string;
  usd?: string;
}

export interface ActiveEffect {
  type: EffectType;
  duration: number; // in seconds
  potency?: number; // e.g., damage per tick
}

export interface AIAbility {
  name: string;
  trigger: string;
  description: string;
  damage: number;
  type: 'physical' | 'magical';
  effect?: {
    type: string;
    chance: number;
    duration: number;
    potency: number;
  type?: 'physical' | 'magical' | 'special';
  manaDrain?: number;
  effect?: {
    type: EffectType;
    chance: number; // 0-1 chance
    duration: number;
    potency?: number;
  };
}

export interface EnemyAIArchetype {
  archetype: string;
  name: string;
  description: string;
  coreBehavior: string;
  defense: number;
  abilities: AIAbility[];
  reactions: string[];
  resourceManagement: string;
  xpValue: number;
  weakness: string;
  timePeriods: string[];
}

  weakness?: string;
  environmentDescription?: string;
}

// Narrative and Dialogue
export interface VoiceProfile {
  characterName: string;
  voiceName: string;
  systemInstruction: string;
}

export interface TranscriptionEntry {
  speaker: 'User' | 'Sky.ix' | string;
  text: string;
  tone?: string;
}

// Quests and Objectives
export interface ObjectiveGoal {
  id: string;
  description: string;
}

export interface ObjectiveGoalGroup {
  id: string;
  title: string;
  goals: ObjectiveGoal[];
  reward: string;
  priority: 'high' | 'medium' | 'low';
}

export interface Item {
  name: string;
  description: string;
}

export interface Weapon extends Item {
  damage: number;
  weapon_type: string;
  damageType?: DamageType;
  equippableBy?: string[];
}

export interface Armor extends Item {
  defense: number;
  equippableBy?: string[];
}

export interface Consumable extends Item {
  amount: number;
}

export interface Artifact extends Item {
  loreSnippet: string;
}

export interface Recipe {
  result: Item;
  ingredients: {
    name: string;
    quantity: number;
  }[];
}

export interface CSharpScript {
  fileName: string;
  code: string;
  priority?: 'low' | 'medium' | 'high';
}

// Player and Game State
export interface Reputation {
  [key: string]: number;
}

export interface PlayerState {
  health: number;
  mana: number;
  rage: number;
  alliance: number;
  level: number;
  experience: number;
  selectedCharacterName: string;
  activeEffects: ActiveEffect[];
  currentTimePeriod: 'present' | 'past_era_of_heroes' | 'future_dystopia';
}

export type LogEntryType = 'system' | 'damage' | 'critical_damage' | 'heal' | 'mana' | 'status_effect' | 'xp' | 'intel' | 'reputation' | 'craft';

export interface CombatLogEntry {
  type: LogEntryType;
  message: string;
  timestamp: string;
  icon?: string;
}

// Scenarios and Missions
export interface Scenario {
  mission: string;
  objective: string;
  location: string;
  assignedCharacters: string[];
  narrative: string;
  dialogue: TranscriptionEntry[];
  loreDeepDive: string;
  csharpScript: string; // Changed from CSharpScript to string to match usage
}

export interface GrandFinaleScenario {
  title: string;
  mission: string;
  narrative: string;
  dialogue: string;
  loreDeepDive: string;
  csharpScript: CSharpScript;
}

export interface MissionGenerationResponse {
  assignedCharacters: string[];
  mission: string;
  objective: string;
  consequences: string;
  debriefScript: string;
  reputationImpact: {
    faction: string;
    change: number;
  }[];
}
