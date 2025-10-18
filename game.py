"""A Python-based prototype of the game's core mechanics.

This script defines the fundamental classes and logic for a text-based RPG,
including game objects, characters, items, combat, and dialogue systems. It
serves as a testing ground for game mechanics before they are implemented in
the main C# Unity project.
"""

import json
import math
import random
import sys
import time

import database  # Import the new database module


# --- Helper to provide class definitions to the database module ---
def get_class_by_name(class_name):
    """Returns a class object from the global scope by its string name.

    Args:
        class_name (str): The name of the class to retrieve.

    Returns:
        type: The class object corresponding to the given name, or None if not found.
    """
    return globals().get(class_name)


# Inject this function into the database module
database.set_class_loader(get_class_by_name)


class GameObject:
    """The base class for all objects in the game world.

    Attributes:
        name (str): The name of the object.
        symbol (str): The character used to represent the object on the map.
        x (int): The x-coordinate of the object.
        y (int): The y-coordinate of the object.
        z (int): The z-coordinate of the object (for 3D positioning).
        health (int): The current health of the object.
        speed (int): The movement speed of the object.
        visible (bool): Whether the object is visible on the map.
        solid (bool): Whether the object blocks movement.
        defense (int): The base defense value of the object.
        attributes (dict): A dictionary for storing additional attributes.
        status_effects (dict): A dictionary for storing active status effects.
    """

    def __init__(self, name="Object", symbol='?', x=0, y=0, z=0, state=None, health=100, speed=1, visible=True,
                 solid=True, defense=0):
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

    def __repr__(self):
        """Returns a string representation of the GameObject, useful for debugging.

        Returns:
            str: A string representation of the object.
        """
        return f"{self.name}(x={self.x}, y={self.y}, z={self.z}, health={self.health})"

    def distance_to(self, other):
        """Calculates the distance to another GameObject.

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
        """Moves the object by the specified amount.

        Args:
            dx (float): The change in x-coordinate.
            dy (float): The change in y-coordinate.
            dz (float): The change in z-coordinate (for 3D).
        """
        self.x += dx
        self.y += dy
        self.z += dz

    def take_damage(self, damage):
        """Reduces the object's health after factoring in defense.

        This method calculates the actual damage taken by subtracting the
        object's defense from the incoming damage. It also checks for status
        effects like 'armor_break' that may modify defense.

        Args:
            damage (int): The amount of incoming damage.
        """
        total_defense = self.defense
        # Check if the object has an equipment manager
        if hasattr(self, 'equipment'):
            equipped_stats = self.equipment.get_total_stats()
            total_defense += equipped_stats["defense"]

        if 'armor_break' in self.status_effects:
            print(f"{self.name} is armor broken! Defense is negated.")
            total_defense = 0

        actual_damage = max(0, damage - total_defense)
        self.health -= actual_damage
        if actual_damage > 0:
            print(f"{self.name} takes {actual_damage} damage.")
        else:
            print(f"{self.name}'s defense holds strong!")

    def update(self, scene_manager):
        """Placeholder for object-specific logic that runs each turn."""
        self.update_status_effects()

    def apply_status_effect(self, effect_name, duration, **kwargs):
        """Applies a status effect to the object."""
        self.status_effects[effect_name] = {'duration': duration, **kwargs}
        print(f"{self.name} is now affected by {effect_name}.")

    def update_status_effects(self):
        """Updates status effects each turn."""
        effects_to_remove = []
        for effect, data in self.status_effects.items():
            if effect == 'poison':
                potency = data.get('potency', 1)
                self.take_damage(potency)
                print(f"{self.name} takes {potency} damage from poison.")

            data['duration'] -= 1
            if data['duration'] <= 0:
                effects_to_remove.append(effect)

        for effect in effects_to_remove:
            del self.status_effects[effect]
            print(f"{self.name} is no longer {effect}.")
        """Placeholder for object-specific logic that runs each turn.

        Args:
            scene_manager (SceneManager): The scene manager controlling the game loop.
        """
        pass

    def update_status_effects(self, delta_time):
        """Updates the duration of active status effects.

        Args:
            delta_time (float): The time elapsed since the last update.
        """
        pass


class Item(GameObject):
    """Represents items that can be picked up or used.

    This class serves as a base for all items in the game, including weapons,
    armor, and consumables.
    """

    def __init__(self, name="Item", symbol='*', x=0, y=0):
        super().__init__(name, symbol, x, y)


class Interactable(GameObject):
    """Represents objects that can be examined for a description.

    Attributes:
        description (str): The text displayed when the object is examined.
    """

    def __init__(self, name, symbol, x, y, description):
        super().__init__(name, symbol, x, y)
        self.description = description

    def on_examine(self):
        """Returns the description of the object.

        Returns:
            str: The description of the object.
        """
        return self.description


class Player(GameObject):
    """Represents the player character.

    This class manages the player's stats, inventory, equipment, and actions,
    such as attacking, using items, and casting spells.

    Attributes:
        inventory (list): A list of items in the player's inventory.
        inventory_capacity (int): The maximum number of items the player can carry.
        level (int): The player's current level.
        experience (int): The player's current experience points.
        max_health (int): The player's maximum health.
        mana (int): The player's current mana.
        max_mana (int): The player's maximum mana.
        mana_regeneration_rate (float): The amount of mana regenerated per second.
        strength (int): The player's strength stat.
        dexterity (int): The player's dexterity stat.
        intelligence (int): The player's intelligence stat.
        equipment (Equipment): The player's equipment manager.
    """

    def __init__(self, name="Player", x=0, y=0, z=0, inventory_capacity=10):
        super().__init__(name=name, x=x, y=y, z=z, health=100, speed=5)
        self.inventory = []
        self.inventory_capacity = inventory_capacity
        self.level = 1
        self.experience = 0
        self.max_health = 100
        self.mana = 100
        self.max_mana = 100
        self.mana_regeneration_rate = 1.5  # Mana per second
        self.strength = 10
        self.dexterity = 10
        self.intelligence = 10
        self.equipment = Equipment(owner=self)

    def drop_item(self, item_name, scene):
        """Drops an item from the inventory onto the ground.

        Args:
            item_name (str): The name of the item to drop.
            scene (Scene): The scene to drop the item into.
        """
        for i, item in enumerate(self.inventory):
            if item.name.lower() == item_name.lower():
                dropped_item = self.inventory.pop(i)
                dropped_item.x = self.x
                dropped_item.y = self.y
                dropped_item.visible = True
                dropped_item.solid = False
                scene.add_object(dropped_item)
                print(f"{self.name} dropped {dropped_item.name}.")
                return
        print(f"'{item_name}' not found in inventory.")

    def attack(self, target):
        """Attacks another GameObject.

        Damage is calculated based on the player's strength and equipped weapon.
        The attack can miss or be a critical hit based on the player's dexterity.

        Args:
            target (GameObject): The target to attack.
        """
        # --- Evasion Check ---
        if 'evasion' in target.status_effects:
            if random.uniform(0, 100) < 50:  # 50% chance to miss against evasion
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
        """Finds an item in the inventory and equips it.

        Args:
            item_name (str): The name of the item to equip.
        """
        item_to_equip = None
        for item in self.inventory:
            if item.name.lower() == item_name.lower():
                item_to_equip = item
                break

        if item_to_equip:
            self.equipment.equip(item_to_equip)
        else:
            print(f"'{item_name}' not found in inventory.")

    def update(self, scene_manager):
        """
        Updates the player's state, including mana regeneration per turn.
    def update(self, delta_time):
        """Updates the player's state, including mana regeneration.

        Args:
            delta_time (float): The time elapsed since the last update.
        """
        super().update(scene_manager)
        # Regenerate 2 mana per turn
        self.mana = min(self.max_mana, self.mana + 2)

    def pickup_item(self, item):
        """Picks up an item and adds it to the inventory.

        If the item is a consumable and a stack of the same item already
        exists in the inventory, the quantity is increased. Otherwise, the
        item is added as a new entry.

        Args:
            item (Item): The item to pick up.
        """
        if len(self.inventory) >= self.inventory_capacity:
            print("Inventory is full. Cannot pick up.")
            return

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

    def use_item(self, item_name, target):
        """
        Uses an item from the inventory on a target. Handles stackable and non-stackable items.
    def use_item(self, item_name):
        """Uses an item from the inventory.

        If the item is a consumable, its effect is applied, and its quantity
        is decreased. If the quantity reaches zero, the item is removed from
        the inventory.

        Args:
            item_name (str): The name of the item to use.

        Returns:
            bool: True if the item was used successfully, False otherwise.
        """
        item_to_use = None
        item_index = -1
        for i, item in enumerate(self.inventory):
            # Find the item by name, case-insensitive
            if item.name.lower() == item_name.lower():
                item_to_use = item
                item_index = i
                break

        if not item_to_use:
            # Using print here since the Player doesn't have a direct link to the game log
            print(f"'{item_name}' not found in inventory.")
            return False

        if isinstance(item_to_use, Consumable):
            item_to_use.use(target)
            # If the item is stackable, decrement quantity and remove if empty.
            if hasattr(item_to_use, 'quantity'):
                item_to_use.quantity -= 1
                if item_to_use.quantity <= 0:
                    self.inventory.pop(item_index)
                    print(f"Used the last {item_to_use.name}.")
                else:
                    print(f"Used {item_to_use.name}. {item_to_use.quantity} remaining.")
            else:
                # If not stackable (no quantity attribute), just remove it.
                self.inventory.pop(item_index)
                print(f"Used {item_to_use.name}.")
            return True
        else:
            print(f"You can't 'use' a {item_to_use.name} in that way.")
            return False

    def gain_experience(self, amount):
        """Adds experience points to the player and checks for level up.

        Args:
            amount (int): The amount of experience to gain.
        """
        self.experience += amount
        print(f"{self.name} gained {amount} experience.")
        self.check_level_up()

    def check_level_up(self):
        """Checks if the player has enough experience to level up.

        If the player has enough experience, their level is increased, stats
        are improved, and their health is fully restored.
        """
        # Example leveling curve: 100 * level * level
        required_experience = 100 * self.level * self.level
        if self.experience >= required_experience:
            self.level += 1
            self.max_health += 10
            self.health = self.max_health  # Fully heal on level up.
            self.speed *= 1.1  # Increase speed by 10%
            print(f"{self.name} leveled up to level {self.level}!")

    def cast_spell(self, spell_name, target):
        """Casts a spell on a target.

        The power of the spell is influenced by the player's intelligence.

        Args:
            spell_name (str): The name of the spell to cast.
            target (GameObject): The target of the spell.
        """
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

    def heal(self, amount):
        """Restores the player's health.

        Args:
            amount (int): The amount of health to restore.
        """
        self.health = min(self.max_health, self.health + amount)


class Anastasia(Player):
    """Implementation of Anastasia the Dreamer.

    Anastasia is a battlefield controller and disruptor who uses a unique
    resource called Dream Weave to power her abilities. Her ultimate ability,
    Lucid Dream, transforms the battlefield and enhances her other skills.
    """

    def __init__(self, name="Anastasia", x=0, y=0, z=0):
        super().__init__(name=name, x=x, y=y, z=z)
        self.mana = 150
        self.max_mana = 150
        self.health = 100
        self.max_health = 100

        # Unique Mechanic: The Dream Weave
        self.max_dream_weave = 100
        self.dream_weave = 0

        # Lucid Dream State
        self.is_lucid_dream_active = False
        self.lucid_dream_duration = 15  # in game ticks/seconds
        self.lucid_dream_timer = 0

    def update(self, delta_time):
        """Called every game tick to update Anastasia's state.

        This method handles her passive Dream Weave generation and manages
        the duration of her Lucid Dream state.

        Args:
            delta_time (float): The time elapsed since the last update.
        """
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
        """Increases the Dream Weave meter.

        Args:
            amount (float): The amount of Dream Weave to generate.
        """
        if not self.is_lucid_dream_active:
            self.dream_weave += amount
            if self.dream_weave > self.max_dream_weave:
                self.dream_weave = self.max_dream_weave

    def activate_lucid_dream(self):
        """Activates the Lucid Dream state if the meter is full.

        Returns:
            bool: True if Lucid Dream was activated, False otherwise.
        """
        if self.dream_weave >= self.max_dream_weave:
            print("\n** Anastasia activates LUCID DREAM! The battlefield warps! **\n")
            self.is_lucid_dream_active = True
            self.lucid_dream_timer = self.lucid_dream_duration
            self.dream_weave = 0
            return True
        else:
            print("Dream Weave is not full yet!")
            return False

    def lulling_whisper(self, targets):
        """Puts one or more targets to sleep.

        If Lucid Dream is active, this ability affects all targets.

        Args:
            targets (list of GameObject): The potential targets of the spell.
        """
        cost = 20
        if self.mana < cost:
            print("Not enough mana!")
            return

        self.mana -= cost
        print(f"{self.name} uses Lulling Whisper.")

        if self.is_lucid_dream_active:
            print("The whisper becomes a wave, affecting all targets!")
            for target in targets:
                target.status_effects['sleep'] = 6
                print(f"{target.name} has fallen asleep.")
        else:
            if targets:
                target = targets[0]  # Affect only the first target
                target.status_effects['sleep'] = 6
                print(f"{target.name} has fallen asleep.")

        self.build_dream_weave(15)

    def phantasmal_grasp(self, target):
        """Slows a target and deals minor damage over time.

        If Lucid Dream is active, the slow effect is enhanced.

        Args:
            target (GameObject): The target of the spell.
        """
        cost = 25
        if self.mana < cost:
            print("Not enough mana!")
            return

        self.mana -= cost
        print(f"{self.name} uses Phantasmal Grasp on {target.name}.")

        target.status_effects['slow'] = 8
        target.status_effects['psychic_damage'] = 8  # Represents the DoT effect
        print(f"{target.name} is slowed by shadowy tendrils.")

        if self.is_lucid_dream_active:
            print("The grasp erupts from the target, slowing nearby enemies!")
            # In a real game, you'd find nearby enemies. Here we just simulate it.
            target.status_effects['slow'] += 4

        self.build_dream_weave(15)

    def fleeting_vision(self, allies):
        """Grants evasion to one or more allies.

        If Lucid Dream is active, this ability affects all allies.

        Args:
            allies (list of GameObject): The potential targets of the spell.
        """
        cost = 30
        if self.mana < cost:
            print("Not enough mana!")
            return

        self.mana -= cost
        print(f"{self.name} uses Fleeting Vision.")

        if self.is_lucid_dream_active:
            print("The vision is shared with the entire party!")
            for ally in allies:
                ally.status_effects['evasion'] = 5
                print(f"{ally.name} is granted enhanced evasion!")
        else:
            if allies:
                ally = allies[0]  # Affect only the first ally
                ally.status_effects['evasion'] = 5
                print(f"{ally.name} is granted enhanced evasion!")

    def oneiric_collapse(self, enemies, allies):
        """Ultimate Ability: Pulls the battlefield into the Dreamscape.

        This ability can only be used during Lucid Dream. It debuffs all
        enemies and buffs all allies.

        Args:
            enemies (list of GameObject): All enemies on the battlefield.
            allies (list of GameObject): All allies on the battlefield.
        """
        if not self.is_lucid_dream_active:
            print("Must be in Lucid Dream to use Oneiric Collapse!")
            return

        print(f"\n!!! {self.name} unleashes her ultimate: ONEIRIC COLLAPSE !!!")
        print("The area is pulled into the Dreamscape!")

        for enemy in enemies:
            enemy.status_effects['confusion'] = 10
            enemy.status_effects['armor_break'] = 10
            print(f"{enemy.name} is confused and vulnerable!")

        for ally in allies:
            ally.status_effects['empowered'] = 10  # Simulate faster cooldowns
            print(f"{ally.name} feels empowered by the dream!")

        self.is_lucid_dream_active = False
        self.lucid_dream_timer = 0


class Reverie(Player):
    """Represents Reverie, a powerful and unpredictable Mage/Controller.

    Reverie builds a unique resource, Enigma, by casting elemental spells.
    When her Enigma is full, she can unleash a powerful, random ultimate
    attack called Chaos Unleashed.
    """

    def __init__(self, name="Reverie", x=0, y=0, z=0):
        # Initialize the parent Player class with Reverie's stats
        super().__init__(name, x, y, z)
        self.health = 110
        self.max_health = 110
        self.mana = 150  # Standard mana pool for her basic spells
        self.max_mana = 150

        # Reverie's unique resource
        self.enigma = 0
        self.max_enigma = 100

        # Her elemental spells build Enigma
        self.spells = {}
        self.spells["fire_blast"] = {"cost": 30, "damage": 25}
        self.spells["ice_shard"] = {"cost": 20, "damage": 15}
        self.spells["lightning_jolt"] = {"cost": 25, "damage": 20}

    def cast_spell(self, spell_name, target):
        """Casts one of her elemental spells.

        This consumes mana, deals damage to the target, and builds Enigma.

        Args:
            spell_name (str): The name of the spell to cast.
            target (GameObject): The target of the spell.

        Returns:
            bool: True if the spell was cast successfully, False otherwise.
        """
        if spell_name in self.spells:
            spell = self.spells[spell_name]
            if self.mana >= spell["cost"]:
                self.mana -= spell["cost"]
                target.take_damage(spell["damage"])

                # Casting a spell builds Enigma, proportional to mana cost
                enigma_gain = spell["cost"] // 2
                self.enigma = min(self.max_enigma, self.enigma + enigma_gain)

                print(f"{self.name} casts {spell_name} on {target.name}, dealing {spell['damage']} damage.")
                print(f"{self.name} gains {enigma_gain} Enigma. (Total: {self.enigma}/{self.max_enigma})")
                return True
            else:
                print(f"{self.name} does not have enough mana for {spell_name}.")
                return False
        else:
            # This is a bit of a hack to reuse the parent's cast_spell method.
            # In a real refactor, we would make the spell system more robust.
            super().cast_spell(spell_name, target)
            return False

    def chaos_unleashed(self, target):
        """Unleashes her ultimate ability when Enigma is at max.

        This ability consumes all Enigma and produces a powerful, random effect.

        Args:
            target (GameObject): The primary target of the ultimate.

        Returns:
            bool: True if the ultimate was used, False otherwise.
        """
        if self.enigma >= self.max_enigma:
            print(f"{self.name} unleashes CHAOS UNLEASHED!")
            self.enigma = 0  # Reset Enigma after use

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
                self.mana = self.max_mana
            elif effect == "double_damage_debuff":
                print(f"The chaotic energy latches onto {target.name}, making them vulnerable.")
                # The take_damage method already checks for and applies this effect
                if "vulnerable" in target.status_effects:
                    target.status_effects["vulnerable"]["duration"] += 2
                else:
                    target.status_effects["vulnerable"] = {"duration": 2}
            elif effect == "mana_drain":
                drained_mana = 0
                if hasattr(target, 'mana'):
                    drained_mana = target.mana
                    target.mana = 0
                print(f"{self.name} drains all of {target.name}'s {drained_mana} mana!")
                self.mana = min(self.max_mana, self.mana + drained_mana)

            return True
        else:
            print(f"{self.name} needs more Enigma to use Chaos Unleashed. ({self.enigma}/{self.max_enigma})")
            return False


class Enemy(GameObject):
    """Represents an enemy character.

    Enemies have simple AI that causes them to attack the player when they
    are within their aggro range.

    Attributes:
        type (str): The type of the enemy (e.g., "Goblin", "Orc").
        attack_damage (int): The amount of damage the enemy deals.
        aggro_range (int): The range at which the enemy will start attacking.
        xp_value (int): The amount of experience awarded for defeating the enemy.
    """

    def __init__(self, name="Enemy", x=0, y=0, z=0, type="Generic", health=50, speed=2, attack_damage=10, xp_value=0):
        super().__init__(name=name, x=x, y=y, z=z, health=health, speed=speed)
        self.type = type
        self.attack_damage = attack_damage
        self.aggro_range = 10
        self.xp_value = xp_value
        self.state = 'idle'  # Possible states: 'idle', 'chasing', 'attacking'

    def attack(self, target):
        """Attacks another GameObject.

        Args:
            target (GameObject): The target to attack.
        """
        print(f"{self.name} attacks {target.name} for {self.attack_damage} damage.")
        target.take_damage(self.attack_damage)

    def update(self, scene_manager):
        """AI logic for the enemy's turn."""
        # --- Start of Turn ---
        # Check for effects that prevent action BEFORE doing anything else.
        if 'stun' in self.status_effects or 'sleep' in self.status_effects:
            print(f"{self.name} is stunned and cannot act.")
            # Still need to update status effects so stun wears off.
            super().update(scene_manager)
            return

        # --- Action Phase ---
        player = scene_manager.scene.player_character
        if player and player.health > 0:
            # Basic AI: move towards player and attack if adjacent
            if self.distance_to(player) < self.aggro_range:
                if self.distance_to(player) < 1.5:  # Attack if adjacent
                    self.attack(player)
                else:  # Move towards player
                    dx = player.x - self.x
                    dy = player.y - self.y
                    # Move one step in the direction of the player
                    if abs(dx) > abs(dy):
                        self.move(1 if dx > 0 else -1, 0)
                    else:
                        self.move(0, 1 if dy > 0 else -1)

        # --- End of Turn ---
        # Status effects are updated after the action is taken.
        super().update(scene_manager)


