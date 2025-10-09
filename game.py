import time
import random
import math

# --- Base Classes ---

class GameObject:
    """
    Base class for all tangible objects in the game world.
    Provides fundamental attributes like name and position.
    """
    def __init__(self, name="GameObject", x=0, y=0, health=100):
        self.name = name
        self.x = int(x)
        self.y = int(y)
        self.health = health
        self.max_health = health

    def __str__(self):
        return f"{self.name} (HP: {self.health}/{self.max_health})"

    def move(self, dx, dy):
        """Moves the object by the specified amount."""
        self.x += dx
        self.y += dy

    def take_damage(self, damage):
        """Reduces the object's health."""
        self.health -= damage
        print(f"{self.name} takes {damage} damage.")
        if self.health <= 0:
            self.health = 0
            self.die()

    def heal(self, amount):
        """Increases the object's health."""
        self.health = min(self.max_health, self.health + amount)
        print(f"{self.name} heals for {amount} HP.")

    def die(self):
        """Handles the object's death."""
        print(f"{self.name} has been defeated.")

    def update(self):
        """Placeholder for game-tick updates. To be overridden."""
        pass


class Item:
    """Base class for all items in the game."""
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
        """Applies the consumable's effect to a character."""
        print(f"{character.name} uses {self.name}!")
        if self.effect == "heal":
            character.heal(self.value)
        elif self.effect == "restore_dream_energy":
            if hasattr(character, 'dream_energy'):
                character.dream_energy = min(character.max_dream_energy, character.dream_energy + self.value)
                print(f"{character.name} restores {self.value} Dream Energy.")
            else:
                print(f"But {character.name} has no Dream Energy to restore.")
        elif self.effect == "restore_mana":
            if hasattr(character, 'mana'):
                character.mana = min(character.max_mana, character.mana + self.value)
                print(f"{character.name} restores {self.value} Mana.")
            else:
                print(f"But {character.name} has no Mana to restore.")
        else:
            print(f"The {self.name} has no effect.")


class Inventory:
    """Manages a character's items."""
    def __init__(self, capacity=20):
        self.items = []
        self.capacity = capacity
        self.owner_name = "Unknown"

    def add_item(self, item):
        if len(self.items) < self.capacity:
            self.items.append(item)
            print(f"'{item.name}' was added to {self.owner_name}'s inventory.")
            return True
        else:
            print("Inventory is full!")
            return False

    def remove_item(self, item_name):
        for item in self.items:
            if item.name.lower() == item_name.lower():
                self.items.remove(item)
                return item # Return the removed item
        return None

    def list_items(self):
        if not self.items:
            print(f"{self.owner_name}'s inventory is empty.")
            return
        print(f"--- {self.owner_name}'s Inventory ---")
        for item in self.items:
            print(f"- {item}")
        print("-----------------")

# --- Character Classes ---

class Character(GameObject):
    """Base class for all characters, including player and NPCs."""
    def __init__(self, name="Character", x=0, y=0, health=100, state=None):
        super().__init__(name, x, y, health=health)
        self.state = state
        self.inventory = Inventory()
        self.inventory.owner_name = self.name
        self.mana = 100
        self.max_mana = 100
        self.is_asleep = False
        self.sleep_duration = 0

    def pickup_item(self, item):
        self.inventory.add_item(item)

    def use_item(self, item_name):
        """Finds an item by name in inventory and uses it."""
        for item in self.inventory.items:
            if item.name.lower() == item_name.lower():
                if isinstance(item, Consumable):
                    item.use(self)
                    self.inventory.remove_item(item_name) # Remove after use
                    return
                else:
                    print(f"'{item.name}' cannot be used.")
                    return
        print(f"'{item_name}' not found in inventory.")

    def talk(self, target):
        """Initiates dialogue with another character."""
        if hasattr(target, 'dialogue'):
            print(f"[{target.name}]: {target.dialogue}")
        else:
            print(f"{target.name} has nothing to say.")

    def cast_spell(self, spell_name, target):
        """Casts a spell at a target."""
        # This is a generic spellbook. Specific characters will override this.
        spell_name = spell_name.lower()
        if spell_name == "dream weave" and self.mana >= 20:
            self.mana -= 20
            print(f"{self.name} weaves a dream, putting {target.name} to sleep!")
            target.is_asleep = True
            target.sleep_duration = 3 # Sleep for 3 game ticks/turns
        elif spell_name == "nightmare" and self.mana >= 35:
            self.mana -= 35
            print(f"{self.name} conjures a nightmare, damaging {target.name}'s mind!")
            target.take_damage(50) # Deals psychic damage
        elif spell_name == "soothing slumber" and self.mana >= 25:
            self.mana -= 25
            print(f"{self.name} casts a soothing slumber, healing {target.name}!")
            target.heal(40)
        else:
            print(f"{self.name} doesn't know the spell '{spell_name}' or lacks the mana.")

    def update(self):
        """Update character state each turn."""
        if self.is_asleep:
            self.sleep_duration -= 1
            print(f"{self.name} is asleep.")
            if self.sleep_duration <= 0:
                self.is_asleep = False
                print(f"{self.name} woke up.")


class NPC(Character):
    """A non-player character that can have dialogue."""
    def __init__(self, name, x, y, dialogue="Hello there."):
        super().__init__(name, x, y, health=50) # NPCs are generally weaker
        self.dialogue = dialogue

# --- Specific Character Implementations ---

class Skyix(Character):
    pass

class Anastasia(Character):
    def __init__(self, name="Anastasia", x=0, y=0):
        super().__init__(name, x, y, health=90)
        self.dream_energy = 100
        self.max_dream_energy = 100
    def __str__(self):
        # Override string representation to include unique resource
        return f"{self.name} (HP: {self.health}/{self.max_health}, Dream Energy: {self.dream_energy}/{self.max_dream_energy})"

