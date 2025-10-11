import json
import math

# --- 1. ALL CORE CLASSES (PASTED FROM PREVIOUS STEPS) ---
# (Includes GameObject, Character, Item, etc.)
# ... (For brevity, imagine all previously defined core classes are here) ...

class GameObject:
    """The base class for all objects in the game world."""
    def __init__(self, name="Object", symbol='?', x=0, y=0, z=0, health=0, speed=0, visible=True, solid=True, defense=0, state=None):
        self.name = name
        self.symbol = symbol
        self.x = x
        self.y = y
        self.z = z
        self.health = health
        self.speed = speed
        self.visible = visible
        self.solid = solid
        self.defense = defense
        self.attributes = {}  # Dictionary for storing additional attributes.
        self.status_effects = {}  # e.g., {'sleep': 6, 'slow': 8}
        self.resistances = {} # e.g., {'fire': 0.5, 'ice': 1.5}
        self.state = state

    def __repr__(self):
        """
        Returns a string representation of the GameObject.  Useful for debugging.
        """
        return f"{self.name}(x={self.x}, y={self.y}, z={self.z}, health={self.health})"

    def distance_to(self, other):
        """
        Calculates the distance to another GameObject.
        Args:
            other (GameObject): The other GameObject.
        Returns:
            float: The distance to the other GameObject.
        """
        dx = self.x - other.x
        dy = self.y - other.y
        dz = self.z - other.z
        return math.sqrt(dx * dx + dy * dy + dz * dz)

    def move(self, dx, dy, dz=0):
        """
        Moves the object by the specified amount.
        Args:
            dx (float): The change in x-coordinate.
            dy (float): The change in y-coordinate.
            dz (float): The change in z-coordinate (for 3D).
        """
        self.x += dx
        self.y += dy
        self.z += dz

    def take_damage(self, damage, damage_type="physical"):
        """
        Reduces the object's health after factoring in defense and resistances.
        Args:
            damage (int): The amount of incoming damage.
            damage_type (str): The type of damage (e.g., 'physical', 'fire', 'ice').
        """
        # Apply vulnerability
        if 'vulnerable' in self.status_effects:
            potency = self.status_effects['vulnerable'].get('potency', 1)
            damage *= potency
            print(f"{self.name} is vulnerable! Damage is multiplied by {potency}!")

        # Apply resistances
        resistance_multiplier = self.resistances.get(damage_type, 1.0)
        modified_damage = damage * resistance_multiplier

        # Apply defense only to physical damage
        total_defense = 0
        if damage_type == "physical":
            total_defense = self.defense
            if hasattr(self, 'equipment'):
                equipped_stats = self.equipment.get_total_stats()
                total_defense += equipped_stats.get("defense", 0)

            if 'armor_break' in self.status_effects:
                print(f"{self.name} is armor broken! Defense is negated.")
                total_defense = 0

        actual_damage = max(0, modified_damage - total_defense)
        self.health -= actual_damage

        if actual_damage > 0:
            print(f"{self.name} takes {int(actual_damage)} {damage_type} damage.")
        else:
            if resistance_multiplier < 1.0:
                print(f"{self.name} resists the {damage_type} damage!")
            else:
                print(f"{self.name}'s defense holds strong against the {damage_type} attack!")

        # self.state = state # This line was causing an error, state is not defined here

    def apply_status_effect(self, effect_name, duration, potency=0):
        """
        Applies a status effect to the object.
        If the effect already exists, it refreshes the duration.
        Args:
            effect_name (str): The name of the effect (e.g., 'poison', 'slow').
            duration (int): The duration of the effect in turns.
            potency (int): The strength of the effect (e.g., damage per turn).
        """
        print(f"{self.name} is now affected by {effect_name}.")
        self.status_effects[effect_name] = {"duration": duration, "potency": potency}

    def update_status_effects(self):
        """
        Updates all active status effects. Called each turn.
        Handles duration countdowns and applies ongoing effects.
        """
        if not hasattr(self, 'status_effects'):
            return

        effects_to_remove = []
        # Create a copy of items to iterate over, allowing modification of the original dict
        for effect, data in list(self.status_effects.items()):
            # Apply ongoing damage/healing effects
            if effect == 'psychic_damage' and 'psychic_damage' in self.status_effects: # From Anastasia's ability
                damage = data.get('potency', 2) # Defaulting to 2 damage per turn
                self.health -= damage
                print(f"{self.name} takes {damage} damage from psychic energies.")

            # Countdown duration
            data['duration'] -= 1
            if data['duration'] <= 0:
                effects_to_remove.append(effect)

        # Remove expired effects
        for effect in effects_to_remove:
            if effect in self.status_effects:
                del self.status_effects[effect]
                print(f"{self.name} is no longer affected by {effect}.")

    def update(self, scene_manager):
        """Placeholder for object-specific logic that runs each turn."""
        self.update_status_effects()

    def to_dict(self):
        """Serializes the object to a dictionary."""
        return {
            "class": self.__class__.__name__,
            "name": self.name,
            "symbol": self.symbol,
            "x": self.x,
            "y": self.y,
            "state": self.state,
        }

    @staticmethod
    def create_from_dict(data):
        """Factory method to deserialize a game object from a dictionary."""
        class_name = data.get("class")
        if not class_name or class_name not in globals():
            raise ValueError(f"Invalid or missing class name in data: {data}")
        target_class = globals()[class_name]
        # Dispatch to the specific class's from_dict method
        return target_class.from_dict(data)

    @classmethod
    def from_dict(cls, data):
        """Deserializes a basic GameObject from a dictionary."""
        # This version is for the base class and serves as a default.
        return cls(
            name=data.get("name"),
            symbol=data.get("symbol"),
            x=data.get("x"),
            y=data.get("y"),
            state=data.get("state"),
        )


