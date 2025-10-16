import time
import random
import math

# --- Item System ---

class Item:
    """Base class for all items in the game.

    Attributes:
        name (str): The name of the item.
        description (str): A description of the item.
        x (int): The x-coordinate of the item in the game world.
        y (int): The y-coordinate of the item in the game world.
    """
    def __init__(self, name, description):
        self.name = name
        self.description = description
        self.x = 0
        self.y = 0

    def __str__(self):
        return f"{self.name}: {self.description}"

class Weapon(Item):
    """A type of item that can be equipped to deal damage.

    Attributes:
        damage (int): The amount of damage the weapon deals.
    """
    def __init__(self, name, description, damage):
        super().__init__(name, description)
        self.damage = damage

    def __str__(self):
        return f"{self.name} (Weapon, +{self.damage} DMG): {self.description}"

class Consumable(Item):
    """A type of item that can be used once for an effect.

    Attributes:
        effect (str): The type of effect the consumable has (e.g., "heal").
        value (int): The magnitude of the effect.
    """
    def __init__(self, name, description, effect, value):
        super().__init__(name, description)
        self.effect = effect
        self.value = value

    def __str__(self):
        return f"{self.name} (Consumable, {self.effect} +{self.value}): {self.description}"

    def use(self, character):
        """Applies the consumable's effect to a character.

        Args:
            character (Character): The character using the item.
        """
        print(f"{character.name} uses {self.name}!")
        if self.effect == "heal":
            character.heal(self.value)
        elif self.effect == "restore_mana":
            if hasattr(character, 'mana'):
                character.mana = min(character.max_mana, character.mana + self.value)
                print(f"{character.name} restored {self.value} mana.")
            else:
                print(f"But {character.name} has no Mana to restore.")
        else:
            print(f"The {self.name} has no effect.")

class Inventory:
    """Manages a character's items.

    Attributes:
        items (list): A list of the items in the inventory.
        capacity (int): The maximum number of items the inventory can hold.
        owner_name (str): The name of the character who owns the inventory.
    """
    def __init__(self, owner_name="Unknown", capacity=20):
        self.items = []
        self.capacity = capacity
        self.owner_name = owner_name

    def add_item(self, item):
        """Adds an item to the inventory.

        Args:
            item (Item): The item to add.

        Returns:
            bool: True if the item was added successfully, False otherwise.
        """
        if len(self.items) < self.capacity:
            self.items.append(item)
            print(f"'{item.name}' was added to {self.owner_name}'s inventory.")
            return True
        else:
            print("Inventory is full!")
            return False

    def remove_item(self, item_name):
        """Removes an item from the inventory by name.

        Args:
            item_name (str): The name of the item to remove.

        Returns:
            Item: The removed item, or None if the item was not found.
        """
        item_name_lower = item_name.lower()
        for item in self.items:
            if item.name.lower() == item_name_lower:
                self.items.remove(item)
                print(f"{item.name} was removed from the inventory.")
                return item
        print(f"'{item_name}' not found in inventory.")
        return None

    def list_items(self):
        """Prints a list of all items in the inventory."""
        if not self.items:
            print(f"{self.owner_name}'s inventory is empty.")
            return
        print(f"--- {self.owner_name}'s Inventory ---")
        for item in self.items:
            print(f"- {item}")
        print("-----------------")


# --- Base Character and Game Object Classes ---

class GameObject:
    """Base class for all tangible objects in the game world.

    Attributes:
        name (str): The name of the object.
        x (int): The x-coordinate of the object.
        y (int): The y-coordinate of the object.
        health (int): The current health of the object.
        max_health (int): The maximum health of the object.
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
        """Moves the object by a given delta.

        Args:
            dx (int): The change in the x-coordinate.
            dy (int): The change in the y-coordinate.
        """
        self.x += dx
        self.y += dy

    def take_damage(self, damage):
        """Reduces the object's health by a given amount.

        Args:
            damage (int): The amount of damage to take.
        """
        self.health -= damage
        print(f"{self.name} takes {damage} damage.")
        if self.health <= 0:
            self.health = 0
            self.die()

    def heal(self, amount):
        """Increases the object's health by a given amount.

        Args:
            amount (int): The amount of health to restore.
        """
        self.health = min(self.max_health, self.health + amount)
        print(f"{self.name} heals for {amount} HP.")

    def die(self):
        """Handles the object's death."""
        print(f"{self.name} has been defeated.")

    def update(self):
        """Updates the object's state. Called once per game loop."""
        pass

