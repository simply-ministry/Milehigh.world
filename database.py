# SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
"""Manages the SQLite database for the game.

This script handles the creation, initialization, and data access for the
game's content database. It defines the schema for all game-related data,
including characters, items, quests, and more. It also provides functions
for saving and loading game state.

This module uses a global `_class_loader` to dynamically instantiate game
object classes, which avoids circular dependencies between the database and
the main game logic.
"""

import sqlite3
import json
from typing import Callable, Optional, Any, Dict, List

# The default filename for the SQLite database.
DB_FILE: str = "game_content.db"

# A global callable used to dynamically load game object classes.
_class_loader: Optional[Callable[[str, Dict[str, Any]], Any]] = None


def get_db_connection(db_file: str = DB_FILE) -> sqlite3.Connection:
    """Establishes a connection to the SQLite database.

    Args:
        db_file: The path to the database file. Defaults to DB_FILE.

    Returns:
        A database connection object with row_factory set to sqlite3.Row,
        which allows for dictionary-like access to rows.
    """
    conn = sqlite3.connect(db_file)
    conn.row_factory = sqlite3.Row
    return conn


def create_schema(cursor: sqlite3.Cursor) -> None:
    """Creates the database schema, defining all tables and relationships.

    This function defines the structure for characters, items, world data,
    quests, and more. It uses `CREATE TABLE IF NOT EXISTS` to avoid errors
    if the database has already been initialized.

    Args:
        cursor: The database cursor to execute SQL commands.
    """
    # Core Tables
    cursor.execute("""
    CREATE TABLE IF NOT EXISTS Characters (
        character_id INTEGER PRIMARY KEY,
        name TEXT UNIQUE NOT NULL,
        title TEXT,
        level INTEGER DEFAULT 1,
        experience INTEGER DEFAULT 0,
        health INTEGER,
        mana INTEGER,
        strength INTEGER,
        agility INTEGER,
        intelligence INTEGER,
        vitality INTEGER,
        background TEXT,
        alignment TEXT
    )""")

    cursor.execute("""
    CREATE TABLE IF NOT EXISTS Players (
        player_id INTEGER PRIMARY KEY,
        user_account_id INTEGER,
        play_time INTEGER,
        FOREIGN KEY (player_id) REFERENCES Characters(character_id)
    )""")

    cursor.execute("""
    CREATE TABLE IF NOT EXISTS NonPlayerCharacters (
        npc_id INTEGER PRIMARY KEY,
        faction_id INTEGER,
        dialogue_id INTEGER,
        FOREIGN KEY (npc_id) REFERENCES Characters(character_id)
    )""")

    cursor.execute("""
    CREATE TABLE IF NOT EXISTS Noviminaad (
        noviminaad_id INTEGER PRIMARY KEY,
        prophecy_role TEXT,
        FOREIGN KEY (noviminaad_id) REFERENCES Characters(character_id)
    )""")

    cursor.execute("""
    CREATE TABLE IF NOT EXISTS Abilities (
        ability_id INTEGER PRIMARY KEY,
        name TEXT UNIQUE NOT NULL,
        description TEXT,
        ability_type TEXT,
        mana_cost INTEGER,
        cooldown REAL
    )""")

    cursor.execute("""
    CREATE TABLE IF NOT EXISTS CharacterAbilities (
        character_id INTEGER,
        ability_id INTEGER,
        ability_level INTEGER,
        PRIMARY KEY (character_id, ability_id),
        FOREIGN KEY (character_id) REFERENCES Characters(character_id),
        FOREIGN KEY (ability_id) REFERENCES Abilities(ability_id)
    )""")

    cursor.execute("""
    CREATE TABLE IF NOT EXISTS Items (
        item_id INTEGER PRIMARY KEY,
        name TEXT UNIQUE NOT NULL,
        description TEXT,
        item_type TEXT,
        value INTEGER,
        weight REAL
    )""")

    cursor.execute("""
    CREATE TABLE IF NOT EXISTS Weapons (
        weapon_id INTEGER PRIMARY KEY,
        damage INTEGER,
        weapon_type TEXT,
        attack_speed REAL,
        FOREIGN KEY (weapon_id) REFERENCES Items(item_id)
    )""")

    cursor.execute("""
    CREATE TABLE IF NOT EXISTS Armor (
        armor_id INTEGER PRIMARY KEY,
        defense INTEGER,
        armor_type TEXT,
        FOREIGN KEY (armor_id) REFERENCES Items(item_id)
    )""")

    cursor.execute("""
    CREATE TABLE IF NOT EXISTS CharacterInventory (
        character_id INTEGER,
        item_id INTEGER,
        quantity INTEGER,
        PRIMARY KEY (character_id, item_id),
        FOREIGN KEY (character_id) REFERENCES Characters(character_id),
        FOREIGN KEY (item_id) REFERENCES Items(item_id)
    )""")

    # World and Lore Tables
    cursor.execute("""
    CREATE TABLE IF NOT EXISTS Locations (
        location_id INTEGER PRIMARY KEY,
        name TEXT UNIQUE NOT NULL,
        description TEXT,
        location_type TEXT,
        parent_location_id INTEGER,
        FOREIGN KEY (parent_location_id) REFERENCES Locations(location_id)
    )""")

    cursor.execute("""
    CREATE TABLE IF NOT EXISTS Factions (
        faction_id INTEGER PRIMARY KEY,
        name TEXT UNIQUE NOT NULL,
        description TEXT,
        reputation_effects TEXT
    )""")

    cursor.execute("""
    CREATE TABLE IF NOT EXISTS Quests (
        quest_id INTEGER PRIMARY KEY,
        name TEXT UNIQUE NOT NULL,
        description TEXT,
        start_location_id INTEGER,
        end_location_id INTEGER,
        reward_experience INTEGER,
        reward_items TEXT,
        faction_id INTEGER,
        FOREIGN KEY (start_location_id) REFERENCES Locations(location_id),
        FOREIGN KEY (end_location_id) REFERENCES Locations(location_id),
        FOREIGN KEY (faction_id) REFERENCES Factions(faction_id)
    )""")

    cursor.execute("""
    CREATE TABLE IF NOT EXISTS QuestObjectives (
        quest_id INTEGER,
        objective_id INTEGER,
        objective_type TEXT,
        objective_target INTEGER,
        objective_amount INTEGER,
        objective_description TEXT,
        is_complete BOOLEAN,
        PRIMARY KEY (quest_id, objective_id)
    )""")

    cursor.execute("""
    CREATE TABLE IF NOT EXISTS Dialogues (
        dialogue_id INTEGER PRIMARY KEY,
        text TEXT,
        next_dialogue_id INTEGER,
        condition_quest_id INTEGER,
        condition_objective_id INTEGER,
        condition_faction_id INTEGER,
        response_text TEXT,
        response_effects TEXT,
        FOREIGN KEY (next_dialogue_id) REFERENCES Dialogues(dialogue_id)
    )""")

    cursor.execute("""
    CREATE TABLE IF NOT EXISTS Lore (
        lore_id INTEGER PRIMARY KEY,
        title TEXT UNIQUE NOT NULL,
        text TEXT,
        location_id INTEGER,
        FOREIGN KEY (location_id) REFERENCES Locations(location_id)
    )""")