# --- Item System ---

class Item(GameObject):
    """Base class for all items (weapons, consumables, armor, etc.)."""
    def update(self, delta_time, player):
        """Updates the enemy's state.

        This method handles the enemy's AI, causing it to move towards and
        attack the player when they are in range.

        Args:
            delta_time (float): The time elapsed since the last update.
            player (Player): The player object.
        """
        self.update_status_effects(delta_time)

        if 'sleep' in self.status_effects:
            print(f"{self.name} is asleep and cannot act.")
            return

        distance_to_player = self.distance_to(player)

        if self.state == 'idle':
            if distance_to_player < self.aggro_range:
                self.state = 'chasing'
        elif self.state == 'chasing':
            if distance_to_player < 1:  # Attack range
                self.state = 'attacking'
            else:
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
                    self.move(dx / distance * current_speed * delta_time, dy / distance * current_speed * delta_time,
                              dz / distance * current_speed * delta_time)
        elif self.state == 'attacking':
            if distance_to_player < 1:
                self.attack(player)
            else:
                self.state = 'chasing'


class Weapon(Item):
    """Represents a weapon that can be equipped.

    Attributes:
        damage (int): The amount of damage the weapon deals.
        weapon_type (str): The type of the weapon (e.g., "Melee", "Ranged").
    """

    def __init__(self, name, description, damage, weapon_type="Melee"):
        super().__init__(name, description)
        self.damage = damage
        self.weapon_type = weapon_type

    def __str__(self):
        return f"{self.name} (Weapon, {self.damage} DMG): {self.description}"


