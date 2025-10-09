import random
import math

# --- Paste ALL the previously created Python classes here ---

class GameObject:
    """Base class for all game objects."""
    def __init__(self, name="GameObject", x=0, y=0, z=0, health=100, defense=0):
        self.name = name
        self.x = x
        self.y = y
        self.z = z
        self.health = health
        self.max_health = health
        self.defense = defense
        self.id = id(self)

    def __repr__(self):
        return f"{self.name}(HP: {self.health}/{self.max_health})"

    def take_damage(self, damage):
        actual_damage = max(0, damage - self.defense)
        self.health -= actual_damage
        print(f"{self.name} takes {actual_damage} damage.")
        if self.health <= 0:
            print(f"{self.name} has been defeated.")

class Player(GameObject):
    """Base class for player characters."""
    def __init__(self, name, x=0, y=0, z=0, health=100, defense=10):
        super().__init__(name, x, y, z, health, defense)
        self.relationships = {}

    def add_relationship(self, character, relationship_type):
        self.relationships[character.id] = relationship_type
        print(f"{self.name} now has a '{relationship_type}' relationship with {character.name}.")

class Enemy(GameObject):
    """Base class for enemy characters."""
    def __init__(self, name, x=0, y=0, z=0, health=100, defense=5):
        super().__init__(name, x, y, z, health, defense)

class Skyix(Player):
    """Represents Sky.ix, the Bionic Goddess."""
    def __init__(self, name="Sky.ix", x=0, y=0, z=0):
        super().__init__(name, x, y, z, health=120, defense=15)
        self.void_energy = 150
        self.max_void_energy = 150
        self.active_drones = 0

    def __str__(self):
        return f"{self.name}(HP: {self.health}/{self.max_health}, Void Energy: {self.void_energy}/{self.max_void_energy}, Drones: {self.active_drones})"

    def deploy_drone(self):
        self.active_drones += 1
        print(f"{self.name} deploys a drone! ({self.active_drones} active).")

    def use_void_tech(self, target):
        damage = 50
        print(f"{self.name} unleashes Void Tech on {target.name} for {damage} damage!")
        target.take_damage(damage)

class Micah(Player):
    """Represents Micah the Unbreakable."""
    def __init__(self, name="Micah", x=0, y=0, z=0):
        super().__init__(name, x, y, z, health=200, defense=25)
        self.mothers_id = None

    def activate_adamantine_skin(self):
        self.defense += 20
        print(f"{self.name} activates Adamantine Skin, increasing his defense!")

    def take_damage(self, damage):
        # Overriding to show he is a tank
        reduced_damage = max(1, damage - 10) # Micah has some innate resistance
        super().take_damage(reduced_damage)

class Anastasia(Player):
    """Represents Anastasia the Dreamer."""
    def __init__(self, name="Anastasia", x=0, y=0, z=0):
        super().__init__(name, x, y, z, health=90, defense=10)
        self.dream_energy = 200
        self.max_dream_energy = 200

    def __str__(self):
        return f"{self.name}(HP: {self.health}/{self.max_health}, Dream Energy: {self.dream_energy}/{self.max_dream_energy})"

    def assume_leadership_role(self):
        print(f"{self.name} assumes a leadership role, empowering her abilities.")

    def weave_dream(self, target, is_healing=True):
        if is_healing:
            heal_amount = 35
            target.health = min(target.max_health, target.health + heal_amount)
            print(f"{self.name} weaves a soothing dream, healing {target.name} for {heal_amount} HP.")
        else:
            damage = 20
            print(f"{self.name} weaves a nightmare, inflicting {damage} psychic damage on {target.name}.")
            target.take_damage(damage)

