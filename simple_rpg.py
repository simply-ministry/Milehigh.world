"""A simplified Python RPG for testing specific features.

This script provides a lightweight implementation of a text-based RPG,
focusing on core mechanics such as character progression, inventory management,
and an ability system. It is designed to be a simple, self-contained
environment for testing and demonstrating specific game features before they
are integrated into the more complex `rpg.py` or the main Unity project.
"""

import time
import random
import math

class GameObject:
    """Base class for all objects in the game world.

    Attributes:
        name (str): The name of the object.
        x (int): The x-coordinate of the object.
        y (int): The y-coordinate of the object.
        max_health (int): The maximum health of the object.
        health (int): The current health of the object.
        is_alive (bool): Whether the object is alive.
    """
    def __init__(self, name="GameObject", x=0, y=0, health=100):
        self.name = name
        self.x = x
        self.y = y
        self.max_health = health
        self.health = health
        self.is_alive = True

    def move(self, dx, dy):
        """Moves the object by a given delta.

        Args:
            dx (int): The change in x-coordinate.
            dy (int): The change in y-coordinate.
        """
        self.x += dx
        self.y += dy

    def heal(self, amount):
        """Heals the object for a given amount.

        Args:
            amount (int): The amount of health to restore.
        """
        self.health = min(self.max_health, self.health + amount)
        print(f"{self.name} heals for {amount}, now at {self.health} HP.")

    def take_damage(self, amount):
        """Reduces the object's health by a given amount.

        Args:
            amount (int): The amount of damage to take.
        """
        self.health -= amount
        if self.health <= 0:
            self.health = 0
            self.is_alive = False
            print(f"{self.name} has been defeated!")
        else:
            print(f"{self.name} takes {amount:.0f} damage, {self.health:.0f} HP remaining.")

    def update(self):
        """A placeholder for future logic, called once per game turn."""
        pass

class Item:
    """Base class for all items in the game.

    Attributes:
        name (str): The name of the item.
        description (str): A description of the item.
    """
    def __init__(self, name, description):
        self.name = name
        self.description = description
    def __str__(self):
        return f"{self.name}: {self.description}"

class Weapon(Item):
    """A type of item that can be equipped to deal damage.

    Attributes:
        damage (int): The amount of damage the weapon deals.
    """
    def __init__(self, name, description, damage):
        super().__init__(name, description)
        self.damage = damage
    def __str__(self):
        return f"{self.name} (Weapon, +{self.damage} DMG)"

class Consumable(Item):
    """A type of item that can be used once for an effect.

    Attributes:
        effect (str): The type of effect the consumable has (e.g., "heal").
        value (int): The magnitude of the effect.
    """
    def __init__(self, name, description, effect, value):
        super().__init__(name, description)
        self.effect = effect
        self.value = value
    def __str__(self):
        return f"{self.name} (Consumable)"
    def use(self, character):
        """Applies the consumable's effect to a character.

        Args:
            character (Character): The character using the item.
        """
        print(f"{character.name} uses {self.name}!")
        if self.effect == "heal":
            character.heal(self.value)

class Inventory:
    """Manages a character's items.

    Attributes:
        items (list): A list of items in the inventory.
        capacity (int): The maximum number of items the inventory can hold.
        owner_name (str): The name of the inventory's owner.
    """
    def __init__(self, owner_name="Unknown", capacity=20):
        self.items = []
        self.capacity = capacity
        self.owner_name = owner_name

    def add_item(self, item):
        """Adds an item to the inventory.

        Args:
            item (Item): The item to add.

        Returns:
            bool: True if the item was added successfully, False otherwise.
        """
        if len(self.items) < self.capacity:
            self.items.append(item)
            return True
        return False

    def remove_item(self, item_name):
        """Removes an item from the inventory by name.

        Args:
            item_name (str): The name of the item to remove.
        """
        self.items = [item for item in self.items if item.name != item_name]

    def list_items(self):
        """Prints a list of all items in the inventory."""
        print(f"--- {self.owner_name}'s Inventory ---")
        if not self.items:
            print("Empty")
        else:
            for item in self.items:
                print(f"- {item}")
        print("-----------------")

class Ability:
    """Base class for all abilities.

    Attributes:
        name (str): The name of the ability.
        description (str): A description of the ability.
        cost (int): The cost to use the ability.
        cost_type (str): The type of resource the ability costs (e.g., "Mana").
    """
    def __init__(self, name, description, cost, cost_type):
        self.name = name
        self.description = description
        self.cost = cost
        self.cost_type = cost_type

    def use(self, caster, target):
        """Uses the ability.

        Args:
            caster (Character): The character using the ability.
            target (Character): The target of the ability.
        """
        print("Ability base class cannot be used.")

class TargetedDamageAbility(Ability):
    """An ability that deals damage to a single target.

    Attributes:
        damage (int): The amount of damage the ability deals.
    """
    def __init__(self, name, description, cost, cost_type, damage):
        super().__init__(name, description, cost, cost_type)
        self.damage = damage

    def use(self, caster, target):
        """Uses the ability on a target.

        Args:
            caster (Character): The character using the ability.
            target (Character): The target of the ability.
        """
        print(f"{caster.name} uses {self.name} on {target.name}!")
        target.take_damage(self.damage)