class Item(GameObject):
    """Represents items that can be picked up or used."""
    def __init__(self, name="Item", symbol='*', x=0, y=0):
        super().__init__(name, symbol, x, y)

class Interactable(GameObject):
    """Represents objects that can be examined for a description."""
    def __init__(self, name, symbol, x, y, description):
        super().__init__(name, symbol, x, y)
        self.description = description

    def on_examine(self):
        """Returns the description of the object."""
        return self.description

    def to_dict(self):
        data = super().to_dict()
        data["description"] = self.description
        return data

    @classmethod
    def from_dict(cls, data):
        return cls(
            name=data.get("name"),
            symbol=data.get("symbol"),
            x=data.get("x"),
            y=data.get("y"),
            description=data.get("description"),
        )

class Player(GameObject):
    """
    Represents the player character.
    """
    def __init__(self, name="Player", x=0, y=0, z=0):
        super().__init__(name=name, x=x, y=y, z=z, health=100, speed=5)
        self.inventory = []
        self.level = 1
        self.experience = 0
        self.max_health = 100
        self.resources = {"mana": {"current": 100, "max": 100, "regen_rate": 1.5}}
        self.strength = 10
        self.dexterity = 10
        self.intelligence = 10
        # --- NEW: Equipment Manager ---
        self.equipment = Equipment(owner=self)
        self.abilities = {
            "fireball": Fireball(),
            "poison_cloud": PoisonCloud(),
        }

    def use_ability(self, ability_name, target):
        """Uses a known ability on a target."""
        ability_name = ability_name.lower().replace(" ", "_")
        if ability_name in self.abilities:
            ability = self.abilities[ability_name]
            ability.use(self, target)
        else:
            print(f"{self.name} does not know the ability {ability_name}.")

    def attack(self, target):
        """
        Attacks another GameObject, with damage influenced by strength and dexterity.
        Args:
            target (GameObject): The target to attack.
        """
        # --- Evasion Check ---
        if 'evasion' in target.status_effects:
            if random.uniform(0, 100) < 50: # 50% chance to miss against evasion
                print(f"{self.name}'s attack was evaded by {target.name}!")
                return

        # --- Critical Hit/Miss Logic (based on dexterity) ---
        miss_chance = max(0, 5 - self.dexterity / 4)
        if random.uniform(0, 100) < miss_chance:
            print(f"{self.name}'s attack missed {target.name}!")
            return

        crit_chance = 5 + self.dexterity / 2
        is_critical = random.uniform(0, 100) < crit_chance

        # --- Damage Calculation (based on strength and equipment) ---
        equipped_stats = self.equipment.get_total_stats()
        weapon_damage = equipped_stats["damage"]
        strength_bonus = self.strength // 2
        total_damage = weapon_damage + strength_bonus

        attack_source = self.equipment.slots["weapon"].name if self.equipment.slots["weapon"] else "bare hands"

        if is_critical:
            total_damage *= 2  # Double damage on a critical hit
            print(f"CRITICAL HIT! {self.name} attacks {target.name} with {attack_source} for {total_damage} damage.")
        else:
            print(f"{self.name} attacks {target.name} with {attack_source} for {total_damage} damage.")

        target.take_damage(total_damage)

    def equip_item(self, item_name):
        """Finds an item in inventory and equips it."""
        item_to_equip = None
        for item in self.inventory:
            if item.name.lower() == item_name.lower():
                item_to_equip = item
                break

        if item_to_equip:
            self.equipment.equip(item_to_equip)
        else:
            print(f"'{item_name}' not found in inventory.")

    def update(self, delta_time):
        """
        Updates the player's state, including resource regeneration.
        """
        super().update(self)
        # Regenerate all resources
        for res, values in self.resources.items():
            new_value = values["current"] + values.get("regen_rate", 0) * delta_time
            values["current"] = min(new_value, values["max"])

    def pickup_item(self, item):
        """
        Picks up an item. If it's a consumable and one of the same
        name already exists, it stacks. Otherwise, it's added as a new item.
        """
        if isinstance(item, Consumable):
            for inventory_item in self.inventory:
                if inventory_item.name == item.name and isinstance(inventory_item, Consumable):
                    inventory_item.quantity += 1
                    print(f"{self.name} picked up another {item.name}. Quantity: {inventory_item.quantity}")
                    # Make the picked-up object disappear from the world
                    item.visible = False
                    item.solid = False
                    return  # Exit after stacking

        # If no stack was found, or it's not a consumable, add as a new item
        self.inventory.append(item)
        item.visible = False
        item.solid = False
        print(f"{self.name} picked up {item.name}.")

    def use_item(self, item_name):
        """
        Uses an item from the inventory. If the item is a consumable,
        it decreases its quantity and removes it if the quantity is zero.
        """
        for i, item in enumerate(self.inventory):
            if item.name == item_name:
                if isinstance(item, Consumable):
                    item.use(self)  # Apply the effect
                    item.quantity -= 1
                    print(f"{self.name} used a {item.name}. {item.quantity} remaining.")
                    if item.quantity <= 0:
                        self.inventory.pop(i)  # Remove the item if quantity is zero
                        print(f"The last {item.name} was used.")
                    return True # Indicate success
                else:
                    print(f"{self.name} cannot use {item.name} as a consumable.")
                    return False
        print(f"{self.name} does not have '{item_name}' in their inventory.")
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

