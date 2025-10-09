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

    print("\n--- Demonstration Complete ---")


if __name__ == "__main__":
    # run_game() # You can comment this out to only run the character demo
    run_character_demonstration()