class Consumable(Item):
    """Represents a consumable item that can be used for an effect.

    Attributes:
        effect (str): A description of the item's effect.
        quantity (int): The number of items in the stack.
    """

    def __init__(self, name, description, effect="None"):
        super().__init__(name, description)
        self.effect = effect
        self.quantity = 1

    def use(self, target):
        """Applies the consumable's effect to the target.

        Args:
            target (GameObject): The target of the consumable.
        """
        print(f"Using {self.name} on {target.name}.")


class HealthPotion(Consumable):
    """A potion that restores health.

    Attributes:
        amount (int): The amount of health restored.
    """

    def __init__(self, name="Health Potion", description="A potion that restores 20 HP.", amount=20):
        super().__init__(name, description, effect=f"Heals {amount} HP")
        self.amount = amount

    def use(self, target):
        """Heals the target.

        Args:
            target (GameObject): The target to heal.
        """
        super().use(target)
        target.heal(self.amount)
        print(f"{target.name} restored {self.amount} HP.")


class ManaPotion(Consumable):
    """A potion that restores mana.

    Attributes:
        amount (int): The amount of mana restored.
    """

    def __init__(self, name="Mana Potion", description="A potion that restores 30 Mana.", amount=30):
        super().__init__(name, description, effect=f"Restores {amount} Mana")
        self.amount = amount

    def use(self, target):
        """Restores mana to the target.

        Args:
            target (GameObject): The target to restore mana to.
        """
        super().use(target)
        if hasattr(target, 'mana'):
            target.mana = min(target.max_mana, target.mana + self.amount)
            print(f"{target.name} restored {self.amount} Mana.")
        else:
            print(f"{target.name} has no mana to restore.")


