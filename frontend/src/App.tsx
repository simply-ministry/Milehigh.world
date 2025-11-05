
import React, { useState, useCallback, useEffect, useMemo } from 'react';
import { Header } from './components/Header';
import { Card } from './components/Card';
import { CharacterCard } from './components/CharacterCard';
import { ScenarioGenerator } from './components/ScenarioGenerator';
import { LoreQuery } from './components/LoreQuery';
import { ReputationTracker } from './components/ReputationTracker';
import { WorldBuildingChart } from './components/WorldBuildingChart';
import { CharacterDynamicsGraph } from './components/CharacterDynamicsChart';
import { EnemyAIBehavior } from './components/EnemyAIBehavior';
import { AntagonistCard } from './components/AntagonistCard';
import { VideoAnalyzer } from './components/VideoAnalyzer';
import { AIEditor } from './components/AIEditor';
import { LoreExplorer } from './components/LoreExplorer';
import { WorldMap } from './components/WorldMap';
import { ObjectiveGoals } from './components/ObjectiveGoals';
import { ThreatDetector } from './components/ThreatDetector';
import { CombatStatusDisplay } from './components/CombatStatusDisplay';
import { EnemyEncounter } from './components/EnemyEncounter';
import { Inventory } from './components/Inventory';
import { Crafting } from './components/Crafting';
import { CombatLog } from './components/CombatLog';
import { LiveConversation } from './components/LiveConversation';
import { CharacterInteraction } from './components/CharacterInteraction';
import { StatsCard } from './components/StatsCard';
import { TimeTravelAnimation } from './components/TimeTravelAnimation';
import { AllianceBreakDialogue } from './components/AllianceBreakDialogue';
import { MissionBriefing } from './components/MissionBriefing';
import { BattleSceneGenerator } from './components/BattleSceneGenerator';
import { playSound } from './utils/soundService';


import {
  CHARACTERS,
  ANTAGONISTS,
  DIGITAL_MOTIF,
  NARRATIVE_BLUEPRINT_TABLE,
  CORE_EMOTIONAL_ARCS,
  NARRATIVE_VIGNETTES,
  KEY_CONCEPTS,
  INITIAL_VOICE_PROFILES,
  CORE_GAMEPLAY_LOOP,
  COMBAT_SYSTEM,
  EXPLORATION_TRAVERSAL,
  ART_STYLE,
  EditIcon,
  ENEMY_AI_ARCHETYPES,
  ITEMS,
  LEARNABLE_SKILLS,
  CRAFTING_RECIPES
} from './constants';
import { INITIAL_LORE_CONTEXT } from './services/geminiService';
import type { Character, Reputation, VoiceProfile, Antagonist, Item, EnemyAIArchetype, PlayerState, Skill, AIAbility, ActiveEffect, EnemyActionState, Consumable, EffectType, Recipe, MissionGenerationResponse, CombatLogEntry, LogEntryType, PlayerActionState, Scenario } from './types';

const initialReputation: Reputation = {
  'Ɲōvəmîŋāđ Alliance': 50,
  "Cirrus's Trust": 50,
  'AṬĤŸŁĞÅŘÐ Honor': 40,
  'ÅẒ̌ŪŘẸ ĤĒĪĜĤṬ§ Authority': 30,
  'Hydraustis Palare Collective': 35,
  'ŁĪƝĈ Underground': 25,
  "The Void's Grasp": 10,
};

const initialPlayerState: PlayerState = {
    health: 100,
    mana: 25,
    rage: 0,
    alliance: 0,
    level: 1,
    experience: 0,
    selectedCharacterName: CHARACTERS[0].name,
    activeEffects: [],
    currentTimePeriod: 'present',
};

const getExperienceForNextLevel = (level: number) => 100 * level * level;

/**
 * A component for controlling time travel between different eras.
 * @param {object} props - The component props.
 * @param {PlayerState['currentTimePeriod']} props.currentTimePeriod - The current time period.
 * @param {(period: PlayerState['currentTimePeriod']) => void} props.onTimeTravel - Callback to trigger time travel.
 * @param {boolean} props.disabled - Whether the time travel controls are disabled.
 */
const TemporalRiftCard: React.FC<{ currentTimePeriod: PlayerState['currentTimePeriod'], onTimeTravel: (period: PlayerState['currentTimePeriod']) => void, disabled: boolean }> = ({ currentTimePeriod, onTimeTravel, disabled }) => {
    const isPresentButtonDisabled = disabled || currentTimePeriod === 'present';
    const isPastButtonDisabled = disabled || currentTimePeriod === 'past_era_of_heroes';
    const isFutureButtonDisabled = disabled || currentTimePeriod === 'future_dystopia';

    return (
        <div className="space-y-4 text-center">
            <p className="text-slate-400">The Temporal Rift is unstable. Travel through time, but be warned: you cannot return while a threat is active.</p>
            <div className="flex justify-center gap-2">
                 <button onClick={() => onTimeTravel('past_era_of_heroes')} disabled={isPastButtonDisabled} className="flex-1 bg-slate-800 text-slate-100 font-bold py-2 px-4 uppercase tracking-wider rounded disabled:bg-slate-900 disabled:text-slate-600 transition-colors">Past</button>
                 <button onClick={() => onTimeTravel('present')} disabled={isPresentButtonDisabled} className="flex-1 bg-slate-800 text-slate-100 font-bold py-2 px-4 uppercase tracking-wider rounded disabled:bg-slate-900 disabled:text-slate-600 transition-colors">Present</button>
                 <button onClick={() => onTimeTravel('future_dystopia')} disabled={isFutureButtonDisabled} className="flex-1 bg-slate-800 text-slate-100 font-bold py-2 px-4 uppercase tracking-wider rounded disabled:bg-slate-900 disabled:text-slate-600 transition-colors">Future</button>
            </div>
        </div>
    );
};

