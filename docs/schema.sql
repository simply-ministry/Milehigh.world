--  Schema for Milehigh.world: Into the Void

--  I. Core Tables

CREATE TABLE Characters (
    character_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    title VARCHAR(255),
    level INT UNSIGNED NOT NULL,
    experience INT UNSIGNED NOT NULL,
    health INT UNSIGNED NOT NULL,
    mana INT UNSIGNED NOT NULL,
    strength INT UNSIGNED NOT NULL,
    agility INT UNSIGNED NOT NULL,
    intelligence INT UNSIGNED NOT NULL,
    vitality INT UNSIGNED NOT NULL,
    background TEXT,
    alignment VARCHAR(50) NOT NULL
);

CREATE TABLE Players (
    player_id INT PRIMARY KEY,
    user_account_id INT NOT NULL,
    play_time INT UNSIGNED NOT NULL,
    FOREIGN KEY (player_id) REFERENCES Characters(character_id) ON DELETE CASCADE,
    FOREIGN KEY (user_account_id) REFERENCES UserAccounts(user_account_id) ON DELETE CASCADE
);

CREATE TABLE NonPlayerCharacters (
    npc_id INT PRIMARY KEY,
    faction_id INT,
    dialogue_id INT,
    FOREIGN KEY (npc_id) REFERENCES Characters(character_id) ON DELETE CASCADE,
    FOREIGN KEY (faction_id) REFERENCES Factions(faction_id) ON DELETE SET NULL,
    FOREIGN KEY (dialogue_id) REFERENCES Dialogues(dialogue_id) ON DELETE SET NULL
);

CREATE TABLE Noviminaad (
    noviminaad_id INT PRIMARY KEY,
    prophecy_role VARCHAR(255) NOT NULL,
    FOREIGN KEY (noviminaad_id) REFERENCES Characters(character_id) ON DELETE CASCADE
);

CREATE TABLE CharacterAbilities (
    character_id INT,
    ability_id INT,
    ability_level INT UNSIGNED NOT NULL,
    PRIMARY KEY (character_id, ability_id),
    FOREIGN KEY (character_id) REFERENCES Characters(character_id) ON DELETE CASCADE,
    FOREIGN KEY (ability_id) REFERENCES Abilities(ability_id) ON DELETE CASCADE
);

CREATE TABLE Abilities (
    ability_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    ability_type VARCHAR(50) NOT NULL,
    mana_cost INT UNSIGNED NOT NULL,
    cooldown FLOAT NOT NULL
);

CREATE TABLE Items (
    item_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    item_type VARCHAR(50) NOT NULL,
    value INT UNSIGNED NOT NULL,
    weight FLOAT NOT NULL
);

CREATE TABLE Weapons (
    weapon_id INT PRIMARY KEY,
    damage INT UNSIGNED NOT NULL,
    weapon_type VARCHAR(50) NOT NULL,
    attack_speed FLOAT NOT NULL,
    FOREIGN KEY (weapon_id) REFERENCES Items(item_id) ON DELETE CASCADE
);

CREATE TABLE Armor (
    armor_id INT PRIMARY KEY,
    defense INT UNSIGNED NOT NULL,
    armor_type VARCHAR(50) NOT NULL,
    FOREIGN KEY (armor_id) REFERENCES Items(item_id) ON DELETE CASCADE
);

CREATE TABLE CharacterInventory (
    character_id INT,
    item_id INT,
    quantity INT UNSIGNED NOT NULL,
    PRIMARY KEY (character_id, item_id),
    FOREIGN KEY (character_id) REFERENCES Characters(character_id) ON DELETE CASCADE,
    FOREIGN KEY (item_id) REFERENCES Items(item_id) ON DELETE CASCADE
);

--  II. World and Lore Tables

CREATE TABLE Locations (
    location_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    location_type VARCHAR(50) NOT NULL,
    parent_location_id INT,
    FOREIGN KEY (parent_location_id) REFERENCES Locations(location_id) ON DELETE SET NULL
);

CREATE TABLE Factions (
    faction_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    reputation_effects JSON
);

CREATE TABLE Quests (
    quest_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    start_location_id INT NOT NULL,
    end_location_id INT NOT NULL,
    reward_experience INT UNSIGNED NOT NULL,
    reward_items JSON,
    faction_id INT,
    FOREIGN KEY (start_location_id) REFERENCES Locations(location_id) ON DELETE CASCADE,
    FOREIGN KEY (end_location_id) REFERENCES Locations(location_id) ON DELETE CASCADE,
    FOREIGN KEY (faction_id) REFERENCES Factions(faction_id) ON DELETE SET NULL
);

CREATE TABLE QuestObjectives (
    quest_id INT,
    objective_id INT,
    objective_type VARCHAR(50) NOT NULL,
    objective_target INT, --  Consider specific FK later
    objective_amount INT UNSIGNED NOT NULL,
    objective_description TEXT,
    is_complete BOOLEAN NOT NULL,
    PRIMARY KEY (quest_id, objective_id),
    FOREIGN KEY (quest_id) REFERENCES Quests(quest_id) ON DELETE CASCADE
);

CREATE TABLE Dialogues (
    dialogue_id INT AUTO_INCREMENT PRIMARY KEY,
    text TEXT NOT NULL,
    next_dialogue_id INT,
    condition_quest_id INT,
    condition_objective_id INT,
    condition_faction_id INT,
    response_text TEXT,
    response_effects JSON,
    FOREIGN KEY (next_dialogue_id) REFERENCES Dialogues(dialogue_id) ON DELETE SET NULL,
    FOREIGN KEY (condition_quest_id) REFERENCES Quests(quest_id) ON DELETE SET NULL,
    FOREIGN KEY (condition_faction_id) REFERENCES Factions(faction_id) ON DELETE SET NULL
);

CREATE TABLE Lore (
    lore_id INT AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    text TEXT NOT NULL,
    location_id INT,
    FOREIGN KEY (location_id) REFERENCES Locations(location_id) ON DELETE SET NULL
);

--  III. Multiplayer/Community Tables

CREATE TABLE UserAccounts (
    user_account_id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(255) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    registration_date DATETIME NOT NULL
);

CREATE TABLE Guilds (
    guild_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    leader_player_id INT NOT NULL,
    creation_date DATETIME NOT NULL,
    description TEXT,
    FOREIGN KEY (leader_player_id) REFERENCES Players(player_id) ON DELETE CASCADE
);

CREATE TABLE GuildMembers (
    guild_id INT,
    player_id INT,
    join_date DATETIME NOT NULL,
    rank VARCHAR(50) NOT NULL,
    PRIMARY KEY (guild_id, player_id),
    FOREIGN KEY (guild_id) REFERENCES Guilds(guild_id) ON DELETE CASCADE,
    FOREIGN KEY (player_id) REFERENCES Players(player_id) ON DELETE CASCADE
);

CREATE TABLE ChatMessages (
    message_id INT AUTO_INCREMENT PRIMARY KEY,
    sender_player_id INT NOT NULL,
    channel VARCHAR(50) NOT NULL,
    timestamp DATETIME NOT NULL,
    content TEXT NOT NULL,
    FOREIGN KEY (sender_player_id) REFERENCES Players(player_id) ON DELETE CASCADE
);

--  IV. Indexing (Example - Add more as needed)

CREATE INDEX idx_player_user_account ON Players(user_account_id);