class PoisonDart(Consumable):
    """A dart that poisons the target."""

    def __init__(self, name="Poison Dart", potency=5, duration=4):
        description = f"Poisons a target for {potency} damage over {duration} turns."
        super().__init__(name, description, effect="poison")
        self.potency = potency
        self.duration = duration

    def use(self, target):
        """Poisons the target."""
        super().use(target)
        if isinstance(target, GameObject):
            target.apply_status_effect('poison', self.duration, potency=self.potency)


class Armor(Item):
    """A type of item that can be equipped to provide defense.

    Attributes:
        defense (int): The amount of defense the armor provides.
    """

    def __init__(self, name, description, defense):
        super().__init__(name, description)
        self.defense = defense

    def __str__(self):
        return f"{self.name} (Armor, +{self.defense} DEF): {self.description}"


class Equipment:
    """Manages a character's equipped items in different slots.

    Attributes:
        owner (GameObject): The character who owns the equipment.
        slots (dict): A dictionary representing the equipment slots.
    """

    def __init__(self, owner):
        self.owner = owner
        self.slots = {
            "weapon": None,
            "shield": None,
            "armor": None
        }

    def equip(self, item):
        """Equips an item into the appropriate slot.

        Args:
            item (Item): The item to equip.
        """
        if isinstance(item, Weapon):
            self.slots["weapon"] = item
            print(f"{self.owner.name} equips the {item.name}.")
        elif isinstance(item, Armor):
            # For simplicity, we'll assume any armor goes in the 'armor' slot.
            self.slots["armor"] = item
            print(f"{self.owner.name} equips the {item.name}.")
        else:
            print(f"'{item.name}' is not an equippable item.")

    def get_total_stats(self):
        """Calculates the total stat bonuses from all equipped items.

        Returns:
            dict: A dictionary of total stat bonuses.
        """
        total_damage = self.slots["weapon"].damage if self.slots["weapon"] else 0
        total_defense = self.slots["armor"].defense if self.slots["armor"] else 0
        return {"damage": total_damage, "defense": total_defense}

    def display(self):
        """Prints the character's currently equipped items."""
        print(f"--- {self.owner.name}'s Equipment ---")
        for slot, item in self.slots.items():
            print(f"- {slot.capitalize()}: {'Empty' if not item else item.name}")
        print("--------------------")