# --- Anastasia's Class Implementation ---

class Anastasia(Player):
    """
    Implementation of Anastasia the Dreamer.
    Playstyle: Battlefield controller and disruptor.
    """
    def __init__(self, name="Anastasia", x=0, y=0, z=0):
        super().__init__(name=name, x=x, y=y, z=z)
        self.health = 100
        self.max_health = 100
        self.resources["mana"] = {"current": 150, "max": 150, "regen_rate": 1.5}
        self.resources["dream_weave"] = {"current": 0, "max": 100, "regen_rate": 0.5}

        # Lucid Dream State
        self.is_lucid_dream_active = False
        self.lucid_dream_duration = 15 # in game ticks/seconds
        self.lucid_dream_timer = 0

    def update(self, delta_time):
        """Called every game tick to update Anastasia's state."""
        # First, call the Player's update for mana regen and status effects
        super().update(delta_time)

        # Passively build a small amount of Dream Weave
        self.build_dream_weave(0.5 * delta_time)

        if self.is_lucid_dream_active:
            self.lucid_dream_timer -= delta_time
            if self.lucid_dream_timer <= 0:
                self.is_lucid_dream_active = False
                self.lucid_dream_timer = 0
                print("\n-- Anastasia's Lucid Dream fades. The world returns to normal. --\n")

    def build_dream_weave(self, amount):
        """Increases the Dream Weave meter."""
        if not self.is_lucid_dream_active:
            dream_weave = self.resources["dream_weave"]
            dream_weave["current"] = min(dream_weave["current"] + amount, dream_weave["max"])

    def activate_lucid_dream(self):
        """Activates the Lucid Dream state if the meter is full."""
        dream_weave = self.resources["dream_weave"]
        if dream_weave["current"] >= dream_weave["max"]:
            print("\n** Anastasia activates LUCID DREAM! The battlefield warps! **\n")
            self.is_lucid_dream_active = True
            self.lucid_dream_timer = self.lucid_dream_duration
            dream_weave["current"] = 0
            return True
        else:
            print("Dream Weave is not full yet!")
            return False

    # --- ABILITIES ---

    def lulling_whisper(self, targets):
        """Puts target(s) to sleep using the new status effect system."""
        cost = 20
        mana = self.resources["mana"]
        if mana["current"] < cost:
            print("Not enough mana!")
            return

        mana["current"] -= cost
        print(f"{self.name} uses Lulling Whisper.")

        if self.is_lucid_dream_active:
            print("The whisper becomes a wave, affecting all targets!")
            for target in targets:
                target.apply_status_effect('sleep', 6)
        else:
            if targets:
                target = targets[0]
                target.apply_status_effect('sleep', 6)

        self.build_dream_weave(15)

    def phantasmal_grasp(self, target):
        """Slows a target and deals minor damage over time."""
        cost = 25
        mana = self.resources["mana"]
        if mana["current"] < cost:
            print("Not enough mana!")
            return

        mana["current"] -= cost
        print(f"{self.name} uses Phantasmal Grasp on {target.name}.")

        duration = 8
        if self.is_lucid_dream_active:
            print("The grasp erupts from the target, slowing nearby enemies!")
            duration += 4 # In a real game, you'd find nearby enemies.

        target.apply_status_effect('slow', duration)
        target.apply_status_effect('psychic_damage', duration, potency=2) # 2 damage per turn

        self.build_dream_weave(15)

    def fleeting_vision(self, allies):
        """Grants evasion and speed to an ally or the whole party."""
        cost = 30
        mana = self.resources["mana"]
        if mana["current"] < cost:
            print("Not enough mana!")
            return

        mana["current"] -= cost
        print(f"{self.name} uses Fleeting Vision.")

        if self.is_lucid_dream_active:
            print("The vision is shared with the entire party!")
            for ally in allies:
                ally.apply_status_effect('evasion', 5)
        else:
            if allies:
                ally = allies[0]
                ally.apply_status_effect('evasion', 5)

    def oneiric_collapse(self, enemies, allies):
        """Ultimate Ability: Pulls the battlefield into the Dreamscape."""
        if not self.is_lucid_dream_active:
            print("Must be in Lucid Dream to use Oneiric Collapse!")
            return

        print(f"\n!!! {self.name} unleashes her ultimate: ONEIRIC COLLAPSE !!!")
        print("The area is pulled into the Dreamscape!")

        for enemy in enemies:
            enemy.apply_status_effect('confusion', 10)
            enemy.apply_status_effect('armor_break', 10)

        for ally in allies:
            ally.apply_status_effect('empowered', 10)

        self.is_lucid_dream_active = False
        self.lucid_dream_timer = 0


# --- (Continuing from the previous Python classes) ---
import random # Import the random library for her ultimate ability