class Aeron(Player):
    """Represents Aeron, the noble warrior."""
    def __init__(self, name="Aeron", x=0, y=0, z=0):
        super().__init__(name, x, y, z, health=180, defense=20)
        self.stance = "Unyielding"
        self.resolve = 0
        self.rival_brother_id = None

    def __str__(self):
        return f"{self.name}(HP: {self.health}/{self.max_health}, Stance: {self.stance}, Resolve: {self.resolve})"

    def switch_stance(self):
        if self.stance == "Unyielding":
            self.stance = "Commanding"
        else:
            self.stance = "Unyielding"
        print(f"{self.name} switches to a {self.stance} stance!")

    def valiant_strike(self, target):
        self.resolve += 15
        damage = 20
        print(f"{self.name}'s valiant strike deals {damage} damage to {target.name} and builds Resolve.")
        target.take_damage(damage)

    def lions_roar(self, allies):
        if self.stance == "Commanding":
            print(f"{self.name}'s roar buffs allies' attack!")
        else:
            print(f"{self.name}'s roar buffs allies' defense!")

class Kane(Enemy):
    """Represents Kane, the Shadow of Aethelgard."""
    def __init__(self, name="Kane", x=0, y=0, z=0):
        super().__init__(name, x, y, z, health=220, defense=15)
        self.rival_brother_id = None

    def vicious_strike(self, target):
        damage = 40
        print(f"{self.name}'s vicious strike deals {damage} damage to {target.name}!")
        target.take_damage(damage)

class Delilah(Enemy):
    """Represents Delilah the Desolate."""
    def __init__(self, name="Delilah", x=0, y=0, z=0):
        super().__init__(name, x, y, z, health=160, defense=10)

    def touch_of_decay(self, target):
        damage = 25
        print(f"{self.name}'s touch of decay deals {damage} damage to {target.name}.")
        target.take_damage(damage)

# Aliases for the simulation script
DelilahTheDesolate = Delilah

# --- Simulation Setup ---

print("="*30)
print("INITIALIZING MILEHIGH.WORLD SIMULATION")
print("="*30)

# 1. Instantiate Heroes
skyix = Skyix(x=10, y=5)
micah = Micah(x=12, y=5)
anastasia = Anastasia(x=8, y=5)
aeron = Aeron(x=10, y=7)

heroes = [skyix, micah, anastasia, aeron]

# 2. Instantiate Villains
kane = Kane(x=50, y=5)
delilah = Delilah(x=48, y=5)

villains = [kane, delilah]

all_characters = heroes + villains

# 3. Establish Relationships
skyix.add_relationship(micah, "Son")
micah.mothers_id = skyix.id # Assuming GameObject has a unique id
aeron.rival_brother_id = kane.id

print("\n--- CHARACTER STATUS ---")
for char in all_characters:
    print(char)

# --- SIMULATE A BATTLE SCENARIO ---

print("\n" + "="*30)
print("BATTLE START: HEROES VS. VILLAINS")
print("="*30 + "\n")

# --- TURN 1 ---
print("--- TURN 1 ---")
# Aeron takes the lead, switching to an offensive stance and buffing his allies.
aeron.switch_stance() # Switches to Commanding
aeron.valiant_strike(kane) # Builds Resolve
aeron.lions_roar(heroes) # Buffs allies

# Kane retaliates against his brother.
kane.vicious_strike(aeron)

print("\n--- TURN 2 ---")
# Micah activates his defensive ability and takes the front line.
micah.activate_adamantine_skin()

# Delilah spreads corruption.
delilah.touch_of_decay(micah)
# Micah's special take_damage method is triggered.
micah.take_damage(20) # Simulating the DoT damage

print("\n--- TURN 3 ---")
# Sky.ix deploys her tech.
skyix.deploy_drone()
skyix.use_void_tech(kane)

# Anastasia supports the team.
anastasia.assume_leadership_role()
anastasia.weave_dream(micah, is_healing=True)

print("\n" + "="*30)
print("BATTLE SIMULATION CONCLUDED")
print("="*30)

print("\n--- FINAL CHARACTER STATUS ---")
for char in all_characters:
    print(char)