class DialogueNode:
    """Represents a single piece of dialogue and potential player choices.

    Attributes:
        text (str): The text of the dialogue.
        character_name (str): The name of the character speaking.
        options (dict): A dictionary of player choices and their target nodes.
    """

    def __init__(self, text, character_name="Narrator", options=None):
        self.text = text
        self.character_name = character_name
        self.options = options if options else {}

    def to_dict(self):
        """Converts the DialogueNode to a dictionary for serialization.

        Returns:
            dict: A dictionary representation of the DialogueNode.
        """
        return {
            "text": self.text,
            "character_name": self.character_name,
            "options": self.options,
        }

    @classmethod
    def from_dict(cls, data):
        """Creates a DialogueNode from a dictionary.

        Args:
            data (dict): A dictionary containing the DialogueNode's data.

        Returns:
            DialogueNode: A new DialogueNode instance.
        """
        return cls(
            text=data.get("text"),
            character_name=data.get("character_name", "Narrator"),
            options=data.get("options"),
        )


class DialogueManager:
    """Controls the flow of a single conversation.

    Attributes:
        nodes (dict): A dictionary of all dialogue nodes in the conversation.
        current_node_key (str): The key of the current dialogue node.
    """

    def __init__(self, start_node_key="start"):
        self.nodes = {}
        self.current_node_key = start_node_key

    def add_node(self, key, node):
        """Adds a dialogue node to the manager.

        Args:
            key (str): The key to identify the node.
            node (DialogueNode): The dialogue node to add.
        """
        self.nodes[key] = node

    def get_current_node(self):
        """Returns the current dialogue node.

        Returns:
            DialogueNode: The current dialogue node.
        """
        return self.nodes.get(self.current_node_key)

    def select_option(self, choice_index):
        """Selects a player choice and advances the conversation.

        Args:
            choice_index (int): The index of the chosen option.

        Returns:
            bool: True if the option was valid, False otherwise.
        """
        node = self.get_current_node()
        if node and node.options:
            option_keys = list(node.options.values())
            if 0 <= choice_index < len(option_keys):
                self.current_node_key = option_keys[choice_index]
                return True
        return False

    def to_dict(self):
        """Converts the DialogueManager to a dictionary for serialization.

        Returns:
            dict: A dictionary representation of the DialogueManager.
        """
        return {
            "nodes": {key: node.to_dict() for key, node in self.nodes.items()},
            "current_node_key": self.current_node_key,
        }

    @classmethod
    def from_dict(cls, data):
        """Creates a DialogueManager from a dictionary.

        Args:
            data (dict): A dictionary containing the DialogueManager's data.

        Returns:
            DialogueManager: A new DialogueManager instance.
        """
        manager = cls(start_node_key=data.get("current_node_key", "start"))
        nodes_data = data.get("nodes", {})
        for key, node_data in nodes_data.items():
            manager.add_node(key, DialogueNode.from_dict(node_data))
        return manager


