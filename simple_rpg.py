import time
import random
import math

class GameObject:
    """
    Base class for all game objects.  Provides fundamental attributes and methods.
    """
    def __init__(self, name="GameObject", x=0, y=0, z=0, health=100, speed=1, visible=True, solid=True):
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
        """
        self.name = name
        self.x = x
        self.y = y
        self.z = z
        self.health = health
        self.speed = speed
        self.visible = visible
        self.solid = solid
        self.attributes = {}  # Dictionary for storing additional attributes.

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
        Reduces the object's health.

        Args:
            damage (int): The amount of damage to take.
        """
        self.health -= damage
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
        self.mana = 100  # Add mana attribute

    def attack(self, target):
        """
        Attacks another GameObject.

        Args:
            target (GameObject): The target to attack.
        """
        if self.weapon:
            damage = self.weapon.damage
            print(f"{self.name} attacks {target.name} with {self.weapon.name} for {damage} damage.")
            target.take_damage(damage)
        else:
            print(f"{self.name} attacks {target.name} with bare hands for 10 damage.")
            target.take_damage(10)

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
        Picks up an item and adds it to the inventory.

        Args:
            item (GameObject): The item to pick up.
        """
        self.inventory.append(item)
        item.visible = False
        item.solid = False
        print(f"{self.name} picked up {item.name}.")

    def use_item(self, item_name):
        """
        Uses an item from the inventory.

        Args:
            item_name (str): The name of the item to use.
        """
        for item in self.inventory:
            if item.name == item_name:
                if isinstance(item, Consumable):
                    item.use(self)  #  Use the item.
                    self.inventory.remove(item)  # Remove used item.
                else:
                    print(f"{self.name} cannot use {item.name}.")
                return
        print(f"{self.name} does not have {item_name} in their inventory.")

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
        """Casts a spell.  Example of using the mana attribute."""
        if spell_name == "fireball":
            if self.mana >= 20:  # Fireball costs 20 mana
                self.mana -= 20
                print(f"{self.name} casts Fireball on {target.name} for 30 damage!")
                target.take_damage(30)
            else:
                print(f"{self.name} does not have enough mana to cast Fireball!")
        elif spell_name == "heal":
            if self.mana >= 10:
                self.mana -= 10
                self.heal(20)
                print(f"{self.name} casts Heal and heals for 20 HP.")
            else:
                print(f"{self.name} does not have enough mana to cast Heal!")
        else:
            print(f"{self.name} does not know the spell {spell_name}.")

class Enemy(GameObject):
    """
    Represents an enemy character.
    """
    def __init__(self, name="Enemy", x=0, y=0, z=0, type="Generic"):
        super().__init__(name=name, x=x, y=y, z=z, health=50, speed=2)
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
        super().update(delta_time) # Call the superclass's update method.
        if self.distance_to(player) < self.aggro_range:
            # Move towards the player
            dx = player.x - self.x
            dy = player.y - self.y
            dz = player.z - self.z
            distance = self.distance_to(player)
            if distance > 0:
              self.move(dx / distance * self.speed * delta_time, dy / distance * self.speed * delta_time, dz/distance * self.speed * delta_time)
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
        Updates the game state.  This is called every frame.

        Args:
            delta_time (float): Time since the last frame.
        """
        for obj in self.objects:
            if isinstance(obj, Enemy):
                obj.update(delta_time, self.player) # Pass the player to the enemy's update
            else:
                obj.update(delta_time) # Call the update method for other objects

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
        The main game loop.  This is called every frame.
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
        #  Add a small delay.  Important for CPU usage.
        time.sleep(0.01) # 10 milliseconds

def run_game():
    """
    Main function to run the game.  Sets up the game and starts it.
    """
    game = Game()

    # Create game objects
    player = Player(name="Hero", x=0, y=0, z=0)
    game.add_object(player)

    enemy1 = Enemy(name="Goblin", x=5, y=5, z=0, type="Goblin")
    game.add_object(enemy1)

    enemy2 = Enemy(name="Orc", x=-5, y=-5, z=0, type="Orc")
    game.add_object(enemy2)

    sword = Weapon(name="Sword", x=1, y=0, z=0, damage=20, weapon_type="Melee")
    game.add_object(sword)

    potion = HealthPotion(name="Potion", x=-1, y=0, z=0, amount=30)
    game.add_object(potion)

    mana_potion = ManaPotion(name="Mana Potion", x=-2, y=0, z=0, amount=40)
    game.add_object(mana_potion)

    player.pickup_item(sword)
    player.equip_weapon(sword)
    player.pickup_item(potion)
    player.pickup_item(mana_potion)
    player.mana = 100 # give player some mana

    # Example of using the new mana and cast_spell
    print("\n--- Game Start ---")
    print(player)  # Print player stats
    player.cast_spell("fireball", enemy1)
    print(enemy1)
    player.cast_spell("heal", player)
    print(player)
    player.cast_spell("fireball", enemy2) #use mana
    print(enemy2)
    player.cast_spell("heal", player)
    print(player)
    game.start() #start the game loop.

if __name__ == "__main__":
    run_game()