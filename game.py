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
        self.strength = 10
        self.dexterity = 10
        self.intelligence = 10
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
        actual_damage = max(0, damage - self.defense)
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
        self.inventory = []
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
            print(f"{self.name} cannot equip {weapon.name}.  It is not a weapon.")

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

    def update(self):
        """
        The character update method. It calls the parent `Player` update
        with a delta_time of 0 to prevent unintended real-time effects
        for turn-based characters.
        """
        super().update(0)


class Enemy(GameObject):
    """
    Represents an enemy character.
    """
    def __init__(self, name="Enemy", x=0, y=0, z=0, type="Generic", health=50):
        super().__init__(name=name, x=x, y=y, z=z, health=health, speed=2)
        self.type = type  # e.g., "Goblin", "Orc", "Dragon"
        self.attack_damage = 10
        self.aggro_range = 10  # Range at which the enemy will start attacking.

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

class ManaPotion(Consumable):
    def __init__(self, name="Mana Potion", x=0, y=0, z=0, amount=30):
        super().__init__(name=name, x=x, y=y, z=z, effect=f"Restores {amount} Mana")
        self.amount = amount

    def use(self, target):
        """Restore mana of the target.  Assumes target has a mana attribute."""
        if hasattr(target, 'mana'):
            target.mana += self.amount
            print(f"{target.name} restored for {self.amount} mana.")
        else:
            print(f"{target.name} does not have mana.")

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

    def handle_input(self, delta_time):
        """
        Handles user input.  This is a placeholder and should be implemented
        using a specific input library (e.g., pygame, keyboard).

        Args:
            delta_time (float): Time since last frame.
        """
        # Example: Move player with arrow keys (using a hypothetical keyboard library)
        # if keyboard.is_pressed('up'):
        #     self.player.move(0, -self.player.speed * delta_time)
        # if keyboard.is_pressed('down'):
        #     self.player.move(0, self.player.speed * delta_time)
        # if keyboard.is_pressed('left'):
        #     self.player.move(-self.player.speed * delta_time, 0)
        # if keyboard.is_pressed('right'):
        #     self.player.move(self.player.speed * delta_time, 0)
        pass #  Do nothing for now.

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

    def draw(self):
        """
        Draws the game objects.  This is a placeholder and should be implemented
        using a specific graphics library (e.g., pygame, OpenGL).
        """
        for obj in self.objects:
            obj.draw()

    def game_loop(self):
        """
        The main game loop. Checks for end-game conditions each frame.
        """
        current_time = time.time()
        delta_time = current_time - self.last_time
        self.last_time = current_time
        self.game_time += delta_time

        # Cap delta_time to prevent issues with very large time steps (e.g., if the game freezes)
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
    sword = Weapon(name="Greatsword", damage=18)
    health_potion1 = HealthPotion(name="Health Potion", amount=40)
    health_potion2 = HealthPotion(name="Health Potion", amount=40) # A second potion to test stacking
    mana_potion = ManaPotion(name="Mana Potion", amount=50)

    # Player finds the items
    player.pickup_item(sword)
    player.equip_weapon(sword)
    player.pickup_item(health_potion1)
    player.pickup_item(health_potion2) # This should stack
    player.pickup_item(mana_potion)

    # --- 4. Demonstrate Initial Actions ---
    print("\n--- Initial Actions ---")
    print(f"Initial state: Player HP: {player.health}, Mana: {player.mana}")

    # Use a potion
    player.use_item("Health Potion")

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

    def __init__(self, name="Sky.ix", x=0, y=0, z=0):
        # Initialize the parent Player class with Sky.ix's specific stats
        super().__init__(name, x, y, z)
        self.health = 120  # Overriding the default health
        self.max_health = 120
        self.mana = 150    # We'll use 'mana' from the Player class to represent 'VoidEnergy'
        self.max_mana = 150
        self.active_drones = 0

        # Add her unique abilities as spells she can cast
        self.spells = {"void_tech": {"cost": 40, "damage": 50}, "energy_blast": {"cost": 10, "damage": 20}}
        print(f"'{self.name}' has entered the world, crackling with Void energy.")

    def __str__(self):
        # Override the string representation to include her unique resource
        player_info = super().__str__()
        return f"{player_info} | Drones: {self.active_drones}"

    def deploy_drone(self, drone_type="Attack"):
        """
        Deploys a drone to assist in combat.
        """
        cost = 25
        if self.mana >= cost:
            self.mana -= cost
            self.active_drones += 1
            print(f"{self.name} deploys an {drone_type} drone! ({self.active_drones} active). ({self.mana}/{self.max_mana} Void Energy)")
        else:
            print(f"{self.name} lacks the Void Energy to deploy a drone.")

    def cast_spell(self, spell_name, target):
        """
        Sky.ix's custom spell casting for her Void-based abilities.
        """
        if spell_name in self.spells:
            spell = self.spells[spell_name]
            cost = spell["cost"]
            if self.mana >= cost:
                self.mana -= cost
                damage = spell["damage"]
                print(f"{self.name} unleashes {spell_name} on {target.name} for {damage} damage! ({self.mana}/{self.max_mana} Void Energy)")
                target.take_damage(damage)
            else:
                print(f"{self.name} lacks the Void Energy to cast {spell_name}.")
        else:
            # Fallback to the parent's cast_spell method if it's a general spell
            super().cast_spell(spell_name, target)