class Character(GameObject):
    """A base class for all characters, both player and non-player.

    Attributes:
        dialogue (DialogueManager): The character's dialogue tree.
    """

    def __init__(self, name="Character", x=0, y=0, health=100, state=None):
        super().__init__(name, 'C', x, y, state)
        self.health = health
        self.max_health = health
        self.dialogue = None


class AnastasiaCharacter(Character):
    """A simple character class for Anastasia."""

    def __init__(self, name="Anastasia", x=0, y=0, health=120, state=None):
        super().__init__(name=name, x=x, y=y, health=health, state=state)
        self.symbol = '@'


class ReverieCharacter(Character):
    """A simple character class for Reverie."""

    def __init__(self, name="Reverie", x=0, y=0, health=100, state=None):
        super().__init__(name=name, x=x, y=y, health=health, state=state)
        self.symbol = 'R'


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
        self.name = name
        self.width = width
        self.height = height
        self.game_objects = []
        self.player_character = None

    def add_object(self, obj):
        """Adds a GameObject to the scene.

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
        """Gets the object at a specific coordinate.

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
    """The main game engine, responsible for the game loop and input handling.

    Attributes:
        width (int): The width of the game map.
        height (int): The height of the game map.
        message_log (list): A list of recent game messages.
        turn_taken (bool): Whether the player has taken their turn.
        game_over (bool): Whether the game has ended.
        in_conversation (bool): Whether the player is in a conversation.
        dialogue_manager (DialogueManager): The active dialogue manager.
    """

    def __init__(self, width=40, height=10):
        self.width = width
        self.height = height
        self.message_log = []
        self.turn_taken = False
        self.game_over = False
        self.in_conversation = False
        self.dialogue_manager = None

    def log_message(self, message):
        """Adds a message to the game's message log.

        Args:
            message (str): The message to log.
        """
        self.message_log.append(message)
        if len(self.message_log) > 5:
            self.message_log.pop(0)

    def handle_input(self, scene_manager):
        """Handles player input and game commands.

        Args:
            scene_manager (SceneManager): The active scene manager.
        """
        player = scene_manager.scene.player_character
        if self.in_conversation:
            choice = input("Choose an option (number): ")
            if choice.isdigit() and self.dialogue_manager.select_option(int(choice) - 1):
                pass
            else:
                self.log_message("Invalid choice.")
            self.turn_taken = True
            return

        command = input("Action: ").lower().strip()
        parts = command.split()
        action = parts[0] if parts else ""

        if action == "move" and len(parts) > 1:
            direction = parts[1]
            dx, dy = 0, 0
            if direction in ["w", "up"]:
                dy = -1
            elif direction in ["s", "down"]:
                dy = 1
            elif direction in ["a", "left"]:
                dx = -1
            elif direction in ["d", "right"]:
                dx = 1

            new_x, new_y = player.x + dx, player.y + dy

            if 0 <= new_x < self.width and 0 <= new_y < self.height:
                target = scene_manager.scene.get_object_at(new_x, new_y)
                if not target or not getattr(target, 'solid', False):
                    player.x = new_x
                    player.y = new_y
                    self.turn_taken = True
                else:
                    self.log_message(f"You can't move there. {target.name} is in the way.")
            else:
                self.log_message("You can't move off the map.")

        elif action == "examine":
            target_name = " ".join(parts[1:]) if len(parts) > 1 else None
            found_something = False
            if target_name:
                # Examine a specific object by name
                target = next((obj for obj in scene_manager.scene.game_objects if
                               isinstance(obj, Interactable) and obj.name.lower() == target_name.lower()), None)
                if target:
                    self.log_message(f"{target.name}: {target.on_examine()}")
                    found_something = True
                else:
                    self.log_message(f"There is no '{target_name}' to examine.")
            else:
                # Examine nearby objects
                for obj in scene_manager.scene.game_objects:
                    if isinstance(obj, Interactable) and player.distance_to(obj) < 1.5:
                        self.log_message(f"{obj.name}: {obj.on_examine()}")
                        found_something = True
                        break  # Only examine one nearby thing
                if not found_something:
                    self.log_message("There is nothing nearby to examine.")
            self.turn_taken = True

        elif action == "talk" and len(parts) > 1:
            target_name = " ".join(parts[1:])
            target = next(
                (obj for obj in scene_manager.scene.game_objects if obj.name.lower() == target_name.lower()), None)
            if target and isinstance(target, Character) and target.dialogue:
                if player.distance_to(target) <= 2:
                    self.start_conversation(target.dialogue)
                else:
                    self.log_message(f"You are too far away to talk to {target.name}.")
            else:
                self.log_message(f"'{target_name}' has nothing to say or isn't here.")
            self.turn_taken = True

        elif action == "attack" and len(parts) > 1:
            target_name = " ".join(parts[1:])
            target = next((obj for obj in scene_manager.scene.game_objects if
                           isinstance(obj, Enemy) and obj.name.lower() == target_name.lower() and obj.health > 0),
                          None)
            if target:
                player.attack(target)
            else:
                self.log_message(f"There is no one to attack named '{target_name}'.")
            self.turn_taken = True

        elif action == "equip" and len(parts) > 1:
            item_name = " ".join(parts[1:])
            player.equip_item(item_name)
            self.turn_taken = True

        elif action == "use" and len(parts) > 2:
            item_name = parts[1]
            target_name = " ".join(parts[2:])
            target = next((obj for obj in scene_manager.scene.game_objects if obj.name.lower() == target_name.lower()), None)
            if target:
                player.use_item(item_name, target)
                self.turn_taken = True
            else:
                self.log_message(f"Target '{target_name}' not found.")

        elif action == "drop" and len(parts) > 1:
            item_name = " ".join(parts[1:])
            player.drop_item(item_name, scene_manager.scene)
            self.turn_taken = True

        elif action == "status":
            self.log_message(f"{player.name} - HP: {player.health}/{player.max_health}, Mana: {int(player.mana)}/{player.max_mana}")
            for obj in scene_manager.scene.game_objects:
                if isinstance(obj, Enemy) and obj.health > 0:
                    self.log_message(f"{obj.name} - HP: {obj.health}")
            self.turn_taken = False  # Does not consume a turn

        elif action == "save":
            save_name = parts[1] if len(parts) > 1 else "quicksave"
            database.save_game(save_name, scene_manager)
            self.log_message(f"Game saved to slot: {save_name}")
            self.turn_taken = False

        elif action == "load":
            save_name = parts[1] if len(parts) > 1 else "quicksave"
            new_manager = database.load_game(save_name)
            if new_manager:
                scene_manager.game = new_manager.game
                scene_manager.scene = new_manager.scene
                self.log_message(f"Game loaded from slot: {save_name}")
            else:
                self.log_message(f"Failed to load game from slot: {save_name}")
            self.turn_taken = True

        elif action == "quit":
            self.game_over = True
        elif action == "pause":
            self.log_message("Game paused. Press Enter to continue...")
            self.draw(scene_manager.scene)
            input()
            self.turn_taken = False
        else:
            self.log_message(
                "Unknown command. Try: move [w/a/s/d], talk [name], examine [name], attack [name], equip [item], use [item], status, save/load, pause, quit")

    def start_conversation(self, dialogue_manager):
        """Initiates a conversation.

        Args:
            dialogue_manager (DialogueManager): The dialogue manager for the conversation.
        """
        self.in_conversation = True
        self.dialogue_manager = dialogue_manager
        self.log_message("A conversation begins.")

    def end_conversation(self):
        """Ends the current conversation."""
        self.in_conversation = False
        self.dialogue_manager = None
        self.log_message("The conversation ends.")

    def draw(self, scene):
        """Draws the game state to the console.

        Args:
            scene (Scene): The scene to draw.
        """
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
                        print(f"  {i + 1}. {option_text}")
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
        print("-- Messages --")
        for msg in self.message_log:
            print(f"- {msg}")
        print("-" * (self.width * 2 - 1))


