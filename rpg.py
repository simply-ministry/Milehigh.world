import json
import math
import database # Import the new database module

# --- 1. ALL CORE CLASSES (PASTED FROM PREVIOUS STEPS) ---
# (Includes GameObject, Character, Item, etc.)
# ... (For brevity, imagine all previously defined core classes are here) ...

class GameObject:
    """The base class for all objects in the game world."""
    def __init__(self, name="Object", symbol='?', x=0, y=0, z=0, state=None, health=100, speed=1, visible=True, solid=True, defense=0):
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

    def take_damage(self, damage):
        """
        Reduces the object's health after factoring in defense from stats and equipment.
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
        # self.state = state # BUG: 'state' is not defined here. Removed.

    def update_status_effects(self, scene_manager):
        """Updates the duration of all status effects and applies their effects."""
        # Use a copy of keys to allow modification during iteration
        for effect in list(self.status_effects.keys()):
            # The value can be a simple duration (int) or a dict with more info
            if isinstance(self.status_effects[effect], dict):
                # Example: {'vulnerable': {'duration': 2}}
                self.status_effects[effect]['duration'] -= 1
                if self.status_effects[effect]['duration'] <= 0:
                    print(f"{self.name}'s {effect} has worn off.")
                    del self.status_effects[effect]
            else:
                # Example: {'sleep': 6}
                self.status_effects[effect] -= 1
                if self.status_effects[effect] <= 0:
                    print(f"{self.name}'s {effect} has worn off.")
                    del self.status_effects[effect]
        # Apply continuous damage effects after ticking down, so they don't happen on the last turn
        if 'psychic_damage' in self.status_effects:
             damage = 5 # Example damage
             print(f"{self.name} takes {damage} psychic damage.")
             self.take_damage(damage)

        if 'life_drain' in self.status_effects:
            effect_data = self.status_effects['life_drain']
            damage = effect_data['damage_per_turn']
            healer_name = effect_data['heals']
            print(f"{self.name} is drained of {damage} health by {healer_name}.")
            self.take_damage(damage)

            # Find the healer in the scene and heal them
            healer = next((obj for obj in scene_manager.scene.game_objects if obj.name == healer_name), None)
            if healer:
                healed_amount = min(healer.max_health - healer.health, damage)
                healer.health += healed_amount
                if healed_amount > 0:
                    print(f"{healer.name} is healed for {healed_amount} health.")


    def update(self, scene_manager):
        """Placeholder for object-specific logic that runs each turn."""
        self.update_status_effects(scene_manager)


class Character(GameObject):
    """A placeholder class to resolve the NameError.
    This can be fleshed out later if needed."""
    pass


class Interactable(GameObject):
    """Represents objects that can be examined for a description."""
    def __init__(self, name, symbol, x, y, description):
        super().__init__(name, symbol, x, y)
        self.description = description

    def on_examine(self):
        """Returns the description of the object."""
        return self.description

class Character(GameObject):
    """A base class for any entity that can act, fight, and has stats."""
    def __init__(self, name, x=0, y=0, z=0, health=100, speed=1):
        super().__init__(name=name, x=x, y=y, z=z, health=health, speed=speed)
        self.max_health = health

class Player(Character):
    """
    Represents the player character.
    """
    def __init__(self, name="Player", x=0, y=0, z=0):
        super().__init__(name=name, x=x, y=y, z=z, health=100, speed=5)
        self.inventory = []
        self.level = 1
        self.experience = 0
        self.max_health = 100
        self.mana = 100
        self.max_mana = 100
        self.mana_regeneration_rate = 1.5  # Mana per second
        self.strength = 10
        self.dexterity = 10
        self.intelligence = 10
        # --- NEW: Equipment Manager ---
        self.equipment = Equipment(owner=self)

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
        """Finds an item in inventory, equips it, and removes it from inventory."""
        item_to_equip = None
        item_index = -1
        for i, item in enumerate(self.inventory):
            if item.name.lower() == item_name.lower():
                item_to_equip = item
                item_index = i
                break

        if item_to_equip:
            self.equipment.equip(item_to_equip)
            self.inventory.pop(item_index) # Remove from inventory
        else:
            print(f"'{item_name}' not found in inventory.")

    def update(self, scene_manager):
        """
        Updates the player's state, including mana regeneration.
        """
        super().update(scene_manager) # This now handles status effects
        # Turn-based mana regeneration
        self.mana += self.mana_regeneration_rate
        if self.mana > self.max_mana:
            self.mana = self.max_mana

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

    def heal(self, amount):
        """Heals the character for a given amount."""
        self.health += amount
        if self.health > self.max_health:
            self.health = self.max_health
        print(f"{self.name} is healed for {amount} HP.")

# --- Anastasia's Class Implementation ---

class Anastasia(Player):
    """
    Implementation of Anastasia the Dreamer.
    Playstyle: Battlefield controller and disruptor.
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
        self.lucid_dream_duration = 15 # in game ticks/seconds
        self.lucid_dream_timer = 0

    def update(self, scene_manager):
        """Called every game turn to update Anastasia's state."""
        # First, call the Player's update for mana regen and status effects
        super().update(scene_manager)

        # Passively build a small amount of Dream Weave each turn
        self.build_dream_weave(1) # Build 1 Dream Weave per turn

        if self.is_lucid_dream_active:
            self.lucid_dream_timer -= 1 # Decrement by 1 turn
            if self.lucid_dream_timer <= 0:
                self.is_lucid_dream_active = False
                self.lucid_dream_timer = 0
                print("\n-- Anastasia's Lucid Dream fades. The world returns to normal. --\n")

    def build_dream_weave(self, amount):
        """Increases the Dream Weave meter."""
        if not self.is_lucid_dream_active:
            self.dream_weave += amount
            if self.dream_weave > self.max_dream_weave:
                self.dream_weave = self.max_dream_weave

    def activate_lucid_dream(self):
        """Activates the Lucid Dream state if the meter is full."""
        if self.dream_weave >= self.max_dream_weave:
            print("\n** Anastasia activates LUCID DREAM! The battlefield warps! **\n")
            self.is_lucid_dream_active = True
            self.lucid_dream_timer = self.lucid_dream_duration
            self.dream_weave = 0
            return True
        else:
            print("Dream Weave is not full yet!")
            return False

    # --- ABILITIES ---

    def lulling_whisper(self, targets):
        """Puts target(s) to sleep."""
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
                target = targets[0] # Affect only the first target
                target.status_effects['sleep'] = 6
                print(f"{target.name} has fallen asleep.")

        self.build_dream_weave(15)

    def phantasmal_grasp(self, target):
        """Slows a target and deals minor damage over time."""
        cost = 25
        if self.mana < cost:
            print("Not enough mana!")
            return

        self.mana -= cost
        print(f"{self.name} uses Phantasmal Grasp on {target.name}.")

        target.status_effects['slow'] = 8
        target.status_effects['psychic_damage'] = 8 # Represents the DoT effect
        print(f"{target.name} is slowed by shadowy tendrils.")

        if self.is_lucid_dream_active:
            print("The grasp erupts from the target, slowing nearby enemies!")
            # In a real game, you'd find nearby enemies. Here we just simulate it.
            target.status_effects['slow'] += 4

        self.build_dream_weave(15)

    def fleeting_vision(self, allies):
        """Grants evasion and speed to an ally or the whole party."""
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
                ally = allies[0] # Affect only the first ally
                ally.status_effects['evasion'] = 5
                print(f"{ally.name} is granted enhanced evasion!")

    def oneiric_collapse(self, enemies, allies):
        """Ultimate Ability: Pulls the battlefield into the Dreamscape."""
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
            ally.status_effects['empowered'] = 10 # Simulate faster cooldowns
            print(f"{ally.name} feels empowered by the dream!")

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
        self.mana = 150   # Standard mana pool for her basic spells
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
        """
        Casts one of her elemental spells.
        This consumes mana, deals damage to the target, and builds Enigma.
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
        """
        Unleashes her ultimate ability when Enigma is at max.
        Consumes all Enigma for a powerful, random effect.
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

