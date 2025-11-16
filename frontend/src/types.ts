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
}

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
}
