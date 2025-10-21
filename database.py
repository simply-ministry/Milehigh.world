"""Manages the game's SQLite database for content and saved games.

This module is responsible for all database operations, including creating the
database schema, populating it with initial game content, and providing an
API for querying and modifying game data. It is designed to be the central
repository for all persistent game information, such as character stats,
item properties, and quest details.

The module also includes placeholder functions for saving and loading game
states, which are intended to be implemented or mocked for testing purposes.
To avoid circular dependencies with the main game logic, it uses a dynamic
class loader (`set_class_loader`) to instantiate game object classes from
database records.
"""

import sqlite3
import json
from typing import Callable, Optional, Any, Dict, List

# The default filename for the SQLite database.
DB_FILE: str = "game_content.db"

# A global callable used to dynamically load game object classes.
_class_loader: Optional[Callable[[str, Dict[str, Any]], Any]] = None


def get_db_connection(db_file: str = DB_FILE) -> sqlite3.Connection:
    """Establishes and configures a connection to the SQLite database.

    This function connects to the specified SQLite database file and sets the
    `row_factory` to `sqlite3.Row`. This allows for accessing query results
    like dictionaries, which is more convenient than using tuple-based access.

    Args:
        db_file (str): The file path for the SQLite database. Defaults to the
            global `DB_FILE` constant.

    Returns:
        sqlite3.Connection: A database connection object ready for use.
    """
    conn = sqlite3.connect(db_file)
    conn.row_factory = sqlite3.Row
    return conn


def create_schema(cursor: sqlite3.Cursor) -> None:
    """Defines and creates the database schema.

    This function executes a series of SQL `CREATE TABLE` statements to build
    the database structure required for the game. It defines tables for
    characters, items, quests, and other core game elements. The use of
    `IF NOT EXISTS` ensures that the function can be run safely multiple
    times without causing errors.

    Args:
        cursor (sqlite3.Cursor): A database cursor to execute the SQL commands.
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
    """Populates the database with the initial set of game content.

    This function inserts the essential data needed to start a new game,
    such as the main characters and some basic items. Using `INSERT OR IGNORE`
    prevents duplicate data from being inserted if the function is run more
    than once.

    Args:
        cursor (sqlite3.Cursor): A database cursor to execute the INSERT commands.
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
    """Initializes the database by creating and populating it.

    This function serves as the main entry point for setting up the database.
    It establishes a connection, creates the schema if it doesn't exist, and
    populates the tables with initial game data.

    Args:
        db_file (str): The file path for the SQLite database. Defaults to the
            global `DB_FILE` constant.
    """
    conn = get_db_connection(db_file)
    cursor = conn.cursor()
    create_schema(cursor)
    populate_initial_data(cursor)
    conn.commit()
    conn.close()


def set_class_loader(loader: Callable[[str, Dict[str, Any]], Any]) -> None:
    """Sets the global function used for dynamically loading classes.

    This function is a key part of decoupling the database module from the
    rest of the game's class structure. The provided loader is called when
    deserializing game objects, allowing the main game module to control how
    classes are instantiated from database records.

    Args:
        loader (Callable): A function that accepts a class name (str) and a
            data dictionary and returns an instance of the corresponding class.
    """
    global _class_loader
    _class_loader = loader


def get_character_data(name: str, conn: Optional[sqlite3.Connection] = None) -> Optional[sqlite3.Row]:
    """Retrieves all data for a single character from the database by name.

    Args:
        name (str): The name of the character to look up.
        conn (Optional[sqlite3.Connection]): An optional existing database
            connection. If not provided, a new one will be created.

    Returns:
        Optional[sqlite3.Row]: A `sqlite3.Row` object containing the
        character's data, which allows for dictionary-style access to columns.
        Returns `None` if no character with the given name is found.
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
    """Retrieves the base data for an item from the `Items` table.

    Args:
        name (str): The name of the item to look up.
        conn (Optional[sqlite3.Connection]): An optional existing database
            connection.

    Returns:
        Optional[sqlite3.Row]: A `sqlite3.Row` object with the item's base
        data (e.g., name, description, value), or `None` if not found.
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
    """Retrieves weapon-specific data from the `Weapons` table.

    Args:
        item_id (int): The ID of the item, which is a foreign key in the
            `Weapons` table.
        conn (Optional[sqlite3.Connection]): An optional existing database
            connection.

    Returns:
        Optional[sqlite3.Row]: A `sqlite3.Row` object with the weapon's
        stats (e.g., damage, type), or `None` if no weapon with the given
        ID is found.
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
    """Retrieves armor-specific data from the `Armor` table.

    Args:
        item_id (int): The ID of the item, which is a foreign key in the
            `Armor` table.
        conn (Optional[sqlite3.Connection]): An optional existing database
            connection.

    Returns:
        Optional[sqlite3.Row]: A `sqlite3.Row` object with the armor's
        stats (e.g., defense), or `None` if no armor with the given ID is
        found.
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
        save_name (str): The identifier for the save slot.
        scene_manager (Any): The main `SceneManager` object, which contains
            the complete state of the game to be saved.
    """
    # In a real implementation, this would serialize the scene_manager
    # and store it in the database. For now, we'll just acknowledge it.
    print(f"Game state for '{save_name}' saved (simulation).")


def load_game(save_name: str) -> None:
    """Loads a game state from the database.

    Note:
        This is a placeholder function. A full implementation would deserialize
        data from the database and reconstruct the `SceneManager` state. It
        currently returns `None` to allow for testing the game's startup
        and loading logic without a full save system.

    Args:
        save_name (str): The identifier for the save slot to load.

    Returns:
        None: This function currently returns `None` to indicate that no save
        file was loaded, allowing calling code to handle the case of starting
        a new game.
    """
    # This function is intended to be mocked in tests.
    # Returning None simulates the behavior of a save not being found.
    return None


if __name__ == '__main__':
    print("Initializing game content database...")
    init_db()
    print("Database initialized successfully.")