class Anastasia(Player):
    """
    Represents Anastasia the Dreamer, a support mage who shapes reality.
    Inherits from the Player class, with a focus on healing and control abilities.
    """

    def __init__(self, name="Anastasia", x=0, y=0, z=0):
        # Initialize the parent Player class with Anastasia's specific stats
        super().__init__(name, x, y, z)
        self.health = 90  # As a mage, slightly less durable
        self.max_health = 90
        self.mana = 200   # Represents her 'Dream Energy'
        self.max_mana = 200
        self.visions = [] # A list to store her prophetic visions

        # Add her spells. Note that "Dream Weaving" is implemented as a custom method
        # because of its dual nature.
        self.spells = {"somnus_aura": {"cost": 50}}
        print(f"'{self.name}' has awoken, her mind filled with prophetic dreams.")

    def __str__(self):
        # Add her unique properties to the string output
        player_info = super().__str__()
        return f"{player_info} | Visions: {len(self.visions)}"

    def weave_dream(self, target, is_healing=True):
        """
        A versatile ability that can either heal an ally or create a debuffing
        illusion on an enemy.
        """
        cost = 30
        if self.mana >= cost:
            self.mana -= cost
            if is_healing:
                # Heal an ally
                target.heal(25)
                print(f"{self.name} weaves a soothing dream, healing {target.name} for 25 health. ({self.mana}/{self.max_mana} Dream Energy)")
            else:
                # Harm an enemy (represented as direct damage for this example)
                target.take_damage(20)
                print(f"{self.name} weaves a nightmare, inflicting 20 psychic damage on {target.name}. ({self.mana}/{self.max_mana} Dream Energy)")
        else:
            print(f"{self.name} lacks the Dream Energy to weave the dream.")

    def glimpse_future(self):
        """
        A narrative/utility skill that reveals information.
        """
        # In a real game, this might reveal an enemy's weakness or a hidden path.
        new_vision = "A vision reveals the enemy's next move!"
        self.visions.append(new_vision)
        print(f"A prophetic vision flashes in {self.name}'s mind: '{new_vision}'")


class Micah(Player):
    """
    Represents Micah the Unbreakable, a formidable tank and earth-shaper.
    Inherits from the Player class, with mechanics focused on defense and a
    unique resource, Fortitude, gained by taking damage.
    """

    def __init__(self, name="Micah", x=0, y=0, z=0):
        # Initialize the parent Player class with Micah's tanky stats
        super().__init__(name, x, y, z)
        self.health = 200  # High health pool for a tank
        self.max_health = 200
        self.mana = 0      # He doesn't use mana; his resource is Fortitude
        self.max_mana = 0

        # Micah's unique properties
        self.fortitude = 0
        self.max_fortitude = 100
        self.is_adamantine_skin_active = False

        # We'll implement his abilities as custom methods
        self.spells = {} # Clear the default spells
        print(f"'{self.name}' stands firm, an unbreakable wall.")

    def __str__(self):
        # Add his unique resource to the string output
        player_info = super().__str__()
        skin_status = "Active" if self.is_adamantine_skin_active else "Inactive"
        return f"{player_info} | Fortitude: {self.fortitude}/{self.max_fortitude} | Adamantine Skin: {skin_status}"

    def take_damage(self, damage):
        """
        Micah takes damage but also gains Fortitude.
        """
        # If Adamantine Skin is active, reduce damage taken
        if self.is_adamantine_skin_active:
            damage_reduction = 0.5 # 50% damage reduction
            damage = int(damage * (1-damage_reduction))
            print(f"{self.name}'s Adamantine Skin reduces the damage!")

        super().take_damage(damage)
        # Gain fortitude equal to 100% of damage taken
        fortitude_gain = int(damage * 1.0)
        self.fortitude = min(self.max_fortitude, self.fortitude + fortitude_gain)
        print(f"{self.name} gains {fortitude_gain} Fortitude from the blow!")


    def activate_adamantine_skin(self):
        """
        Activates a defensive stance that reduces incoming damage but consumes
        Fortitude over time.
        """
        cost = 20
        if self.fortitude >= cost:
            self.fortitude -= cost
            self.is_adamantine_skin_active = True
            print(f"{self.name} activates Adamantine Skin, hardening his defenses!")
        else:
            print(f"{self.name} doesn't have enough Fortitude to activate Adamantine Skin.")

    def earthen_smash(self, target):
        """
        An attack that consumes Fortitude to deal damage.
        """
        cost = 30
        if self.fortitude >= cost:
            self.fortitude -= cost
            damage = 40
            print(f"{self.name} smashes {target.name} with the force of the earth, dealing {damage} damage!")
            target.take_damage(damage)
        else:
            print(f"{self.name} lacks the Fortitude for an Earthen Smash.")