# --- Nyxar's Class Implementation ---

class Nyxar(Player):
    """
    Implementation of Nyxar, the Void-Touched.
    Playstyle: High-risk, high-reward glass cannon who uses his own health
               and a unique resource, Dominion, to fuel his powerful abilities.
    """
    def __init__(self, name="Nyxar", x=0, y=0, z=0):
        super().__init__(name=name, x=x, y=y, z=z)
        self.health = 80  # Lower base health
        self.max_health = 80
        self.mana = 0 # Nyxar does not use Mana
        self.max_mana = 0

        # Unique Mechanic: Dominion
        self.dominion = 0
        self.max_dominion = 100

        # Unique Mechanic: Abyssal Aegis Shield
        self.aegis_shield_hp = 0

    def take_damage(self, damage):
        """
        Nyxar's version of take_damage, which first passes damage to his
        Abyssal Aegis shield if it's active. Also generates Dominion.
        """
        # Generate Dominion when taking damage
        self.gain_dominion(damage // 2)

        if self.aegis_shield_hp > 0:
            absorbed = min(self.aegis_shield_hp, damage)
            self.aegis_shield_hp -= absorbed
            damage -= absorbed
            print(f"Abyssal Aegis absorbs {absorbed} damage!")
            if self.aegis_shield_hp <= 0:
                print("The aegis shatters!")

        # Call the original take_damage for any remaining damage
        if damage > 0:
            super().take_damage(damage)

    def gain_dominion(self, amount):
        """Increases Nyxar's Dominion, capped at max_dominion."""
        self.dominion = min(self.max_dominion, self.dominion + amount)
        print(f"{self.name} gains {amount} Dominion. (Total: {self.dominion}/{self.max_dominion})")

    def spend_health(self, amount):
        """A helper method for abilities that cost health."""
        if self.health > amount:
            self.health -= amount
            print(f"{self.name} sacrifices {amount} health.")
            return True
        else:
            print(f"Not enough health to use this ability!")
            return False

    # --- ABILITIES (Cost Health, Generate/Use Dominion) ---

    def sanguine_strike(self, target):
        """A powerful melee strike that costs health but generates significant Dominion."""
        cost = 10
        if self.spend_health(cost):
            damage = 25 + self.strength # High base damage
            print(f"{self.name} uses Sanguine Strike on {target.name}!")
            target.take_damage(damage)
            self.gain_dominion(20) # High Dominion gain
            return True
        return False

    def abyssal_aegis(self):
        """Creates a temporary shield by sacrificing health."""
        cost = 15
        if self.spend_health(cost):
            shield_amount = 30
            self.aegis_shield_hp += shield_amount
            print(f"{self.name} summons an Abyssal Aegis, gaining {shield_amount} shield HP.")
            self.gain_dominion(10)
            return True
        return False

    def void_lash(self, target):
        """A ranged attack that damages and slows the target."""
        cost = 5
        if self.spend_health(cost):
            damage = 15
            print(f"{self.name} lashes out with void energy at {target.name}!")
            target.take_damage(damage)
            target.status_effects['slow'] = 4 # Apply a 4-turn slow
            print(f"{target.name} is slowed by the void.")
            self.gain_dominion(15)
            return True
        return False

    def reign_of_chaos(self, scene_manager):
        """
        Ultimate Ability: Consumes all Dominion to deal massive damage to all
        enemies in the scene and apply a life-draining effect.
        """
        if self.dominion >= self.max_dominion:
            print(f"\n!!! {self.name} unleashes his ultimate: REIGN OF CHAOS !!!")
            cost = self.dominion
            self.dominion = 0

            # Damage is proportional to the Dominion spent
            damage = cost * 2

            # Find all enemies in the scene and apply the effect
            enemies = [obj for obj in scene_manager.scene.game_objects if isinstance(obj, Enemy)]
            print(f"A wave of chaotic energy erupts, striking all enemies for {damage} damage!")
            for enemy in enemies:
                enemy.take_damage(damage)
                # Apply a life drain effect
                enemy.status_effects['life_drain'] = {'duration': 3, 'damage_per_turn': 10, 'heals': self.name}
                print(f"{enemy.name} is afflicted with life drain!")

            return True
        else:
            print(f"Not enough Dominion for Reign of Chaos. ({self.dominion}/{self.max_dominion})")
            return False


class Enemy(Character):
    """
    Represents an enemy character.
    """
    def __init__(self, name="Enemy", x=0, y=0, z=0, type="Generic"):
        super().__init__(name=name, x=x, y=y, z=z, health=50, speed=2)
        self.type = type  # e.g., "Goblin", "Orc", "Dragon"
        self.attack_damage = 10
        self.aggro_range = 10  # Range at which the enemy will start attacking.
        self.xp_value = 0

    def attack(self, target):
        """
        Attacks another GameObject.
        Args:
            target (GameObject): The target to attack.
        """
        # --- Evasion Check ---
        if 'evasion' in target.status_effects:
            if random.uniform(0, 100) < 50: # 50% chance to miss against evasion
                print(f"{self.name}'s attack was evaded by {target.name}!")
                return

        print(f"{self.name} attacks {target.name} for {self.attack_damage} damage.")
        target.take_damage(self.attack_damage)

    def update(self, scene_manager):
        """
        Updates the enemy's state each turn.
        Args:
            scene_manager (SceneManager): The manager for the current scene.
        """
        super().update(scene_manager) # Handles status effects

        if 'sleep' in self.status_effects:
            print(f"{self.name} is asleep and cannot act.")
            return

        player = scene_manager.scene.player_character
        if self.distance_to(player) < self.aggro_range:
            # Attack the player if close enough.
            if self.distance_to(player) < 1.5:  # Attack range
                self.attack(player)
            else:
                # Move towards the player
                dx = player.x - self.x
                dy = player.y - self.y
                # Basic integer-based movement for a grid
                if abs(dx) > abs(dy):
                    self.move(1 if dx > 0 else -1, 0)
                else:
                    self.move(0, 1 if dy > 0 else -1)


class Troll(Enemy):
    """
    Represents a Troll enemy with health regeneration.
    """
    def __init__(self, name="Troll", x=0, y=0, z=0, type="Troll"):
        super().__init__(name=name, x=x, y=y, z=z, type=type)
        # Trolls are tougher and stronger than generic enemies
        self.health = 80
        self.max_health = 80
        self.attack_damage = 15
        self.regeneration_rate = 5 # Regenerates 5 HP per turn

    def update(self, scene_manager):
        """
        Updates the Troll's state, including its health regeneration.
        """
        super().update(scene_manager) # Run base enemy logic first
        if self.health > 0 and self.health < self.max_health:
            self.health += self.regeneration_rate
            if self.health > self.max_health:
                self.health = self.max_health
            print(f"{self.name} regenerates {self.regeneration_rate} health!")


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
        if hasattr(target, 'mana'):
            target.mana = min(target.max_mana, target.mana + self.amount)
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
        # This is kept ONLY for serializing dialogue to the database, not for general save/load.
        return {
            "nodes": {key: node.to_dict() for key, node in self.nodes.items()},
            "current_node_key": self.current_node_key,
        }

    @classmethod
    def from_dict(cls, data):
        # This is kept ONLY for deserializing dialogue from the database.
        manager = cls(start_node_key=data.get("current_node_key", "start"))
        nodes_data = data.get("nodes", {})
        for key, node_data in nodes_data.items():
            manager.add_node(key, DialogueNode.from_dict(node_data))
        return manager

# --- 3. UPDATING THE CHARACTER AND GAME ENGINE ---
# --- 3. UPDATING THE GAME ENGINE ---

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

class Game:
    def __init__(self, width=40, height=10):
        self.width = width
        self.height = height
        self.message_log = []
        self.turn_taken = False
        self.game_over = False
        self.in_conversation = False
        self.dialogue_manager = None

    def log_message(self, message):
        self.message_log.append(message)
        if len(self.message_log) > 5:
            self.message_log.pop(0)

    def handle_input(self, scene_manager):
        """Handles player input and game commands."""
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
            if direction in ["w", "up"]: dy = -1
            elif direction in ["s", "down"]: dy = 1
            elif direction in ["a", "left"]: dx = -1
            elif direction in ["d", "right"]: dx = 1

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
                target = next((obj for obj in scene_manager.scene.game_objects if isinstance(obj, Interactable) and obj.name.lower() == target_name.lower()), None)
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
                        break # Only examine one nearby thing
                if not found_something:
                    self.log_message("There is nothing nearby to examine.")
            self.turn_taken = True

        elif action == "talk" and len(parts) > 1:
            target_name = " ".join(parts[1:])
            target = next((obj for obj in scene_manager.scene.game_objects if obj.name.lower() == target_name.lower()), None)
            if target and hasattr(target, 'dialogue') and target.dialogue:
                if player.distance_to(target) <= 2:
                    self.start_conversation(target.dialogue)
                else:
                    self.log_message(f"You are too far away to talk to {target.name}.")
            else:
                self.log_message(f"'{target_name}' has nothing to say or isn't here.")
            self.turn_taken = True

        elif action == "attack" and len(parts) > 1:
            target_name = " ".join(parts[1:])
            target = next((obj for obj in scene_manager.scene.game_objects if isinstance(obj, Enemy) and obj.name.lower() == target_name.lower() and obj.health > 0), None)
            if target:
                player.attack(target)
            else:
                self.log_message(f"There is no one to attack named '{target_name}'.")
            self.turn_taken = True

        elif action == "equip" and len(parts) > 1:
            item_name = " ".join(parts[1:])
            player.equip_item(item_name)
            self.turn_taken = True

        elif action == "use" and len(parts) > 1:
            item_name = " ".join(parts[1:])
            player.use_item(item_name)
            self.turn_taken = True

        elif action == "status":
            self.log_message(f"{player.name} - HP: {player.health}/{player.max_health}, Mana: {int(player.mana)}/{player.max_mana}")
            for obj in scene_manager.scene.game_objects:
                if isinstance(obj, Enemy) and obj.health > 0:
                     self.log_message(f"{obj.name} - HP: {obj.health}")
            self.turn_taken = False # Does not consume a turn

        elif action == "quit":
            self.game_over = True
        else:
            self.log_message("Unknown command. Try: move [w/a/s/d], talk [name], examine [name], attack [name], equip [item], use [item], status, quit")

    def start_conversation(self, dialogue_manager):
        """Initiates a conversation."""
        self.in_conversation = True
        self.dialogue_manager = dialogue_manager
        self.log_message("A conversation begins.")

    def end_conversation(self):
        """Ends the current conversation."""
        self.in_conversation = False
        self.dialogue_manager = None
        self.log_message("The conversation ends.")

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
        print("-- Messages --")
        for msg in self.message_log:
            print(f"- {msg}")
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

            # --- AI and World Turn ---
            if self.game.turn_taken and not self.game.game_over:
                # Update all other objects in the scene
                for obj in self.scene.game_objects:
                    if obj is not self.scene.player_character:
                        obj.update(self)

class Aeron(Player):
    """A placeholder class for the character Aeron."""
    def __init__(self, name="Aeron", x=0, y=0, z=0):
        super().__init__(name, x, y, z)
        self.symbol = '@'
        data = database.get_character_data(name)
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
    def __init__(self, name="Kane", x=0, y=0, z=0, type="Boss"):
        super().__init__(name, x, y, z, type)
        self.symbol = 'K'
        data = database.get_character_data(name)
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
        player = Aeron(name="Aeron", x=5, y=5)
        enemy = Kane(name="Kane", x=10, y=5)

        # Give player items
        item_data = database.get_item_data("Valiant Sword")
        if item_data:
            weapon_data = database.get_weapon_data(item_data['item_id'])
            if weapon_data:
                player.pickup_item(Weapon(item_data['name'], item_data['description'], weapon_data['damage']))

        item_data = database.get_item_data("Aethelgard Plate")
        if item_data:
            armor_data = database.get_armor_data(item_data['item_id'])
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
        self.scene.set_player(player)
        self.scene.add_object(enemy)
        self.scene.add_object(ancient_statue)
        self.game.log_message("Aethelgard stands silent. Your brother, Kane, awaits.")
        self.game.log_message("You feel the weight of the Aethelgard Plate. Type 'equip Aethelgard Plate' to wear it.")
        if self.game.turn_taken:
            # AI turn logic would go here
            for obj in self.scene.game_objects:
                obj.update(self)

        self.update() # Check for scene-specific win/loss conditions


class TrollCaveScene(SceneManager):
    """A scene for fighting a troll."""
    def setup(self):
        player = Aeron(name="Hero", x=5, y=5)
        player.pickup_item(Weapon("Mighty Axe", "An axe fit for a troll slayer.", 30))
        player.equip_item("Mighty Axe")

        troll = Troll(name="Cave Troll", x=8, y=5)
        troll.symbol = 'T'

        self.scene.set_player(player)
        self.scene.add_object(troll)
        self.game.log_message("A massive Cave Troll blocks the path!")


# --- Dialogue and Scene Setup ---

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
        self.game.log_message("You approach a skeptical-looking woman leaning against a monolith.")

# --- 6. RUNNING THE DIALOGUE SCENE ---
def main(argv):
    """Main function to run the game."""
    # Initialize the database first
    database.init_db()

    scene_manager = None

    # Check for a command-line argument to load a game
    if len(argv) > 2 and argv[1] == 'load':
        save_name = argv[2]
        print(f"Attempting to load game from slot: {save_name}")
        # In a real scenario, database.load_game would be implemented
        # For this test, we'll assume it returns None if the save doesn't exist
        scene_manager = database.load_game(save_name)
        if not scene_manager:
            print(f"Could not load game '{save_name}'. A new game will be started.")

    # If no scene_manager was loaded (or loading failed), start a new game.
    if not scene_manager:
        print("Starting a new game.")
        game_engine = Game()
        # Default to TrollCaveScene as it was the original default
        troll_scene = Scene("Troll Cave")
        scene_manager = TrollCaveScene(troll_scene, game_engine)

    # Run the game
    if scene_manager:
        scene_manager.run()
        print("Game over.")
    return scene_manager # Return for testing purposes

if __name__ == "__main__":
    import sys
    main(sys.argv)