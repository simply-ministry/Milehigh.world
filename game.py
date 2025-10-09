import time
import random
import math

# --- Base Classes ---

class GameObject:
    """
    Base class for all tangible objects in the game world.
    Provides fundamental attributes like name, position, and health.
    """
    def __init__(self, name="GameObject", x=0, y=0, health=100):
        self.name = name
        self.x = int(x)
        self.y = int(y)
        self.health = health
        self.max_health = health
        self.visible = True
        self.solid = True

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
        Handles the object's death. This can be overridden in subclasses.
        """
        self.visible = False
        self.solid = False
        print(f"{self.name} has been defeated.")

# --- Item System ---

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
        self.effect = effect  # e.g., "heal"
        self.value = value

    def __str__(self):
        return f"{self.name} (Consumable, {self.effect} +{self.value}): {self.description}"

    def use(self, character):
        """Applies the consumable's effect to a character."""
        print(f"{character.name} uses {self.name}!")
        if self.effect == "heal":
            character.heal(self.value)
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
            print(f"'{item.name}' was added to the inventory.")
            return True
        else:
            print("Inventory is full!")
            return False

    def remove_item(self, item_name):
        """Removes an item from the inventory by name."""
        for item in self.items:
            if item.name.lower() == item_name.lower():
                self.items.remove(item)
                return item
        return None

    def list_items(self):
        """Prints a list of all items in the inventory."""
        if not self.items:
            print("Inventory is empty.")
            return
        print("--- Inventory ---")
        for item in self.items:
            print(f"- {item}")
        print("-----------------")


# --- Ability System ---

class Ability:
    """Base class for all character abilities and skills."""
    def __init__(self, name, description, cost, cost_type):
        self.name = name
        self.description = description
        self.cost = cost
        self.cost_type = cost_type # e.g., "Void Energy", "Fortitude", "Resolve"

    def can_use(self, caster):
        """Checks if the caster has enough resources to use the ability."""
        # This will be implemented in specific character classes.
        print(f"ERROR: can_use() not implemented for {self.name}")
        return False

    def use(self, caster, target):
        """Executes the ability's effect."""
        print(f"ERROR: use() not implemented for {self.name}")

class TargetedDamageAbility(Ability):
    """An ability that deals damage to a single target."""
    def __init__(self, name, description, cost, cost_type, damage):
        super().__init__(name, description, cost, cost_type)
        self.damage = damage

    def use(self, caster, target):
        """Deals damage to the target if the caster can pay the cost."""
        print(f"{caster.name} uses {self.name} on {target.name}!")
        target.take_damage(self.damage)


# --- Character Classes ---

class Character(GameObject):
    """
    UPDATED base class for all characters, with an ability system.
    """
    def __init__(self, name="Character", x=0, y=0, health=100):
        super().__init__(name, x, y, health=health)
        self.inventory = Inventory()
        self.level = 1
        self.xp = 0
        self.xp_to_next_level = 100
        # --- NEW ---
        self.abilities = [] # List of (ability, required_level) tuples
        self.unlocked_abilities = []

    def learn_ability(self, ability):
        """Adds a new ability to the character's unlocked list."""
        if ability not in self.unlocked_abilities:
            # Avoid learning duplicates
            if not any(a.name == ability.name for a in self.unlocked_abilities):
                self.unlocked_abilities.append(ability)
                print(f"{self.name} has learned the ability: {ability.name}!")

    def cast_ability(self, ability_name, target):
        """Finds and uses an unlocked ability."""
        ability_to_cast = next((a for a in self.unlocked_abilities if a.name.lower() == ability_name.lower()), None)

        if ability_to_cast:
            # The specific character class will override this to check resources
            # and deduct costs before calling the ability's use() method.
            print(f"'{ability_name}' is a valid ability, but the base Character class cannot use it.")
        else:
            print(f"{self.name} does not know the ability '{ability_name}'.")

    def gain_xp(self, amount):
        """Gains experience points and checks for level up."""
        self.xp += amount
        print(f"{self.name} gained {amount} XP.")
        # Check for level up after gaining XP
        while self.xp >= self.xp_to_next_level:
            self.xp -= self.xp_to_next_level
            self.level_up()

    def level_up(self):
        """
        UPDATED to handle leveling up and check for new abilities to learn.
        """
        self.level += 1
        self.xp_to_next_level = int(self.xp_to_next_level * 1.5)
        self.max_health += 10
        self.health = self.max_health
        print(f"{self.name} leveled up to Level {self.level}!")

        # --- NEW --- Check for new abilities to learn
        for ability, required_level in self.abilities:
            if self.level >= required_level:
                self.learn_ability(ability)