class Cyrus(Enemy):
    """
    Represents Cyrus, the tyrannical ruler from another dimension and father of Cirrus.
    He is the primary antagonist who initiates the invasion of Mîlēhîgh.wørld.
    """
    def __init__(self, name="Cyrus", x=0, y=0):
        # As a final boss, his health is immense.
        super().__init__(name=name, x=x, y=y, health=300)
        self.max_health = 300

        self.son_id = None # Would be set to Cirrus's unique ID
        self.power_source = "The Onalym Nexus"
        self.max_tyranny = 100
        # His power is relentless and doesn't need to be generated.
        self.tyranny = self.max_tyranny
        self.attack_damage = 25 # Cyrus is a powerful foe

    def __str__(self):
        """String representation of Cyrus's status."""
        return (f"{self.name} | Health: {self.health}/{self.max_health} | "
                f"Tyranny: {self.tyranny}/{self.max_tyranny}")

    def worldbreaker_strike(self, target):
        """
        An ability that shatters the defenses of a target, representing his power to break worlds.
        """
        print(f"{self.name} strikes with the force of a collapsing dimension, shattering {target.name}'s defenses!")
        # In-game logic would deal heavy damage and apply a significant defense-reduction debuff.
        target.take_damage(75) # High base damage

    def dimensional_rift(self):
        """
        Tears open an unstable, damaging rift on the battlefield.
        """
        cost = 40
        if self.tyranny >= cost:
            # While his resource is always full, abilities still have costs and would likely
            # have cooldowns in a real game to prevent spamming.
            self.tyranny -= cost
            print(f"{self.name} tears open a dimensional rift on the battlefield!")
            # In-game logic to create a hazardous area (AoE).
            # This is a good place to simulate a cooldown by not resetting tyranny immediately.
        else:
            print(f"{self.name} is still recovering his power.")

    def onalym_purge(self, targets_in_line):
        """
        An ultimate ability that channels the Onalym Nexus to unleash a devastating beam.
        """
        cost = 100
        if self.tyranny >= cost:
            self.tyranny = 0 # Consumes all power, initiating a long cooldown.
            print(f"{self.name} channels the full power of the Onalym Nexus, unleashing a beam of pure destruction!")
            for target in targets_in_line:
                 print(f"...The beam obliterates everything in its path, striking {target.name}!")
                 # ... logic for extremely high, likely lethal, damage ...
                 target.take_damage(200)
        else:
            print(f"{self.name} has not gathered enough power for the Onalym Purge.")
