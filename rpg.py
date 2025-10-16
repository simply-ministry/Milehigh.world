import json
import math
import random
import database

class GameObject:
    """The base class for all objects in the game world."""
    def __init__(self, name="Object", symbol='?', x=0, y=0, z=0, health=100, defense=0):
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
        return f"{self.name}(x={self.x}, y={self.y}, health={self.health})"

    def distance_to(self, other):
        """Calculates the distance to another GameObject."""
        return math.sqrt((self.x - other.x)**2 + (self.y - other.y)**2)

    def move(self, dx, dy):
        """Moves the object by the specified amount."""
        self.x += dx
        self.y += dy

    def take_damage(self, damage):
        """Reduces the object's health after factoring in defense."""
        actual_damage = max(0, damage - self.defense)
        self.health -= actual_damage
        print(f"{self.name} takes {actual_damage} damage.")
        if self.health <= 0:
            self.health = 0
            self.die()

    def heal(self, amount):
        """Heals the object for a given amount."""
        self.health = min(self.max_health, self.health + amount)
        print(f"{self.name} heals for {amount} HP.")

    def die(self):
        """Handles the object's death."""
        print(f"{self.name} has been defeated.")
        # This object should be removed from the game by the game engine

    def update(self, scene_manager):
        """Updates the object's state each turn."""
        pass

class Item(GameObject):
    """Base class for all items."""
    def __init__(self, name, description, x=0, y=0):
        super().__init__(name, symbol='i', x=x, y=y)
        self.description = description

    def __str__(self):
        return f"{self.name}: {self.description}"

class Weapon(Item):
    """A weapon that can be equipped to increase damage."""
    def __init__(self, name, description, damage, weapon_type="Melee"):
        super().__init__(name, description)
        self.damage = damage
        self.weapon_type = weapon_type

    def __str__(self):
        return f"{self.name} (Weapon, {self.damage} DMG)"

class Armor(Item):
    """Armor that can be equipped to increase defense."""
    def __init__(self, name, description, defense):
        super().__init__(name, description)
        self.defense = defense

    def __str__(self):
        return f"{self.name} (Armor, +{self.defense} DEF)"

class Consumable(Item):
    """An item that can be used for a one-time effect."""
    def __init__(self, name, description, effect, value):
        super().__init__(name, description)
        self.effect = effect
        self.value = value

    def use(self, character):
        """Applies the consumable's effect to a character."""
        print(f"{character.name} uses {self.name}!")
        if self.effect == "heal":
            character.heal(self.value)

class Character(GameObject):
    """Base class for all characters."""
    def __init__(self, name, x=0, y=0, health=100, defense=5):
        super().__init__(name, symbol='C', x=x, y=y, health=health, defense=defense)
        self.inventory = []
        self.mana = 100
        self.max_mana = 100

    def attack(self, target, damage):
        """A generic attack method."""
        print(f"{self.name} attacks {target.name} for {damage} damage.")
        target.take_damage(damage)

class Player(Character):
    """The player character."""
    def __init__(self, name="Player", x=0, y=0):
        super().__init__(name, x, y, health=100, defense=5)
        self.symbol = '@'
        self.level = 1
        self.experience = 0
        self.strength = 10
        self.dexterity = 10
        self.intelligence = 10
        self.equipment = {"weapon": None, "armor": None}

    def attack(self, target):
        """Attacks a target, with damage modified by stats and equipment."""
        weapon_damage = self.equipment["weapon"].damage if self.equipment["weapon"] else 5
        total_damage = weapon_damage + self.strength // 2
        super().attack(target, total_damage)
        if target.health <= 0:
            if hasattr(target, 'xp_value'):
                self.gain_experience(target.xp_value)

    def equip(self, item):
        """Equips an item."""
        if isinstance(item, Weapon):
            self.equipment["weapon"] = item
            print(f"Equipped {item.name}.")
        elif isinstance(item, Armor):
            self.equipment["armor"] = item
            self.defense = item.defense
            print(f"Equipped {item.name}.")

    def pickup_item(self, item):
        """Picks up an item."""
        self.inventory.append(item)
        print(f"Picked up {item.name}.")

    def gain_experience(self, amount):
        """Gains experience and checks for level up."""
        self.experience += amount
        print(f"Gained {amount} experience.")
        required_xp = 100 * self.level
        if self.experience >= required_xp:
            self.level_up()

    def level_up(self):
        """Levels up the character."""
        self.level += 1
        self.max_health += 10
        self.health = self.max_health
        self.strength += 2
        self.dexterity += 2
        self.intelligence += 2
        print(f"Leveled up to level {self.level}!")

