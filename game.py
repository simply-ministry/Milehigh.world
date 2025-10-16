import time
import random
import math

# --- Item System ---

class Item:
    """Base class for all items in the game."""
    def __init__(self, name, description):
        self.name = name
        self.description = description
        self.x = 0
        self.y = 0

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
        elif self.effect == "restore_mana":
            if hasattr(character, 'mana'):
                character.mana = min(character.max_mana, character.mana + self.value)
                print(f"{character.name} restored {self.value} mana.")
            else:
                print(f"But {character.name} has no Mana to restore.")
        else:
            print(f"The {self.name} has no effect.")

class Inventory:
    """Manages a character's items."""
    def __init__(self, owner_name="Unknown", capacity=20):
        self.items = []
        self.capacity = capacity
        self.owner_name = owner_name

    def add_item(self, item):
        if len(self.items) < self.capacity:
            self.items.append(item)
            print(f"'{item.name}' was added to {self.owner_name}'s inventory.")
            return True
        else:
            print("Inventory is full!")
            return False

    def remove_item(self, item_name):
        item_name_lower = item_name.lower()
        for item in self.items:
            if item.name.lower() == item_name_lower:
                self.items.remove(item)
                print(f"{item.name} was removed from the inventory.")
                return item
        print(f"'{item_name}' not found in inventory.")
        return None

    def list_items(self):
        if not self.items:
            print(f"{self.owner_name}'s inventory is empty.")
            return
        print(f"--- {self.owner_name}'s Inventory ---")
        for item in self.items:
            print(f"- {item}")
        print("-----------------")


# --- Base Character and Game Object Classes ---

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
        print(f"{self.name} takes {damage} damage.")
        if self.health <= 0:
            self.health = 0
            self.die()

    def heal(self, amount):
        self.health = min(self.max_health, self.health + amount)
        print(f"{self.name} heals for {amount} HP.")

    def die(self):
        print(f"{self.name} has been defeated.")

    def update(self):
        pass

class Character(GameObject):
    """Base class for all characters, including player and NPCs."""
    def __init__(self, name="Character", x=0, y=0, health=100):
        super().__init__(name, x, y, health=health)
        self.inventory = Inventory(owner_name=self.name)
        self.mana = 100
        self.max_mana = 100
        self.is_asleep = False
        self.sleep_duration = 0

    def pickup_item(self, item):
        self.inventory.add_item(item)

    def use_item(self, item_name):
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
        if hasattr(target, 'dialogue'):
            print(f"[{target.name}]: {target.dialogue}")
        else:
            print(f"{target.name} has nothing to say.")

    def cast_spell(self, spell_name, target):
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
        if self.is_asleep:
            self.sleep_duration -= 1
            print(f"{self.name} is asleep.")
            if self.sleep_duration <= 0:
                self.is_asleep = False
                print(f"{self.name} woke up.")

class Player(Character):
    """Represents the player character."""
    def __init__(self, name="Player", x=0, y=0, health=100):
        super().__init__(name, x, y, health=health)
        self.weapon = None
        self.level = 1
        self.experience = 0
        self.strength = 10
        self.dexterity = 10
        self.intelligence = 10

    def attack(self, target):
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
        if isinstance(weapon, Weapon):
            self.weapon = weapon
            print(f"{self.name} equipped {weapon.name}.")
        else:
            print(f"{self.name} cannot equip {weapon.name}. It is not a weapon.")

    def gain_experience(self, amount):
        self.experience += amount
        print(f"{self.name} gained {amount} experience.")
        self.check_level_up()

    def check_level_up(self):
        required_experience = 100 * self.level * self.level
        if self.experience >= required_experience:
            self.level += 1
            self.max_health += 10
            self.health = self.max_health
            print(f"{self.name} leveled up to level {self.level}!")

class NPC(Character):
    """A non-player character that can have dialogue."""
    def __init__(self, name, x, y, dialogue="Hello there."):
        super().__init__(name, x, y, health=50)
        self.dialogue = dialogue

class Enemy(Character):
    """An enemy character."""
    def __init__(self, name, x=0, y=0, health=50, attack_damage=10):
        super().__init__(name, x, y, health)
        self.attack_damage = attack_damage

    def attack(self, target):
        print(f"{self.name} attacks {target.name}!")
        target.take_damage(self.attack_damage)

# --- Specific Character Implementations ---

class Skyix(Player): pass
class Anastasia(Player):
    def __init__(self, name="Anastasia", x=0, y=0):
        super().__init__(name, x, y, health=90)
        self.dream_energy = 100
        self.max_dream_energy = 100
    def __str__(self):
        return f"{self.name} (HP: {self.health}/{self.max_health}, Dream Energy: {self.dream_energy}/{self.max_dream_energy})"

class Reverie(Player):
    def __init__(self, name="Reverie", x=0, y=0):
        super().__init__(name, x, y, health=110)
        self.mana = 150
        self.max_mana = 150

class Aeron(NPC): pass
class Zaia(Player): pass
class Micah(Player): pass
class Cirrus(Player): pass
class Ingris(Player): pass
class Otis(Player): pass
class Kai(Player): pass
class Kane(Enemy): pass
class Delilah(Enemy):
    def __init__(self, name="Delilah the Desolate", x=0, y=0):
        super().__init__(name, x, y, health=200)
        self.dialogue = "You cannot stop the inevitable."


# --- Game Engine ---

class Game:
    """Manages the game state, objects, and the main game loop for a text-based RPG."""
    def __init__(self, width=80, height=24):
        self.game_objects = []
        self.player_character = None
        self.is_running = True
        self.width = width
        self.height = height
        self.message_log = []
        self.event_flags = {}

    def add_object(self, obj):
        self.game_objects.append(obj)
        if isinstance(obj, Item):
            # Place item at a random spot if not explicitly set
            if obj.x == 0 and obj.y == 0:
                obj.x = random.randint(0, self.width - 1)
                obj.y = random.randint(0, self.height - 1)

    def set_player_character(self, character):
        self.player_character = character
        if character not in self.game_objects:
            self.add_object(character)

    def get_object_at(self, x, y):
        for obj in reversed(self.game_objects):
            if obj.x == x and obj.y == y and obj is not self.player_character:
                return obj
        return None

    def remove_object(self, obj):
        if obj in self.game_objects:
            self.game_objects.remove(obj)

    def trigger_event(self, event_name):
        self.event_flags[event_name] = True
        self.message_log.append(f"Event triggered: {event_name}")

    def event_triggered(self, event_name):
        return self.event_flags.get(event_name, False)

    def draw(self):
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
        for obj in self.game_objects[:]:
            obj.update()

        if self.player_character and self.player_character.x > 15 and not self.event_triggered("delilah_battle"):
            self.message_log.append("Suddenly, the air grows cold. Delilah the Desolate appears!")
            delilah = Delilah(x=self.player_character.x + 2, y=self.player_character.y)
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