class Aeron(Player):
    """
    Represents Aeron, a noble warrior of Aethelgard and one of the Ɲōvəmîŋāđ.
    His relationship with his brother, Kane, is a central conflict in his story.
    """
    def __init__(self, name="Aeron", x=0, y=0):
        # Starts in a defensive 'Unyielding' stance
        super().__init__(name, x, y)
        self.health = 180
        self.max_health = 180
        self.state = "Unyielding" # Using state to manage stance

        self.rival_brother_id = None # Would be set to Kane's unique ID
        self.max_resolve = 100
        self.resolve = 0

    @property
    def stance(self):
        """A property to get his current combat stance from the state."""
        return self.state

    def __str__(self):
        """String representation of Aeron's status."""
        return (f"{self.name} | Health: {self.health}/{self.max_health} | "
                f"Resolve: {self.resolve}/{self.max_resolve} | "
                f"Stance: {self.stance}")

    def switch_stance(self):
        """
        Switches Aeron's combat stance, changing the effect of his other abilities.
        """
        if self.state == "Unyielding":
            self.state = "Commanding"
            print(f"{self.name} switches to a Commanding stance, ready to lead the attack!")
        else:
            self.state = "Unyielding"
            print(f"{self.name} switches to an Unyielding stance, focusing on defense!")

    def valiant_strike(self, target):
        """
        A basic melee attack that builds Resolve.
        """
        # For simplicity, we'll use the base attack method and then add resolve
        super().attack(target)
        resolve_gained = 10
        self.resolve = min(self.max_resolve, self.resolve + resolve_gained)
        print(f"{self.name} strikes {target.name} with valor! (+{resolve_gained} Resolve)")

    def lions_roar(self, allies):
        """
        A powerful ability whose effect changes based on Aeron's current stance.
        """
        cost = 50
        if self.resolve >= cost:
            self.resolve -= cost
            print(f"{self.name} lets out a mighty roar!")

            if self.stance == "Unyielding":
                # In defensive stance, the roar grants a defensive buff to allies.
                print("...His roar inspires resilience, granting a defensive shield to his allies!")
                for ally in allies:
                    # Let's simulate a shield by giving temporary bonus defense
                    ally.defense += 10
                    print(f"{ally.name}'s defense was boosted!")
            elif self.stance == "Commanding":
                # In offensive stance, the roar grants an offensive buff.
                print("...His roar ignites courage, boosting the attack power of his allies!")
                for ally in allies:
                    # Let's simulate a damage boost by increasing strength
                    ally.strength += 10
                    print(f"{ally.name}'s strength was boosted!")
        else:
            print(f"{self.name} does not have enough Resolve.")

    def skyfall_charge(self, target):
        """
        An ultimate ability representing a diving attack, leveraging his winged nature.
        """
        cost = 90
        if self.resolve >= cost:
            self.resolve -= cost
            print(f"{self.name} soars into the sky and dives down upon {target.name} in a devastating charge!")
            # High single-target damage
            target.take_damage(150)
        else:
            print(f"{self.name} lacks the Resolve for this attack.")

# We assume the Character and GameObject classes from the previous steps exist.
import math

class Nyxar(Character):
    """
    Represents Nyxar, the ruler of The Shadow Dominion and the original invader
    responsible for the fragmentation of Mîlēhîgh.wørld. He is a primordial force
    of darkness, seeking to extend his control over all realities.
    """
    def __init__(self, name="Nyxar, Ruler of The Shadow Dominion", x=0, y=0):
        # As the ultimate antagonist, his power level is immense.
        super().__init__(name, x, y, health=1000)

        self.realm = "The Shadow Dominion"
        self.max_dominion = 100
        self.dominion = 0
        self.tethered_enemies = [] # Keep track of who is tethered

    def __str__(self):
        """String representation of Nyxar's status."""
        return (f"{self.name} | Health: {self.health}/{self.max_health} | "
                f"Dominion: {self.dominion}/{self.max_dominion}")

    def update(self):
        """
        An update method called each game tick.
        Used here to passively generate Dominion based on tethered enemies.
        """
        # His Dominion grows for each enemy currently ensnared.
        dominion_gain = 5 * len(self.tethered_enemies)
        self.dominion = min(self.max_dominion, self.dominion + dominion_gain)
        super().update()

    def shadow_tether(self, target):
        """
        Tethers a target with living shadows, slowing them and dealing damage over time.
        Each tethered enemy increases Dominion generation.
        """
        print(f"{self.name} extends a tendril of living shadow, tethering {target.name}!")
        if target not in self.tethered_enemies:
            self.tethered_enemies.append(target)
        # In-game logic would apply a "slow" and "damage-over-time" debuff.

    def create_umbral_clone(self, target):
        """
        Consumes Dominion to create a perfect clone of an enemy out of shadow.
        """
        cost = 60
        if self.dominion >= cost:
            self.dominion -= cost
            print(f"{self.name} spends Dominion to weave a perfect shadow clone of {target.name}!")
            # In-game logic to spawn a temporary, hostile copy of the target character.
        else:
            print(f"{self.name} lacks the Dominion to create a clone.")

    def worldless_chasm(self):
        """
        The ultimate ability. Plunges the battlefield into absolute darkness,
        blinding enemies and empowering Nyxar.
        """
        cost = 100
        if self.dominion >= cost:
            self.dominion = 0
            print(f"{self.name} unleashes his ultimate power, plunging the world into a Worldless Chasm!")
            print("...All light is extinguished, and only the shadows remain!")
            # In-game logic for a powerful, battlefield-wide debuff that severely hinders heroes
            # while providing a massive buff to Nyxar for a limited time.
        else:
            print(f"{self.name} has not established enough Dominion to end the world.")
