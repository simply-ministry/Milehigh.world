// frontend/src/types.ts

export type DamageType = 'Void' | 'Elemental' | 'Physical' | 'True';

export interface FileTreeNode {
  name: string;
  children?: FileTreeNode[];
}

export interface CharacterRole {
    name: string;
    description: string;
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

export interface AbilityEffect {
    type: 'slow' | 'stun' | 'corruption';
    chance?: number;
    duration: number;
}

export interface EnemyAbility {
    name: string;
    type?: 'special' | 'physical'; // Making type optional as it's missing in one entry
    trigger: string;
    description: string;
    damage: number;
    effect?: AbilityEffect;
}

export interface EnemyAIArchetype {
    archetype: string;
    name: string;
    description: string;
    coreBehavior: string;
    defense: number;
    abilities: EnemyAbility[];
    reactions: string[];
    resourceManagement: string;
    xpValue: number;
    weakness: string;
    environmentDescription: string;
}

export interface Antagonist {
    name: string;
    title: string;
    description: string;
    imageUrl: string;
}

export interface Objective {
    id: string;
    description: string;
}

export interface ObjectiveGoalGroup {
    id: string;
    title: string;
    goals: Objective[];
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

export interface CharacterAbility {
    name: string;
    description: string;
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
    usd: string;
    imageUrl: string;
    limitBreak: CharacterAbility;
    spiritBreak: CharacterAbility;
    novaminaadFinisher: CharacterAbility;
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
