import time
import random
import math

# --- Core Infrastructure Classes ---

class Item:
    """Base class for all items."""
    def __init__(self, name, description):
        self.name = name
        self.description = description
    def __str__(self):
        return f"{self.name}: {self.description}"

class Weapon(Item):
    """A type of item that can be equipped to deal damage."""
    def __init__(self, name, description, damage):
        super().__init__(name, description)
        self.damage = damage
    def __str__(self):
        return f"{self.name} (Weapon, +{self.damage} DMG): {self.description}"

class Consumable(Item):
    """A type of item that can be used once for an effect."""
    def __init__(self, name, description, effect, value):
        super().__init__(name, description)
        self.effect = effect
        self.value = value
    def __str__(self):
        return f"{self.name} (Consumable, {self.effect} +{self.value}): {self.description}"
    def use(self, character):
        print(f"{character.name} uses {self.name}!")
        if self.effect == "heal":
            character.heal(self.value)
        else:
            print(f"The {self.name} has no effect.")

class Inventory:
    """Manages a character's items."""
    def __init__(self, capacity=20):
        self.items = []
        self.capacity = capacity
    def add_item(self, item):
        if len(self.items) < self.capacity:
            self.items.append(item)
            print(f"'{item.name}' was added to the inventory.")
            return True
        print("Inventory is full!")
        return False
    def remove_item(self, item_name):
        item_to_remove = next((item for item in self.items if item.name.lower() == item_name.lower()), None)
        if item_to_remove:
            self.items.remove(item_to_remove)
            return item_to_remove
        return None
    def list_items(self):
        print("--- Inventory ---")
        if not self.items:
            print("  (Empty)")
        for item in self.items:
            print(f"- {item}")
        print("-----------------")

# --- Core Game Object and Character Classes ---

class GameObject:
    """Base class for all tangible objects in the game world."""
    def __init__(self, name="GameObject", x=0, y=0, health=100):
        self.name = name
        self.x = int(x)
        self.y = int(y)
        self.health = health
        self.max_health = health
    def __str__(self):
        return f"{self.name} (HP: {self.health}/{self.max_health})"
    def move(self, dx, dy):
        self.x += dx
        self.y += dy
    def take_damage(self, damage):
        self.health -= damage
        if self.health < 0: self.health = 0
        print(f"{self.name} takes {damage} damage.")
        if self.health == 0:
            self.die()
    def heal(self, amount):
        self.health = min(self.max_health, self.health + amount)
        print(f"{self.name} heals for {amount} HP.")
    def die(self):
        print(f"{self.name} has been defeated.")
    def update(self):
        pass

class Character(GameObject):
    """Base class for all characters, player and NPCs."""
    def __init__(self, name="Character", x=0, y=0, health=100, state=None):
        super().__init__(name, x, y, health)
        self.inventory = Inventory()
        self.state = state
        self.is_asleep = False
        self.sleep_duration = 0
    def pickup_item(self, item):
        return self.inventory.add_item(item)
    def use_item(self, item_name):
        item = next((i for i in self.inventory.items if i.name.lower() == item_name.lower()), None)
        if item and isinstance(item, Consumable):
            item.use(self)
            self.inventory.remove_item(item.name)
        elif item:
            print(f"You cannot use '{item.name}'.")
        else:
            print(f"You do not have '{item_name}'.")
    def talk(self, target):
        if hasattr(target, 'dialogue'):
            print(f"[{target.name}]: {target.dialogue}")
        else:
            print(f"{target.name} has nothing to say.")
    def update(self):
        if self.is_asleep:
            self.sleep_duration -= 1
            print(f"{self.name} is asleep.")
            if self.sleep_duration <= 0:
                self.is_asleep = False
                print(f"{self.name} woke up.")

class Player(Character):
    """Represents the player character."""
    pass

class Enemy(Character):
    """Represents an enemy character."""
    pass

class NPC(Character):
    """A non-player character that can have dialogue."""
    def __init__(self, name, x, y, dialogue="Hello there."):
        super().__init__(name, x, y, health=50)
        self.dialogue = dialogue

# --- Specific, Detailed Character Classes ---

class Skyix(Player):
    pass
class Anastasia(Player):
    pass
class Micah(Player):
    pass
class Zaia(Player):
    pass
class DelilahTheDesolate(Enemy):
    def __init__(self, name="Delilah the Desolate", x=0, y=0):
        super().__init__(name, x, y, health=200)
        self.dialogue = "You cannot stop the inevitable."
class Nyxar(Enemy):
    pass
class Reverie(Player):
    pass
class Aeron(NPC): pass
class Cirrus(Player): pass
class Ingris(Player): pass
class Otis(Player): pass
class Kai(Player): pass
class Kane(Enemy): pass

# --- Quest System ---
class Quest:
    def __init__(self, name, description, objectives, rewards):
        self.name = name
        self.description = description
        self.objectives = objectives
        self.rewards = rewards
        self.is_complete = False
    def check_completion(self, player):
        if 'find_sword' in self.objectives:
            for item in player.inventory.items:
                if item.name == "Ancient Sword":
                    self.is_complete = True
                    return True
        return False
    def grant_rewards(self, player):
        print(f"Quest '{self.name}' complete! You earned {self.rewards}!")