class Reverie(Character):
    def __init__(self, name="Reverie", x=0, y=0):
        super().__init__(name, x, y, health=110)
        self.mana = 150
        self.max_mana = 150
        self.enigma = 0
        self.max_enigma = 100

class Aeron(NPC): pass
class Zaia(Character): pass
class Micah(Character): pass
class Cirrus(Character): pass
class Ingris(Character): pass
class Otis(Character): pass
class Kai(Character): pass
class Kane(Character): pass


class Delilah(Character):
    def __init__(self, name="Delilah the Desolate", x=0, y=0):
        super().__init__(name, x, y, health=200)
        self.dialogue = "You cannot stop the inevitable."


# --- Quest System ---

class Quest:
    def __init__(self, name, description, objectives, rewards):
        self.name = name
        self.description = description
        self.objectives = objectives  # e.g., {'kill_goblins': 5, 'find_sword': 1}
        self.rewards = rewards        # e.g., {'experience': 100, 'gold': 50}
        self.is_complete = False

    def check_completion(self, player, game_events):
        """Checks if quest objectives are met."""
        # This is complex and depends on game state.
        if 'find_sword' in self.objectives:
            for item in player.inventory.items:
                if item.name == "Ancient Sword":
                    self.is_complete = True
                    return True
        return False

    def grant_rewards(self, player):
        # In a real game, the player would have an experience attribute.
        print(f"Quest '{self.name}' complete! You earned {self.rewards}!")

# --- Game Engine ---

class Game:
    """
    Manages the game state, objects, and the main game loop for a text-based RPG.
    """
    def __init__(self, width=80, height=24):
        self.game_objects = []
        self.player_character = None
        self.is_running = True
        self.width = width
        self.height = height
        self.message_log = []
        self.event_flags = {} # For tracking scripted events like "delilah_battle"

    def add_object(self, obj):
        """Adds a game object to the world."""
        self.game_objects.append(obj)

    def set_player_character(self, character):
        """Sets the character the player will control."""
        self.player_character = character
        if character not in self.game_objects:
            self.add_object(character)

    def get_object_at(self, x, y):
        """Gets the top-most object at a specific coordinate."""
        for obj in reversed(self.game_objects):
            if obj.x == x and obj.y == y:
                return obj
        return None

    def trigger_event(self, event_name):
        """Sets an event flag to true."""
        self.event_flags[event_name] = True
        self.message_log.append(f"Event triggered: {event_name}")

    def event_triggered(self, event_name):
        """Checks if an event has been triggered."""
        return self.event_flags.get(event_name, False)

    def draw(self):
        """Draws the game world, characters, and UI to the console."""
        print("\033c", end="") # Clear console
        grid = [['.' for _ in range(self.width)] for _ in range(self.height)]

        for obj in sorted(self.game_objects, key=lambda o: isinstance(o, Character)):
            if 0 <= obj.x < self.width and 0 <= obj.y < self.height:
                grid[int(obj.y)][int(obj.x)] = obj.name[0]

        print("=" * (self.width + 2))
        for row in grid:
            print("=" + "".join(row) + "=")
        print("=" * (self.width + 2))

        print(f"--- {self.player_character} ---")
        for message in self.message_log[-5:]:
            print(f"> {message}")
        self.message_log.clear()

    def handle_input(self):
        """Handles player input from the command line."""
        if self.player_character.is_asleep:
            return

        command = input("Action (move w/a/s/d, look, talk [target], get, inv, use [item], cast [spell] on [target], quit): ").lower().strip()
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
        elif action == "cast" and len(parts) > 3 and parts[2] == "on":
            spell_name = parts[1]
            target_name = " ".join(parts[3:])
            target = next((obj for obj in self.game_objects if obj.name.lower() == target_name.lower()), None)
            if target:
                self.player_character.cast_spell(spell_name, target)
            else:
                self.message_log.append(f"You can't find anyone named '{target_name}'.")
        else:
            self.message_log.append("Invalid command.")

    def update(self):
        """Update all game objects."""
        for obj in self.game_objects[:]: # Iterate on a copy
            obj.update()

        if self.player_character.x > 15 and not self.event_triggered("delilah_battle"):
            self.message_log.append("Suddenly, the air grows cold. Delilah the Desolate appears!")
            delilah = Delilah(x=self.player_character.x + 2, y=self.player_character.y)
            self.add_object(delilah)
            self.trigger_event("delilah_battle")

    def start(self):
        """Starts and runs the main game loop."""
        while self.is_running:
            self.draw()
            self.handle_input()
            if not self.is_running: break
            self.update()
        print("\nThank you for playing Milehigh.World!")


# --- Main Execution Block ---

if __name__ == "__main__":
    game = Game()

    player_zaia = Zaia(name="Zaia", x=5, y=5)
    npc_aeron = Aeron(name="Aeron", x=10, y=8, dialogue="Greetings, traveler. The world is in peril.")
    enemy_kane = Kane(name="Kane", x=20, y=12, health=80)
    enemy_kane.dialogue = "You cannot defeat me."

    game.set_player_character(player_zaia)
    game.add_object(npc_aeron)
    game.add_object(enemy_kane)

    health_potion_item = Consumable("Health Potion", "Restores 50 health.", "heal", 50)
    health_potion_item.x = 2
    health_potion_item.y = 2
    game.add_object(health_potion_item)

    quest_sword = Item("Ancient Sword", "A sword of ancient power.")
    quest_sword.x = 25
    quest_sword.y = 15
    game.add_object(quest_sword)

    print("Game world initialized. Type 'quit' to exit.")
    game.start()