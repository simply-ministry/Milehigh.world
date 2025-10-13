import sqlite3
import json

DB_FILE = "rpg.db"

def get_db_connection(db_file=DB_FILE):
    """Establishes a connection to the database."""
    conn = sqlite3.connect(db_file)
    conn.row_factory = sqlite3.Row
    return conn

def init_db(db_file=DB_FILE):
    """Initializes the database and creates tables if they don't exist."""
    conn = get_db_connection(db_file)
    cursor = conn.cursor()

    # Table to store high-level information about each save file
    cursor.execute("""
    CREATE TABLE IF NOT EXISTS SaveGames (
        save_name TEXT PRIMARY KEY,
        scene_manager_class TEXT NOT NULL,
        scene_name TEXT NOT NULL,
        player_character_name TEXT NOT NULL,
        game_state_json TEXT NOT NULL
    )
    """)

    # Table to store all characters (player, NPCs, enemies) for each save
    cursor.execute("""
    CREATE TABLE IF NOT EXISTS Characters (
        character_id INTEGER PRIMARY KEY AUTOINCREMENT,
        save_name TEXT NOT NULL,
        class_name TEXT NOT NULL,
        name TEXT NOT NULL,
        symbol TEXT,
        x INTEGER NOT NULL,
        y INTEGER NOT NULL,
        health INTEGER NOT NULL,
        max_health INTEGER,
        state TEXT,
        -- Player-specific fields
        level INTEGER,
        experience INTEGER,
        mana REAL,
        max_mana REAL,
        strength INTEGER,
        dexterity INTEGER,
        intelligence INTEGER,
        -- Enemy-specific fields
        attack_damage INTEGER,
        xp_value INTEGER,
        -- Dialogue
        dialogue_json TEXT,
        UNIQUE(save_name, name)
    )
    """)

    # Table to store all items for each save (both in world and in inventory)
    # This simplifies the schema by combining Items, Weapons, Armor, etc.
    cursor.execute("""
    CREATE TABLE IF NOT EXISTS Items (
        item_id INTEGER PRIMARY KEY AUTOINCREMENT,
        save_name TEXT NOT NULL,
        class_name TEXT NOT NULL,
        name TEXT NOT NULL,
        description TEXT,
        -- Weapon fields
        damage INTEGER,
        weapon_type TEXT,
        -- Armor fields
        defense INTEGER,
        -- Consumable fields
        effect TEXT,
        quantity INTEGER,
        -- For items dropped in the world
        x INTEGER,
        y INTEGER,
        in_inventory_of TEXT -- Name of the character who has this item
    )
    """)

    conn.commit()
    conn.close()

# --- Helper functions to get class instances from strings ---
# This is a bit of a hack for this single-file structure. In a real project,
# you'd use a more robust factory or registration system.
def get_class(class_name):
    # This function will be populated by rpg.py at runtime
    pass

def set_class_loader(loader_func):
    """Allows rpg.py to inject its own class loader."""
    global get_class
    get_class = loader_func


def save_game(save_name, scene_manager, db_file=DB_FILE):
    """Saves the current game state to the database."""
    conn = get_db_connection(db_file)
    cursor = conn.cursor()

    # Clear any previous data for this save name
    cursor.execute("DELETE FROM SaveGames WHERE save_name = ?", (save_name,))
    cursor.execute("DELETE FROM Characters WHERE save_name = ?", (save_name,))
    cursor.execute("DELETE FROM Items WHERE save_name = ?", (save_name,))

    # --- Save High-Level Game and Scene State ---
    game_state_dict = {
        "message_log": scene_manager.game.message_log,
        "game_over": scene_manager.game.game_over,
    }
    cursor.execute(
        """
        INSERT INTO SaveGames (save_name, scene_manager_class, scene_name, player_character_name, game_state_json)
        VALUES (?, ?, ?, ?, ?)
        """,
        (
            save_name,
            scene_manager.__class__.__name__,
            scene_manager.scene.name,
            scene_manager.scene.player_character.name,
            json.dumps(game_state_dict)
        )
    )

    # --- Save Characters and Items ---
    for obj in scene_manager.scene.game_objects:
        mro_names = [base.__name__ for base in obj.__class__.__mro__]
        # Check if the object is a character-type (Player or Enemy)
        if "Player" in mro_names or "Enemy" in mro_names:
            dialogue_json = json.dumps(obj.dialogue.to_dict()) if hasattr(obj, 'dialogue') and obj.dialogue else None
            cursor.execute(
                """
                INSERT INTO Characters (save_name, class_name, name, symbol, x, y, health, max_health, state,
                                       level, experience, mana, max_mana, strength, dexterity, intelligence,
                                       attack_damage, xp_value, dialogue_json)
                VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
                """,
                (
                    save_name, obj.__class__.__name__, obj.name, obj.symbol, obj.x, obj.y, obj.health,
                    getattr(obj, 'max_health', None), getattr(obj, 'state', None), getattr(obj, 'level', None),
                    getattr(obj, 'experience', None), getattr(obj, 'mana', None), getattr(obj, 'max_mana', None),
                    getattr(obj, 'strength', None), getattr(obj, 'dexterity', None), getattr(obj, 'intelligence', None),
                    getattr(obj, 'attack_damage', None), getattr(obj, 'xp_value', None), dialogue_json
                )
            )
            # Save inventory
            if hasattr(obj, 'inventory'):
                for item in obj.inventory:
                    save_item(cursor, save_name, item, owner_name=obj.name)
            # Save equipment
            if hasattr(obj, 'equipment'):
                for slot, item in obj.equipment.slots.items():
                    if item:
                        # We add a special marker to distinguish equipped items
                        save_item(cursor, save_name, item, owner_name=f"EQUIPPED:{obj.name}:{slot}")

        # Check if the object is an item-type on the ground (not in an inventory)
        elif "Item" in mro_names:
            save_item(cursor, save_name, obj)

    conn.commit()
    conn.close()