# --- Game Engine ---
class Game:
    """Manages the game state, objects, and the main interactive game loop."""
    def __init__(self, width=80, height=20):
        self.game_objects = []
        self.player_character = None
        self.is_running = True
        self.width = width
        self.height = height
        self.message_log = []
        self.event_flags = {}

    def add_object(self, obj):
        self.game_objects.append(obj)

    def set_player_character(self, character):
        self.player_character = character
        if character not in self.game_objects:
            self.add_object(character)

    def get_object_at(self, x, y):
        for obj in reversed(self.game_objects):
            if obj.x == x and obj.y == y:
                return obj
        return None

    def trigger_event(self, event_name):
        self.event_flags[event_name] = True
        self.message_log.append(f"Event triggered: {event_name}")

    def event_triggered(self, event_name):
        return self.event_flags.get(event_name, False)

    def draw(self):
        print("\033c", end="") # Clear console
        grid = [['.' for _ in range(self.width)] for _ in range(self.height)]
        for obj in sorted(self.game_objects, key=lambda o: isinstance(o, Character)):
            if 0 <= obj.x < self.width and 0 <= obj.y < self.height:
                grid[int(obj.y)][int(obj.x)] = obj.name[0]

        print("=" * (self.width + 2))
        for row in grid:
            print("=" + "".join(row) + "=")
        print("=" * (self.width + 2))

        if self.player_character:
            print(f"--- {self.player_character} ---")
        for message in self.message_log[-5:]:
            print(f"> {message}")
        self.message_log.clear()

    def handle_input(self):
        if self.player_character.is_asleep:
            return
        command = input("Action (move w/a/s/d, look, talk [target], get, inv, use [item], quit): ").lower().strip()
        parts = command.split()
        action = parts[0] if parts else ""

        if action == "quit":
            self.is_running = False
        elif action == "move" and len(parts) > 1:
            direction = parts[1]
            dx, dy = 0, 0
            if direction == 'w': dy = -1
            elif direction == 's': dy = 1
            elif direction == 'a': dx = -1
            elif direction == 'd': dx = 1
            target_x, target_y = self.player_character.x + dx, self.player_character.y + dy
            if not self.get_object_at(target_x, target_y):
                self.player_character.move(dx, dy)
            else:
                self.message_log.append("Something is in your way.")
        elif action == "look":
            for obj in self.game_objects:
                if obj != self.player_character:
                     self.message_log.append(f"You see {obj.name} at ({obj.x}, {obj.y}).")
        elif action == "get":
            item_found = None
            for obj in self.game_objects:
                if obj != self.player_character and obj.x == self.player_character.x and obj.y == self.player_character.y and isinstance(obj, Item):
                    item_found = obj
                    break
            if item_found:
                if self.player_character.inventory.add_item(item_found):
                    self.game_objects.remove(item_found)
            else:
                self.message_log.append("There is nothing to get here.")
        elif action == "inv":
            self.player_character.inventory.list_items()
            input("Press Enter to continue...")
        elif action == "use" and len(parts) > 1:
            item_name = " ".join(parts[1:])
            self.player_character.use_item(item_name)
        elif action == "talk" and len(parts) > 1:
            target_name = " ".join(parts[1:])
            target = next((obj for obj in self.game_objects if obj.name.lower() == target_name.lower()), None)
            if target:
                self.player_character.talk(target)
            else:
                self.message_log.append(f"You can't find anyone named '{target_name}'.")
        else:
            self.message_log.append("Invalid command.")

    def update(self):
        for obj in self.game_objects[:]:
            obj.update()
        # Example of a scripted event
        if self.player_character.x > 15 and not self.event_triggered("delilah_battle"):
            self.message_log.append("Suddenly, the air grows cold. Delilah the Desolate appears!")
            delilah = DelilahTheDesolate(x=self.player_character.x + 2, y=self.player_character.y)
            self.add_object(delilah)
            self.trigger_event("delilah_battle")

    def start(self):
        while self.is_running:
            self.draw()
            self.handle_input()
            if not self.is_running: break
            self.update()
        print("\nThank you for playing Milehigh.World!")


if __name__ == "__main__":
    # --- Comprehensive Game Demonstration ---
    game = Game()

    # Create player, NPC, and enemy characters
    player_zaia = Zaia(name="Zaia", x=5, y=5)
    npc_aeron = Aeron(name="Aeron", x=10, y=8, dialogue="Greetings, traveler. The world is in peril.")
    enemy_kane = Kane(name="Kane", x=20, y=12, health=80)
    enemy_kane.dialogue = "You cannot defeat me." # Enemies can have dialogue too

    # Add characters to the game
    game.set_player_character(player_zaia)
    game.add_object(npc_aeron)
    game.add_object(enemy_kane)

    # Add items to the game world for the player to find
    health_potion_item = Consumable("Health Potion", "Restores 50 health.", "heal", 50)
    health_potion_item.x = 2
    health_potion_item.y = 2
    game.add_object(health_potion_item)

    quest_sword = Item("Ancient Sword", "A sword of ancient power.")
    quest_sword.x = 25
    quest_sword.y = 15
    game.add_object(quest_sword)

    # Create and add a quest
    main_quest = Quest(
        name="The Ancient Sword",
        description="Find the Ancient Sword to defeat the encroaching darkness.",
        objectives={'find_sword': 1},
        rewards={'experience': 100, 'gold': 50}
    )
    # In a real game, you'd have a quest log to manage this
    print(f"New Quest: {main_quest.name} - {main_quest.description}")


    # Start the game
    print("--- Welcome to the Milehigh.World RPG Demo ---")
    print("Use commands like 'move d', 'look', 'talk aeron', 'get', 'inv', 'use health potion', or 'quit'.")
    game.start()