class Player(Character):
    """
    Represents the player character.
    """
    def __init__(self, name="Player", x=0, y=0):
        super().__init__(name, x, y, health=100)
        self.weapon = None

    def attack(self, target):
        """Attacks another character."""
        damage = self.weapon.damage if self.weapon else 5 # Bare-handed damage
        print(f"{self.name} attacks {target.name}!")
        target.take_damage(damage)

    def equip_weapon(self, weapon):
        """Equips a weapon."""
        if isinstance(weapon, Weapon):
            self.weapon = weapon
            print(f"{self.name} equipped {weapon.name}.")
        else:
            print(f"{weapon.name} is not a weapon.")


class Enemy(Character):
    """
    Represents an enemy character.
    """
    def __init__(self, name="Enemy", x=0, y=0, health=50, damage=10):
        super().__init__(name, x, y, health=health)
        self.damage = damage

    def attack(self, target):
        """Attacks another character."""
        print(f"{self.name} attacks {target.name}!")
        target.take_damage(self.damage)


# --- Specific Character Implementations ---

class Skyix(Character):
    """
    Represents the character Sky.ix, the Bionic Goddess.
    Inherits from Character, adding unique Void-based mechanics.
    """
    def __init__(self, name="Sky.ix the Bionic Goddess", x=0, y=0):
        super().__init__(name, x, y, health=120)
        self.void_energy = 100
        self.max_void_energy = 100
        # Define her potential abilities and the level they unlock
        self.abilities = [
            (TargetedDamageAbility("Void Tech", "Devastating Void attack.", 40, "Void Energy", 60), 1),
            (TargetedDamageAbility("Energy Blast", "A quick energy projectile.", 10, "Void Energy", 25), 3)
        ]
        # Automatically learn level 1 abilities by running the check at initialization
        # We call level_up() directly, but with a "fresh" character, it only
        # runs the ability check, not the full level-up logic.
        self.level_up()

    def __str__(self):
        # Override string representation to include unique resource
        return (f"{self.name} (HP: {self.health}/{self.max_health}, "
                f"Void Energy: {self.void_energy}/{self.max_void_energy})")

    def cast_ability(self, ability_name, target):
        """Skyix's specific implementation for using her abilities."""
        ability_to_cast = next((a for a in self.unlocked_abilities if a.name.lower() == ability_name.lower()), None)

        if ability_to_cast:
            if self.void_energy >= ability_to_cast.cost:
                self.void_energy -= ability_to_cast.cost
                ability_to_cast.use(self, target)
            else:
                print(f"Not enough Void Energy for {ability_to_cast.name}.")
        else:
            print(f"{self.name} does not know the ability '{ability_name}'.")

class Kane(Enemy):
    """A specific enemy character, Kane, for demonstration purposes."""
    def __init__(self, name="Kane", x=0, y=0, health=150):
        super().__init__(name, x, y, health=health, damage=20)


if __name__ == "__main__":
    # --- DEMONSTRATION OF THE ABILITY SYSTEM ---

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


class BachirimBase(Character):
    """
    A base class for members of The Bachirim, the mysterious beings from the
    shattered celestial realm of ƁÅČ̣ĤÎŘØN̈. This class provides the foundational
    abilities and properties common to all Bachirim.
    """
    def __init__(self, name="Nameless Bachirim", x=0, y=0):
        super().__init__(name, x, y, health=150)

        self.home_realm = "ƁÅČ̣ĤÎŘØN̈"
        self.max_aether = 200
        self.aether = self.max_aether

    def __str__(self):
        """String representation of a Bachirim's status."""
        return (f"{self.name} | Health: {self.health}/{self.max_health} | "
                f"Aether: {self.aether}/{self.max_aether} | "
                f"Realm: {self.home_realm}")

    def glimpse_the_fracture(self):
        """
        A foundational ability representing the Bachirim's knowledge of the Verse.
        Child classes can override this with a more specific implementation.
        """
        print(f"{self.name} gazes into the fractured reality, discerning a hidden truth.")
        # In-game logic to provide a tactical advantage, like revealing an invisible enemy.

    def channel_celestial_energy(self, target):
        """
        A foundational ability to channel celestial energy.
        Child classes can implement this as an attack, a shield, or another utility.
        """
        cost = 30
        if self.aether >= cost:
            self.aether -= cost
            print(f"{self.name} channels pure celestial energy at {target.name}.")
        else:
            print(f"{self.name} lacks the Aether to channel this energy.")


