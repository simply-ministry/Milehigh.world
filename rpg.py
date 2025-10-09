import time
import random
import math

class GameObject:
    """
    Base class for all game objects.  Provides fundamental attributes and methods.
    """
    def __init__(self, name="GameObject", x=0, y=0, z=0, health=100, speed=1, visible=True, solid=True, defense=0):
        """
        Constructor for GameObject.
        Args:
            name (str): The name of the object.
            x (float): The x-coordinate of the object's position.
            y (float): The y-coordinate of the object's position.
            z (float): The z-coordinate of the object's position (for 3D).
            health (int): The health of the object.
            speed (float): The speed of the object.
            visible (bool): Whether the object is visible.
            solid (bool): Whether the object is solid (can collide with other objects).
            defense (int): The defense of the object.
        """
        self.name = name
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
        Reduces the object's health after factoring in defense.
        Args:
            damage (int): The amount of incoming damage.
        """
        current_defense = self.defense
        if 'armor_break' in self.status_effects:
            print(f"{self.name} is armor broken! Defense is negated.")
            current_defense = 0

        actual_damage = max(0, damage - current_defense)
        self.health -= actual_damage
        if actual_damage > 0:
            print(f"{self.name} takes {actual_damage} damage.")
        else:
            print(f"{self.name}'s defense holds strong!")

        if self.health <= 0:
            self.die()

    def heal(self, amount):
        """
        Increases the object's health.
        Args:
            amount (int): The amount to heal.
        """
        self.health += amount
        if self.health > 100:  # Assuming max health is 100
            self.health = 100

    def die(self):
        """
        Handles the object's death.  This can be overridden in subclasses.
        """
        self.visible = False
        self.solid = False
        print(f"{self.name} has died.")

    def update(self, delta_time):
        """
        Updates the object's state.  This method is called every frame.
        This is meant to be overridden in subclasses.
        Args:
           delta_time: Time since the last frame in seconds.
        """
        pass

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
            print(f"{self.name} cannot equip {weapon.name}.  It is not a weapon.")

    def update(self, delta_time):
        """
        Updates the player's state, including mana regeneration.
        """
        super().update(delta_time)
        self.update_status_effects(delta_time)
        self.mana += self.mana_regeneration_rate * delta_time
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

class Enemy(GameObject):
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
        print(f"{self.name} attacks {target.name} for {self.attack_damage} damage.")
        target.take_damage(self.attack_damage)

    def update(self, delta_time, player):
        """
        Updates the enemy's state.  This is called every frame.
        Args:
            delta_time (float): Time since last frame.
            player (Player): The player object.
        """
        self.update_status_effects(delta_time)

        if 'sleep' in self.status_effects:
            print(f"{self.name} is asleep and cannot act.")
            return

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

class Weapon(GameObject):
    """
    Represents a weapon.
    """
    def __init__(self, name="Weapon", x=0, y=0, z=0, damage=10, weapon_type="Melee"):
        super().__init__(name=name, x=x, y=y, z=z, visible=False, solid=False)  # Weapons are not solid by default.
        self.damage = damage
        self.weapon_type = weapon_type # e.g., "Melee", "Ranged", "Magic"

class Consumable(GameObject):
    """
    Represents a consumable item.
    """
    def __init__(self, name="Consumable", x=0, y=0, z=0, effect="None"):
        super().__init__(name=name, x=x, y=y, z=z, visible=False, solid=False)
        self.effect = effect
        self.quantity = 1

    def use(self, target):
        """
        Applies the consumable's effect to the target.  Must be overridden.
        Args:
            target (GameObject): The target of the consumable's effect.
        """
        print(f"{self.name} used on {target.name}.  Effect: {self.effect}") # default

class HealthPotion(Consumable):
    def __init__(self, name="Health Potion", x=0, y=0, z=0, amount=20):
        super().__init__(name=name, x=x, y=y, z=z, effect=f"Heals {amount} HP")
        self.amount = amount

    def use(self, target):
        """Heals the target."""
        if isinstance(target, GameObject):
            target.heal(self.amount)
            print(f"{target.name} healed for {self.amount} HP.")
        else:
            print(f"{self.name} cannot be used on {target}.")


class Interactable(GameObject):
    """
    Represents an object in the world that the player can examine for information.
    """
    def __init__(self, name, x, y, description, symbol='?'):
        super().__init__(name=name, x=x, y=y)
        self.description = description
        self.symbol = symbol # The character that will represent it on the map
        self.is_alive = True # To make it drawable in the current draw logic

    def examine(self):
        """Returns the description of the object when examined."""
        return self.description

class ManaPotion(Consumable):
    def __init__(self, name="Mana Potion", x=0, y=0, z=0, amount=30):
        super().__init__(name=name, x=x, y=y, z=z, effect=f"Restores {amount} Mana")
        self.amount = amount

    def use(self, target):
        """Restore mana of the target.  Assumes target has a mana attribute."""
        if isinstance(target, GameObject):
            if hasattr(target, 'mana'):
                target.mana += self.amount
                print(f"{target.name} restored for {self.amount} mana.")
            else:
                print(f"{target.name} does not have mana.")
        else:
            print(f"{self.name} cannot be used on {target}.")

class Game:
    """
    Represents the game engine.  Handles game logic, object management, and the game loop.
    """
    def __init__(self):
        self.objects = []
        self.player = None
        self.running = False
        self.last_time = time.time()
        self.game_time = 0 # Keep track of total game time.
        self.message_log = []

    def log_message(self, message):
        """Adds a message to the game log queue."""
        self.message_log.append(message)
        if len(self.message_log) > 5: # Keep log from getting too long
            self.message_log.pop(0)

    def add_object(self, obj):
        """
        Adds a GameObject to the game.
        Args:
            obj (GameObject): The GameObject to add.
        """
        self.objects.append(obj)
        if isinstance(obj, Player):
            self.player = obj

    def remove_object(self, obj):
        """
        Removes a GameObject from the game.
        Args:
            obj (GameObject): The GameObject to remove.
        """
        self.objects.remove(obj)

    def start(self):
        """
        Starts the game loop.
        """
        if self.player is None:
            print("Cannot start game without a Player.")
            return
        self.running = True
        print("Game started.")
        self.last_time = time.time()  # Initialize last_time here
        while self.running:
            self.game_loop()

    def stop(self):
        """
        Stops the game loop.
        """
        self.running = False
        print("Game stopped.")

    def handle_input(self, scene_manager):
        """
        Handles user input for the current scene.
        """
        player = scene_manager.scene.player_character
        if not player or player.health <= 0:
            return # No input if player is dead

        command = input(f"What will {player.name} do? (attack, use [item], examine, status, quit): ").lower().strip()
        parts = command.split()
        action = parts[0]

        if action == "quit":
            scene_manager.is_running = False
            return

        if action == "attack":
            # In this simple scene, the only target is Kane
            target = next((obj for obj in scene_manager.scene.game_objects if isinstance(obj, Kane)), None)
            if target and target.health > 0:
                player.attack(target)
            else:
                self.log_message("There is no one to attack.")

        elif action == "use" and len(parts) > 1:
            item_name = " ".join(parts[1:])
            player.use_item(item_name)

        elif action == "examine":
            found_something = False
            for obj in scene_manager.scene.game_objects:
                if isinstance(obj, Interactable) and player.distance_to(obj) < 1.5:
                    self.log_message(obj.examine())
                    found_something = True
                    break
            if not found_something:
                self.log_message("There is nothing nearby to examine.")

        elif action == "status":
            self.log_message(f"{player.name} HP: {player.health}/{player.max_health}")
            kane = next((obj for obj in scene_manager.scene.game_objects if isinstance(obj, Kane)), None)
            if kane:
                 self.log_message(f"{kane.name} HP: {kane.health}")

        else:
            self.log_message("Invalid command.")


    def update(self, delta_time):
        """
        Updates the game state. This is called every frame.
        """
        # Update each object based on its type
        for obj in self.objects:
            if isinstance(obj, Enemy):
                obj.update(delta_time, self.player)  # Enemy update requires the player
            else:
                obj.update(delta_time)  # Other objects have a simpler update

        # Example: Check for collisions (very basic)
        for i in range(len(self.objects)):
            for j in range(i + 1, len(self.objects)):
                obj1 = self.objects[i]
                obj2 = self.objects[j]
                if obj1.solid and obj2.solid and obj1.distance_to(obj2) < 1:  # Simple collision check
                    self.handle_collision(obj1, obj2)

    def handle_collision(self, obj1, obj2):
        """
        Handles collisions between two GameObjects.  This is a placeholder.
        Args:
            obj1 (GameObject): The first GameObject.
            obj2 (GameObject): The second GameObject.
        """
        print(f"Collision between {obj1.name} and {obj2.name}")
        # Example:  Make them bounce
        obj1.move(-0.5, 0)
        obj2.move(0.5, 0)

    def draw(self, scene):
        """Draws the scene with a grid, showing objects and player status."""
        # Create the grid
        grid = [['.' for _ in range(scene.width)] for _ in range(scene.height)]

        # Place all game objects on the grid
        for obj in scene.game_objects:
            # Check if the object is within the grid boundaries
            if 0 <= int(obj.x) < scene.width and 0 <= int(obj.y) < scene.height:
                if isinstance(obj, Interactable):
                    grid[int(obj.y)][int(obj.x)] = obj.symbol
                elif isinstance(obj, Player):
                    grid[int(obj.y)][int(obj.x)] = '@' # Player symbol
                elif isinstance(obj, Enemy):
                    grid[int(obj.y)][int(obj.x)] = 'E' # Enemy symbol
                elif obj.visible: # For other visible objects like items
                    grid[int(obj.y)][int(obj.x)] = '*'

        # Clear screen and print
        print("\033[H\033[J") # Clears the console screen
        print(f"--- {scene.name} ---")
        for row in grid:
            print(" ".join(row))
        print("-" * (scene.width * 2))

        # Print Player Status
        player = scene.player_character
        if player:
            print(f"{player.name} | HP: {player.health}/{player.max_health} | Mana: {int(player.mana)}/{player.max_mana}")

        # Print recent messages
        print("Log:")
        for msg in self.message_log:
            print(f"- {msg}")
        self.message_log.clear() # Clear messages after printing


    def game_loop(self):
        """
        The main game loop. Checks for end-game conditions each frame.
        """
        current_time = time.time()
        delta_time = current_time - self.last_time
        self.last_time = current_time
        self.game_time += delta_time

        delta_time = min(delta_time, 0.1)

        self.handle_input(delta_time)
        self.update(delta_time)
        self.draw()

        # --- Check for Game-End Conditions ---
        if self.player.health <= 0:
            print("\n--- GAME OVER ---")
            self.stop()
            return

        # Check if any enemies are still alive (visible)
        if not any(isinstance(obj, Enemy) and obj.visible for obj in self.objects):
            print("\n--- YOU WIN! ---")
            print("All enemies have been defeated.")
            self.stop()
            return

        time.sleep(0.01)

# --- (I will add the character classes Aeron and Kane here in a later step) ---
# For now, let's assume they exist and inherit from Player.
class Aeron(Player):
    pass
class Kane(Enemy): # For simplicity, Kane will be an Enemy for this scene
    pass


# --- 2. NEW Scene Management Classes ---

class Scene:
    """Represents a specific location or encounter in the game."""
    def __init__(self, name, width=40, height=10):
        self.name = name
        self.width = width
        self.height = height
        self.game_objects = []
        self.player_character = None

    def add_object(self, obj):
        # This will now include all objects, not just enemies
        self.game_objects.append(obj)

    def set_player(self, player):
        self.player_character = player
        self.add_object(player) # Player is also a game object

class SceneManager:
    """Controls the setup, execution, and conclusion of a specific scene."""
    def __init__(self, scene, game):
        self.scene = scene
        self.game = game # A reference to the main game engine
        self.is_running = True

    def setup(self):
        """Sets up the initial state of the scene."""
        # This method will be customized for each specific scene
        pass

    def update(self):
        """Runs one tick of the scene's logic and checks for end conditions."""
        # This will also be customized
        pass

    def run(self):
        """The main loop for the scene."""
        self.setup()
        while self.is_running:
            # The game class methods will now need to be adapted
            # to take the scene as an argument
            self.game.draw(self.scene)
            self.game.handle_input(self) # Pass self to handle scene-specific logic
            self.update()

