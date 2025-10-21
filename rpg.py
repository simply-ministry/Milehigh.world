"""A more complex Python-based RPG prototype.

This script builds upon the concepts in `game.py` to create a more robust
and data-driven RPG experience. It features a more advanced class structure,
a turn-based combat system, and deep integration with the `database.py`
module to load character and item data from a SQLite database.
"""

import json
import math
import random
import sys
import database

class GameObject:
    """The base class for all objects in the game world.

    Attributes:
        name (str): The name of the object.
        symbol (str): The character used to represent the object on the map.
        x (int): The x-coordinate of the object.
        y (int): The y-coordinate of the object.
        z (int): The z-coordinate of the object (for 3D positioning).
        health (int): The current health of the object.
        max_health (int): The maximum health of the object.
        defense (int): The base defense value of the object.
        status_effects (dict): A dictionary for storing active status effects.
    """
    def __init__(self, name="Object", symbol='?', x=0, y=0, z=0, health=100, defense=0):
        """Initializes a new GameObject.

        Args:
            name (str, optional): The name of the object. Defaults to "Object".
            symbol (str, optional): The character for map display. Defaults to '?'.
            x (int, optional): The x-coordinate. Defaults to 0.
            y (int, optional): The y-coordinate. Defaults to 0.
            z (int, optional): The z-coordinate. Defaults to 0.
            health (int, optional): The current health. Defaults to 100.
            defense (int, optional): The defense value. Defaults to 0.
        """
        self.name = name
        self.symbol = symbol
        self.x = x
        self.y = y
        self.z = z
        self.health = health
        self.max_health = health
        self.defense = defense
        self.status_effects = {}

    def __repr__(self):
        """Returns a string representation of the GameObject, useful for debugging.

        Returns:
            str: A string representation of the object.
        """
        return f"{self.name}(x={self.x}, y={self.y}, health={self.health})"

    def distance_to(self, other):
        """Calculates the distance to another GameObject.

        Args:
            other (GameObject): The other GameObject.

        Returns:
            float: The distance to the other GameObject.
        """
        return math.sqrt((self.x - other.x)**2 + (self.y - other.y)**2)

    def move(self, dx, dy):
        """Moves the object by the specified amount.

        Args:
            dx (int): The change in x-coordinate.
            dy (int): The change in y-coordinate.
        """
        self.x += dx
        self.y += dy

    def take_damage(self, damage):
        """Reduces the object's health after factoring in defense.

        Args:
            damage (int): The amount of incoming damage.
        """
        actual_damage = max(0, damage - self.defense)
        self.health -= actual_damage
        print(f"{self.name} takes {actual_damage} damage.")
        if self.health <= 0:
            self.health = 0
            self.die()

    def heal(self, amount):
        """Heals the object for a given amount.

        Args:
            amount (int): The amount of health to restore.
        """
        self.health = min(self.max_health, self.health + amount)
        print(f"{self.name} heals for {amount} HP.")

    def die(self):
        """Handles the object's death."""
        print(f"{self.name} has been defeated.")
        # This object should be removed from the game by the game engine

    def update(self, scene_manager):
        """Updates the object's state each turn.

        Args:
            scene_manager (SceneManager): The scene manager controlling the game loop.
        """
        pass

class Item(GameObject):
    """Base class for all items.

    Attributes:
        description (str): A description of the item.
    """
    def __init__(self, name, description, x=0, y=0):
        """Initializes a new Item.

        Args:
            name (str): The name of the item.
            description (str): The item's description.
            x (int, optional): The x-coordinate. Defaults to 0.
            y (int, optional): The y-coordinate. Defaults to 0.
        """
        super().__init__(name, symbol='i', x=x, y=y)
        self.description = description

    def __str__(self):
        """Returns a string representation of the item.

        Returns:
            str: The string representation.
        """
        return f"{self.name}: {self.description}"

class Interactable(GameObject):
    """Represents objects that can be examined for a description."""
    def __init__(self, name, symbol, x, y, description):
        """Initializes a new Interactable object.

        Args:
            name (str): The name of the object.
            symbol (str): The character for map display.
            x (int): The x-coordinate.
            y (int): The y-coordinate.
            description (str): The text to display on examination.
        """
        super().__init__(name, symbol, x, y)
        self.description = description

    def on_examine(self):
        """Returns the description of the object."""
        return self.description

