export type DamageType = 'Physical' | 'Void' | 'Elemental';

export interface Character {
  name: string;
  title: string;
  archetype: string;
  description: string;
  usd: string;
  strength: number; // Raw physical/magical power
  dexterity: number; // Agility, precision, and critical hit chance
  defense: number; // Damage reduction
  vigor: number; // Governs Health and Stamina
  heart: number; // Governs Mana and Spirit
  voidAffinity: number; // Attunement to Void energies
  nexusAttunement: number; // Proficiency with technology
  oneiricResonance: number; // Connection to the Dreamscape
  propheticClarity: number; // Tactical insight and critical chance
  damageType: DamageType;
  skills?: Skill[];
  imageUrl: string;
  limitBreak?: {
    name: string;
    description: string;
  };
  spiritBreak?: {
    name: string;
    description: string;
  };
  novaminaadFinisher?: {
    name: string;
    description: string;
  };
}

export interface Item {
  name: string;
  description: string;
}

export interface Weapon extends Item {
  damage: number;
  weapon_type: string;
  damageType?: DamageType;
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

export interface Recipe {
  result: Item;
  ingredients: {
    name: string;
    quantity: number;
  }[];
}

export interface Antagonist {
  name: string;
  title: string;
  description: string;
  imageUrl: string;
  usd?: string;
}

export interface Skill {
  name: string;
  description: string;
  effects: string[];
}

export interface FileTreeNode {
  name: string;
  type: 'folder' | 'file';
  children?: FileTreeNode[];
  description?: string;
  path?: string;
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
}

export interface TranscriptionEntry {
  speaker: 'User' | 'Sky.ix' | string;
  text: string;
  tone?: string;
}

export interface KeyConcept {
    name: string;
    description: string;
}

export interface DigitalMotif {
  name: string;
  binary: string;
}

export interface Reputation {
  [key: string]: number;
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

export interface VoiceProfile {
  characterName: string;
  voiceName: string;
  systemInstruction: string;
}

export type EffectType = 'poison' | 'slow' | 'armor_break' | 'outburst' | 'weakness_exposed';

export interface ActiveEffect {
  type: EffectType;
  duration: number; // in seconds
  potency?: number; // e.g., damage per tick for poison
}

export interface AIAbility {
  name: string;
  trigger: string;
  description: string;
  damage: number;
  type?: 'physical' | 'magical' | 'special';
  manaDrain?: number;
  effect?: {
    type: EffectType;
    chance: number; // 0-1 chance to apply
    duration: number; // in seconds
    potency?: number; // e.g., damage per tick
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
  weakness?: string;
  environmentDescription?: string;
}

export type EnemyActionState = 'idle' | 'attacking' | 'casting' | 'taking_damage';
export type PlayerActionState = 'idle' | 'attacking' | 'taking_damage';

export interface CampaignStatus {
  title: string;
  description: string;
  type: 'positive' | 'negative' | 'neutral';
}

export interface KeyFigureReputation {
  [key: string]: number;
}

export interface ObjectiveGoal {
  id: string;
  description: string;
}

export interface ObjectiveGoalGroup {
  id: string;
  title: string;
  goals: ObjectiveGoal[];
  reward: string;
  priority?: 'low' | 'medium' | 'high';
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

export interface Scenario {
    mission: string;
    objective: string;
    location: string;
    assignedCharacters: string[];
    narrative: string;
    dialogue: TranscriptionEntry[];
    loreDeepDive: string;
    csharpScript: string;
}

export interface CSharpScript {
  fileName: string;
  code: string;
}

export interface GrandFinaleScenario {
  title: string;
  mission: string;
  narrative: string;
  dialogue: string;
  loreDeepDive: string;
  csharpScript: CSharpScript;
}