def populate_initial_data(cursor: sqlite3.Cursor) -> None:
    """Populates the database with essential game data for a new game.

    This includes creating default characters, items, weapons, and armor
    to ensure the game world is not empty on first run. It uses
    `INSERT OR IGNORE` to prevent duplicate entries on subsequent runs.

    Args:
        cursor: The database cursor to execute commands.
    """
    # Characters
    cursor.execute("INSERT OR IGNORE INTO Characters (name, title, health, mana, strength, agility, intelligence, vitality) VALUES (?, ?, ?, ?, ?, ?, ?, ?)",
                   ('Aeron', 'The Brave', 100, 50, 15, 10, 5, 12))
    cursor.execute("INSERT OR IGNORE INTO Characters (name, title, health, mana, strength, agility, intelligence, vitality) VALUES (?, ?, ?, ?, ?, ?, ?, ?)",
                   ('Kane', 'The Rival', 250, 20, 20, 8, 5, 15))

    # Items
    cursor.execute("INSERT OR IGNORE INTO Items (name, description, item_type, value, weight) VALUES (?, ?, ?, ?, ?)",
                   ('Valiant Sword', 'A blade that shines with honor.', 'Weapon', 100, 5.0))
    cursor.execute("INSERT OR IGNORE INTO Items (name, description, item_type, value, weight) VALUES (?, ?, ?, ?, ?)",
                   ('Aethelgard Plate', 'Sturdy plate armor of a royal knight.', 'Armor', 150, 20.0))

    # Weapons
    cursor.execute("INSERT OR IGNORE INTO Weapons (weapon_id, damage, weapon_type, attack_speed) SELECT item_id, 25, 'Sword', 1.0 FROM Items WHERE name='Valiant Sword'")
    # Armor
    cursor.execute("INSERT OR IGNORE INTO Armor (armor_id, defense, armor_type) SELECT item_id, 15, 'Heavy' FROM Items WHERE name='Aethelgard Plate'")