class Zaia(Player):
    """
    Represents Zaia, a swift and lethal member of the Ɲōvəmîŋāđ.
    She operates as a Rogue/Assassin, specializing in stealth, mobility, and high-precision strikes.
    """
    def __init__(self, name="Zaia", x=0, y=0, z=0):
        super().__init__(name, x, y, z)
        self.health = 130
        self.max_health = 130
        self.aeron_companion_id = None # Would be set to Aeron's unique ID
        self.max_momentum = 100
        self.momentum = 0
        self.is_stealthed = False

    def __str__(self):
        """String representation of Zaia's status."""
        stealth_status = "Active" if self.is_stealthed else "Inactive"
        return (f"{self.name} | Health: {self.health}/{self.max_health} | "
                f"Momentum: {self.momentum}/{self.max_momentum} | "
                f"Stealth: {stealth_status}")

    def swift_strike(self, target):
        """
        A basic, quick attack that generates Momentum.
        """
        damage = 15
        target.take_damage(damage)
        momentum_gained = 15
        self.momentum = min(self.max_momentum, self.momentum + momentum_gained)
        print(f"{self.name} strikes {target.name} with blinding speed for {damage} damage! (+{momentum_gained} Momentum)")

    def shadow_vanish(self):
        """
        Enters a stealth mode, making Zaia invisible to enemies.
        """
        cost = 30
        if self.momentum >= cost:
            self.momentum -= cost
            self.is_stealthed = True
            print(f"{self.name} spends {cost} Momentum to vanish into the shadows.")
            # In-game logic would make her untargetable by most enemies.
        else:
            print(f"{self.name} lacks the Momentum to vanish.")

    def exploit_weakness(self, target):
        """
        A high-damage, precision attack that consumes Momentum and breaks stealth for a bonus.
        """
        cost = 50
        if self.momentum >= cost:
            self.momentum -= cost
            print(f"{self.name} analyzes {target.name} and strikes at a vital point!")

            if self.is_stealthed:
                self.is_stealthed = False # Attacking breaks stealth
                damage = 120
                print(f"...The attack from the shadows is a devastating critical hit for {damage} damage!")
                target.take_damage(damage) # High bonus damage
            else:
                damage = 60
                print(f"...The attack deals {damage} damage.")
                target.take_damage(damage) # Standard high damage
        else:
            print(f"{self.name} does not have enough Momentum to exploit a weakness.")

    def evasive_dash(self):
        """
        An evasive maneuver for repositioning and avoiding damage.
        Likely has a cooldown rather than a resource cost.
        """
        print(f"{self.name} performs a nimble dash, evading incoming attacks.")
        # In-game logic would move her character quickly and grant brief invulnerability.
class DelilahTheDesolate(Player):
    """
    Represents Delilah the Desolate, the embodiment of The Omen and an agent of the Void.
    Born from the darkness expelled from Ingris, she is a being of decay and destruction.
    """
    def __init__(self, name="Delilah the Desolate", x=0, y=0):
        super().__init__(name, x, y)
        self.health = 160
        self.max_health = 160
        self.original_self_id = None # Would be set to Ingris's unique ID
        self.power_source = "The Omen"
        self.max_blight = 100
        self.blight = 25 # Starts with some Blight

    def __str__(self):
        """String representation of Delilah's status."""
        return (f"{self.name} | Health: {self.health}/{self.max_health} | "
                f"Blight: {self.blight}/{self.max_blight}")

    def touch_of_decay(self, target):
        """
        A basic ranged attack that applies a "decay" status effect, dealing damage
        over time and generating Blight.
        """
        print(f"{self.name} touches {target.name}, afflicting them with a decaying curse.")
        # In-game logic would apply a damage-over-time (DoT) effect to the target.
        # As the DoT deals damage, it would generate Blight for Delilah.
        blight_gained = 5
        self.blight = min(self.max_blight, self.blight + blight_gained)
        print(f"({self.name} gains {blight_gained} Blight.)")

    def summon_omen_avatar(self, target):
        """
        Spends Blight to summon a manifestation of The Omen to attack her enemies.
        """
        cost = 60
        if self.blight >= cost:
            self.blight -= cost
            print(f"{self.name} spends Blight to summon a terrifying avatar of The Omen to assault {target.name}!")
            # In-game logic would create a temporary AI-controlled creature.
        else:
            print(f"{self.name} does not have enough Blight to summon an avatar.")

    def voidblight_zone(self):
        """
        An ultimate ability that corrupts a large area of the battlefield.
        Enemies within the area take continuous damage and are weakened.
        """
        cost = 90
        if self.blight >= cost:
            self.blight -= cost
            print(f"{self.name} unleashes her full power, creating a large Voidblight Zone on the ground!")
            print("...Enemies inside are weakened and slowly consumed by decay!")
            # ... logic to create a persistent damaging and debuffing area of effect (AoE) ...
        else:
            print(f"{self.name} lacks the Blight to create a Voidblight Zone.")