class Reverie(Player):
    """
    Represents Reverie, a powerful and unpredictable Mage/Controller.
    She builds a unique resource, Enigma, by casting spells, which she then
    unleashes in a powerful, random ultimate attack.
    """

    def __init__(self, name="Reverie", x=0, y=0, z=0):
        # Initialize the parent Player class with Reverie's stats
        super().__init__(name, x, y, z)
        self.health = 110
        self.max_health = 110
        self.resources["mana"] = {"current": 150, "max": 150, "regen_rate": 1.5}
        self.resources["enigma"] = {"current": 0, "max": 100, "regen_rate": 0}

    def chaos_unleashed(self, target):
        """
        Unleashes her ultimate ability when Enigma is at max.
        Consumes all Enigma for a powerful, random effect.
        """
        enigma = self.resources["enigma"]
        if enigma["current"] >= enigma["max"]:
            print(f"{self.name} unleashes CHAOS UNLEASHED!")
            enigma["current"] = 0  # Reset Enigma after use

            # Determine the random, powerful effect
            possible_effects = [
                "massive_damage",
                "full_heal_and_mana",
                "double_damage_debuff",
                "mana_drain"
            ]
            effect = random.choice(possible_effects)

            if effect == "massive_damage":
                damage = random.randint(100, 200)
                print(f"A torrent of pure chaotic energy strikes {target.name} for {damage} damage!")
                target.take_damage(damage)
            elif effect == "full_heal_and_mana":
                print(f"The chaotic energy surges inward, restoring {self.name} to full power!")
                self.health = self.max_health
                self.resources["mana"]["current"] = self.resources["mana"]["max"]
            elif effect == "double_damage_debuff":
                print(f"The chaotic energy latches onto {target.name}, making them vulnerable.")
                target.apply_status_effect('vulnerable', 2, potency=2) # 2x damage for 2 turns
            elif effect == "mana_drain":
                drained_mana = 0
                if "mana" in target.resources:
                    drained_mana = target.resources["mana"]["current"]
                    target.resources["mana"]["current"] = 0
                print(f"{self.name} drains all of {target.name}'s {drained_mana} mana!")
                self.resources["mana"]["current"] = min(self.resources["mana"]["max"], self.resources["mana"]["current"] + drained_mana)

            return True
        else:
            print(f"{self.name} needs more Enigma to use Chaos Unleashed. ({self.enigma}/{self.max_enigma})")
            return False

class Enemy(GameObject):
    """
    Represents an enemy character.
    """
    def __init__(self, name="Enemy", x=0, y=0, z=0, type="Generic"):
        super().__init__(name=name, x=x, y=y, z=z, health=50, speed=2)
        self.type = type  # e.g., "Goblin", "Orc", "Dragon"
        self.attack_damage = 10
        self.aggro_range = 8
        self.attack_range = 1.5
        self.xp_value = 0
        self.state = 'idle' # Can be 'idle', 'chasing', 'attacking'
        self.patrol_target = None
        # Add some default resistances for demonstration
        self.resistances = {"fire": 0.5, "ice": 1.5}

    def attack(self, target):
        """
        Attacks another GameObject.
        Args:
            target (GameObject): The target to attack.
        """
        print(f"{self.name} attacks {target.name} for {self.attack_damage} damage.")
        target.take_damage(self.attack_damage)

    def update(self, delta_time, player, scene):
        """
        Updates the enemy's state machine. This is called every frame.
        """
        self.update_status_effects()

        if 'sleep' in self.status_effects or 'confusion' in self.status_effects:
            print(f"{self.name} is incapacitated and cannot act.")
            return

        distance_to_player = self.distance_to(player)

        # State transitions
        if distance_to_player <= self.attack_range:
            self.state = 'attacking'
        elif distance_to_player <= self.aggro_range:
            self.state = 'chasing'
        else:
            self.state = 'idle'

        # Actions based on state
        if self.state == 'attacking':
            self.attack(player)
        elif self.state == 'chasing':
            self.move_towards(player, delta_time)
        elif self.state == 'idle':
            self.patrol(delta_time, scene)

    def move_towards(self, target, delta_time):
        """Moves the enemy towards a target."""
        current_speed = self.speed
        if 'slow' in self.status_effects:
            print(f"{self.name} is slowed!")
            current_speed /= 2

        dx = target.x - self.x
        dy = target.y - self.y
        distance = self.distance_to(target)
        if distance > 0:
            self.move(dx / distance * current_speed * delta_time, dy / distance * current_speed * delta_time)

    def patrol(self, delta_time, scene):
        """Wanders around when idle."""
        if not self.patrol_target or self.distance_to(self.patrol_target) < 1:
            # Pick a new random point to patrol to
            new_x = random.randint(0, scene.width - 1)
            new_y = random.randint(0, scene.height - 1)
            self.patrol_target = GameObject(x=new_x, y=new_y)
            print(f"{self.name} starts patrolling towards ({new_x}, {new_y}).")

        self.move_towards(self.patrol_target, delta_time)

class Aeron(Player):
    """A valiant knight, a playable character."""
    def __init__(self, name="Aeron", x=0, y=0, z=0):
        super().__init__(name, x, y, z)
        self.symbol = 'A'

class Kane(Enemy):
    """Aeron's brother and rival, an enemy character."""
    def __init__(self, name="Kane", x=0, y=0, z=0):
        super().__init__(name, x, y, z)
        self.symbol = 'K'

# --- Item System ---

class Item(GameObject):
    """Base class for all items (weapons, consumables, armor, etc.)."""
    def __init__(self, name, description, x=0, y=0, z=0):
        # Items are not visible or solid by default, as they are usually in an inventory.
        super().__init__(name=name, x=x, y=y, z=z, visible=False, solid=False)
        self.description = description

    def __str__(self):
        return f"{self.name}: {self.description}"

