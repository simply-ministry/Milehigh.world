import time
import random
import math

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
        self.effect = effect  # e.g., "heal", "restore_mana"
        self.value = value

    def __str__(self):
        return f"{self.name} (Consumable, {self.effect} +{self.value}): {self.description}"

    def use(self, character):
        """Applies the consumable's effect to a character."""
        print(f"{character.name} uses {self.name}!")
        if self.effect == "heal":
            character.heal(self.value)
        elif self.effect == "restore_mana":
            if hasattr(character, 'mana'):
                character.mana += self.value
                if character.mana > character.max_mana:
                    character.mana = character.max_mana
                print(f"{character.name} restored {self.value} mana.")
            else:
                print(f"{character.name} does not have mana.")
        else:
            print(f"The {self.name} has no effect.")

class Inventory:
    """Manages a character's items."""
    def __init__(self, capacity=20):
        self.items = []
        self.capacity = capacity

    def add_item(self, item):
        """Adds an item to the inventory if there is space."""
        if len(self.items) < self.capacity:
            self.items.append(item)
            print(f"{item.name} was added to the inventory.")
            return True
        else:
            print("Inventory is full!")
            return False

    def remove_item(self, item_name):
        """Removes an item from the inventory by name."""
        for item in self.items:
            if item.name == item_name:
                self.items.remove(item)
                print(f"{item.name} was removed from the inventory.")
                return
        print(f"'{item_name}' not found in inventory.")

    def list_items(self):
        """Prints a list of all items in the inventory."""
        if not self.items:
            print("Inventory is empty.")
            return
        print("--- Inventory ---")
        for item in self.items:
            print(f"- {item}")
        print("-----------------")
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
        """
        Handles the object's death.  This can be overridden in subclasses.
        """
        self.visible = False
        self.solid = False
        print(f"{self.name} has died.")

    def update_status_effects(self, delta_time):
        """Updates status effects, applying their effects and decrementing timers."""
        # Using list() to create a copy, allowing modification during iteration
        for effect, duration in list(self.status_effects.items()):
            # Decrement timer
            new_duration = duration - delta_time
            if new_duration > 0:
                self.status_effects[effect] = new_duration
            else:
                del self.status_effects[effect]
                print(f"{self.name}'s {effect} has worn off.")
                continue # Skip to next effect once it's removed

            # Apply passive effects
            if effect == 'psychic_damage':
                dot_damage = 5 * delta_time # 5 damage per second
                print(f"{self.name} is taking psychic damage.")
                self.take_damage(dot_damage)

    def update(self, delta_time):
        """
        Updates the object's state.  This method is called every frame.
        This is meant to be overridden in subclasses.
        Args:
           delta_time: Time since the last frame in seconds.
        """
        pass

    def draw(self):
        """
        Draws the object.  This method is called every frame.
        This is meant to be overridden in subclasses, using a graphics library.
        """
        if self.visible:
            print(f"Drawing {self.name} at ({self.x}, {self.y}, {self.z})") # Basic drawing for now.

class Player(GameObject):
    """
    Represents the player character.
    """
    def __init__(self, name="Player", x=0, y=0, z=0):
        super().__init__(name=name, x=x, y=y, z=z, health=100, speed=5)
        self.weapon = None
        self.inventory = Inventory()
        self.level = 1
        self.experience = 0
        self.max_health = 100
        self.mana = 100
        self.max_mana = 100
        self.mana_regeneration_rate = 1.5  # Mana per second

    def update(self, delta_time):
        """
        Updates the player's state, including mana regeneration.
        """
        super().update(delta_time)
        self.update_status_effects(delta_time)
        self.mana += self.mana_regeneration_rate * delta_time
        if self.mana > self.max_mana:
            self.mana = self.max_mana

    def attack(self, target):
        """
        Attacks another GameObject, with damage influenced by strength and dexterity.

        Args:
            target (GameObject): The target to attack.
        """
        # --- Critical Hit/Miss Logic (based on dexterity) ---
        miss_chance = max(0, 5 - self.dexterity / 4)
        if random.uniform(0, 100) < miss_chance:
            print(f"{self.name}'s attack missed {target.name}!")
            return

        crit_chance = 5 + self.dexterity / 2
        is_critical = random.uniform(0, 100) < crit_chance

        # --- Damage Calculation (based on strength) ---
        if self.weapon:
            base_damage = self.weapon.damage
            strength_bonus = self.strength // 2
            total_damage = base_damage + strength_bonus
            attack_source = self.weapon.name
        else:
            base_damage = 2  # Low base damage for bare hands
            strength_bonus = self.strength // 2
            total_damage = base_damage + strength_bonus
            attack_source = "bare hands"

        if is_critical:
            total_damage *= 2  # Double damage on a critical hit
            print(f"CRITICAL HIT! {self.name} attacks {target.name} with {attack_source} for {total_damage} damage.")
        else:
            print(f"{self.name} attacks {target.name} with {attack_source} for {total_damage} damage.")

        target.take_damage(total_damage)

    def equip_weapon(self, weapon):
        """
        Equips a weapon.

        Args:
            weapon (Weapon): The weapon to equip.
        """
        if isinstance(weapon, Weapon):
            self.weapon = weapon
            print(f"{self.name} equipped {weapon.name}.")
        else:
            print(f"{self.name} cannot equip {weapon.name}. It is not a weapon.")

    def pickup_item(self, item):
        """
        Adds an item to the player's inventory.
        """
        self.inventory.add_item(item)

    def use_item(self, item_name):
        """
        Uses a consumable item from the inventory.
        """
        for item in self.inventory.items:
            if item.name == item_name:
                if isinstance(item, Consumable):
                    item.use(self)
                    self.inventory.remove_item(item_name)
                    return True
                else:
                    print(f"{self.name} cannot use {item.name}.")
                    return False
        print(f"'{item_name}' not found in inventory.")
        return False

    def gain_experience(self, amount):
        """
        Gains experience points.

        Args:
            amount (int): The amount of experience to gain.
        """
        self.experience += amount
        print(f"{self.name} gained {amount} experience.")
        self.check_level_up()

    def check_level_up(self):
        """
        Checks if the player has enough experience to level up.
        """
        # Example leveling curve: 100 * level * level
        required_experience = 100 * self.level * self.level
        if self.experience >= required_experience:
            self.level += 1
            self.max_health += 10
            self.health = self.max_health # Fully heal on level up.
            self.speed *= 1.1 # Increase speed by 10%
            print(f"{self.name} leveled up to level {self.level}!")

    def cast_spell(self, spell_name, target):
        """Casts a spell, with power influenced by intelligence."""
        if spell_name == "fireball":
            mana_cost = 20
            if self.mana >= mana_cost:
                self.mana -= mana_cost
                spell_damage = 15 + int(self.intelligence * 1.5)
                print(f"{self.name} casts Fireball on {target.name} for {spell_damage} damage!")
                target.take_damage(spell_damage)
            else:
                print(f"{self.name} does not have enough mana to cast Fireball!")
        elif spell_name == "heal":
            mana_cost = 10
            if self.mana >= mana_cost:
                self.mana -= mana_cost
                heal_amount = 10 + self.intelligence
                self.heal(heal_amount)
                print(f"{self.name} casts Heal and recovers {heal_amount} HP.")
            else:
                print(f"{self.name} does not have enough mana to cast Heal!")
        else:
            print(f"{self.name} does not know the spell {spell_name}.")