class Cirrus(Character):
    """
    Represents Cirrus, the Dragon King of Mîlēhîgh.wørld.
    A powerful and enigmatic ruler, he acts as a mentor and a central figure in the world's power struggles.
    """
    def __init__(self, name="Cirrus the Dragon King", x=0, y=0):
        # Starts in his Humanoid form
        super().__init__(name, x, y, health=250, state="Humanoid")

        self.fathers_id = None # Would be set to Cyrus's unique ID
        self.max_sovereignty = 100
        self.sovereignty = 0

    @property
    def form(self):
        """A property to get his current form from the state."""
        return self.state

    def __str__(self):
        """String representation of Cirrus's status."""
        return (f"{self.name} | Health: {self.health}/{self.max_health} | "
                f"Sovereignty: {self.sovereignty}/{self.max_sovereignty} | "
                f"Form: {self.form}")

    def update(self):
        """
        An update method that could be called each game tick.
        Used here to passively generate Sovereignty.
        """
        # Passively gains Sovereignty over time, representing his authority.
        sovereignty_gain = 0.5
        self.sovereignty = min(self.max_sovereignty, self.sovereignty + sovereignty_gain)
        super().update() # Call parent update if it exists

    def assume_dragon_form(self):
        """
        Transforms Cirrus into his true dragon form, consuming all Sovereignty.
        """
        if self.sovereignty >= self.max_sovereignty:
            self.sovereignty = 0
            self.state = "Dragon"
            print(f"{self.name} unleashes his true power, transforming into a magnificent dragon!")
            # In a real game, this would change his character model, stats, and abilities.
        else:
            print(f"{self.name} has not accumulated enough Sovereignty to transform.")

    def revert_to_humanoid_form(self):
        """
        Reverts Cirrus back to his humanoid form.
        """
        if self.form == "Dragon":
            self.state = "Humanoid"
            print(f"{self.name} returns to his humanoid form.")

    def kings_decree(self, target):
        """
        A commanding ability that marks a target, with different effects based on form.
        """
        if self.form == "Humanoid":
            print(f"{self.name}, in his kingly form, issues a decree against {target.name}, marking them as a priority target!")
            # In-game logic: apply a "vulnerability" debuff to the target.
        elif self.form == "Dragon":
            print(f"{self.name}, in his dragon form, roars a challenge at {target.name}, terrifying them!")
            # In-game logic: apply a "fear" or "defense reduction" debuff.

    def draconic_breath(self, enemies_in_path):
        """
        A powerful breath attack, primarily used in Dragon form.
        """
        if self.form == "Dragon":
            print(f"{self.name} unleashes a torrent of dragon fire!")
            for enemy in enemies_in_path:
                print(f"...The fire engulfs {enemy.name}!")
                # ... logic to apply heavy fire damage ...
        else:
            print(f"{self.name} cannot use his full draconic breath in humanoid form.")


def run_character_demonstration():
    """
    A new function to demonstrate the unique abilities of the new characters.
    """
    print("\n--- Character Demonstration ---")

    # --- 1. Create Characters and an Enemy ---
    skyix = Skyix(x=0, y=0)
    anastasia = Anastasia(x=2, y=0)
    micah = Micah(x=4, y=0)
    enemy = Enemy(name="Void Beast", x=10, y=0, health=500)
    enemy.attack_damage = 25 # Increase damage to build Fortitude faster

    print("\n--- Initial State ---")
    print(skyix)
    print(anastasia)
    print(micah)
    print(enemy)

    # --- 2. Showcase Abilities ---
    print("\n--- Turn 1: Sky.ix and Anastasia take action ---")
    skyix.deploy_drone()
    anastasia.weave_dream(micah, is_healing=True) # Heal Micah to show support role
    anastasia.weave_dream(enemy, is_healing=False) # Harm the enemy

    print("\n--- Turn 2: Micah takes damage and builds Fortitude ---")
    enemy.attack(micah) # Enemy attacks Micah
    enemy.attack(micah) # Attack again to build more Fortitude
    print(micah)

    print("\n--- Turn 3: Micah retaliates ---")
    micah.activate_adamantine_skin()
    micah.earthen_smash(enemy)
    print(micah)
    print(enemy)

    print("\n--- Turn 4: Sky.ix unleashes a powerful attack ---")
    skyix.cast_spell("void_tech", enemy)
    print(skyix)
    print(enemy)

    print("\n--- Turn 5: Zaia enters the fray ---")
    zaia = Zaia(x=0, y=1)
    print(zaia)
    zaia.swift_strike(enemy)
    zaia.swift_strike(enemy)
    zaia.swift_strike(enemy)
    zaia.swift_strike(enemy)
    zaia.swift_strike(enemy)
    zaia.swift_strike(enemy)
    print(zaia)

    print("\n--- Turn 6: Zaia uses her Momentum ---")
    zaia.shadow_vanish()
    print(zaia)
    zaia.exploit_weakness(enemy)
    print(zaia)
    print(enemy)


    print("\n--- Demonstration Complete ---")