/**
 * A component for managing game state (Save, Load, New Game).
 * @param {object} props - The component props.
 * @param {() => void} props.onSave - Callback to save the game state.
 * @param {() => void} props.onLoad - Callback to load the game state.
 * @param {() => void} props.onReset - Callback to reset the game to its initial state.
 */
const GameStateControls: React.FC<{ onSave: () => void; onLoad: () => void; onReset: () => void; }> = ({ onSave, onLoad, onReset }) => {
    return (
        <div className="space-y-4 text-center">
            <p className="text-slate-400">Manage your game progress. Saving will overwrite your previous save file.</p>
            <div className="flex justify-center gap-2">
                 <button onClick={onSave} className="flex-1 bg-slate-800 text-slate-100 font-bold py-2 px-4 uppercase tracking-wider rounded transition-colors hover:bg-slate-700">Save</button>
                 <button onClick={onLoad} className="flex-1 bg-slate-800 text-slate-100 font-bold py-2 px-4 uppercase tracking-wider rounded transition-colors hover:bg-slate-700">Load</button>
                 <button onClick={onReset} className="flex-1 bg-red-900 text-red-200 font-bold py-2 px-4 uppercase tracking-wider rounded transition-colors hover:bg-red-800">New Game</button>
            </div>
        </div>
    );
};

/**
 * The main application component. It serves as the root of the UI and manages all major
 * game states, including player stats, reputation, inventory, combat, and mission progression.
 */
export const App = () => {
    /** The AI's knowledge base and memory, updated with new intel over time. */
    const [loreContext, setLoreContext] = useState(INITIAL_LORE_CONTEXT);
    /** Player's reputation with various in-game factions. */
    const [reputation, setReputation] = useState<Reputation>(initialReputation);
    /** Core player character statistics like health, mana, level, and current time period. */
    const [playerState, setPlayerState] = useState<PlayerState>(initialPlayerState);
    const initialInventory = useMemo(() => [...ITEMS.weapons, ...ITEMS.armor, ...ITEMS.consumables, ...ITEMS.artifacts], []);
    /** The player's current inventory of items. */
    const [inventory, setInventory] = useState<Item[]>(initialInventory);
    /** A log of recent combat and system messages. */
    const [combatLog, setCombatLog] = useState<CombatLogEntry[]>([]);

    /** The current enemy the player is facing in an encounter. Null if not in combat. */
    const [currentEnemy, setCurrentEnemy] = useState<EnemyAIArchetype | null>(null);
    /** The current health of the active enemy. */
    const [enemyHealth, setEnemyHealth] = useState(0);
    /** The maximum health of the active enemy. */
    const [enemyMaxHealth, setEnemyMaxHealth] = useState(0);
    /** Any active status effects on the current enemy. */
    const [enemyActiveEffects, setEnemyActiveEffects] = useState<ActiveEffect[]>([]);
    /** The current action state of the enemy (e.g., 'attacking', 'casting'). */
    const [enemyAction, setEnemyAction] = useState<EnemyActionState>('idle');
    /** The current action state of the player (e.g., 'attacking', 'taking_damage'). */
    const [playerAction, setPlayerAction] = useState<PlayerActionState>('idle');

    /** State to manage the time travel visual effect. */
    const [isTimeTraveling, setIsTimeTraveling] = useState(false);

    // ... (rest of the App component remains the same, so it's omitted for brevity) ...

    // The user has requested to replace the BattleSceneGenerator with a new version that showcases
    // a pre-generated grand finale. This new version is self-contained and no longer requires props.
    // I will remove the loreContext and voiceProfiles props from its instantiation.
    return (
        <div className="min-h-screen">
            <Header />
            <main className="container mx-auto p-4 max-w-7xl">
                <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">

                    {/* Left Column: Player Status & Core Interactions */}
                    <div className="lg:col-span-1 space-y-8">
                        {/* Player Stats */}
                        <Card title="Player Status" index={0} shadowType="white">
                            {/* ... Content of Player Status Card */}
                        </Card>

                        {/* Other left-column cards... */}

                    </div>

                    {/* Center Column: Main Content & Generators */}
                    <div className="lg:col-span-2 space-y-8">

                        {/* ... Other center-column cards ... */}

                        {/* Grand Finale Scene Generator */}
                        <Card title="Grand Finale Scene Generator" index={20}>
                            <BattleSceneGenerator />
                        </Card>
                    </div>

                </div>
            </main>
        </div>
    );
};