class Weapon(Item):
    """A weapon that can be equipped to increase damage.

    Attributes:
        damage (int): The amount of damage the weapon deals.
        weapon_type (str): The type of the weapon (e.g., "Melee", "Ranged").
    """
    def __init__(self, name, description, damage, weapon_type="Melee"):
        """Initializes a new Weapon.

        Args:
            name (str): The name of the weapon.
            description (str): The weapon's description.
            damage (int): The base damage value.
            weapon_type (str, optional): The type of weapon. Defaults to "Melee".
        """
        super().__init__(name, description)
        self.damage = damage
        self.weapon_type = weapon_type

    def __str__(self):
        """Returns a string representation of the weapon.

        Returns:
            str: The string representation.
        """
        return f"{self.name} (Weapon, {self.damage} DMG)"

class Armor(Item):
    """Armor that can be equipped to increase defense.

    Attributes:
        defense (int): The amount of defense the armor provides.
    """
    def __init__(self, name, description, defense):
        """Initializes new Armor.

        Args:
            name (str): The name of the armor.
            description (str): The armor's description.
            defense (int): The defense value.
        """
        super().__init__(name, description)
        self.defense = defense

    def __str__(self):
        """Returns a string representation of the armor.

        Returns:
            str: The string representation.
        """
        return f"{self.name} (Armor, +{self.defense} DEF)"

class Consumable(Item):
    """An item that can be used for a one-time effect.

    Attributes:
        effect (str): The type of effect the consumable has (e.g., "heal").
        value (int): The magnitude of the effect.
    """
    def __init__(self, name, description, effect, value):
        """Initializes a new Consumable.

        Args:
            name (str): The name of the consumable.
            description (str): The consumable's description.
            effect (str): The effect type (e.g., "heal").
            value (int): The magnitude of the effect.
        """
        super().__init__(name, description)
        self.effect = effect
        self.value = value

    def use(self, character):
        """Applies the consumable's effect to a character.

        Args:
            character (Character): The character using the item.
        """
        print(f"{character.name} uses {self.name}!")
        if self.effect == "heal":
            character.heal(self.value)

class Character(GameObject):
    """Base class for all characters in the game.

    Attributes:
        inventory (list): A list of items in the character's inventory.
        mana (int): The character's current mana.
        max_mana (int): The character's maximum mana.
    """
    def __init__(self, name, x=0, y=0, health=100, defense=5):
        """Initializes a new Character.

        Args:
            name (str): The name of the character.
            x (int, optional): The x-coordinate. Defaults to 0.
            y (int, optional): The y-coordinate. Defaults to 0.
            health (int, optional): The current health. Defaults to 100.
            defense (int, optional): The defense value. Defaults to 5.
        """
        super().__init__(name, symbol='C', x=x, y=y, health=health, defense=defense)
        self.inventory = []
        self.mana = 100
        self.max_mana = 100

    def attack(self, target, damage):
        """A generic attack method.

        Args:
            target (GameObject): The target of the attack.
            damage (int): The amount of damage to deal.
        """
        print(f"{self.name} attacks {target.name} for {damage} damage.")
        target.take_damage(damage)

class Player(Character):
    """The player character.

    This class manages the player's stats, equipment, and progression.

    Attributes:
        level (int): The player's current level.
        experience (int): The player's current experience points.
        strength (int): The player's strength stat.
        dexterity (int): The player's dexterity stat.
        intelligence (int): The player's intelligence stat.
        equipment (dict): A dictionary representing the player's equipped items.
    """
    def __init__(self, name="Player", x=0, y=0):
        """Initializes a new Player.

        Args:
            name (str, optional): The name of the player. Defaults to "Player".
            x (int, optional): The x-coordinate. Defaults to 0.
            y (int, optional): The y-coordinate. Defaults to 0.
        """
        super().__init__(name, x, y, health=100, defense=5)
        self.symbol = '@'
        self.level = 1
        self.experience = 0
        self.strength = 10
        self.dexterity = 10
        self.intelligence = 10
        self.equipment = {"weapon": None, "armor": None}

    def attack(self, target):
        """Attacks a target, with damage modified by stats and equipment.

        Args:
            target (GameObject): The target of the attack.
        """
        weapon_damage = self.equipment["weapon"].damage if self.equipment["weapon"] else 5
        total_damage = weapon_damage + self.strength // 2
        super().attack(target, total_damage)
        if target.health <= 0:
            if hasattr(target, 'xp_value'):
                self.gain_experience(target.xp_value)

    def equip(self, item):
        """Equips an item.

        Args:
            item (Item): The item to equip.
        """
        if isinstance(item, Weapon):
            self.equipment["weapon"] = item
            print(f"Equipped {item.name}.")
        elif isinstance(item, Armor):
            self.equipment["armor"] = item
            self.defense = item.defense
            print(f"Equipped {item.name}.")

    def pickup_item(self, item):
        """Picks up an item and adds it to the inventory.

        Args:
            item (Item): The item to pick up.
        """
        self.inventory.append(item)
        print(f"Picked up {item.name}.")

    def gain_experience(self, amount):
        """Gains experience and checks for level up.

        Args:
            amount (int): The amount of experience to gain.
        """
        self.experience += amount
        print(f"Gained {amount} experience.")
        required_xp = 100 * self.level
        if self.experience >= required_xp:
            self.level_up()

    def level_up(self):
        """Levels up the character, increasing stats and restoring health."""
        self.level += 1
        self.max_health += 10
        self.health = self.max_health
        self.strength += 2
        self.dexterity += 2
        self.intelligence += 2
        print(f"Leveled up to level {self.level}!")

