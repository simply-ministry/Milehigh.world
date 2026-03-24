import React, { useState, useEffect, useCallback, useRef } from 'react';

// Components
import { Header } from './components/Header';
import { Card } from './components/Card';
import { CharacterShowcase } from './components/CharacterShowcase';
import { LoreExplorer } from './components/LoreExplorer';
import { WorldMap } from './components/WorldMap';
import { ReputationTracker } from './components/Milehigh.world-main/ReputationTracker';
import { IntelLogger } from './components/IntelLogger';
import { MissionControl } from './components/MissionControl';
import { LiveConversation } from './components/LiveConversation';
import { CombatStatusDisplay } from './components/CombatStatusDisplay';
import { EnemyEncounter } from './components/EnemyEncounter';
import { CombatLog } from './components/CombatLog';
import { ObjectiveGoals } from './components/ObjectiveGoals';
import { ProductionHub } from './components/ProductionHub';
import { ThreatDetector } from './components/ThreatDetector';
import { BackgroundMusicPlayer } from './components/BackgroundMusicPlayer';
import { TimeTravelAnimation } from './components/Milehigh.world-main/TimeTravelAnimation';

// Constants and Types
import {
    CHARACTERS,
    ITEMS,
    CRAFTING_RECIPES,
    ENEMY_AI_ARCHETYPES,
    INITIAL_VOICE_PROFILES
} from './constants';
// Fix: INITIAL_LORE_CONTEXT is exported from geminiService, not constants.
import { INITIAL_LORE_CONTEXT } from './services/geminiService';

import type {
    Character, Item, Recipe, Reputation, MissionGenerationResponse,
    CombatLogEntry, LogEntryType, PlayerState, ActiveEffect, EffectType,
    EnemyAIArchetype, EnemyActionState, StatBoosts, Scenario, CSharpScript
} from './types';

// Services and Utils
import { initAudio } from './utils/soundService';

const LEVEL_XP_BASE = 1000;