def run_bachirim_demonstration():
    """
    A function to demonstrate the abilities of the new BachirimBase class.
    """
    print("\n--- Bachirim Demonstration ---")

    # --- 1. Create a Bachirim and an Enemy ---
    bachirim = BachirimBase(name="Elarion the Star-Gazer", x=0, y=0)
    enemy = Enemy(name="Void Hound", x=5, y=0, health=100)

    print("\n--- Initial State ---")
    print(bachirim)
    print(enemy)

    # --- 2. Showcase Bachirim Abilities ---
    print("\n--- Turn 1: The Bachirim uses its foundational powers ---")
    bachirim.glimpse_the_fracture()
    bachirim.channel_celestial_energy(enemy)
    print(bachirim)

    print("\n--- Turn 2: The Bachirim acts again ---")
    bachirim.channel_celestial_energy(enemy)
    print(bachirim)


    print("\n--- Bachirim Demonstration Complete ---")


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
def run_era_demonstration():
    """
    A function to demonstrate the abilities of the new antagonist, Era.
    """
    print("\n--- Era Demonstration ---")

    # --- 1. Create Era and a Player to fight ---
    era = Era(x=10, y=0)
    hero = Player(name="Hero", x=0, y=0)
    # Give the hero more health to survive the demonstration
    hero.health = 400
    hero.max_health = 400
    all_heroes = [hero]

    print("\n--- Initial State ---")
    print(era)
    print(hero)

    # --- 2. Showcase Era's Abilities ---
    print("\n--- Turn 1: Era starts spreading corruption ---")
    era.spread_the_void()
    print(era)

    print("\n--- Turn 2: Era attacks with chaotic energy ---")
    era.chaotic_outburst(hero)
    print(era)
    print(hero)

    print("\n--- Turn 3: Era unleashes her ultimate ability ---")
    era.inevitable_collapse(all_heroes)
    print(era)
    print(hero)

    print("\n--- Era Demonstration Complete ---")
def run_omega_one_demonstration():
    """
    A function to demonstrate the unique abilities of Omega.one.
    """
    print("\n--- Character Demonstration: Omega.one ---")

    # --- 1. Create Omega.one and an Enemy ---
    omega_one = OmegaOne(x=0, y=0)
    enemy1 = Enemy(name="Drone Target", x=10, y=0, health=200)
    enemy2 = Enemy(name="Collateral Target", x=12, y=0, health=200)
    enemies = [enemy1, enemy2]
    print("--- ABILITY SYSTEM SIMULATION ---")

    # 1. Create a character and an enemy
    hero_skyix = Skyix()
    enemy_bandit = Kane(name="Void Bandit", x=1, y=0)
    enemy_bandit.health = 200
    enemy_bandit.max_health = 200


    print(f"\nInitial Status:")
    print(hero_skyix)
    print(enemy_bandit)


    # 2. Use a starting ability
    print("\n--- Level 1 Combat ---")
    # At level 1, Skyix should know "Void Tech"
    hero_skyix.cast_ability("void tech", enemy_bandit)
    print(hero_skyix)
    print(enemy_bandit)

    # 3. Simulate gaining levels to unlock a new ability
    print("\n--- Gaining Levels ---")
    hero_skyix.gain_xp(500) # This should trigger multiple level ups and unlock "Energy Blast"

    print(f"\nUnlocked abilities: {[ability.name for ability in hero_skyix.unlocked_abilities]}")

if __name__ == "__main__":
    # run_game() # You can comment this out to only run the character demo
    # run_character_demonstration()
    run_cyrus_demonstration()
    run_aeron_demonstration()
    run_delilah_demonstration()
    run_cirrus_demonstration()
    run_bachirim_demonstration()
    run_nyxar_demonstration()
    run_era_demonstration()
    run_omega_one_demonstration()
    # 4. Use the newly unlocked ability
    print("\n--- Level 3+ Combat ---")
    hero_skyix.cast_ability("energy blast", enemy_bandit)
    print(hero_skyix)
    print(enemy_bandit)