class Character(GameObject):
    """Base class for all characters, including player and NPCs.

    Attributes:
        inventory (Inventory): The character's inventory.
        mana (int): The character's current mana.
        max_mana (int): The character's maximum mana.
        is_asleep (bool): Whether the character is asleep.
        sleep_duration (int): The remaining duration of sleep.
    """
    def __init__(self, name="Character", x=0, y=0, health=100):
        super().__init__(name, x, y, health=health)
        self.inventory = Inventory(owner_name=self.name)
        self.mana = 100
        self.max_mana = 100
        self.is_asleep = False
        self.sleep_duration = 0

    def pickup_item(self, item):
        """Picks up an item and adds it to the inventory.

        Args:
            item (Item): The item to pick up.
        """
        self.inventory.add_item(item)

    def use_item(self, item_name):
        """Uses an item from the inventory.

        Args:
            item_name (str): The name of the item to use.
        """
        item_to_use = None
        for item in self.inventory.items:
            if item.name.lower() == item_name.lower():
                item_to_use = item
                break

        if item_to_use:
            if isinstance(item_to_use, Consumable):
                item_to_use.use(self)
                self.inventory.remove_item(item_name)
            else:
                print(f"'{item_to_use.name}' cannot be used.")
        else:
            print(f"'{item_name}' not found in inventory.")

    def talk(self, target):
        """Initiates a conversation with another character.

        Args:
            target (Character): The character to talk to.
        """
        if hasattr(target, 'dialogue'):
            print(f"[{target.name}]: {target.dialogue}")
        else:
            print(f"{target.name} has nothing to say.")

    def cast_spell(self, spell_name, target):
        """Casts a spell on a target.

        Args:
            spell_name (str): The name of the spell to cast.
            target (Character): The target of the spell.
        """
        spell_name_lower = spell_name.lower()
        if spell_name_lower == "dream weave" and self.mana >= 20:
            self.mana -= 20
            print(f"{self.name} weaves a dream, putting {target.name} to sleep!")
            target.is_asleep = True
            target.sleep_duration = 3
        elif spell_name_lower == "nightmare" and self.mana >= 35:
            self.mana -= 35
            print(f"{self.name} conjures a nightmare, damaging {target.name}'s mind!")
            target.take_damage(50)
        elif spell_name_lower == "soothing slumber" and self.mana >= 25:
            self.mana -= 25
            print(f"{self.name} casts a soothing slumber, healing {target.name}!")
            target.heal(40)
        else:
            print(f"{self.name} doesn't know the spell '{spell_name}' or lacks the mana.")

    def update(self):
        """Updates the character's state, including status effects."""
        if self.is_asleep:
            self.sleep_duration -= 1
            print(f"{self.name} is asleep.")
            if self.sleep_duration <= 0:
                self.is_asleep = False
                print(f"{self.name} woke up.")

class Player(Character):
    """Represents the player character.

    Attributes:
        weapon (Weapon): The player's equipped weapon.
        level (int): The player's current level.
        experience (int): The player's current experience points.
        strength (int): The player's strength stat.
        dexterity (int): The player's dexterity stat.
        intelligence (int): The player's intelligence stat.
    """
    def __init__(self, name="Player", x=0, y=0, health=100):
        super().__init__(name, x, y, health=health)
        self.weapon = None
        self.level = 1
        self.experience = 0
        self.strength = 10
        self.dexterity = 10
        self.intelligence = 10

    def attack(self, target):
        """Attacks a target, calculating damage based on stats and equipment.

        Args:
            target (Character): The character to attack.
        """
        miss_chance = max(0, 5 - self.dexterity / 4)
        if random.uniform(0, 100) < miss_chance:
            print(f"{self.name}'s attack missed {target.name}!")
            return

        crit_chance = 5 + self.dexterity / 2
        is_critical = random.uniform(0, 100) < crit_chance

        if self.weapon:
            base_damage = self.weapon.damage
            strength_bonus = self.strength // 2
            total_damage = base_damage + strength_bonus
            attack_source = self.weapon.name
        else:
            base_damage = 2
            strength_bonus = self.strength // 2
            total_damage = base_damage + strength_bonus
            attack_source = "bare hands"

        if is_critical:
            total_damage *= 2
            print(f"CRITICAL HIT! {self.name} attacks {target.name} with {attack_source} for {total_damage} damage.")
        else:
            print(f"{self.name} attacks {target.name} with {attack_source} for {total_damage} damage.")

        target.take_damage(total_damage)

    def equip_weapon(self, weapon):
        """Equips a weapon.

        Args:
            weapon (Weapon): The weapon to equip.
        """
        if isinstance(weapon, Weapon):
            self.weapon = weapon
            print(f"{self.name} equipped {weapon.name}.")
        else:
            print(f"{self.name} cannot equip {weapon.name}. It is not a weapon.")

    def gain_experience(self, amount):
        """Gains experience points and checks for level up.

        Args:
            amount (int): The amount of experience to gain.
        """
        self.experience += amount
        print(f"{self.name} gained {amount} experience.")
        self.check_level_up()

    def check_level_up(self):
        """Checks if the player has enough experience to level up."""
        required_experience = 100 * self.level * self.level
        if self.experience >= required_experience:
            self.level += 1
            self.max_health += 10
            self.health = self.max_health
            print(f"{self.name} leveled up to level {self.level}!")