class SceneManager:
    """Base class for controlling scenes, events, and game logic.

    Attributes:
        scene (Scene): The scene being managed.
        game (Game): The main game engine.
        is_running (bool): Whether the scene is currently running.
    """

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


class Aeron(Player):
    """A placeholder class for the character Aeron."""

    def __init__(self, name="Aeron", x=0, y=0, z=0):
        super().__init__(name, x, y, z)
        self.symbol = '@'


class Kane(Enemy):
    """A placeholder class for the enemy Kane."""



    def __init__(self, name="Kane", x=0, y=0, z=0, type="Boss"):
        super().__init__(name, x, y, z, type)
        self.symbol = 'K'


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
        enemy.xp_value = 500  # This would be a new attribute on Enemy

        # Give player items
        player.pickup_item(Weapon("Valiant Sword", "A blade that shines with honor.", 25))
        player.pickup_item(Armor("Aethelgard Plate", "Sturdy plate armor of a royal knight.", 15))
        player.pickup_item(PoisonDart(potency=5, duration=4))


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
        self.game.log_message("Aethelgard stands silent. Your brother, Kane, awaits.")
        self.game.log_message("You feel the weight of the Aethelgard Plate. Type 'equip Aethelgard Plate' to wear it.")

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
                    obj.update(self)

        self.update()  # Check for scene-specific win/loss conditions