class Weapon(Item):
    """Represents a weapon that can be equipped."""
    def __init__(self, name, description, damage, weapon_type="Melee"):
        super().__init__(name, description)
        self.damage = damage
        self.weapon_type = weapon_type

    def __str__(self):
        return f"{self.name} (Weapon, {self.damage} DMG): {self.description}"

class Consumable(Item):
    """Represents a consumable item that can be used for an effect."""
    def __init__(self, name, description, effect="None"):
        super().__init__(name, description)
        self.effect = effect
        self.quantity = 1

    def use(self, target):
        """Applies the consumable's effect to the target."""
        print(f"Using {self.name} on {target.name}.")

class HealthPotion(Consumable):
    """A potion that restores health."""
    def __init__(self, name="Health Potion", description="A potion that restores 20 HP.", amount=20):
        super().__init__(name, description, effect=f"Heals {amount} HP")
        self.amount = amount

    def use(self, target):
        """Heals the target."""
        super().use(target)
        target.heal(self.amount)
        print(f"{target.name} restored {self.amount} HP.")

class ManaPotion(Consumable):
    """A potion that restores mana."""
    def __init__(self, name="Mana Potion", description="A potion that restores 30 Mana.", amount=30):
        super().__init__(name, description, effect=f"Restores {amount} Mana")
        self.amount = amount

    def use(self, target):
        """Restores mana to the target."""
        super().use(target)
        if "mana" in target.resources:
            mana = target.resources["mana"]
            mana["current"] = min(mana["max"], mana["current"] + self.amount)
            print(f"{target.name} restored {self.amount} Mana.")
        else:
            print(f"{target.name} has no mana to restore.")

class Armor(Item):
    """A type of item that can be equipped to provide defense."""
    def __init__(self, name, description, defense):
        super().__init__(name, description)
        self.defense = defense

    def __str__(self):
        return f"{self.name} (Armor, +{self.defense} DEF): {self.description}"

class Equipment:
    """Manages a character's equipped items in different slots."""
    def __init__(self, owner):
        self.owner = owner
        self.slots = {
            "weapon": None,
            "shield": None,
            "armor": None
        }

    def equip(self, item):
        """Equips an item into the appropriate slot."""
        if isinstance(item, Weapon):
            self.slots["weapon"] = item
            print(f"{self.owner.name} equips the {item.name}.")
        elif isinstance(item, Armor):
            # For simplicity, we'll assume any armor goes in the 'armor' slot.
            # A more complex system could have slots for head, chest, legs, etc.
            self.slots["armor"] = item
            print(f"{self.owner.name} equips the {item.name}.")
        # We can create a 'Shield' class later if needed.
        else:
            print(f"'{item.name}' is not an equippable item.")

    def get_total_stats(self):
        """Calculates the total stat bonuses from all equipped items."""
        total_damage = self.slots["weapon"].damage if self.slots["weapon"] else 0
        total_defense = self.slots["armor"].defense if self.slots["armor"] else 0
        return {"damage": total_damage, "defense": total_defense}

    def display(self):
        print(f"--- {self.owner.name}'s Equipment ---")
        for slot, item in self.slots.items():
            print(f"- {slot.capitalize()}: {'Empty' if not item else item.name}")
        print("--------------------")

# --- NEW: Generic Ability System ---

class Ability:
    """Base class for all abilities."""
    def __init__(self, name, cost=0, resource="mana"):
        self.name = name
        self.cost = cost
        self.resource = resource

    def can_use(self, caster):
        """Checks if the caster has enough resources to use the ability."""
        if self.resource not in caster.resources:
            print(f"{caster.name} does not have the {self.resource} resource.")
            return False
        if caster.resources[self.resource]["current"] < self.cost:
            print(f"{caster.name} does not have enough {self.resource} to use {self.name}.")
            return False
        return True

    def use(self, caster, target):
        """Uses the ability."""
        if self.can_use(caster):
            caster.resources[self.resource]["current"] -= self.cost
            print(f"{caster.name} uses {self.name} on {target.name}.")
            self.apply_effect(caster, target)

    def apply_effect(self, caster, target):
        """Applies the ability's effect. To be overridden by subclasses."""
        raise NotImplementedError

class Fireball(Ability):
    """A powerful fire-based attack."""
    def __init__(self):
        super().__init__(name="Fireball", cost=20, resource="mana")

    def apply_effect(self, caster, target):
        """Deals fire damage to the target."""
        damage = 15 + int(caster.intelligence * 1.5)
        print(f"A roaring fireball engulfs {target.name} for {damage} fire damage!")
        target.take_damage(damage, "fire")

class PoisonCloud(Ability):
    """Creates a cloud that poisons the target."""
    def __init__(self):
        super().__init__(name="Poison Cloud", cost=15, resource="mana")

    def apply_effect(self, caster, target):
        """Applies a poison status effect to the target."""
        print(f"A toxic cloud envelops {target.name}!")
        target.apply_status_effect("poison", duration=5, potency=3) # 3 damage per turn for 5 turns

# --- 2. NEW Dialogue System Classes ---