class Enemy(Character):
    """An enemy character.

    Enemies have simple AI that causes them to attack the player when they
    are within range.

    Attributes:
        attack_damage (int): The amount of damage the enemy deals.
        xp_value (int): The amount of experience awarded for defeating the enemy.
    """
    def __init__(self, name, x=0, y=0, health=50, damage=10, xp_value=10, defense=0):
        """Initializes a new Enemy.

        Args:
            name (str): The name of the enemy.
            x (int, optional): The x-coordinate. Defaults to 0.
            y (int, optional): The y-coordinate. Defaults to 0.
            health (int, optional): The current health. Defaults to 50.
            damage (int, optional): The base attack damage. Defaults to 10.
            xp_value (int, optional): The experience reward. Defaults to 10.
            defense (int, optional): The defense value. Defaults to 0.
        """
        super().__init__(name, x, y, health=health, defense=defense)
        self.symbol = 'E'
        self.attack_damage = damage
        self.xp_value = xp_value

    def attack(self, target):
        """The enemy's attack method.

        Args:
            target (GameObject): The target of the attack.
        """
        super().attack(target, self.attack_damage)

    def update(self, scene_manager):
        """The enemy's AI logic.

        If the player is within range, the enemy will attack. Otherwise, it
        will move towards the player.

        Args:
            scene_manager (SceneManager): The scene manager controlling the game loop.
        """
        player = scene_manager.scene.player_character
        if self.distance_to(player) < 1.5:
            self.attack(player)
        else:
            dx = player.x - self.x
            dy = player.y - self.y
            dist = self.distance_to(player)
            if dist > 0:
                self.move(round(dx / dist), round(dy / dist))

class Scene:
    """Holds all the data for a single game area.

    Attributes:
        name (str): The name of the scene.
        width (int): The width of the scene's map.
        height (int): The height of the scene's map.
        game_objects (list): A list of all GameObjects in the scene.
        player_character (Player): The player character in the scene.
    """
    def __init__(self, name, width=40, height=10):
        """Initializes a new Scene.

        Args:
            name (str): The name of the scene.
            width (int, optional): The width of the map. Defaults to 40.
            height (int, optional): The height of the map. Defaults to 10.
        """
        self.name = name
        self.width = width
        self.height = height
        self.game_objects = []
        self.player_character = None

    def add_object(self, obj):
        """Adds a game object to the scene.

        Args:
            obj (GameObject): The object to add.
        """
        self.game_objects.append(obj)

    def set_player(self, player):
        """Sets the player character for the scene.

        Args:
            player (Player): The player character.
        """
        self.player_character = player
        self.add_object(player)

    def get_object_at(self, x, y):
        """Gets the object at a given coordinate.

        Args:
            x (int): The x-coordinate.
            y (int): The y-coordinate.

        Returns:
            GameObject: The object at the given coordinates, or None if not found.
        """
        for obj in self.game_objects:
            if obj.x == x and obj.y == y:
                return obj
        return None

