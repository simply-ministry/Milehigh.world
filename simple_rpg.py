import time
import random
import math

# --- 1. ALL CORE CLASSES (PASTED FROM PREVIOUS STEPS) ---

class GameObject:
    def __init__(self, name="GameObject", x=0, y=0, health=100):
        self.name = name
        self.x = x
        self.y = y
        self.max_health = health
        self.health = health
        self.is_alive = True
    def move(self, dx, dy):
        self.x += dx
        self.y += dy
    def heal(self, amount):
        self.health = min(self.max_health, self.health + amount)
        print(f"{self.name} heals for {amount}, now at {self.health} HP.")
    def take_damage(self, amount):
        self.health -= amount
        if self.health <= 0:
            self.health = 0
            self.is_alive = False
            print(f"{self.name} has been defeated!")
        else:
            print(f"{self.name} takes {amount:.0f} damage, {self.health:.0f} HP remaining.")
    def update(self):
        pass # Placeholder for future logic

class Item:
    def __init__(self, name, description):
        self.name = name
        self.description = description
    def __str__(self):
        return f"{self.name}: {self.description}"

class Weapon(Item):
    def __init__(self, name, description, damage):
        super().__init__(name, description)
        self.damage = damage
    def __str__(self):
        return f"{self.name} (Weapon, +{self.damage} DMG)"

class Consumable(Item):
    def __init__(self, name, description, effect, value):
        super().__init__(name, description)
        self.effect = effect
        self.value = value
    def __str__(self):
        return f"{self.name} (Consumable)"
    def use(self, character):
        print(f"{character.name} uses {self.name}!")
        if self.effect == "heal":
            character.heal(self.value)

class Inventory:
    def __init__(self, capacity=20):
        self.items = []
        self.capacity = capacity
        self.owner_name = "Unknown"
    def add_item(self, item):
        if len(self.items) < self.capacity: self.items.append(item)
    def remove_item(self, item_name):
        self.items = [item for item in self.items if item.name != item_name]
    def list_items(self):
        print(f"--- {self.owner_name}'s Inventory ---")
        if not self.items: print("Empty")
        else:
            for item in self.items: print(f"- {item}")
        print("-----------------")

class Ability:
    def __init__(self, name, description, cost, cost_type):
        self.name = name
        self.description = description
        self.cost = cost
        self.cost_type = cost_type
    def use(self, caster, target):
        print("Ability base class cannot be used.")

class TargetedDamageAbility(Ability):
    def __init__(self, name, description, cost, cost_type, damage):
        super().__init__(name, description, cost, cost_type)
        self.damage = damage
    def use(self, caster, target):
        print(f"{caster.name} uses {self.name} on {target.name}!")
        target.take_damage(self.damage)

class Quest:
    def __init__(self, title, description, objectives):
        self.title = title
        self.description = description
        self.objectives = objectives
        self.status = "Inactive"
    def is_complete(self):
        return all(obj['current'] >= obj['required'] for obj in self.objectives)
    def update_objective(self, objective_type, target_name):
        for obj in self.objectives:
            if obj['type'] == objective_type and obj['target'].lower() == target_name.lower():
                obj['current'] = min(obj['required'], obj['current'] + 1)

class QuestJournal:
    def __init__(self, owner_name="Unknown"):
        self.active_quests = []
        self.completed_quests = []
        self.owner_name = owner_name
    def add_quest(self, quest):
        quest.status = "Active"
        self.active_quests.append(quest)
    def complete_quest(self, quest):
        quest.status = "Completed"
        self.active_quests.remove(quest)
        self.completed_quests.append(quest)
    def display(self):
        print(f"--- {self.owner_name}'s Quest Journal ---")
        print("\n-- Active Quests --")
        if not self.active_quests: print("None")
        for q in self.active_quests: print(f"- {q.title} ({q.objectives[0]['current']}/{q.objectives[0]['required']})")

# --- CHARACTER BASE CLASS (Fully Integrated) ---

class Character(GameObject):
    def __init__(self, name="Character", x=0, y=0, health=100, state=None):
        super().__init__(name, x, y, health)
        self.state = state
        self.inventory = Inventory()
        self.inventory.owner_name = self.name
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
        return f"{self.name} (Lvl {self.level}) | HP: {self.health}/{self.max_health} | XP: {self.xp}/{self.xp_to_next_level}"
    def gain_xp(self, amount):
        self.xp += amount
        print(f"{self.name} gains {amount} XP!")
        if self.xp >= self.xp_to_next_level: self.level_up()
    def level_up(self):
        """
        Handles the character's level-up process, including multiple levels at once.
        Increases stats, heals the character, and checks for new abilities.
        """
        while self.xp >= self.xp_to_next_level:
            self.level += 1
            self.xp -= self.xp_to_next_level
            self.xp_to_next_level = int(self.xp_to_next_level * 1.5)
            self.max_health += 10
            self.health = self.max_health # Fully heal on level up
            print(f"{self.name} has reached Level {self.level}!")
        # Check for new abilities after all level-ups are processed
        for ability, required_level in self.abilities:
            if self.level >= required_level:
                self.learn_ability(ability)
    def learn_ability(self, ability):
        """Adds a new ability to the character's unlocked list, avoiding duplicates."""
        if not any(a.name == ability.name for a in self.unlocked_abilities):
            self.unlocked_abilities.append(ability)
            print(f"{self.name} has learned the ability: {ability.name}!")

# --- 2. PLAYER AND ENEMY SUBCLASSES (FOR DEMONSTRATION) ---

class Player(Character):
    """A player-controlled character."""
    def __init__(self, name="Player", x=0, y=0, health=100):
        super().__init__(name, x, y, health)
        # Define a list of abilities the player can learn and at what level
        self.abilities = [
            (TargetedDamageAbility("Power Strike", "A strong melee attack.", 10, "Stamina", 25), 3),
            (TargetedDamageAbility("Fireball", "A basic fire spell.", 20, "Mana", 40), 5)
        ]

class Enemy(Character):
    """A basic enemy character."""
    def __init__(self, name="Enemy", x=0, y=0, health=50, xp_value=50):
        super().__init__(name, x, y, health)
        self.xp_value = xp_value # How much XP the player gets for defeating it

# --- 3. DEMONSTRATION FUNCTION ---

def run_level_up_demonstration():
    """
    Shows the level-up system in action, including gaining multiple levels
    and learning new abilities.
    """
    print("\n--- Level-Up and Ability Learning Demonstration ---")

    # 1. Create a player and an enemy
    player = Player(name="Hero")
    enemy = Enemy(name="Goblin", xp_value=80)

    print("\n--- Initial State ---")
    print(player)
    print(f"Unlocked abilities: {[a.name for a in player.unlocked_abilities]}")

    # 2. Simulate defeating an enemy to gain XP (not enough to level up)
    print(f"\n--- {player.name} defeats {enemy.name}! ---")
    player.gain_xp(enemy.xp_value)
    print(player)

    # 3. Simulate gaining a large amount of XP to trigger multiple level-ups
    print("\n--- Hero completes a major quest! ---")
    player.gain_xp(740) # This should trigger levels 2, 3, 4, and 5
    print(player)
    print(f"Unlocked abilities: {[a.name for a in player.unlocked_abilities]}")

    # 4. Verify that abilities were learned correctly
    unlocked_names = [a.name for a in player.unlocked_abilities]
    assert "Power Strike" in unlocked_names
    assert "Fireball" in unlocked_names
    print("\nDemonstration complete. Abilities learned as expected.")

if __name__ == "__main__":
    run_level_up_demonstration()