class DialogueNode:
    """Represents a single piece of dialogue and potential player choices."""
    def __init__(self, text, character_name="Narrator", options=None):
        self.text = text
        self.character_name = character_name
        self.options = options if options else {}

    def to_dict(self):
        return {
            "text": self.text,
            "character_name": self.character_name,
            "options": self.options,
        }

    @classmethod
    def from_dict(cls, data):
        return cls(
            text=data.get("text"),
            character_name=data.get("character_name", "Narrator"),
            options=data.get("options"),
        )

class DialogueManager:
    """Controls the flow of a single conversation."""
    def __init__(self, start_node_key="start"):
        self.nodes = {}
        self.current_node_key = start_node_key

    def add_node(self, key, node):
        self.nodes[key] = node

    def get_current_node(self):
        return self.nodes.get(self.current_node_key)

    def select_option(self, choice_index):
        node = self.get_current_node()
        if node and node.options:
            option_keys = list(node.options.values())
            if 0 <= choice_index < len(option_keys):
                self.current_node_key = option_keys[choice_index]
                return True
        return False

    def to_dict(self):
        return {
            "nodes": {key: node.to_dict() for key, node in self.nodes.items()},
            "current_node_key": self.current_node_key,
        }

    @classmethod
    def from_dict(cls, data):
        manager = cls(start_node_key=data.get("current_node_key", "start"))
        nodes_data = data.get("nodes", {})
        for key, node_data in nodes_data.items():
            manager.add_node(key, DialogueNode.from_dict(node_data))
        return manager

# --- 3. UPDATING THE CHARACTER AND GAME ENGINE ---

class Character(GameObject):
    def __init__(self, name="Character", x=0, y=0, health=100, state=None):
        super().__init__(name, 'C', x, y, state)
        self.health = health
        self.max_health = health
        self.dialogue = None

    def to_dict(self):
        data = super().to_dict()
        data.update({
            "health": self.health,
            "max_health": self.max_health,
            "dialogue": self.dialogue.to_dict() if self.dialogue else None,
        })
        return data

    @classmethod
    def from_dict(cls, data):
        character = cls(
            name=data.get("name"),
            x=data.get("x"),
            y=data.get("y"),
            health=data.get("health"),
            state=data.get("state")
        )
        character.max_health = data.get("max_health", character.health)
        dialogue_data = data.get("dialogue")
        if dialogue_data:
            character.dialogue = DialogueManager.from_dict(dialogue_data)
        return character

class Anastasia(Character):
    def __init__(self, name="Anastasia", x=0, y=0, health=120, state=None):
        super().__init__(name=name, x=x, y=y, health=health, state=state)
        self.symbol = '@'

class Reverie(Character):
    def __init__(self, name="Reverie", x=0, y=0, health=100, state=None):
        super().__init__(name=name, x=x, y=y, health=health, state=state)
        self.symbol = 'R'

class Scene:
    """Holds all the data for a single game area: map, objects, etc."""
    def __init__(self, name, width=40, height=10):
        self.name = name
        self.width = width
        self.height = height
        self.game_objects = []
        self.player_character = None

    def add_object(self, obj):
        self.game_objects.append(obj)

    def set_player(self, player):
        self.player_character = player
        self.add_object(player)

    def get_object_at(self, x, y):
        for obj in self.game_objects:
            if obj.x == x and obj.y == y:
                return obj
        return None

    def to_dict(self):
        player_name = self.player_character.name if self.player_character else None
        return {
            "name": self.name,
            "width": self.width,
            "height": self.height,
            "game_objects": [obj.to_dict() for obj in self.game_objects],
            "player_character_name": player_name,
        }

    @classmethod
    def from_dict(cls, data):
        scene = cls(name=data["name"], width=data["width"], height=data["height"])
        scene.game_objects = [GameObject.create_from_dict(obj_data) for obj_data in data.get("game_objects", [])]
        player_name = data.get("player_character_name")
        if player_name:
            scene.player_character = next((obj for obj in scene.game_objects if obj.name == player_name), None)
        return scene