def run_aeron_demonstration():
    """
    A function to demonstrate the unique abilities of Aeron.
    """
    print("\n--- Character Demonstration: Aeron ---")

    # --- 1. Create Characters and an Enemy ---
    aeron = Aeron(x=0, y=0)
    # An ally to demonstrate buffing abilities
    ally = Player(name="Cirrus", x=1, y=0)
    allies = [aeron, ally]
    enemy = Enemy(name="Shadow Mercenary", x=10, y=0, health=300)

    print("\n--- Initial State ---")
    print(aeron)
    print(ally)
    print(enemy)

    # --- 2. Showcase Abilities ---
    print("\n--- Turn 1: Aeron builds Resolve ---")
    aeron.valiant_strike(enemy)
    aeron.valiant_strike(enemy)
    aeron.valiant_strike(enemy)
    aeron.valiant_strike(enemy)
    aeron.valiant_strike(enemy)
    print(aeron)

    print("\n--- Turn 2: Aeron uses his defensive stance ability ---")
    aeron.lions_roar(allies)
    print(aeron)
    print(f"Ally defense is now: {ally.defense}")


    print("\n--- Turn 3: Aeron switches stances and uses his offensive ability ---")
    aeron.switch_stance()
    # Build up resolve again
    aeron.valiant_strike(enemy)
    aeron.valiant_strike(enemy)
    aeron.valiant_strike(enemy)
    aeron.valiant_strike(enemy)
    aeron.valiant_strike(enemy)
    print(aeron)
    aeron.lions_roar(allies)
    print(aeron)
    print(f"Ally strength is now: {ally.strength}")


    print("\n--- Turn 4: Aeron unleashes his ultimate ---")
    # Build up resolve for the ultimate
    aeron.valiant_strike(enemy)
    aeron.valiant_strike(enemy)
    aeron.valiant_strike(enemy)
    aeron.valiant_strike(enemy)
    aeron.valiant_strike(enemy)
    aeron.valiant_strike(enemy)
    aeron.valiant_strike(enemy)
    aeron.valiant_strike(enemy)
    aeron.valiant_strike(enemy)
    print(aeron)
    aeron.skyfall_charge(enemy)
    print(enemy)
    print(aeron)


    print("\n--- Demonstration Complete ---")
def run_delilah_demonstration():
    """
    A function to demonstrate the unique abilities of Delilah the Desolate.
    """
    print("\n--- Character Demonstration: Delilah the Desolate ---")

    # --- 1. Create Delilah and an Enemy ---
    delilah = DelilahTheDesolate(x=0, y=0)
    enemy = Enemy(name="Void Spawn", x=10, y=0, health=200)

    print("\n--- Initial State ---")
    print(delilah)
    print(enemy)

    # --- 2. Showcase Abilities ---
    print("\n--- Turn 1: Delilah generates Blight ---")
    delilah.touch_of_decay(enemy)
    print(delilah)

    print("\n--- Turn 2: Delilah attempts to use an ability without enough Blight ---")
    delilah.summon_omen_avatar(enemy)
    print(delilah)

    print("\n--- Turn 3: Delilah generates more Blight ---")
    # In a real scenario, this would happen over time or through other actions.
    delilah.blight = 70
    print(f"(Delilah's Blight is now {delilah.blight})")
    delilah.summon_omen_avatar(enemy)
    print(delilah)

    print("\n--- Turn 4: Delilah uses her ultimate ability ---")
    delilah.blight = 100
    print(f"(Delilah's Blight is now {delilah.blight})")
    delilah.voidblight_zone()
    print(delilah)


    print("\n--- Demonstration Complete ---")
