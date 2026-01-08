import * as fs from 'fs';
import * as path from 'path';
import {
  CHARACTERS,
  ANTAGONISTS,
  PHYSICS_SCRIPTS,
  SCENE_MANAGEMENT_SCRIPTS,
  CINEMATICS_SCRIPTS,
  ABILITIES_BASE_SCRIPT,
  ALLIANCE_POWER_SCRIPT,
  CHARACTER_DATA_SCRIPTS,
  generateCharacterScripts,
  generateAntagonistScripts,
} from './constants';

const assetsDir = path.join(__dirname, '..', '..', 'Assets', 'Scripts');

const outputDirs = {
  heroes: path.join(assetsDir, 'Heroes'),
  villains: path.join(assetsDir, 'Villains'),
  data: path.join(assetsDir, 'Data'),
  core: path.join(assetsDir, 'Core'),
  physics: path.join(assetsDir, 'Physics'),
  sceneManagement: path.join(assetsDir, 'SceneManagement'),
  editor: path.join(assetsDir, 'SceneManagement', 'Editor'),
  cinematics: path.join(assetsDir, 'Cinematics'),
  base: path.join(assetsDir, 'Characters', '_Base'),
};

function ensureDirectoryExistence(filePath: string) {
  const dirname = path.dirname(filePath);
  if (fs.existsSync(dirname)) {
    return true;
  }
  ensureDirectoryExistence(dirname);
  fs.mkdirSync(dirname);
}

function writeScript(filePath: string, content: string) {
  ensureDirectoryExistence(filePath);
  fs.writeFileSync(filePath, content);
  console.log(`Wrote ${filePath}`);
}

function generate() {
  // Generate character ability scripts
  const characterScripts = generateCharacterScripts();
  for (const charName in characterScripts) {
    const script = characterScripts[charName];
    writeScript(path.join(outputDirs.heroes, script.fileName), script.code);
  }

  // Generate antagonist AI scripts
  const antagonistScripts = generateAntagonistScripts();
  for (const antName in antagonistScripts) {
    const script = antagonistScripts[antName];
    writeScript(path.join(outputDirs.villains, script.fileName), script.code);
  }

  // Write physics scripts
  for (const key in PHYSICS_SCRIPTS) {
    const script = PHYSICS_SCRIPTS[key as keyof typeof PHYSICS_SCRIPTS];
    writeScript(path.join(outputDirs.physics, script.fileName), script.code);
  }

  // Write scene management scripts
  writeScript(path.join(outputDirs.sceneManagement, SCENE_MANAGEMENT_SCRIPTS.runtime.fileName), SCENE_MANAGEMENT_SCRIPTS.runtime.code);
  writeScript(path.join(outputDirs.editor, SCENE_MANAGEMENT_SCRIPTS.editor.fileName), SCENE_MANAGEMENT_SCRIPTS.editor.code);

  // Write cinematics scripts
  for (const key in CINEMATICS_SCRIPTS) {
    const script = CINEMATICS_SCRIPTS[key as keyof typeof CINEMATICS_SCRIPTS];
    writeScript(path.join(outputDirs.cinematics, script.fileName), script.code);
  }

  // Write base and manager scripts
  writeScript(path.join(outputDirs.base, ABILITIES_BASE_SCRIPT.fileName), ABILITIES_BASE_SCRIPT.code);
  writeScript(path.join(outputDirs.core, ALLIANCE_POWER_SCRIPT.fileName), ALLIANCE_POWER_SCRIPT.code);

  // Write data scripts
  writeScript(path.join(outputDirs.data, CHARACTER_DATA_SCRIPTS.dataClass.fileName), CHARACTER_DATA_SCRIPTS.dataClass.code);
  writeScript(path.join(outputDirs.data, 'Editor', CHARACTER_DATA_SCRIPTS.factory.fileName), CHARACTER_DATA_SCRIPTS.factory.code);
  // Note: jsonData is not a C# script and will not be written by this generator.
}

generate();