export const App: React.FC = () => {
    // State Management
    const [intelLog, setIntelLog] = useState<string[]>([]);
    const loreContext = `${INITIAL_LORE_CONTEXT}\n\n--- OMEGA.ONE MEMORY CORE UPDATES ---\n${intelLog.join('\n\n')}`;

    const [reputation, setReputation] = useState<Reputation>({
        'Ɲōvəmîŋāđ Alliance': 50,
        "Cirrus's Trust": 40,
        'AṬĤŸŁĞÅŘÐ Honor': 30,
        'ÅẒ̌ŪŘẸ ĤĒĪĜĤṬ§ Authority': 20,
        'Hydraustis Palare Collective': 15,
        'ŁĪƝĈ Underground': 25,
        "The Void's Grasp": 10,
    });

    const [combatLog, setCombatLog] = useState<CombatLogEntry[]>([]);

    const [playerState, setPlayerState] = useState<PlayerState>(() => {
        try {
            const savedState = localStorage.getItem('playerState_mhw');
            return savedState ? JSON.parse(savedState) : {
                health: 100,
                mana: 100,
                rage: 0,
                alliance: 0,
                level: 1,
                experience: 0,
                skillPoints: 1,
                selectedCharacterName: CHARACTERS[0].name,
                activeEffects: [],
                currentTimePeriod: 'present',
                statBoosts: { strength: 0, dexterity: 0, defense: 0, vigor: 0, heart: 0, voidAffinity: 0, nexusAttunement: 0, oneiricResonance: 0, propheticClarity: 0 },
                equippedItems: {},
                difficulty: 'normal',
                winStreak: 0,
            };
        } catch {
            // If parsing fails, return default state
            return {
                health: 100, mana: 100, rage: 0, alliance: 0, level: 1, experience: 0, skillPoints: 1,
                selectedCharacterName: CHARACTERS[0].name, activeEffects: [], currentTimePeriod: 'present',
                statBoosts: { strength: 0, dexterity: 0, defense: 0, vigor: 0, heart: 0, voidAffinity: 0, nexusAttunement: 0, oneiricResonance: 0, propheticClarity: 0 },
                equippedItems: {}, difficulty: 'normal', winStreak: 0,
            };
        }
    });

    const [inventory, setInventory] = useState<Item[]>(() => [
        ...ITEMS.weapons, ...ITEMS.armor, ...ITEMS.consumables
    ]);

    // Combat State
    const [currentEnemy, setCurrentEnemy] = useState<EnemyAIArchetype | null>(null);
    const [enemyHealth, setEnemyHealth] = useState(100);
    const [enemyMaxHealth, setEnemyMaxHealth] = useState(100);
    const [enemyActiveEffects, setEnemyActiveEffects] = useState<ActiveEffect[]>([]);
    const [currentTurn, setCurrentTurn] = useState<'player' | 'enemy' | 'processing' | 'none'>('none');

    // UI/FX State
    const [timeTravelEffect, setTimeTravelEffect] = useState<'to_past' | 'to_present' | 'to_future' | null>(null);

    // Mission State
    const [currentMission, setCurrentMission] = useState<MissionGenerationResponse | null>(null);
    const [missionHistory, setMissionHistory] = useState<MissionGenerationResponse[]>([]);

    // Audio State
    const [isAudioInitialized, setIsAudioInitialized] = useState(false);
    const [musicState, setMusicState] = useState({ isPlaying: false, track: 'classical' as 'classical' | 'dubstep', isMuted: false });
    const musicTracks = {
        classical: 'https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/sounds/classical-lofi.mp3',
        dubstep: 'https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/sounds/dubstep.mp3'
    };

    // Callbacks & Handlers
    const log = useCallback((message: string, type: LogEntryType, icon?: string) => {
        const timestamp = new Date().toLocaleTimeString('en-US', { hour12: false });
        setCombatLog(prev => [{ type, message, timestamp, icon }, ...prev].slice(0, 100));
    }, []);

    const addIntelToLog = useCallback((intel: string) => {
        setIntelLog(prev => [...prev, intel]);
        log(`New intel added to Omega.one memory core.`, 'intel');
    }, [log]);

    const updateReputation = useCallback((faction: string, change: number) => {
        setReputation(prev => {
            const newScore = Math.max(0, Math.min(100, (prev[faction] || 0) + change));
            log(`Reputation with ${faction} changed by ${change}. New score: ${newScore}`, 'reputation');
            return { ...prev, [faction]: newScore };
        });
    }, [log]);

    const addMissionToHistory = useCallback((mission: MissionGenerationResponse) => {
        setMissionHistory(prev => [...prev, mission]);
    }, []);

    const handleScenarioGenerated = (scenario: Scenario, type: 'narrative' | 'combat') => {
        log(`New scenario generated: "${scenario.objective}"`, 'system');
    };

    // Simplified combat handler placeholders
    const handleAttack = () => { log("Player attacks!", "damage") };
    const handleTakeDamage = (damage: number) => { log(`Player takes ${damage} damage.`, "damage") };
    const handleFindEncounter = () => {
        const randomEnemy = ENEMY_AI_ARCHETYPES[Math.floor(Math.random() * ENEMY_AI_ARCHETYPES.length)];
        setCurrentEnemy(randomEnemy);
        setEnemyHealth(randomEnemy.defense * 20); // Example health calculation
        setEnemyMaxHealth(randomEnemy.defense * 20);
        setCurrentTurn('player');
        log(`Encountered ${randomEnemy.name}!`, 'system');
    };

    const onSpendPoint = (stat: keyof StatBoosts) => {
        if (playerState.skillPoints > 0) {
            setPlayerState(p => ({
                ...p,
                skillPoints: p.skillPoints - 1,
                statBoosts: { ...p.statBoosts, [stat]: p.statBoosts[stat] + 1 }
            }));
            log(`Allocated skill point to ${stat}.`, 'system');
        }
    };

    // Initialize audio on first user interaction
    useEffect(() => {
        const initialize = async () => {
            if (!isAudioInitialized) {
                try {
                    await initAudio();
                    setIsAudioInitialized(true);
                } catch (error) {
                    console.error("Audio could not be initialized by user gesture:", error);
                }
            }
        };
        window.addEventListener('click', initialize, { once: true });
        return () => window.removeEventListener('click', initialize);
    }, [isAudioInitialized]);


    return (
        <div className="p-4 md:p-8" onClick={() => !isAudioInitialized && initAudio().then(() => setIsAudioInitialized(true))}>
            {timeTravelEffect && <TimeTravelAnimation travelDirection={timeTravelEffect} />}
            <Header />
            <main className="grid grid-cols-1 lg:grid-cols-2 xl:grid-cols-3 2xl:grid-cols-4 gap-8">

                <Card title="Character Dossier" index={0} isCollapsible={true} headerGlow>
                    <CharacterShowcase />
                </Card>

                <Card title="Reputation & Faction Standing" index={1} isCollapsible={true}>
                    <ReputationTracker reputation={reputation} />
                </Card>

                <Card title="Verse Topography" index={2} isCollapsible={true}>
                    <WorldMap currentTimePeriod={playerState.currentTimePeriod} />
                </Card>

                <Card title="Lore Archives" index={3} isCollapsible={true}>
                    <LoreExplorer loreContext={loreContext} currentTimePeriod={playerState.currentTimePeriod} />
                </Card>

                <Card title="Mission Control" index={4} isCollapsible={true} startCollapsed={true}>
                    <MissionControl
                        loreContext={loreContext}
                        addIntelToLog={addIntelToLog}
                        updateReputation={updateReputation}
                        voiceProfiles={INITIAL_VOICE_PROFILES}
                        currentMission={currentMission}
                        setCurrentMission={setCurrentMission}
                        missionHistory={missionHistory}
                        addMissionToHistory={addMissionToHistory}
                    />
                </Card>

                <Card title="Live Comms" index={5} isCollapsible={true} startCollapsed={true}>
                    <LiveConversation loreContext={loreContext} onNewIntel={addIntelToLog} voiceProfiles={INITIAL_VOICE_PROFILES} />
                </Card>

                <Card title="Objective Tracker" index={6} isCollapsible={true} startCollapsed={true}>
                    <ObjectiveGoals loreContext={loreContext} currentTimePeriod={playerState.currentTimePeriod} />
                </Card>

                <Card title="Production Hub" index={7} isCollapsible={false} className="xl:col-span-2 2xl:col-span-3">
                    <ProductionHub
                        loreContext={loreContext}
                        voiceProfiles={INITIAL_VOICE_PROFILES}
                        onNewIntel={addIntelToLog}
                        currentTimePeriod={playerState.currentTimePeriod}
                        onScenarioGenerated={handleScenarioGenerated}
                    />
                </Card>

                <div className="space-y-8 xl:col-span-2 2xl:col-span-1">
                    <Card title="Combat Encounter" index={8}>
                        <EnemyEncounter
                            currentEnemy={currentEnemy}
                            enemyHealth={enemyHealth}
                            enemyMaxHealth={enemyMaxHealth}
                            selectedCharacter={CHARACTERS.find(c => c.name === playerState.selectedCharacterName)!}
                            playerState={playerState}
                            setPlayerState={setPlayerState}
                            onAttack={handleAttack}
                            onTakeDamage={handleTakeDamage}
                            onFindEncounter={handleFindEncounter}
                            onLoot={() => setCurrentEnemy(null)}
                            onUseRageBurst={() => log('Used Rage Burst!', 'critical_damage')}
                            onUseSpiritBreak={() => log('Used Spirit Break!', 'critical_damage')}
                            onUseAllianceBreak={() => log('Used Alliance Break!', 'critical_damage')}
                            activeBreakEffect={null}
                            enemyAction={'idle'}
                            enemyActiveEffects={enemyActiveEffects}
                            enemyEffectIndicator={null}
                            isPlayerDefeated={playerState.health <= 0}
                            onRetryCombat={() => { /* Placeholder */ }}
                            onLoadGame={() => { /* Placeholder */ }}
                            currentTurn={currentTurn}
                            isWeaknessHit={false}
                            cooldownTimers={{}}
                        />
                    </Card>

                    <Card title="Player Status" index={9}>
                        <CombatStatusDisplay
                            playerState={playerState}
                            setPlayerState={setPlayerState}
                            experienceForNextLevel={LEVEL_XP_BASE * playerState.level}
                            playerEffectIndicator={null}
                            inventory={inventory}
                            onEquipItem={() => {}}
                            onUnequipItem={() => {}}
                            isPlayerTurn={currentTurn === 'player'}
                            onSpendPoint={onSpendPoint}
                            cooldownTimers={{}}
                            selectedCharacter={CHARACTERS.find(c => c.name === playerState.selectedCharacterName)!}
                        />
                    </Card>
                </div>

                 <Card title="Combat Log" index={10} isCollapsible={true} startCollapsed={true}>
                    <CombatLog log={combatLog} />
                </Card>

                <Card title="Intel Injection" index={11} isCollapsible={true} startCollapsed={true}>
                    <IntelLogger addIntelToLog={addIntelToLog} />
                </Card>

                <Card title="Background Music" index={12} isCollapsible={true} startCollapsed={true}>
                    <BackgroundMusicPlayer
                         musicState={musicState}
                         onPlayPause={() => { /* Placeholder */ }}
                         onSwitchTrack={() => { /* Placeholder */ }}
                         onMuteToggle={() => { /* Placeholder */ }}
                    />
                </Card>

                <Card title="Threat Detector" index={13} isCollapsible={true} startCollapsed={true}>
                    <ThreatDetector loreContext={loreContext} />
                </Card>
            </main>
        </div>
    );
}