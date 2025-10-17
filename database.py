"""Manages the SQLite database for the game.

This script handles the creation, initialization, and data access for the
game's content database. It defines the schema for all game-related data,
including characters, items, quests, and more. It also provides functions
for saving and loading game state.
"""

import sqlite3
import json

DB_FILE = "game_content.db"


def get_db_connection(db_file=DB_FILE):
    """Establishes a connection to the database.

    Args:
        db_file (str, optional): The path to the database file.
            Defaults to DB_FILE.

    Returns:
        sqlite3.Connection: A connection object to the database.
    """
    conn = sqlite3.connect(db_file)
    conn.row_factory = sqlite3.Row
    return conn


def create_schema(cursor):
    """Creates the database schema if it doesn't already exist.

    This function defines the table structure for the entire game, including
    core game elements like characters and items, as well as world and lore
    information.

    Args:
        cursor (sqlite3.Cursor): The database cursor to execute commands.
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


def populate_initial_data(cursor):
    """Populates the database with initial game data.

    This function inserts a small set of starting data into the database,
    such as main characters and essential items, to ensure the game world
    is not empty on first run.

    Args:
        cursor (sqlite3.Cursor): The database cursor to execute commands.
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


def init_db(db_file=DB_FILE):
    """Initializes the database, creating and populating tables.

    This function serves as the main entry point for setting up the database.
    It establishes a connection, creates the schema, populates it with
    initial data, and then closes the connection.

    Args:
        db_file (str, optional): The path to the database file.
            Defaults to DB_FILE.
    """
    conn = get_db_connection(db_file)
    cursor = conn.cursor()
    create_schema(cursor)
    populate_initial_data(cursor)
    conn.commit()
    conn.close()

_class_loader = None


def set_class_loader(loader):
    """Sets the class loader function used for deserialization.

    This function allows the database to dynamically instantiate game object
    classes without needing to import them directly, avoiding circular
    dependencies.

    Args:
        loader (callable): A function that takes a class name and returns
            the corresponding class object.
    """
    global _class_loader
    _class_loader = loader


def get_character_data(name, conn=None):
    """Fetches a character's data from the database by name.

    Args:
        name (str): The name of the character to retrieve.
        conn (sqlite3.Connection, optional): An existing database connection.
            If not provided, a new one will be created. Defaults to None.

    Returns:
        sqlite3.Row: A row object containing the character's data, or None
            if the character is not found.
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


def get_item_data(name, conn=None):
    """Fetches an item's base data from the Items table by name.

    Args:
        name (str): The name of the item to retrieve.
        conn (sqlite3.Connection, optional): An existing database connection.
            If not provided, a new one will be created. Defaults to None.

    Returns:
        sqlite3.Row: A row object containing the item's data, or None
            if the item is not found.
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


def get_weapon_data(item_id, conn=None):
    """Fetches a weapon's specific data from the Weapons table by item ID.

    Args:
        item_id (int): The ID of the weapon to retrieve.
        conn (sqlite3.Connection, optional): An existing database connection.
            If not provided, a new one will be created. Defaults to None.

    Returns:
        sqlite3.Row: A row object containing the weapon's data, or None
            if the weapon is not found.
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


def get_armor_data(item_id, conn=None):
    """Fetches armor's specific data from the Armor table by item ID.

    Args:
        item_id (int): The ID of the armor to retrieve.
        conn (sqlite3.Connection, optional): An existing database connection.
            If not provided, a new one will be created. Defaults to None.

    Returns:
        sqlite3.Row: A row object containing the armor's data, or None
            if the armor is not found.
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


def save_game(save_name, scene_manager):
    """Saves the current game state to the database.

    Note:
        This is a placeholder function. A full implementation would involve
        serializing the state of the `scene_manager` and storing it in the
        database.

    Args:
        save_name (str): The name of the save file.
        scene_manager (SceneManager): The scene manager object containing the
            current game state.
    """
    # In a real implementation, this would serialize the scene_manager
    # and store it in the database. For now, we'll just acknowledge it.
    print(f"Game state for '{save_name}' saved (simulation).")


def load_game(save_name):
    """Loads a game state from the database.

    Note:
        This is a placeholder for loading a game state. In a real
        implementation, this would load data from the database and
        reconstruct the game state. For now, it returns None to allow for
        testing of the main runner.

    Args:
        save_name (str): The name of the save file to load.

    Returns:
        None: This function currently returns None to simulate the behavior
            of a save not being found.
    """
    # This function is intended to be mocked in tests.
    # Returning None simulates the behavior of a save not being found.
    return None


if __name__ == '__main__':
    print("Initializing game content database...")
    init_db()
    print("Database initialized successfully.")