class Enemy(Character):
    """An enemy character."""
    def __init__(self, name, x=0, y=0, health=50, damage=10, xp_value=10, defense=0):
        super().__init__(name, x, y, health=health, defense=defense)
        self.symbol = 'E'
        self.attack_damage = damage
        self.xp_value = xp_value

    def attack(self, target):
        """The enemy's attack method."""
        super().attack(target, self.attack_damage)

    def update(self, scene_manager):
        """The enemy's AI logic."""
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
    """Holds all the data for a single game area."""
    def __init__(self, name, width=40, height=10):
        self.name = name
        self.width = width
        self.height = height
        self.game_objects = []
        self.player_character = None

    def add_object(self, obj):
        """Adds a game object to the scene."""
        self.game_objects.append(obj)

    def set_player(self, player):
        """Sets the player character for the scene."""
        self.player_character = player
        self.add_object(player)

    def get_object_at(self, x, y):
        """Gets the object at a given coordinate."""
        for obj in self.game_objects:
            if obj.x == x and obj.y == y:
                return obj
        return None

class Game:
    def __init__(self, width=40, height=10, db_file="game_content.db"):
    """Manages the game state and main loop."""
    def __init__(self, width=40, height=10):
        self.width = width
        self.height = height
        self.message_log = []
        self.game_over = False
        self.in_conversation = False
        self.dialogue_manager = None
        self.db_conn = database.get_db_connection(db_file)

    def log_message(self, message):
        """Adds a message to the game log."""
        self.message_log.append(message)
        if len(self.message_log) > 5:
            self.message_log.pop(0)

    def draw(self, scene):
        """Draws the game world."""
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
    """Controls scenes, events, and game logic."""
    def __init__(self, game):
        self.game = game
        self.scene = None

    def load_scene(self, scene):
        """Loads a new scene."""
        self.scene = scene
        self.setup_scene()

    def setup_scene(self):
        """Sets up the current scene."""
        raise NotImplementedError

    def run(self):
        """The main game loop."""
        while not self.game.game_over:
            self.game.draw(self.scene)
            self.handle_input()
            self.update()

    def handle_input(self):
        """Handles player input."""
        player = self.scene.player_character
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
        elif action == "quit":
            self.game.game_over = True
        else:
            self.game.log_message("Unknown command.")

    def update(self):
        """Runs every game loop, checking for win/loss conditions, etc."""
        pass

    def run(self):
        """Main game loop for this scene."""
        while not self.game.game_over and self.is_running:
            self.game.draw(self.scene)
            if self.game.game_over: break

            self.game.turn_taken = False
            while not self.game.turn_taken and not self.game.game_over:
                self.game.handle_input(self)

            # --- AI and World Turn ---
            if self.game.turn_taken and not self.game.game_over:
                # Update all other objects in the scene
                for obj in self.scene.game_objects:
                    if obj is not self.scene.player_character:
                        obj.update(self)

class Aeron(Player):
    """A placeholder class for the character Aeron."""
    def __init__(self, name="Aeron", x=0, y=0, z=0, db_conn=None):
        super().__init__(name, x, y, z)
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
    """A placeholder class for the enemy Kane."""
    def __init__(self, name="Kane", x=0, y=0, z=0, type="Boss", db_conn=None):
        super().__init__(name, x, y, z, type)
        self.symbol = 'K'
        data = database.get_character_data(name, conn=db_conn)
        if data:
            self.health = data['health']
            self.max_health = data['health']
            # Assuming attack_damage is derived from strength for now
            self.attack_damage = data['strength']
            self.xp_value = 500

class AethelgardBattle(SceneManager):
    """A specific scene manager for the Aeron vs. Kane fight."""
    def setup(self):
        """Sets up the characters, items, and quest for this specific battle."""
        # Create characters
        player = Aeron(name="Aeron", x=5, y=5, db_conn=self.game.db_conn)
        enemy = Kane(name="Kane", x=10, y=5, db_conn=self.game.db_conn)

        # Give player items
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


        # A simple quest system could be added to the Player class later
        # player.journal.add_quest(Quest("The Sibling Rivalry", "Defeat Kane.", [{'type': 'defeat', 'target': 'Kane', 'current': 0, 'required': 1}]))

        # Add a test interactable object
        ancient_statue = Interactable(
            name="Ancient Statue",
            x=5,
            y=4,
            symbol='S',
            description="The statue depicts a forgotten king. A faint inscription reads: 'Only the worthy may pass.'"
        )

        # Add them to the scene
        """Updates the scene."""
        for obj in self.scene.game_objects[:]:
            if hasattr(obj, 'health') and obj.health <= 0:
                self.scene.game_objects.remove(obj)
            else:
                obj.update(self)
        if self.scene.player_character.health <= 0:
            self.game.game_over = True
            self.game.log_message("You have been defeated.")

class AethelgardBattle(SceneManager):
    """A specific scene manager for the Aeron vs. Kane fight."""
    def setup_scene(self):
        """Sets up the characters and items for this specific battle."""
        self.scene = Scene("Aethelgard")
        player = Player(name="Aeron", x=5, y=5)
        enemy = Enemy(name="Kane", x=10, y=5, health=150, damage=20, xp_value=100, defense=2)
        self.scene.set_player(player)
        self.scene.add_object(enemy)
        self.game.log_message("Kane stands before you, his eyes burning with hatred.")

if __name__ == "__main__":
    game_engine = Game()
    scene_manager = AethelgardBattle(game_engine)
    scene_manager.setup_scene()
    scene_manager.run()
    print("Game over.")