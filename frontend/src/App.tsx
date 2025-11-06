import React, { useState, useCallback, useEffect, useMemo, useRef } from 'react';
import { Header } from './components/Header';
import { Card } from './components/Card';
import { CharacterCard } from './components/CharacterCard';
import { CharacterShowcase } from './components/CharacterShowcase';
import { ScenarioGenerator } from './components/ScenarioGenerator';
import { ReputationTracker } from './components/Milehigh.world-main/ReputationTracker';
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
import { AllianceBreakDialogue } from './components/AllianceBreakDialogue';
import { MissionBriefing } from './components/MissionBriefing';
import { BattleSceneGenerator } from './components/BattleSceneGenerator';
import { BackgroundMusicPlayer } from './components/BackgroundMusicPlayer';
import { playSound, initAudio } from './utils/soundService';
import { IntelLogger } from './components/IntelLogger';
import { PowerSystemsVisualizer } from './components/PowerSystemsVisualizer';

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

const MUSIC_TRACKS = {
  classical: 'https://storage.googleapis.com/iam-dev-project-id-2-sound-bucket/lofi.mp3',
  dubstep: 'https://storage.googleapis.com/iam-dev-project-id-2-sound-bucket/dubstep.mp3'
};

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
  const [isPlayerDefeated, setIsPlayerDefeated] = useState(false);

  // State to manage screen-wide visual effects for special skills
  const [screenEffect, setScreenEffect] = useState<string | null>(null);

  /** The current turn in combat ('player', 'enemy', 'processing' during actions, or 'none' outside combat). */
  const [currentTurn, setCurrentTurn] = useState<'player' | 'enemy' | 'processing' | 'none'>('none');

  /** State to manage the time travel visual effect. */
  const [isTimeTraveling, setIsTimeTraveling] = useState(false);
  // State for animated status effect indicators
  const [playerEffectIndicator, setPlayerEffectIndicator] = useState<{ type: EffectType, key: number } | null>(null);
  const [enemyEffectIndicator, setEnemyEffectIndicator] = useState<{ type: EffectType, key: number } | null>(null);

  const [currentMission, setCurrentMission] = useState<MissionGenerationResponse | null>(null);
  const [missionHistory, setMissionHistory] = useState<MissionGenerationResponse[]>([]);
  const [activeScenario, setActiveScenario] = useState<Scenario | null>(null);
  const [activeBreakEffect, setActiveBreakEffect] = useState<string | null>(null);
  const [isEditorOpen, setIsEditorOpen] = useState(false);

  // Background Music State
  const audioRef = useRef<HTMLAudioElement | null>(null);
  // Start with isPlaying: false to prevent autoplay error on initial load.
  const [musicState, setMusicState] = useState({ isPlaying: false, track: 'classical' as 'classical' | 'dubstep', isMuted: false });

  // Initialize Audio System on first user interaction
  useEffect(() => {
   const handleFirstInteraction = () => {
    console.log("User interaction detected, initializing audio system.");
    initAudio().catch(err => {
     console.error("Audio initialization failed on user interaction:", err);
    });

    // Set music to play now that we have user interaction.
    setMusicState(prev => ({ ...prev, isPlaying: true }));

    // Remove listeners after the first interaction to avoid re-triggering.
    window.removeEventListener('click', handleFirstInteraction);
    window.removeEventListener('keydown', handleFirstInteraction);
    window.removeEventListener('touchstart', handleFirstInteraction);
   };

   window.addEventListener('click', handleFirstInteraction);
   window.addEventListener('keydown', handleFirstInteraction);
   window.addEventListener('touchstart', handleFirstInteraction);

   return () => {
    window.removeEventListener('click', handleFirstInteraction);
    window.removeEventListener('keydown', handleFirstInteraction);
    window.removeEventListener('touchstart', handleFirstInteraction);
   };
  }, []); // Empty dependency array ensures this setup runs only once.

  useEffect(() => {
   // Initialize audio element on first render
   if (!audioRef.current) {
    const audio = new Audio();
    audio.loop = true;
    audio.volume = 0.2; // Start with a low volume for background music
    audioRef.current = audio;
   }

   const audio = audioRef.current;
   const newSrc = MUSIC_TRACKS[musicState.track];

   // Update source if track has changed
   if (audio.src !== newSrc) {
    audio.src = newSrc;
   }

   audio.muted = musicState.isMuted;

   // Handle play/pause state
   if (musicState.isPlaying) {
    // play() is async and returns a promise
    // This will now be called only after the first user interaction.
    audio.play().catch(e => console.error("Audio playback failed:", e));
   } else {
    audio.pause();
   }
  }, [musicState]);

  // Cleanup audio on component unmount
  useEffect(() => {
   return () => {
    audioRef.current?.pause();
   };
  }, []);

  const handlePlayPauseMusic = () => {
   setMusicState(prev => ({ ...prev, isPlaying: !prev.isPlaying }));
  };

  const handleSwitchTrack = (track: 'classical' | 'dubstep') => {
   setMusicState(prev => ({ ...prev, track }));
  };

  const handleMuteToggle = () => {
   setMusicState(prev => ({ ...prev, isMuted: !prev.isMuted }));
  };

  // Combat Log Utility
  const addLogEntry = useCallback((type: LogEntryType, message: string, icon?: string) => {
   const timestamp = new Date().toLocaleTimeString('en-US', { hour12: false });
   setCombatLog(prev => [{ type, message, timestamp, icon }, ...prev.slice(0, 49)]);
  }, []);

  // Game State Management
  const SAVE_GAME_KEY = 'milehigh_save_game';

  const handleSaveGame = useCallback(() => {
   try {
    const gameState = {
     playerState,
     reputation,
     inventory,
     combatLog,
     loreContext,
     missionHistory,
    };
    localStorage.setItem(SAVE_GAME_KEY, JSON.stringify(gameState));
    addLogEntry('system', 'Game state saved.', '💾');
   } catch (e) {
    console.error("Failed to save game state:", e);
    addLogEntry('system', 'Error: Failed to save game state.', '☠️');
   }
  }, [playerState, reputation, inventory, combatLog, loreContext, missionHistory, addLogEntry]);

  const handleLoadGame = useCallback(() => {
   try {
    const savedGame = localStorage.getItem(SAVE_GAME_KEY);
    if (savedGame) {
     const gameState = JSON.parse(savedGame);
     if (gameState.playerState && gameState.reputation) {
      setPlayerState(gameState.playerState);
      setReputation(gameState.reputation);
      setInventory(gameState.inventory || initialInventory);
      setCombatLog(gameState.combatLog || []);
      setLoreContext(gameState.loreContext || INITIAL_LORE_CONTEXT);
      setMissionHistory(gameState.missionHistory || []);
      addLogEntry('system', 'Game state loaded.', '💾');

      // Reset any active combat when loading
      setCurrentEnemy(null);
      setCurrentTurn('none');
      setIsPlayerDefeated(false);
      localStorage.removeItem('milehigh_combat_session');
     } else {
      addLogEntry('system', 'Error: Save file corrupted or invalid.', '☠️');
     }
    } else {
     addLogEntry('system', 'No saved game found.', '💾');
    }
   } catch (e) {
    console.error("Failed to load game state:", e);
    addLogEntry('system', 'Error: Failed to load game state.', '☠️');
   }
  }, [addLogEntry, initialInventory]);

  const handleResetGame = useCallback(() => {
   if (window.confirm("Are you sure you want to start a New Game? All unsaved progress will be lost.")) {
    localStorage.removeItem(SAVE_GAME_KEY);
    localStorage.removeItem('milehigh_combat_session');
    setPlayerState(initialPlayerState);
    setReputation(initialReputation);
    setInventory(initialInventory);
    setCombatLog([]);
    setLoreContext(INITIAL_LORE_CONTEXT);
    setCurrentMission(null);
    setMissionHistory([]);
    setCurrentEnemy(null);
    setCurrentTurn('none');
    setIsPlayerDefeated(false);
    addLogEntry('system', 'New game started. Welcome to The Verse.', '🌟');
   }
  }, [addLogEntry, initialInventory]);

  // Load combat session from local storage on initial mount
  useEffect(() => {
   const savedSession = localStorage.getItem('milehigh_combat_session');
   if (savedSession) {
    try {
     const parsedState = JSON.parse(savedSession);
     // Simple validation to ensure we have a valid combat state
     if (parsedState.currentEnemy && parsedState.playerState) {
      setPlayerState(parsedState.playerState);
      setInventory(parsedState.inventory);
      setCombatLog(parsedState.combatLog);
      setCurrentEnemy(parsedState.currentEnemy);
      setEnemyHealth(parsedState.enemyHealth);
      setEnemyMaxHealth(parsedState.enemyMaxHealth);
      setEnemyActiveEffects(parsedState.enemyActiveEffects);
      setCurrentTurn(parsedState.currentTurn);
      setIsPlayerDefeated(parsedState.isPlayerDefeated || false);
      console.log("Resuming combat session from local storage.");
     } else {
      // The saved data is not a valid combat session, so clear it.
      localStorage.removeItem('milehigh_combat_session');
     }
    } catch (e) {
     console.error("Failed to parse saved combat session:", e);
     localStorage.removeItem('milehigh_combat_session');
    }
   }
  }, []); // Empty array ensures this runs only once on mount.

  // Save combat session to local storage when state changes during combat
  const combatState = useMemo(() => ({
   playerState,
   inventory,
   combatLog,
   currentEnemy,
   enemyHealth,
   enemyMaxHealth,
   enemyActiveEffects,
   currentTurn,
   isPlayerDefeated
  }), [playerState, inventory, combatLog, currentEnemy, enemyHealth, enemyMaxHealth, enemyActiveEffects, currentTurn, isPlayerDefeated]);

  useEffect(() => {
   if (combatState.currentEnemy && !combatState.isPlayerDefeated && combatState.enemyHealth > 0) {
    try {
     localStorage.setItem('milehigh_combat_session', JSON.stringify(combatState));
    } catch (e) {
     console.error("Failed to save combat session:", e);
    }
   } else if (!combatState.currentEnemy || combatState.isPlayerDefeated || combatState.enemyHealth <= 0) {
     localStorage.removeItem('milehigh_combat_session');
   }
  }, [combatState]);

  const selectedCharacter = useMemo(() => CHARACTERS.find(c => c.name === playerState.selectedCharacterName) as Character, [playerState.selectedCharacterName]);

  // Intel Log Utility
  const addIntelToLog = useCallback((intel: string) => {
   setLoreContext(prev => `${prev}\n\n**NEW INTEL - ${new Date().toISOString()}**\n${intel}`);
   addLogEntry('intel', `New Intel Added: ${intel.substring(0, 50)}...`);
  }, [addLogEntry]);

  // Reputation Utility
  const updateReputation = useCallback((faction: string, change: number) => {
   setReputation(prev => {
    const current = prev[faction] || 0;
    const newScore = Math.max(0, Math.min(100, current + change));
    return { ...prev, [faction]: newScore };
   });
   addLogEntry('reputation', `Reputation with ${faction} ${change > 0 ? `+${change}` : change}.`);
  }, [addLogEntry]);

  // Time travel handler
  const handleTimeTravel = useCallback((period: PlayerState['currentTimePeriod']) => {
   if (currentEnemy) {
    addLogEntry('system', "Cannot time travel while a threat is active.");
    return;
   }
   playSound('temporal_rift');
   setIsTimeTraveling(true);
   setTimeout(() => {
    setPlayerState(prev => ({...prev, currentTimePeriod: period}));
    addLogEntry('system', `Temporal Rift stabilized in: ${period.replace(/_/g, ' ')}.`);
    setTimeout(() => setIsTimeTraveling(false), 1000); // Animation duration
   }, 500);
  }, [currentEnemy, addLogEntry]);

  // Helper function to apply status effects
  const applyEffect = useCallback((target: 'player' | 'enemy', effect: Omit<ActiveEffect, 'duration'> & { duration: number }) => {
   const fullEffect = { ...effect, key: Date.now() }; // Add key for unique indicators
   if (target === 'player') {
    setPlayerState(p => ({
     ...p,
     activeEffects: [...p.activeEffects, { type: fullEffect.type, duration: fullEffect.duration, potency: fullEffect.potency }]
    }));
    setPlayerEffectIndicator({ type: fullEffect.type, key: fullEffect.key });
    addLogEntry('status_effect', `You are afflicted with ${fullEffect.type.replace('_', ' ')}!`);
   } else if (currentEnemy) {
    setEnemyActiveEffects(prev => [...prev, { type: fullEffect.type, duration: fullEffect.duration, potency: fullEffect.potency }]);
    setEnemyEffectIndicator({ type: fullEffect.type, key: fullEffect.key });
    addLogEntry('status_effect', `${currentEnemy.name} is afflicted with ${fullEffect.type.replace('_', ' ')}!`);
   }
  }, [currentEnemy, addLogEntry]);

  const handleTakeDamage = useCallback((damage: number) => {
   if (isPlayerDefeated) return;
   playSound('enemy_attack_hit');
   setPlayerAction('taking_damage');
   setTimeout(() => setPlayerAction('idle'), 300);

   setPlayerState(prev => ({
    ...prev,
    health: Math.max(0, prev.health - damage),
    rage: Math.min(100, prev.rage + damage),
   }));
   addLogEntry('damage', `You took ${damage} damage.`);
  }, [addLogEntry, isPlayerDefeated]);

  // Handles the start of the player's turn (status effects, etc.)
  useEffect(() => {
   if (currentTurn === 'player' && currentEnemy && !isPlayerDefeated && playerState.health > 0) {
    let poisonDamage = 0;
    const updatedEffects = playerState.activeEffects
     .map(effect => {
      if (effect.type === 'poison' && effect.potency) {
       poisonDamage += effect.potency;
      }
      return { ...effect, duration: effect.duration - 1 };
     })
     .filter(effect => effect.duration > 0);

    if (poisonDamage > 0) {
     addLogEntry('status_effect', `You take ${poisonDamage} poison damage.`);
     // State update will trigger the defeat useEffect if health goes <= 0
     setPlayerState(p => ({
      ...p,
      health: Math.max(0, p.health - poisonDamage),
      activeEffects: updatedEffects
     }));
    } else {
     setPlayerState(p => ({ ...p, activeEffects: updatedEffects }));
    }
    // Only log "Your turn" if player isn't defeated by poison this turn
    if (playerState.health - poisonDamage > 0) {
      addLogEntry('system', 'Your turn!');
    }
   }
  }, [currentTurn]); // Simplified dependencies

  // This handles the enemy's entire turn
  useEffect(() => {
   if (currentTurn === 'enemy' && currentEnemy && !isPlayerDefeated && playerState.health > 0) {
    const performEnemyTurn = async () => {
     addLogEntry('system', "Enemy's turn...");
     await new Promise(resolve => setTimeout(resolve, 1000));

     // A. Apply enemy status effects
     let poisonDamage = 0;
     const updatedEffects = enemyActiveEffects
      .map(effect => {
       if (effect.type === 'poison' && effect.potency) poisonDamage += effect.potency;
       return { ...effect, duration: effect.duration - 1 };
      })
      .filter(effect => effect.duration > 0);

     setEnemyActiveEffects(updatedEffects);

     if (poisonDamage > 0) {
      addLogEntry('status_effect', `${currentEnemy.name} takes ${poisonDamage} poison damage.`);
      const newHealth = Math.max(0, enemyHealth - poisonDamage);
      setEnemyHealth(newHealth);
      // Victory is handled by the other useEffect, so we can stop here.
      if (newHealth <= 0) return;
     }

     // B. Enemy "thinks"
     await new Promise(resolve => setTimeout(resolve, 1200));
     if (isPlayerDefeated) return; // Re-check defeat state before acting

     // C. Enemy acts
     const ability = currentEnemy.abilities[Math.floor(Math.random() * currentEnemy.abilities.length)];
     addLogEntry('system', `${currentEnemy.name} uses ${ability.name}!`);

     let actionType: EnemyActionState = 'attacking';
     let animationDuration = 500;
     if (ability.type === 'special' || ability.type === 'magical') {
      playSound('enemy_cast');
      actionType = 'casting';
      animationDuration = 800;
     } else {
      playSound('enemy_attack_lunge');
     }
     setEnemyAction(actionType);
     await new Promise(resolve => setTimeout(resolve, animationDuration));
     setEnemyAction('idle');

     // D. Apply damage/effects to player
     handleTakeDamage(ability.damage);
     if (ability.effect && Math.random() < ability.effect.chance) {
      applyEffect('player', { type: ability.effect.type, duration: ability.effect.duration, potency: ability.effect.potency });
     }

     // E. Transition to player's turn (defeat is handled by useEffect)
     setCurrentTurn('player');
    };

    performEnemyTurn();
   }
  }, [currentTurn]); // Simplified dependencies


  const handleFindEncounter = useCallback(() => {
   const randomEnemy = ENEMY_AI_ARCHETYPES[Math.floor(Math.random() * ENEMY_AI_ARCHETYPES.length)];
   setCurrentEnemy(randomEnemy);
   const maxHealth = Math.floor(randomEnemy.defense * 10 + (playerState.level * 20));
   setEnemyHealth(maxHealth);
   setEnemyMaxHealth(maxHealth);
   setEnemyActiveEffects([]);
   addLogEntry('system', `Threat detected: ${randomEnemy.name}!`);
   setCurrentTurn('player'); // Kick off combat
  }, [playerState.level, addLogEntry]);

  const handlePlayerAttack = useCallback(() => {
   if (currentTurn !== 'player' || !currentEnemy) return;
   setCurrentTurn('processing');
   playSound('player_attack_swing');
   setPlayerAction('attacking');

   const isCrit = Math.random() < 0.2;
   const damage = selectedCharacter.strength * (isCrit ? 1.5 : 1);

   if (isCrit) {
    addLogEntry('critical_damage', `${selectedCharacter.name} lands a critical hit for ${Math.round(damage)} damage!`, '💥');
   } else {
    addLogEntry('damage', `${selectedCharacter.name} attacks ${currentEnemy.name} for ${Math.round(damage)} damage.`);
   }

   // Timeout to reset player animation state after it finishes
   setTimeout(() => {
    setPlayerAction('idle');
   }, 500); // Matches `player-attack-lunge` animation duration (0.5s)

   // Timeout for impact effects (sound, enemy damage animation)
   setTimeout(() => {
    playSound('player_attack_hit', isCrit ? 0.8 : 0.6);
    setEnemyAction('taking_damage');
    const newEnemyHealth = Math.max(0, enemyHealth - damage);
    setEnemyHealth(newEnemyHealth);

    // Timeout to reset enemy animation and switch turn
    setTimeout(() => {
     setEnemyAction('idle');
     if (newEnemyHealth > 0) {
      setCurrentTurn('enemy');
     }
    }, 300); // Matches `enemy-take-damage` animation duration (0.3s)
   }, 250); // Impact happens at the peak of the lunge animation (0.5s / 2)

  }, [currentTurn, currentEnemy, enemyHealth, selectedCharacter, addLogEntry]);

  const handleLoot = useCallback(() => {
   if (!currentEnemy) return;
   const xpGained = currentEnemy.xpValue;
   setPlayerState(prev => {
    const newExperience = prev.experience + xpGained;
    const expForNext = getExperienceForNextLevel(prev.level);
    if (newExperience >= expForNext) {
     playSound('level_up');
     addLogEntry('xp', `LEVEL UP! You reached Level ${prev.level + 1}!`, '🌟');
     return { ...prev, level: prev.level + 1, experience: newExperience - expForNext };
    }
    return { ...prev, experience: newExperience };
   });
   addLogEntry('xp', `Gained ${xpGained} experience points.`);
   setCurrentEnemy(null);
   setCurrentTurn('none');
   localStorage.removeItem('milehigh_combat_session');
  }, [currentEnemy, addLogEntry]);

  const handleResetCombat = useCallback(() => {
   addLogEntry('system', 'Combat reset. Ready for next encounter.');
   setPlayerState(initialPlayerState);
   setCurrentEnemy(null);
   setEnemyHealth(0);
   setEnemyMaxHealth(0);
   setEnemyActiveEffects([]);
   setIsPlayerDefeated(false);
   setCombatLog([]);
   setCurrentTurn('none');
   localStorage.removeItem('milehigh_combat_session');
  }, [addLogEntry]);

  // Check for win/loss conditions. These are the final arbiters of the combat state.
  useEffect(() => {
   if (currentEnemy && enemyHealth <= 0 && currentTurn !== 'none') {
    addLogEntry('system', `${currentEnemy.name} has been defeated!`, '🏆');
    playSound('enemy_defeated');
    setCurrentTurn('none'); // End combat
   }
  }, [enemyHealth, currentEnemy, addLogEntry, currentTurn]);

  useEffect(() => {
   if (playerState.health <= 0 && !isPlayerDefeated && currentEnemy) {
    addLogEntry('system', `You have been defeated!`, '☠️');
    playSound('enemy_defeated');
    setIsPlayerDefeated(true);
    setCurrentTurn('none'); // End combat
   }
  }, [playerState.health, isPlayerDefeated, currentEnemy, addLogEntry]);

  const handleUseItem = useCallback((item: Item) => {
   if (currentTurn !== 'player' || !currentEnemy) return;

   if ((item as Consumable).amount) {
    setCurrentTurn('processing');
    playSound('item_use_potion');
    const consumable = item as Consumable;
    if (item.name.toLowerCase().includes('health')) {
     setPlayerState(p => ({ ...p, health: Math.min(100, p.health + consumable.amount) }));
     addLogEntry('heal', `Used ${item.name}, recovered ${consumable.amount} health.`);
    } else if (item.name.toLowerCase().includes('mana')) {
     setPlayerState(p => ({ ...p, mana: Math.min(100, p.mana + consumable.amount) }));
     addLogEntry('mana', `Used ${item.name}, recovered ${consumable.amount} mana.`);
    }
    setInventory(prev => prev.filter(i => i.name !== item.name));
    setTimeout(() => setCurrentTurn('enemy'), 1000);
   }
  }, [currentTurn, currentEnemy, addLogEntry]);

  const handleCraftItem = useCallback((recipe: Recipe) => {
   const canCraft = recipe.ingredients.every(ing => inventory.filter(i => i.name === ing.name).length >= ing.quantity);
   if (canCraft) {
    playSound('crafting_success');
    let tempInventory = [...inventory];
    recipe.ingredients.forEach(ing => {
     for (let i = 0; i < ing.quantity; i++) {
      const itemIndex = tempInventory.findIndex(item => item.name === ing.name);
      if (itemIndex > -1) {
       tempInventory.splice(itemIndex, 1);
      }
     }
    });
    setInventory([...tempInventory, recipe.result]);
    addLogEntry('craft', `Successfully crafted ${recipe.result.name}.`);
   } else {
    playSound('ui_error');
    addLogEntry('system', `Missing ingredients to craft ${recipe.result.name}.`);
   }
  }, [inventory, addLogEntry]);

  const showBreakEffect = (name: string) => {
   setActiveBreakEffect(name);
   setTimeout(() => setActiveBreakEffect(null), 2000);
  };

  const onUseRageBurst = () => {
   if (playerState.rage < 100 || !selectedCharacter.limitBreak || currentTurn !== 'player' || !currentEnemy) return;

   setCurrentTurn('processing');
   playSound('rage_burst');
   showBreakEffect(selectedCharacter.limitBreak.name);
   setScreenEffect('rage-burst-flash');
   setTimeout(() => setScreenEffect(null), 800);

   setPlayerState(p => ({...p, rage: 0}));

   const damage = Math.round(selectedCharacter.strength * 3);
   addLogEntry('critical_damage', `${selectedCharacter.name} unleashes ${selectedCharacter.limitBreak.name} for ${damage} damage!`, '🔥');

   setTimeout(() => {
    if (isPlayerDefeated || !currentEnemy) return;

    setEnemyAction('taking_damage');
    playSound('enemy_take_damage', 0.8);
    const newEnemyHealth = Math.max(0, enemyHealth - damage);
    setEnemyHealth(newEnemyHealth);

    setTimeout(() => {
     setEnemyAction('idle');
     if (newEnemyHealth > 0) {
      setCurrentTurn('enemy');
     }
    }, 500);
   }, 1000);
  };

  const onUseSpiritBreak = () => {
   if (playerState.mana < 100 || !selectedCharacter.spiritBreak || currentTurn !== 'player' || !currentEnemy) return;

   setCurrentTurn('processing');
   playSound('spirit_break');
   showBreakEffect(selectedCharacter.spiritBreak.name);
   setScreenEffect('spirit-break-flash');
   setTimeout(() => setScreenEffect(null), 800);

   setPlayerState(p => ({...p, mana: 0}));

   const damage = Math.round(selectedCharacter.heart * 2.5);
   addLogEntry('critical_damage', `${selectedCharacter.name} channels ${selectedCharacter.spiritBreak.name} for ${damage} damage!`, '✨');

   setTimeout(() => {
    if (isPlayerDefeated || !currentEnemy) return;

    applyEffect('enemy', { type: 'armor_break', duration: 5 });
    setEnemyAction('taking_damage');
    playSound('enemy_take_damage', 0.8);
    const newEnemyHealth = Math.max(0, enemyHealth - damage);
    setEnemyHealth(newEnemyHealth);

    setTimeout(() => {
     setEnemyAction('idle');
     if (newEnemyHealth > 0) {
      setCurrentTurn('enemy');
     }
    }, 500);
   }, 1000);
  };

  const onUseAllianceBreak = () => {
   if (playerState.alliance < 100 || currentTurn !== 'player' || !currentEnemy) return;

   setCurrentTurn('processing');
   playSound('alliance_finisher');
   showBreakEffect(selectedCharacter.novaminaadFinisher?.name || "Alliance Break");
   setScreenEffect('alliance-finisher-flash');
   setTimeout(() => setScreenEffect(null), 1500);

   setPlayerState(p => ({...p, alliance: 0}));

   const damage = Math.round(selectedCharacter.strength * 5);
   addLogEntry('critical_damage', `The Alliance strikes! ${selectedCharacter.novaminaadFinisher?.name || "Alliance Break"} hits for ${damage} damage!`, '🌟');

   setTimeout(() => {
    if (isPlayerDefeated || !currentEnemy) return;

    applyEffect('enemy', { type: 'weakness_exposed', duration: 10 });
    setEnemyAction('taking_damage');
    playSound('enemy_take_damage', 1.0);
    const newEnemyHealth = Math.max(0, enemyHealth - damage);
    setEnemyHealth(newEnemyHealth);

    setTimeout(() => {
     setEnemyAction('idle');
     if (newEnemyHealth > 0) {
      setCurrentTurn('enemy');
     }
    }, 500);
   }, 1500);
  };

  const isCombatActive = currentTurn === 'player' || currentTurn === 'enemy' || currentTurn === 'processing';

  const getPlayerAnimationClass = () => {
   switch (playerAction) {
    case 'attacking':
     return 'animate-player-attack';
    case 'taking_damage':
     return 'animate-player-damage';
    default:
     return '';
   }
  };

  return (
   <div className={`min-h-screen ${screenEffect || ''}`}>
    <Header />
    <main className="container mx-auto p-4 max-w-7xl">
     <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">

      {/* Left Column: Player Dashboard */}
      <div className="lg:col-span-1 space-y-8">
       <Card
        title="Player Status"
        index={0}
        shadowType="white"
        className={getPlayerAnimationClass()}
        headerGlow
       >
        <CombatStatusDisplay
         playerState={playerState}
         setPlayerState={setPlayerState}
         experienceForNextLevel={getExperienceForNextLevel(playerState.level)}
         playerEffectIndicator={playerEffectIndicator}
        />
       </Card>

       <Card title="Character Sheet" index={1} isCollapsible headerGlow>
        <StatsCard
         character={selectedCharacter}
         level={playerState.level}
         experience={playerState.experience}
         experienceForNextLevel={getExperienceForNextLevel(playerState.level)}
        />
       </Card>

       <Card title="Inventory" index={2} isCollapsible>
        <Inventory items={inventory} onUseItem={handleUseItem} isPlayerTurn={currentTurn === 'player'} />
       </Card>

       <Card title="Crafting" index={3} isCollapsible>
        <Crafting recipes={CRAFTING_RECIPES} inventory={inventory} onCraft={handleCraftItem} />
       </Card>

       <Card title="Objectives" index={4} isCollapsible headerGlow>
        <ObjectiveGoals loreContext={loreContext} currentTimePeriod={playerState.currentTimePeriod} />
       </Card>

       <Card title="Combat Log" index={5} isCollapsible>
        <CombatLog log={combatLog} />
       </Card>

       <Card title="Reputation & Standings" index={6} isCollapsible startCollapsed>
        <ReputationTracker reputation={reputation} />
       </Card>

       <Card title="Temporal Controls" index={7}>
        <TemporalRiftCard currentTimePeriod={playerState.currentTimePeriod} onTimeTravel={handleTimeTravel} disabled={isCombatActive} />
       </Card>

       <Card title="Background Music" index={8}>
        <BackgroundMusicPlayer
         musicState={musicState}
         onPlayPause={handlePlayPauseMusic}
         onSwitchTrack={handleSwitchTrack}
         onMuteToggle={handleMuteToggle}
        />
       </Card>

        <Card title="Game State" index={9}>
         <GameStateControls onSave={handleSaveGame} onLoad={handleLoadGame} onReset={handleResetGame} />
       </Card>
      </div>

      {/* Center Column: World Interaction & Intel */}
      <div className="lg:col-span-2 space-y-8">

       <Card title="Combat Encounter" index={10} headerGlow>
        <EnemyEncounter
         currentEnemy={currentEnemy}
         enemyHealth={enemyHealth}
         enemyMaxHealth={enemyMaxHealth}
         selectedCharacter={selectedCharacter}
         playerState={playerState}
         setPlayerState={setPlayerState}
         onAttack={handlePlayerAttack}
         onTakeDamage={handleTakeDamage}
         onFindEncounter={handleFindEncounter}
         onLoot={handleLoot}
         onUseRageBurst={onUseRageBurst}
         onUseSpiritBreak={onUseSpiritBreak}
         onUseAllianceBreak={onUseAllianceBreak}
         activeBreakEffect={activeBreakEffect}
         enemyAction={enemyAction}
         enemyActiveEffects={enemyActiveEffects}
         enemyEffectIndicator={enemyEffectIndicator}
         isPlayerDefeated={isPlayerDefeated}
         onResetCombat={handleResetCombat}
         currentTurn={currentTurn}
        />
       </Card>

       <Card title="World Map" index={11}>
        <WorldMap currentTimePeriod={playerState.currentTimePeriod} />
       </Card>

       <Card title="Character Showcase" index={12} headerGlow>
        <CharacterShowcase characters={CHARACTERS} />
       </Card>

       <Card title="Threat Detection System" index={13} isCollapsible headerGlow>
        <ThreatDetector loreContext={loreContext} />
       </Card>

       <Card title="Full Scenario Generator" index={14} isCollapsible headerGlow>
        <ScenarioGenerator
         loreContext={loreContext}
         voiceProfiles={INITIAL_VOICE_PROFILES}
         onNewIntel={addIntelToLog}
         currentTimePeriod={playerState.currentTimePeriod}
         onScenarioGenerated={setActiveScenario}
        />
       </Card>

       <Card title="Live Conversation" index={15} isCollapsible headerGlow>
        <LiveConversation loreContext={loreContext} onNewIntel={addIntelToLog} voiceProfiles={INITIAL_VOICE_PROFILES} />
       </Card>

       <Card title="Lore Explorer" index={16} isCollapsible headerGlow>
        <LoreExplorer loreContext={loreContext} currentTimePeriod={playerState.currentTimePeriod} />
       </Card>

       <Card title="Manual Intel Injection" index={17} isCollapsible>
        <IntelLogger addIntelToLog={addIntelToLog} />
       </Card>

        <Card title="Antagonists" index={18} isCollapsible startCollapsed>
         <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          {ANTAGONISTS.map((ant, i) => (
           <AntagonistCard key={ant.name} antagonist={ant} loreContext={loreContext} index={i} />
          ))}
         </div>
       </Card>

       <Card title="Grand Finale Scene Generator" index={19} isCollapsible startCollapsed headerGlow>
        <BattleSceneGenerator loreContext={loreContext} />
       </Card>

       <Card title="Power Systems C# Showcase" index={20} isCollapsible startCollapsed headerGlow>
        <PowerSystemsVisualizer />
       </Card>

      </div>

     </div>
    </main>
   </div>
  );
};