class Game:
    def __init__(self, width=40, height=10):
        self.width = width
        self.height = height
        self.turn_taken = False
        self.game_over = False
        self.in_conversation = False
        self.dialogue_manager = None

    def to_dict(self):
        return {
            "width": self.width,
            "height": self.height,
            "game_over": self.game_over,
            "in_conversation": self.in_conversation,
            "dialogue_manager": self.dialogue_manager.to_dict() if self.dialogue_manager else None,
        }

    @classmethod
    def from_dict(cls, data):
        game = cls(width=data["width"], height=data["height"])
        game.game_over = data.get("game_over", False)
        game.in_conversation = data.get("in_conversation", False)
        dialogue_data = data.get("dialogue_manager")
        if dialogue_data:
            game.dialogue_manager = DialogueManager.from_dict(dialogue_data)
        return game

    def handle_input(self, scene_manager):
        """UPDATED to handle dialogue, saving, and loading."""
        player = scene_manager.scene.player_character
        if self.in_conversation:
            choice = input("Choose an option (number): ")
            if choice.isdigit() and self.dialogue_manager.select_option(int(choice) - 1):
                pass
            else:
                print("Invalid choice.")
            self.turn_taken = True
            return

        command = input(f"What will {player.name} do? (attack, equip [item], use [item], examine, status, quit): ").lower().strip()
        parts = command.split()
        action = parts[0] if parts else ""

        if action == "move" and len(parts) > 1:
            direction = parts[1]
            dx, dy = 0, 0
            if direction == "up": dy = -1
            elif direction == "down": dy = 1
            elif direction == "left": dx = -1
            elif direction == "right": dx = 1

            player = scene_manager.scene.player_character
            new_x, new_y = player.x + dx, player.y + dy

            if 0 <= new_x < self.width and 0 <= new_y < self.height:
                target = scene_manager.scene.get_object_at(new_x, new_y)
                if not target:
                    player.x = new_x
                    player.y = new_y
                    self.turn_taken = True
                else:
                    print(f"You can't move there. {target.name} is in the way.")
            else:
                print("You can't move off the map.")

        elif action == "examine" and len(parts) > 1:
            target_name = " ".join(parts[1:])
            target = next((obj for obj in scene_manager.scene.game_objects if isinstance(obj, Interactable) and obj.name.lower() == target_name.lower()), None)
            if target:
                print(f"{target.name}: {target.on_examine()}")
            else:
                print(f"There is no '{target_name}' to examine.")
            self.turn_taken = True

        elif action == "talk" and len(parts) > 1:
            target_name = " ".join(parts[1:])
            target = next((obj for obj in scene_manager.scene.game_objects if obj.name.lower() == target_name.lower()), None)
            if target and isinstance(target, Character) and target.dialogue:
                distance = abs(scene_manager.scene.player_character.x - target.x) + abs(scene_manager.scene.player_character.y - target.y)
                if distance <= 2:
                    self.start_conversation(target.dialogue)
                else:
                    print(f"You are too far away to talk to {target.name}.")
            else:
                # Simple attack (for single-enemy scenes)
                target = next((obj for obj in scene_manager.scene.game_objects if isinstance(obj, Enemy) and obj.health > 0), None)
                if target:
                    player.attack(target)
                else:
                    print("There is no one to attack.")

        elif action == "equip" and len(parts) > 1:
            item_name = " ".join(parts[1:])
            player.equip_item(item_name)

        elif action == "use" and len(parts) > 1:
            item_name = " ".join(parts[1:])
            if not player.use_item(item_name):
                # If it's not an item, try to use it as an ability
                target = next((obj for obj in scene_manager.scene.game_objects if isinstance(obj, Enemy) and obj.health > 0), None)
                if target:
                    player.use_ability(item_name, target)
                else:
                    print("There is no target for this ability.")

        elif action == "examine":
            found_something = False
            for obj in scene_manager.scene.game_objects:
                if isinstance(obj, Interactable) and player.distance_to(obj) < 1.5:
                    print(obj.on_examine())
                    found_something = True
                    break
            if not found_something:
                print("There is nothing nearby to examine.")

        elif action == "status":
            print(f"{player.name} - HP: {player.health}/{player.max_health}, Mana: {int(player.resources['mana']['current'])}/{player.resources['mana']['max']}")
            # Making status more general
            for obj in scene_manager.scene.game_objects:
                if isinstance(obj, Enemy) and obj.health > 0:
                     print(f"{obj.name} - HP: {obj.health}")
            self.turn_taken = True

        elif action == "save":
            filename = parts[1] if len(parts) > 1 else "savegame.json"
            save_game(scene_manager, filename)
            print(f"Game saved to {filename}")
            self.turn_taken = False # Saving does not consume a turn

        elif action == "load":
            filename = parts[1] if len(parts) > 1 else "savegame.json"
            new_manager = load_game(filename)
            if new_manager:
                if scene_manager.__class__ is not new_manager.__class__:
                    print(f"Error: Save is for a different scene type ('{new_manager.__class__.__name__}').")
                else:
                    scene_manager.game = new_manager.game
                    scene_manager.scene = new_manager.scene
                    print("Game loaded successfully.")
            else:
                print("Failed to load game.")
            self.turn_taken = True

        elif action == "quit":
            self.game_over = True
        else:
            print("Unknown command. Try: move [dir], talk [name], examine [name], save/load, quit")


    def start_conversation(self, dialogue_manager):
        """Initiates a conversation."""
        self.in_conversation = True
        self.dialogue_manager = dialogue_manager
        print("A conversation begins.")

    def end_conversation(self):
        """Ends the current conversation."""
        self.in_conversation = False
        self.dialogue_manager = None
        print("The conversation ends.")

    def draw(self, scene):
        """Draws the game state to the console."""
        # Clear screen
        print("\033c", end="")

        print(f"--- {scene.name} ---")

        if self.in_conversation:
            node = self.dialogue_manager.get_current_node()
            if not node:
                self.end_conversation()
                # Fall through to draw the map on the turn the conversation ends
            else:
                print(f"\n--- Conversation with {node.character_name} ---")
                print(f"> \"{node.text}\"")
                if node.options:
                    for i, option_text in enumerate(node.options.keys()):
                        print(f"  {i+1}. {option_text}")
                else:
                    # If there are no options, the conversation ends on the next player input
                    self.end_conversation()
                # Don't draw map while in conversation
                return

        # --- Draw Map ---
        grid = [['.' for _ in range(self.width)] for _ in range(self.height)]
        for obj in sorted(scene.game_objects, key=lambda o: 0 if isinstance(o, Character) else -1):
             if 0 <= obj.x < self.width and 0 <= obj.y < self.height:
                grid[obj.y][obj.x] = obj.symbol

        for row in grid:
            print(" ".join(row))

        # --- Draw Player Status and Message Log ---
        player = scene.player_character
        print("-" * (self.width * 2 - 1))
        print(f"{player.name} | Health: {player.health}/{player.max_health}")
        print("-" * (self.width * 2 - 1))