class Character(Player):
    """
    An intermediary class for specialized characters.
    It allows for setting health and state during initialization.
    """
    def __init__(self, name, x=0, y=0, z=0, health=100, state=None):
        super().__init__(name, x, y, z)
        self.health = health
        self.max_health = health
        self.state = state
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

        if self.distance_to(player) < self.aggro_range:
            current_speed = self.speed
            if 'slow' in self.status_effects:
                print(f"{self.name} is slowed!")
                current_speed /= 2
            # Move towards the player
            dx = player.x - self.x
            dy = player.y - self.y
            dz = player.z - self.z
            distance = self.distance_to(player)
            if distance > 0:
              self.move(dx / distance * current_speed * delta_time, dy / distance * current_speed * delta_time, dz/distance * current_speed * delta_time)
            # Attack the player if close enough.
            if self.distance_to(player) < 1:  # Attack range
                self.attack(player)

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

        time.sleep(0.01)

def run_game():
    """
    Main function to set up and run a demonstration of the game.
    """
    game = Game()

    # --- 1. Create Player and Set Attributes ---
    player = Player(name="Hero", x=0, y=0, z=0)
    player.strength = 15
    player.dexterity = 12
    player.intelligence = 18
    player.defense = 5
    game.add_object(player)
    print(f"A {player.name} has appeared! Str: {player.strength}, Dex: {player.dexterity}, Int: {player.intelligence}, Def: {player.defense}")

    # --- 2. Create Enemies ---
    enemy1 = Enemy(name="Goblin", x=5, y=5, z=0, type="Goblin")
    enemy1.defense = 2
    game.add_object(enemy1)

    enemy2 = Enemy(name="Orc", x=-5, y=-5, z=0, type="Orc")
    enemy2.attack_damage = 15
    enemy2.defense = 8
    game.add_object(enemy2)
    print(f"Enemies have spawned: A {enemy1.name} and an {enemy2.name}!")

    # --- 3. Create and Distribute Items ---
    sword = Weapon(name="Greatsword", description="A heavy, two-handed sword.", damage=18)
    health_potion = Consumable(name="Health Potion", description="Restores 40 HP.", effect="heal", value=40)

    # Player finds the items
    player.pickup_item(sword)
    player.equip_weapon(sword)
    player.pickup_item(health_potion)
    player.inventory.list_items()

    # --- 4. Demonstrate Initial Actions ---
    print("\n--- Initial Actions ---")
    print(f"Initial state: Player HP: {player.health}, Mana: {player.mana}")

    # Use a potion
    player.use_item("Health Potion")
    player.inventory.list_items()

    # Cast a powerful spell, influenced by high intelligence
    player.cast_spell("fireball", enemy1)

    # A powerful physical attack, influenced by strength
    player.attack(enemy2)

    print(f"\n--- Starting Game Loop ---")
    print("Watch as the hero and enemies fight to the death! Mana will regenerate over time.\n")

    # --- 5. Start the Game ---
    game.start()

class Skyix(Player):
    """
    Represents the character Sky.ix, the Bionic Goddess.
    Inherits from the Player class, adding unique Void-based mechanics.
    """
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