class NPC(Character):
    """A non-player character that can have dialogue.

    Attributes:
        dialogue (str): The dialogue the NPC will say when spoken to.
    """
    def __init__(self, name, x, y, dialogue="Hello there."):
        super().__init__(name, x, y, health=50)
        self.dialogue = dialogue

class Enemy(Character):
    """An enemy character.

    Attributes:
        attack_damage (int): The amount of damage the enemy deals.
    """
    def __init__(self, name, x=0, y=0, health=50, attack_damage=10):
        super().__init__(name, x, y, health)
        self.attack_damage = attack_damage

    def attack(self, target):
        """Attacks a target.

        Args:
            target (Character): The character to attack.
        """
        print(f"{self.name} attacks {target.name}!")
        target.take_damage(self.attack_damage)

# --- Specific Character Implementations ---

class Skyix(Player):
    """A specific implementation of the Player class for the character Skyix."""
    pass
class Anastasia(Player):
    """A specific implementation of the Player class for the character Anastasia."""
    def __init__(self, name="Anastasia", x=0, y=0):
        super().__init__(name, x, y, health=90)
        self.dream_energy = 100
        self.max_dream_energy = 100
    def __str__(self):
        return f"{self.name} (HP: {self.health}/{self.max_health}, Dream Energy: {self.dream_energy}/{self.max_dream_energy})"

class Reverie(Player):
    """A specific implementation of the Player class for the character Reverie."""
    def __init__(self, name="Reverie", x=0, y=0):
        super().__init__(name, x, y, health=110)
        self.mana = 150
        self.max_mana = 150

class Aeron(NPC):
    """A specific implementation of the NPC class for the character Aeron."""
    pass
class Zaia(Player):
    """A specific implementation of the Player class for the character Zaia."""
    pass
class Micah(Player):
    """A specific implementation of the Player class for the character Micah."""
    pass
class Cirrus(Player):
    """A specific implementation of the Player class for the character Cirrus."""
    pass
class Ingris(Player):
    """A specific implementation of the Player class for the character Ingris."""
    pass
class Otis(Player):
    """A specific implementation of the Player class for the character Otis."""
    pass
class Kai(Player):
    """A specific implementation of the Player class for the character Kai."""
    pass
class Kane(Enemy):
    """A specific implementation of the Enemy class for the character Kane."""
    pass
class Delilah(Enemy):
    """A specific implementation of the Enemy class for the character Delilah."""
    def __init__(self, name="Delilah the Desolate", x=0, y=0):
        super().__init__(name, x, y, health=200)
        self.dialogue = "You cannot stop the inevitable."


# --- Game Engine ---