def run_cirrus_demonstration():
    """
    A function to demonstrate the unique abilities of Cirrus.
    """
    print("\n--- Cirrus Demonstration ---")

    # --- 1. Create Cirrus and an Enemy ---
    cirrus = Cirrus(x=0, y=0)
    enemy1 = Enemy(name="Rebel Captain", x=10, y=0, health=150)
    enemy2 = Enemy(name="Renegade Knight", x=12, y=0, health=180)
    enemies = [enemy1, enemy2]

    print("\n--- Initial State ---")
    print(cirrus)
    print(enemy1)
    print(enemy2)

    # --- 2. Showcase Humanoid Abilities ---
    print("\n--- Turn 1: Cirrus asserts his authority ---")
    cirrus.kings_decree(enemy1)
    cirrus.draconic_breath(enemies) # Should fail in this form

    # --- 3. Build Sovereignty ---
    print("\n--- Building Sovereignty... ---")
    # Simulate time passing to build up his resource
    for _ in range(200):
        cirrus.update()
    print(cirrus)

    # --- 4. Transform and Unleash Dragon Form ---
    print("\n--- Turn 2: Cirrus transforms! ---")
    cirrus.assume_dragon_form()
    print(cirrus)

    # --- 5. Showcase Dragon Abilities ---
    print("\n--- Turn 3: Cirrus uses his dragon abilities ---")
    cirrus.kings_decree(enemy2)
    cirrus.draconic_breath(enemies)

    # --- 6. Revert to Humanoid Form ---
    print("\n--- Turn 4: The Dragon King returns to his mortal guise ---")
    cirrus.revert_to_humanoid_form()
    print(cirrus)

    print("\n--- Cirrus Demonstration Complete ---")


def run_cyrus_demonstration():
    """
    A function to demonstrate the abilities of the new antagonist, Cyrus.
    """
    print("\n--- Cyrus Demonstration ---")

    # --- 1. Create Cyrus and a Player to fight ---
    cyrus = Cyrus(x=10, y=0)
    hero = Player(name="Hero", x=0, y=0)
    # Give the hero more health to survive the demonstration
    hero.health = 250
    hero.max_health = 250

    print("\n--- Initial State ---")
    print(cyrus)
    print(hero)

    # --- 2. Showcase Cyrus's Abilities ---
    print("\n--- Turn 1: Cyrus uses a standard attack and a special ability ---")
    cyrus.attack(hero) # A standard attack
    cyrus.dimensional_rift()
    print(cyrus)
    print(hero)

    print("\n--- Turn 2: Cyrus uses his Worldbreaker Strike ---")
    cyrus.worldbreaker_strike(hero)
    print(cyrus)
    print(hero)

    print("\n--- Turn 3: Cyrus unleashes his ultimate ability ---")
    # We create another player to demonstrate the line attack
    hero2 = Player(name="Sidekick", x=-1, y=0)
    hero2.health = 200
    targets = [hero, hero2]
    # To showcase the ultimate, we'll reset his Tyranny as if he has powered up
    print(f"{cyrus.name} gathers his full power for a final strike!")
    cyrus.tyranny = cyrus.max_tyranny
    cyrus.onalym_purge(targets)
    print(cyrus)
    print(hero)
    print(hero2)

    print("\n--- Cyrus Demonstration Complete ---")


def run_nyxar_demonstration():
    """
    A function to demonstrate the abilities of the antagonist, Nyxar.
    """
    print("\n--- Nyxar Demonstration ---")

    # --- 1. Create Nyxar and some heroes to fight ---
    nyxar = Nyxar(x=0, y=0)
    hero1 = Player(name="Aeron", x=10, y=5)
    hero2 = Player(name="Zaia", x=8, y=-5)

    print("\n--- Initial State ---")
    print(nyxar)
    print(hero1)
    print(hero2)

    # --- 2. Showcase Nyxar's Abilities ---
    print("\n--- Turn 1: Nyxar begins to dominate the field ---")
    nyxar.shadow_tether(hero1)
    print(f"(Nyxar's tethered enemies: {[e.name for e in nyxar.tethered_enemies]})")
    nyxar.update() # Simulate a game tick to generate Dominion
    print(nyxar)

    print("\n--- Turn 2: Nyxar extends his influence ---")
    nyxar.shadow_tether(hero2)
    print(f"(Nyxar's tethered enemies: {[e.name for e in nyxar.tethered_enemies]})")

    print("\n...Time passes, Nyxar's dominion grows...")
    for _ in range(5): # Simulate a few more ticks
        nyxar.update()
    print(nyxar)


    print("\n--- Turn 3: Nyxar creates a clone ---")
    nyxar.create_umbral_clone(hero1)
    print(nyxar)

    print("\n--- Turn 4: Nyxar gathers his full power ---")
    # Manually set dominion to max for the ultimate
    nyxar.dominion = 100
    print(nyxar)
    nyxar.worldless_chasm()
    print(nyxar)

    print("\n--- Nyxar Demonstration Complete ---")


if __name__ == "__main__":
    # run_game() # You can comment this out to only run the character demo
    # run_character_demonstration()
    run_cyrus_demonstration()
    run_aeron_demonstration()
    run_delilah_demonstration()
    run_cirrus_demonstration()
    run_nyxar_demonstration()