class Quest:
    """Represents a quest with objectives.

    Attributes:
        title (str): The title of the quest.
        description (str): A description of the quest.
        objectives (list): A list of objectives for the quest.
        status (str): The current status of the quest (e.g., "Active").
    """
    def __init__(self, title, description, objectives):
        self.title = title
        self.description = description
        self.objectives = objectives
        self.status = "Inactive"

    def is_complete(self):
        """Checks if all objectives are complete.

        Returns:
            bool: True if all objectives are complete, False otherwise.
        """
        return all(obj['current'] >= obj['required'] for obj in self.objectives)

    def update_objective(self, objective_type, target_name):
        """Updates a quest objective.

        Args:
            objective_type (str): The type of objective to update (e.g., "defeat").
            target_name (str): The name of the target of the objective.
        """
        for obj in self.objectives:
            if obj['type'] == objective_type and obj['target'].lower() == target_name.lower():
                obj['current'] = min(obj['required'], obj['current'] + 1)

class QuestJournal:
    """Manages a character's quests.

    Attributes:
        active_quests (list): A list of active quests.
        completed_quests (list): A list of completed quests.
        owner_name (str): The name of the journal's owner.
    """
    def __init__(self, owner_name="Unknown"):
        self.active_quests = []
        self.completed_quests = []
        self.owner_name = owner_name

    def add_quest(self, quest):
        """Adds a quest to the journal.

        Args:
            quest (Quest): The quest to add.
        """
        quest.status = "Active"
        self.active_quests.append(quest)

    def complete_quest(self, quest):
        """Moves a quest from active to completed.

        Args:
            quest (Quest): The quest to complete.
        """
        quest.status = "Completed"
        self.active_quests.remove(quest)
        self.completed_quests.append(quest)

    def display(self):
        """Displays the quest journal."""
        print(f"--- {self.owner_name}'s Quest Journal ---")
        print("\n-- Active Quests --")
        if not self.active_quests:
            print("None")
        for q in self.active_quests:
            print(f"- {q.title} ({q.objectives[0]['current']}/{q.objectives[0]['required']})")

class Character(GameObject):
    """The base class for all characters in the game.

    Attributes:
        inventory (Inventory): The character's inventory.
        journal (QuestJournal): The character's quest journal.
        level (int): The character's current level.
        xp (int): The character's current experience points.
        xp_to_next_level (int): The experience points required for the next level.
        xp_value (int): The experience points awarded for defeating the character.
        equipped_weapon (Weapon): The character's equipped weapon.
        base_attack_damage (int): The character's base attack damage.
        abilities (list): A list of all possible abilities for the character.
        unlocked_abilities (list): A list of abilities the character has unlocked.
    """
    def __init__(self, name="Character", x=0, y=0, health=100):
        super().__init__(name, x, y, health)
        self.inventory = Inventory(owner_name=self.name)
        self.journal = QuestJournal(owner_name=self.name)
        self.level = 1
        self.xp = 0
        self.xp_to_next_level = 100
        self.xp_value = 50
        self.equipped_weapon = None
        self.base_attack_damage = 5
        self.abilities = []
        self.unlocked_abilities = []

    def __str__(self):
        return f"{self.name} (Lvl {self.level}) | HP: {self.health}/{self.max_health}"

    def gain_xp(self, amount):
        """Gains experience and checks for level up.

        Args:
            amount (int): The amount of experience to gain.
        """
        self.xp += amount
        print(f"{self.name} gains {amount} XP!")
        if self.xp >= self.xp_to_next_level:
            self.level_up()

    def level_up(self):
        """Levels up the character, increasing stats and learning new abilities."""
        while self.xp >= self.xp_to_next_level:
            self.level += 1
            self.xp -= self.xp_to_next_level
            self.xp_to_next_level = int(self.xp_to_next_level * 1.5)
            self.max_health += 10
            self.health = self.max_health
            print(f"{self.name} has reached Level {self.level}!")
        for ability, required_level in self.abilities:
            if self.level >= required_level and not any(a.name == ability.name for a in self.unlocked_abilities):
                self.unlocked_abilities.append(ability)
                print(f"{self.name} has learned the ability: {ability.name}!")

class Player(Character):
    """A player-controlled character."""
    def __init__(self, name="Player", x=0, y=0, health=100):
        super().__init__(name, x, y, health)
        self.abilities = [
            (TargetedDamageAbility("Power Strike", "A strong melee attack.", 10, "Stamina", 25), 3),
            (TargetedDamageAbility("Fireball", "A basic fire spell.", 20, "Mana", 40), 5)
        ]

class Enemy(Character):
    """A basic enemy character."""
    def __init__(self, name="Enemy", x=0, y=0, health=50, xp_value=50):
        super().__init__(name, x, y, health)
        self.xp_value = xp_value

def run_level_up_demonstration():
    """Shows the level-up system in action."""
    print("\n--- Level-Up and Ability Learning Demonstration ---")
    player = Player(name="Hero")
    enemy = Enemy(name="Goblin", xp_value=80)
    print("\n--- Initial State ---")
    print(player)
    print(f"Unlocked abilities: {[a.name for a in player.unlocked_abilities]}")
    print(f"\n--- {player.name} defeats {enemy.name}! ---")
    player.gain_xp(enemy.xp_value)
    print(player)
    print("\n--- Hero completes a major quest! ---")
    player.gain_xp(740)
    print(player)
    print(f"Unlocked abilities: {[a.name for a in player.unlocked_abilities]}")
    assert "Power Strike" in [a.name for a in player.unlocked_abilities]
    assert "Fireball" in [a.name for a in player.unlocked_abilities]
    print("\nDemonstration complete. Abilities learned as expected.")

if __name__ == "__main__":
    """Main execution block to run the level-up demonstration."""
    run_level_up_demonstration()