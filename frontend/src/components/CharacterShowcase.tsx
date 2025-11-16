import React, { useState } from 'react';
import type { Character } from '../types';
import { CharacterCard } from './CharacterCard';

interface CharacterShowcaseProps {
  characters: Character[];
}

export const CharacterShowcase: React.FC<CharacterShowcaseProps> = ({ characters }) => {
  const [selectedCharacter, setSelectedCharacter] = useState<Character | null>(null);

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="text-center space-y-2">
        <h2 className="text-4xl font-black text-cyan-400 tracking-tight">Character Showcase</h2>
        <p className="text-slate-400 text-lg">Explore the Champions and Bring Them to Life</p>
      </div>

      {/* Characters Grid */}
      <div className="space-y-4">
        <h3 className="text-2xl font-bold text-slate-200">Characters</h3>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {characters.map((character, index) => (
            <div 
              key={character.name}
              onClick={() => setSelectedCharacter(character)}
              className="cursor-pointer transform transition-transform hover:scale-105"
            >
              <CharacterCard character={character} index={index} expanded={false} />
            </div>
          ))}
        </div>
      </div>

      {/* Selected Character Detail Modal */}
      {selectedCharacter && (
        <div 
          className="fixed inset-0 bg-black bg-opacity-75 flex items-center justify-center p-4 z-50"
          onClick={() => setSelectedCharacter(null)}
        >
          <div 
            className="bg-slate-900 rounded-lg max-w-4xl w-full max-h-[90vh] overflow-y-auto"
            onClick={(e) => e.stopPropagation()}
          >
            <div className="p-6 space-y-6">
              {/* Close Button */}
              <div className="flex justify-between items-center">
                <h2 className="text-3xl font-bold text-cyan-400">{selectedCharacter.name}</h2>
                <button
                  onClick={() => setSelectedCharacter(null)}
                  className="text-slate-400 hover:text-white text-2xl font-bold"
                >
                  ✕
                </button>
              </div>

              {/* Character Details */}
              <CharacterCard character={selectedCharacter} expanded={true} />
            </div>
          </div>
        </div>
      )}
    </div>
  );
};
