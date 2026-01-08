import React from 'react';
import type { Character } from '../types';

interface CharacterCardProps {
  character: Character;
  index?: number;
  loreContext?: string;
  expanded?: boolean;
}

export const CharacterCard: React.FC<CharacterCardProps> = ({ character, expanded = false }) => {
  return (
    <div className="bg-slate-800 rounded-lg shadow-xl p-6 border border-slate-700 hover:border-cyan-400 transition-colors">
      <div className="flex flex-col space-y-4">
        {/* Character Header */}
        <div className="text-center">
          <h3 className="text-2xl font-bold text-cyan-400">{character.name}</h3>
          <p className="text-slate-400 text-sm italic">{character.title}</p>
          <p className="text-slate-500 text-xs mt-1">{character.archetype}</p>
        </div>

        {/* Character Image */}
        {character.imageUrl && (
          <div className="w-full h-48 bg-slate-900 rounded-lg overflow-hidden">
            <img 
              src={character.imageUrl} 
              alt={character.name}
              className="w-full h-full object-cover"
            />
          </div>
        )}

        {/* Description */}
        <p className="text-slate-300 text-sm">{character.description}</p>

        {expanded && (
          <>
            {/* Attributes Section */}
            <div className="space-y-2">
              <h4 className="text-lg font-semibold text-cyan-400 border-b border-slate-700 pb-2">Attributes</h4>
              <div className="grid grid-cols-2 gap-2">
                <AttributeBar label="Strength" value={character.strength} />
                <AttributeBar label="Dexterity" value={character.dexterity} />
                <AttributeBar label="Defense" value={character.defense} />
                <AttributeBar label="Vigor" value={character.vigor} />
                <AttributeBar label="Heart" value={character.heart} />
                <AttributeBar label="Void Affinity" value={character.voidAffinity} />
                <AttributeBar label="Nexus Attunement" value={character.nexusAttunement} />
                <AttributeBar label="Oneiric Resonance" value={character.oneiricResonance} />
                <AttributeBar label="Prophetic Clarity" value={character.propheticClarity} />
              </div>
            </div>

            {/* Special Abilities */}
            {(character.limitBreak || character.spiritBreak || character.novaminaadFinisher) && (
              <div className="space-y-3">
                <h4 className="text-lg font-semibold text-cyan-400 border-b border-slate-700 pb-2">Special Abilities</h4>
                
                {character.limitBreak && (
                  <div className="bg-slate-900 p-3 rounded">
                    <p className="text-red-400 font-semibold text-sm">🔥 {character.limitBreak.name}</p>
                    <p className="text-slate-400 text-xs mt-1">{character.limitBreak.description}</p>
                  </div>
                )}

                {character.spiritBreak && (
                  <div className="bg-slate-900 p-3 rounded">
                    <p className="text-purple-400 font-semibold text-sm">✨ {character.spiritBreak.name}</p>
                    <p className="text-slate-400 text-xs mt-1">{character.spiritBreak.description}</p>
                  </div>
                )}

                {character.novaminaadFinisher && (
                  <div className="bg-slate-900 p-3 rounded">
                    <p className="text-cyan-400 font-semibold text-sm">🌟 {character.novaminaadFinisher.name}</p>
                    <p className="text-slate-400 text-xs mt-1">{character.novaminaadFinisher.description}</p>
                  </div>
                )}
              </div>
            )}
          </>
        )}

        {/* Damage Type Badge */}
        <div className="flex justify-center">
          <span className={`px-3 py-1 rounded-full text-xs font-semibold ${
            character.damageType === 'Physical' ? 'bg-red-900 text-red-200' :
            character.damageType === 'Void' ? 'bg-purple-900 text-purple-200' :
            'bg-blue-900 text-blue-200'
          }`}>
            {character.damageType} Damage
          </span>
        </div>
      </div>
    </div>
  );
};

interface AttributeBarProps {
  label: string;
  value: number;
  maxValue?: number;
}

const AttributeBar: React.FC<AttributeBarProps> = ({ label, value, maxValue = 50 }) => {
  const percentage = Math.min((value / maxValue) * 100, 100);
  
  return (
    <div className="space-y-1">
      <div className="flex justify-between items-center">
        <span className="text-slate-400 text-xs">{label}</span>
        <span className="text-cyan-400 text-xs font-semibold">{value}</span>
      </div>
      <div className="w-full bg-slate-700 rounded-full h-2">
        <div 
          className="bg-gradient-to-r from-cyan-500 to-blue-500 h-2 rounded-full transition-all duration-300"
          style={{ width: `${percentage}%` }}
        />
      </div>
    </div>
  );
};