class FirstMeetingScene(SceneManager):
    """A scene that sets up the game from the loaded JSON data."""

    def setup(self):
        """Initializes the scene with objects, characters, etc. from game_data."""
        # Create player from game_data
        player_data = game_data['player']
        player = Player(
            name=player_data['name'],
            x=player_data['x'],
            y=player_data['y'],
            inventory_capacity=player_data['inventory_capacity']
        )
        self.scene.set_player(player)

        # Create enemies from game_data
        for enemy_data in game_data['enemies']:
            enemy = Enemy(
                name=enemy_data['name'],
                x=enemy_data['x'],
                y=enemy_data['y'],
                type=enemy_data['type'],
                health=enemy_data['health'],
                speed=enemy_data['speed'],
                attack_damage=enemy_data['attack_damage'],
                xp_value=enemy_data['xp_value']
            )
            self.scene.add_object(enemy)

        # Create a weapon and place it in the scene
        weapon_data = game_data['items']['weapons'][0]
        weapon = Weapon(
            name=weapon_data['name'],
            description=weapon_data['description'],
            damage=weapon_data['damage'],
            weapon_type=weapon_data['weapon_type']
        )
        weapon.x = 2
        weapon.y = 2
        self.scene.add_object(weapon)

        # Create a health potion and place it in the scene
        potion_data = game_data['items']['consumables']['health_potions'][0]
        potion = HealthPotion(
            name=potion_data['name'],
            description=potion_data['description'],
            amount=potion_data['amount']
        )
        potion.x = 3
        potion.y = 3
        self.scene.add_object(potion)

        self.game.log_message("Welcome to the game! An adventure awaits.")
        self.game.log_message("You see a sword and a potion nearby. Try 'move d' then 'pickup sword'.")


def main():
    """Sets up and runs the game."""
if __name__ == "__main__":
    # Load game data from JSON file
    with open("game_data.json", "r") as f:
        game_data = json.load(f)

    # Initialize the database first
    database.init_db()

    # --- Game Start ---
    # Check for a command-line argument to load a game
    if len(sys.argv) > 2 and sys.argv[1] == 'load':
        save_name = sys.argv[2]
        print(f"Attempting to load game from slot: {save_name}")
        scene_manager = database.load_game(save_name)
        if not scene_manager:
            print(f"Could not load '{save_name}'. Starting a new game.")
            # Fallback to new game if load fails
            game_engine = Game()
            battle_scene = Scene("Aethelgard Battle")
            scene_manager = AethelgardBattle(battle_scene, game_engine)
            scene = Scene("Monolith Clearing")
            scene_manager = FirstMeetingScene(scene, game_engine)
    else:
        # Start a new game by default
        print("Starting a new game.")
        game_engine = Game()
        battle_scene = Scene("Aethelgard Battle")
        scene_manager = AethelgardBattle(battle_scene, game_engine)

    if scene_manager:
        scene_manager.run()
        print("Game over.")


if __name__ == "__main__":
    main()
        scene = Scene("Monolith Clearing")
        scene_manager = FirstMeetingScene(scene, game_engine)

    if scene_manager:
        scene_manager.run()
        print("Game over.")