class Game:
    """Manages the game state, objects, and the main game loop for a text-based RPG.

    Attributes:
        game_objects (list): A list of all game objects in the world.
        player_character (Player): The player character.
        is_running (bool): Whether the game is currently running.
        width (int): The width of the game world grid.
        height (int): The height of the game world grid.
        message_log (list): A log of messages to display to the player.
        event_flags (dict): A dictionary of event flags.
    """
    def __init__(self, width=80, height=24):
        self.game_objects = []
        self.player_character = None
        self.is_running = True
        self.width = width
        self.height = height
        self.message_log = []
        self.event_flags = {}

    def add_object(self, obj):
        """Adds a game object to the world.

        Args:
            obj (GameObject): The object to add.
        """
        self.game_objects.append(obj)
        if isinstance(obj, Item):
            # Place item at a random spot if not explicitly set
            if obj.x == 0 and obj.y == 0:
                obj.x = random.randint(0, self.width - 1)
                obj.y = random.randint(0, self.height - 1)

    def set_player_character(self, character):
        """Sets the player character.

        Args:
            character (Player): The player character.
        """
        self.player_character = character
        if character not in self.game_objects:
            self.add_object(character)

    def get_object_at(self, x, y):
        """Gets the topmost object at a given coordinate.

        Args:
            x (int): The x-coordinate.
            y (int): The y-coordinate.

        Returns:
            GameObject: The object at the given coordinates, or None if no object is found.
        """
        for obj in reversed(self.game_objects):
            if obj.x == x and obj.y == y and obj is not self.player_character:
                return obj
        return None

    def remove_object(self, obj):
        """Removes a game object from the world.

        Args:
            obj (GameObject): The object to remove.
        """
        if obj in self.game_objects:
            self.game_objects.remove(obj)

    def trigger_event(self, event_name):
        """Triggers an event.

        Args:
            event_name (str): The name of the event to trigger.
        """
        self.event_flags[event_name] = True
        self.message_log.append(f"Event triggered: {event_name}")

    def event_triggered(self, event_name):
        """Checks if an event has been triggered.

        Args:
            event_name (str): The name of the event to check.

        Returns:
            bool: True if the event has been triggered, False otherwise.
        """
        return self.event_flags.get(event_name, False)

    def draw(self):
        """Draws the game world to the console."""
        print("\033c", end="") # Clear console
        grid = [['.' for _ in range(self.width)] for _ in range(self.height)]

        # Draw items first
        for obj in self.game_objects:
            if isinstance(obj, Item):
                if 0 <= obj.x < self.width and 0 <= obj.y < self.height:
                    grid[int(obj.y)][int(obj.x)] = 'i'

        # Draw characters on top
        for obj in self.game_objects:
            if isinstance(obj, Character):
                if 0 <= obj.x < self.width and 0 <= obj.y < self.height:
                    if isinstance(obj, Player):
                        grid[int(obj.y)][int(obj.x)] = '@'
                    elif isinstance(obj, Enemy):
                        grid[int(obj.y)][int(obj.x)] = 'E'
                    elif isinstance(obj, NPC):
                        grid[int(obj.y)][int(obj.x)] = 'N'

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
        """Handles player input."""
        if not self.player_character or self.player_character.is_asleep:
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
            blocking_object = self.get_object_at(target_x, target_y)
            if not blocking_object:
                self.player_character.move(dx, dy)
            else:
                self.message_log.append(f"{blocking_object.name} is in your way.")
        elif action == "look":
            for obj in self.game_objects:
                if obj != self.player_character:
                     self.message_log.append(f"You see {obj.name} at ({obj.x}, {obj.y}).")
        elif action == "get":
            item_found = self.get_object_at(self.player_character.x, self.player_character.y)
            if item_found and isinstance(item_found, Item):
                if self.player_character.inventory.add_item(item_found):
                    self.remove_object(item_found)
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
        """Updates the game state."""
        for obj in self.game_objects[:]:
            obj.update()

        if self.player_character and self.player_character.x > 15 and not self.event_triggered("delilah_battle"):
            self.message_log.append("Suddenly, the air grows cold. Delilah the Desolate appears!")
            delilah = Delilah(x=self.player_character.x + 2, y=self.player_character.y)
            self.add_object(delilah)
            self.trigger_event("delilah_battle")

    def start(self):
        """Starts the main game loop."""
        while self.is_running:
            self.draw()
            self.handle_input()
            if not self.is_running: break
            self.update()
        print("\nThank you for playing Milehigh.World!")


if __name__ == "__main__":
    game = Game()

    player_zaia = Zaia(name="Zaia", x=5, y=5)
    npc_aeron = Aeron(name="Aeron", x=10, y=8, dialogue="Greetings, traveler. The world is in peril.")
    enemy_kane = Kane(name="Kane", x=20, y=12, health=80)

    game.set_player_character(player_zaia)
    game.add_object(npc_aeron)
    game.add_object(enemy_kane)

    health_potion_item = Consumable("Health Potion", "Restores 50 health.", "heal", 50)
    game.add_object(health_potion_item)

    quest_sword = Weapon("Ancient Sword", "A sword of ancient power.", 20)
    quest_sword.x = 25
    quest_sword.y = 15
    game.add_object(quest_sword)

    print("Game world initialized. Type 'quit' to exit.")
    game.start()