# --- 3. SCRIPTING THE "BATTLE OF AETHELGARD" SCENE ---

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

    def update(self):
        """Handles the AI and checks for victory/defeat conditions."""
        player = self.scene.player_character
        # Find Kane in the scene's game objects
        kane = next((obj for obj in self.scene.game_objects if isinstance(obj, Kane) and obj.name == "Kane"), None)

        if not player or not kane:
            self.is_running = False
            return

        # Simple AI: Kane attacks Aeron if he's alive
        if kane.health > 0 and player.health > 0:
             kane.attack(player)

        # Victory Condition
        if kane.health <= 0:
            self.game.log_message("With a final blow, your brother falls. Aethelgard is safe, but at what cost?")
            # player.journal.update_quest("The Sibling Rivalry", "defeat", "Kane")
            self.is_running = False
            return

        # Defeat Condition
        if player.health <= 0:
            self.game.log_message("You have been defeated. The kingdom is lost.")
            self.is_running = False
            return


def run_game():
    """
    Main function to set up and run the Aethelgard Battle scene.
    """
    print("--- Milehigh.World RPG ---")
    print("--- SCENE: The Battle of Aethelgard ---")

    # 1. Initialize the main game engine
    game = Game()

    # 2. Create the specific scene
    battle_scene = Scene("Aethelgard Bridge")

    # 3. Initialize the scene manager with the scene and game engine
    scene_manager = AethelgardBattle(battle_scene, game)

    # 4. Run the scene
    # The scene_manager.run() method will now control the game loop
    scene_manager.run()

    print("\n--- Scene Concluded ---")


if __name__ == "__main__":
    run_game()