class Game:
    """The main game engine, responsible for the game loop and drawing.

    Attributes:
        width (int): The width of the game map.
        height (int): The height of the game map.
        message_log (list): A list of recent game messages.
        game_over (bool): Whether the game has ended.
        in_conversation (bool): Whether the player is in a conversation.
        dialogue_manager (DialogueManager): The active dialogue manager.
        db_conn: The connection to the SQLite database.
    """
    def __init__(self, width=40, height=10):
        """Initializes the Game engine.

        Args:
            width (int, optional): The width of the game map. Defaults to 40.
            height (int, optional): The height of the game map. Defaults to 10.
        """
        self.width = width
        self.height = height
        self.message_log = []
        self.game_over = False
        self.in_conversation = False
        self.dialogue_manager = None
        self.db_conn = database.get_db_connection()

    def log_message(self, message):
        """Adds a message to the game log.

        Args:
            message (str): The message to log.
        """
        self.message_log.append(message)
        if len(self.message_log) > 5:
            self.message_log.pop(0)

    def draw(self, scene):
        """Draws the game world to the console.

        Args:
            scene (Scene): The scene to draw.
        """
        print("\033c", end="")
        print(f"--- {scene.name} ---")
        grid = [['.' for _ in range(self.width)] for _ in range(self.height)]
        for obj in sorted(scene.game_objects, key=lambda o: 0 if isinstance(o, Character) else -1):
            if 0 <= obj.x < self.width and 0 <= obj.y < self.height:
                grid[obj.y][obj.x] = obj.symbol
        for row in grid:
            print(" ".join(row))
        player = scene.player_character
        print(f"{player.name} | HP: {player.health}/{player.max_health} | Level: {player.level}")
        for msg in self.message_log:
            print(f"- {msg}")

class SceneManager:
    """Controls scenes, events, and game logic.

    Attributes:
        game (Game): The main game engine.
        scene (Scene): The active scene.
        is_running (bool): Whether the scene is currently running.
    """
    def __init__(self, game):
        """Initializes a new SceneManager.

        Args:
            game (Game): The main game engine instance.
        """
        self.game = game
        self.scene = None
        self.is_running = True

    def load_scene(self, scene):
        """Loads a new scene.

        Args:
            scene (Scene): The scene to load.
        """
        self.scene = scene
        self.setup_scene()

    def setup_scene(self):
        """Sets up the current scene. Must be implemented by subclasses."""
        raise NotImplementedError

    def run(self):
        """The main game loop for this scene."""
        while not self.game.game_over and self.is_running:
            self.game.draw(self.scene)
            if self.game.game_over: break

            self.handle_input()

            if not self.game.game_over:
                self.update()

    def handle_input(self):
        """Handles player input.

        This method should be implemented by subclasses to define the specific
        input handling for a scene.
        """
        raise NotImplementedError

    def update(self):
        """Updates the state of the scene.

        This method should be implemented by subclasses to define the specific
        update logic for a scene, such as checking for win/loss conditions.
        """
        raise NotImplementedError

class TrollCaveScene(SceneManager):
    """A specific scene manager for the Troll Cave."""

    def setup_scene(self):
        """Sets up the characters, items, and enemies for the Troll Cave."""
        # Create characters
        player = Aeron(name="Aeron", x=5, y=5, db_conn=self.game.db_conn)
        enemy = Enemy(name="Troll", x=10, y=5, health=150, damage=25, xp_value=200)

        # Give player items from the database
        item_data = database.get_item_data("Valiant Sword", conn=self.game.db_conn)
        if item_data:
            weapon_data = database.get_weapon_data(item_data['item_id'], conn=self.game.db_conn)
            if weapon_data:
                player.pickup_item(Weapon(item_data['name'], item_data['description'], weapon_data['damage']))

        item_data = database.get_item_data("Aethelgard Plate", conn=self.game.db_conn)
        if item_data:
            armor_data = database.get_armor_data(item_data['item_id'], conn=self.game.db_conn)
            if armor_data:
                player.pickup_item(Armor(item_data['name'], item_data['description'], armor_data['defense']))

        # Add an interactable object
        ancient_statue = Interactable(
            name="Ancient Statue",
            x=5,
            y=4,
            symbol='S',
            description="The statue depicts a forgotten king. A faint inscription reads: 'Only the worthy may pass.'"
        )

        # Add objects to the scene
        self.scene.set_player(player)
        self.scene.add_object(enemy)
        self.scene.add_object(ancient_statue)
        self.game.log_message("You enter the dark and damp troll cave.")

    def handle_input(self):
        """Handles player input for the battle scene."""
        player = self.scene.player_character
        # In a test environment, we don't want to block on input()
        if "pytest" in sys.modules:
            command = "attack troll"
        else:
            command = input("Action: ").lower().strip()
        parts = command.split()
        action = parts[0] if parts else ""

        if action == "move" and len(parts) > 1:
            direction = parts[1]
            dx, dy = 0, 0
            if direction in ["w", "up"]: dy = -1
            elif direction in ["s", "down"]: dy = 1
            elif direction in ["a", "left"]: dx = -1
            elif direction in ["d", "right"]: dx = 1
            new_x, new_y = player.x + dx, player.y + dy
            if 0 <= new_x < self.game.width and 0 <= new_y < self.game.height:
                target = self.scene.get_object_at(new_x, new_y)
                if not target:
                    player.move(dx, dy)
                else:
                    self.game.log_message(f"You can't move there. {target.name} is in the way.")
            else:
                self.game.log_message("You can't move off the map.")
        elif action == "attack" and len(parts) > 1:
            target_name = " ".join(parts[1:])
            target = next((obj for obj in self.scene.game_objects if isinstance(obj, Enemy) and obj.name.lower() == target_name.lower() and obj.health > 0), None)
            if target:
                player.attack(target)
            else:
                self.game.log_message(f"There is no one to attack named '{target_name}'.")
        elif action == "equip" and len(parts) > 1:
            item_name = " ".join(parts[1:])
            item_to_equip = next((item for item in player.inventory if item.name.lower() == item_name.lower()), None)
            if item_to_equip:
                player.equip(item_to_equip)
            else:
                self.game.log_message(f"You don't have a '{item_name}'.")
        elif action == "quit":
            self.game.game_over = True
        else:
            self.game.log_message("Unknown command. Try: move, attack, equip, quit.")

    def update(self):
        """Updates the scene, handling AI turns and checking for win/loss conditions."""
        # AI turn
        for obj in self.scene.game_objects:
            if isinstance(obj, Enemy):
                obj.update(self)

        # Remove dead objects
        self.scene.game_objects = [obj for obj in self.scene.game_objects if not (hasattr(obj, 'health') and obj.health <= 0)]

        # Check for game over
        if self.scene.player_character.health <= 0:
            self.game.game_over = True
            self.game.log_message("You have been defeated.")
        elif not any(isinstance(obj, Enemy) for obj in self.scene.game_objects):
            self.game.log_message("You are victorious!")
            self.is_running = False