class SceneManager:
    """Base class for controlling scenes, events, and game logic."""
    def __init__(self, scene, game, setup_scene=True):
        self.scene = scene
        self.game = game
        self.is_running = True
        if setup_scene:
            self.setup()

    def setup(self):
        """Initializes the scene with objects, characters, etc."""
        raise NotImplementedError

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

class AethelgardBattle(SceneManager):
    """A specific scene manager for the Aeron vs. Kane fight."""
    def setup(self):
        """Sets up the characters, items, and quest for this specific battle."""
        # Create characters
        player = Aeron(name="Aeron", x=5, y=5)
        enemy = Kane(name="Kane", x=10, y=5)
        # Let's make Kane a bit tougher for this encounter
        enemy.health = 250
        enemy.attack_damage = 20
        enemy.xp_value = 500 # This would be a new attribute on Enemy

        # Give player items
        player.pickup_item(Weapon("Valiant Sword", "A blade that shines with honor.", 25))
        player.pickup_item(Armor("Aethelgard Plate", "Sturdy plate armor of a royal knight.", 15))

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
        self.scene.set_player(player)
        self.scene.add_object(enemy)
        self.scene.add_object(ancient_statue)
        print("Aethelgard stands silent. Your brother, Kane, awaits.")
        print("You feel the weight of the Aethelgard Plate. Type 'equip Aethelgard Plate' to wear it.")
        if self.game.turn_taken:
            # AI turn logic would go here
            for obj in self.scene.game_objects:
                if isinstance(obj, Enemy):
                    obj.update(0.5, self.scene.player_character, self.scene) # Assuming a fixed delta_time for simplicity
                else:
                    obj.update(self)

        self.update()

# --- 4. NEW Save/Load Functionality ---

def save_game(scene_manager, filename="savegame.json"):
    """Saves the current game state to a file."""
    if not scene_manager:
        print("Cannot save a null scene manager.")
        return
    try:
        # We need to save the state of the Game, the Scene, and the SceneManager's class name
        state = {
            "scene_manager_class": scene_manager.__class__.__name__,
            "game_state": scene_manager.game.to_dict(),
            "scene_state": scene_manager.scene.to_dict(),
        }
        with open(filename, "w") as f:
            json.dump(state, f, indent=4)
    except Exception as e:
        print(f"Error saving game: {e}")

def load_game(filename="savegame.json"):
    """Loads a game state from a file."""
    try:
        with open(filename, "r") as f:
            state = json.load(f)

        # Re-create the Game and Scene objects
        game_state = Game.from_dict(state["game_state"])
        scene_state = Scene.from_dict(state["scene_state"])

        # Re-create the SceneManager
        manager_class_name = state["scene_manager_class"]
        manager_class = globals().get(manager_class_name)
        if not manager_class:
            raise ValueError(f"SceneManager class '{manager_class_name}' not found.")

        # The manager needs the scene and game objects during initialization
        # We pass setup_scene=False to prevent re-populating the loaded scene
        loaded_manager = manager_class(scene_state, game_state, setup_scene=False)
        return loaded_manager
    except FileNotFoundError:
        print(f"Save file not found: {filename}")
        return None
    except Exception as e:
        print(f"Error loading game: {e}")
        return None

# --- 5. SCRIPTING THE ANASTASIA & REVERIE DIALOGUE ---

class FirstMeetingScene(SceneManager):
    """A scene where Anastasia and Reverie meet for the first time."""
    def setup(self):
        player = Anastasia(name="Anastasia", x=5, y=5)
        npc = Reverie(name="Reverie", x=7, y=5)

        # Create the dialogue tree for Reverie
        reverie_dialogue = DialogueManager()
        reverie_dialogue.add_node("start", DialogueNode(
            "Another one drawn by these old stones. You have the look of a believer. Are you one of the ten the prophecy speaks of?",
            "Reverie",
            {"I am. My name is Anastasia.": "anastasia_intro", "Who's asking?": "who_asking"}
        ))
        reverie_dialogue.add_node("anastasia_intro", DialogueNode(
            "Anastasia the Dreamer. I've heard the whispers. They say you're meant to lead us. I remain unconvinced.",
            "Reverie" # Ends conversation
        ))
        reverie_dialogue.add_node("who_asking", DialogueNode(
            "Someone who finds prophecies to be... unreliable. I am Reverie. Now, answer the question.",
            "Reverie",
            {"I am Anastasia. And we need to work together.": "anastasia_intro"}
        ))
        npc.dialogue = reverie_dialogue

        self.scene.set_player(player)
        self.scene.add_object(npc)
        print("You approach a skeptical-looking woman leaning against a monolith.")

# --- 6. RUNNING THE DIALOGUE SCENE ---
if __name__ == "__main__":
    # To start a new game:
    game_engine = Game()
    meeting_scene = Scene("Monolith Clearing")
    meeting_manager = FirstMeetingScene(meeting_scene, game_engine)

    # To load a game instead, you could do:
    # meeting_manager = load_game()
    # if not meeting_manager:
    #     print("Starting a new game because load failed.")
    #     game_engine = Game()
    #     meeting_scene = Scene("Monolith Clearing")
    #     meeting_manager = FirstMeetingScene(meeting_scene, game_engine)

    if meeting_manager:
        meeting_manager.run()
        print("Game over.")