def save_item(cursor, save_name, item, owner_name=None):
    """Helper function to save an item to the database."""
    cursor.execute(
        """
        INSERT INTO Items (save_name, class_name, name, description, damage, weapon_type,
                           defense, effect, quantity, x, y, in_inventory_of)
        VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
        """,
        (
            save_name, item.__class__.__name__, item.name, item.description,
            getattr(item, 'damage', None), getattr(item, 'weapon_type', None),
            getattr(item, 'defense', None), getattr(item, 'effect', None),
            getattr(item, 'quantity', None), item.x, item.y, owner_name
        )
    )

def load_game(save_name, db_file=DB_FILE):
    """Loads a game state from the database and returns a new scene_manager."""
    conn = get_db_connection(db_file)
    cursor = conn.cursor()

    # --- Load High-Level Game State ---
    cursor.execute("SELECT * FROM SaveGames WHERE save_name = ?", (save_name,))
    save_data = cursor.fetchone()
    if not save_data:
        print(f"Save file '{save_name}' not found.")
        return None

    # Recreate Game object
    game_state_dict = json.loads(save_data["game_state_json"])
    Game = get_class("Game")
    game = Game() # We need a way to get the class definition
    game.message_log = game_state_dict.get("message_log", [])
    game.game_over = game_state_dict.get("game_over", False)

    # Recreate Scene object
    Scene = get_class("Scene")
    scene = Scene(save_data["scene_name"])

    # --- Load All Characters for the Save ---
    characters = {}
    cursor.execute("SELECT * FROM Characters WHERE save_name = ?", (save_name,))
    for row in cursor.fetchall():
        CharacterClass = get_class(row["class_name"])
        char = CharacterClass(name=row["name"], x=row["x"], y=row["y"])
        # Populate all fields from DB
        for key in row.keys():
            if row[key] is not None and hasattr(char, key):
                setattr(char, key, row[key])

        # Handle dialogue deserialization
        if row["dialogue_json"]:
            DialogueManager = get_class("DialogueManager")
            dialogue_data = json.loads(row["dialogue_json"])
            char.dialogue = DialogueManager.from_dict(dialogue_data)

        scene.add_object(char)
        characters[char.name] = char

    # Set the player character on the scene
    scene.player_character = characters.get(save_data["player_character_name"])

    # --- Load All Items for the Save ---
    cursor.execute("SELECT * FROM Items WHERE save_name = ?", (save_name,))
    for row in cursor.fetchall():
        ItemClass = get_class(row["class_name"])

        # --- NEW: Handle different item constructors ---
        class_name = row["class_name"]
        if class_name == "Weapon":
            item = ItemClass(name=row["name"], description=row["description"], damage=row["damage"])
        elif class_name == "Armor":
            item = ItemClass(name=row["name"], description=row["description"], defense=row["defense"])
        elif class_name == "HealthPotion":
            # Assuming a default amount if not specified, or fetch from DB if stored
            item = ItemClass(name=row["name"], description=row["description"])
        else: # Fallback for simple items
            item = ItemClass(name=row["name"], description=row["description"])

        # Populate all fields from DB (this will handle quantity, etc.)
        for key in row.keys():
            if row[key] is not None and hasattr(item, key):
                setattr(item, key, row[key])

        owner_info = row["in_inventory_of"]
        if owner_info:
            if owner_info.startswith("EQUIPPED:"):
                _, owner_name, slot = owner_info.split(":", 2)
                if owner_name in characters:
                    characters[owner_name].equipment.slots[slot] = item
            elif owner_info in characters:
                 characters[owner_info].inventory.append(item)
        else:
            # Item is on the ground
            scene.add_object(item)


    # Recreate the SceneManager
    SceneManagerClass = get_class(save_data["scene_manager_class"])
    # Pass `setup_scene=False` to prevent it from overwriting our loaded data
    manager = SceneManagerClass(scene, game, setup_scene=False)

    conn.close()
    return manager


if __name__ == '__main__':
    print("Initializing database...")
    init_db()
    print("Database initialized successfully.")