def init_db(db_file: str = DB_FILE) -> None:
    """Initializes the database, creating the schema and populating data.

    This is the main function to set up a new game database. It ensures
    the schema exists and that it contains the initial set of game content.

    Args:
        db_file: The path to the database file. Defaults to DB_FILE.
    """
    conn = get_db_connection(db_file)
    cursor = conn.cursor()
    create_schema(cursor)
    populate_initial_data(cursor)
    conn.commit()
    conn.close()


def set_class_loader(loader: Callable[[str, Dict[str, Any]], Any]) -> None:
    """Sets the class loader function for deserializing game objects.

    This function allows the database module to be decoupled from the main
    game logic. The loader function is responsible for taking a class name
    and a data dictionary and instantiating the correct Python class.

    Args:
        loader: A function that takes a class name and data dict and
            returns an instance of a game object class.
    """
    global _class_loader
    _class_loader = loader


def get_character_data(name: str, conn: Optional[sqlite3.Connection] = None) -> Optional[sqlite3.Row]:
    """Fetches a single character's data from the database by name.

    Args:
        name: The name of the character to retrieve.
        conn: An existing database connection. If None, a new connection
            will be established. Defaults to None.

    Returns:
        A row object containing the character's data, or None if the
        character is not found.
    """
    close_conn = False
    if conn is None:
        conn = get_db_connection()
        close_conn = True

    cursor = conn.cursor()
    cursor.execute("SELECT * FROM Characters WHERE name = ?", (name,))
    character_data = cursor.fetchone()

    if close_conn:
        conn.close()
    return character_data


def get_item_data(name: str, conn: Optional[sqlite3.Connection] = None) -> Optional[sqlite3.Row]:
    """Fetches base data for a single item from the Items table.

    Args:
        name: The name of the item to retrieve.
        conn: An existing database connection. If None, a new connection
            will be established. Defaults to None.

    Returns:
        A row object containing the item's data, or None if the item is
        not found.
    """
    close_conn = False
    if conn is None:
        conn = get_db_connection()
        close_conn = True

    cursor = conn.cursor()
    cursor.execute("SELECT * FROM Items WHERE name = ?", (name,))
    item_data = cursor.fetchone()

    if close_conn:
        conn.close()
    return item_data


def get_weapon_data(item_id: int, conn: Optional[sqlite3.Connection] = None) -> Optional[sqlite3.Row]:
    """Fetches specific weapon data from the Weapons table using an item ID.

    Args:
        item_id: The ID of the item, which corresponds to a weapon.
        conn: An existing database connection. If None, a new connection
            will be established. Defaults to None.

    Returns:
        A row object containing the weapon's specific data, or None if no
        weapon is found for the given ID.
    """
    close_conn = False
    if conn is None:
        conn = get_db_connection()
        close_conn = True

    cursor = conn.cursor()
    cursor.execute("SELECT * FROM Weapons WHERE weapon_id = ?", (item_id,))
    weapon_data = cursor.fetchone()

    if close_conn:
        conn.close()
    return weapon_data


def get_armor_data(item_id: int, conn: Optional[sqlite3.Connection] = None) -> Optional[sqlite3.Row]:
    """Fetches specific armor data from the Armor table using an item ID.

    Args:
        item_id: The ID of the item, which corresponds to armor.
        conn: An existing database connection. If None, a new connection
            will be established. Defaults to None.

    Returns:
        A row object containing the armor's specific data, or None if no
        armor is found for the given ID.
    """
    close_conn = False
    if conn is None:
        conn = get_db_connection()
        close_conn = True

    cursor = conn.cursor()
    cursor.execute("SELECT * FROM Armor WHERE armor_id = ?", (item_id,))
    armor_data = cursor.fetchone()

    if close_conn:
        conn.close()
    return armor_data


def save_game(save_name: str, scene_manager: Any) -> None:
    """Saves the current game state to the database.

    Note:
        This is a placeholder function. A full implementation would involve
        serializing the state of the `scene_manager` and all its contained
        game objects into the database.

    Args:
        save_name: The name for the save file or slot.
        scene_manager: The main scene manager object containing the game
            state to be saved.
    """
    # In a real implementation, this would serialize the scene_manager
    # and store it in the database. For now, we'll just acknowledge it.
    print(f"Game state for '{save_name}' saved (simulation).")


def load_game(save_name: str) -> None:
    """Loads a game state from the database.

    Note:
        This is a placeholder function. A full implementation would deserialize
        data from the database and reconstruct the `SceneManager` state. It
        currently returns None to allow for testing the game's startup
        and loading logic without a full save system.

    Args:
        save_name: The name of the save file or slot to load.

    Returns:
        This function currently returns None to indicate that no save file
        was loaded.
    """
    # This function is intended to be mocked in tests.
    # Returning None simulates the behavior of a save not being found.
    return None


if __name__ == '__main__':
    print("Initializing game content database...")
    init_db()
    print("Database initialized successfully.")