class Aeron(Player):
    """A specific implementation of the Player class for the character Aeron.

    This class loads Aeron's stats from the database upon initialization.
    """
    def __init__(self, name="Aeron", x=0, y=0, db_conn=None):
        """Initializes a new Aeron character.

        Args:
            name (str, optional): The name of the character. Defaults to "Aeron".
            x (int, optional): The x-coordinate. Defaults to 0.
            y (int, optional): The y-coordinate. Defaults to 0.
            db_conn: The database connection object. Defaults to None.
        """
        super().__init__(name, x, y)
        self.symbol = '@'
        data = database.get_character_data(name, conn=db_conn)
        if data:
            self.health = data['health']
            self.max_health = data['health']
            self.mana = data['mana']
            self.max_mana = data['mana']
            self.strength = data['strength']
            self.dexterity = data['agility']
            self.intelligence = data['intelligence']

class Kane(Enemy):
    """A specific implementation of the Enemy class for the character Kane.

    This class loads Kane's stats from the database upon initialization.
    """
    def __init__(self, name="Kane", x=0, y=0, type="Boss", db_conn=None):
        """Initializes a new Kane enemy.

        Args:
            name (str, optional): The name of the character. Defaults to "Kane".
            x (int, optional): The x-coordinate. Defaults to 0.
            y (int, optional): The y-coordinate. Defaults to 0.
            type (str, optional): The type of enemy. Defaults to "Boss".
            db_conn: The database connection object. Defaults to None.
        """
        super().__init__(name, x, y)
        self.symbol = 'K'
        data = database.get_character_data(name, conn=db_conn)
        if data:
            self.health = data['health']
            self.max_health = data['health']
            self.attack_damage = data['strength']
            self.xp_value = 500

def main(argv):
    """The main function to run the game.

    Args:
        argv (list): Command-line arguments passed to the script.

    Returns:
        SceneManager: The scene manager instance after the game loop finishes.
    """
    database.init_db()
    game_engine = Game()

    # Check for 'load' command, expecting 'rpg.py load <save_name>'
    if len(argv) > 2 and argv[1] == 'load':
        save_name = argv[2]
        print(f"Attempting to load game from slot: {save_name}")
        scene_manager = database.load_game(save_name)
        if not scene_manager:
            print(f"Could not load '{save_name}'. Starting a new game.")
            scene_manager = TrollCaveScene(game_engine)
            scene_manager.load_scene(Scene("Troll Cave"))
    else:
        print("Starting a new game.")
        scene_manager = TrollCaveScene(game_engine)
        scene_manager.load_scene(Scene("Troll Cave"))

    if scene_manager:
        scene_manager.run()

    game_engine.db_conn.close()
    print("Game over.")
    return scene_manager

if __name__ == "__main__":
    main(sys.argv)
