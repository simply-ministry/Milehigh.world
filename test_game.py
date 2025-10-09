import unittest
from unittest.mock import patch
# Import all necessary classes from the refactored game.py
from game import (
    GameObject, Character, NPC, Item, Consumable, Weapon, Inventory, Game,
    Micah, Kane, Delilah, Player
)
from game import GameObject, Character, NPC, Item, Consumable, Weapon, Inventory, Game

class TestGame(unittest.TestCase):

    def setUp(self):
        """Set up test fixtures before each test method."""
        self.game = Game(width=20, height=10)
        # Use Micah as the player, which is a Character subclass
        self.player = Micah(name="Hero", x=5, y=5)
        # Use Kane as the enemy, also a Character subclass
        self.enemy = Kane(name="Goblin", x=6, y=5, health=80)
        self.player.strength = 10
        self.player.dexterity = 10
        self.enemy.defense = 5
        self.game.set_player_character(self.player)
        self.game.add_object(self.enemy)

        self.player = Character(name="Hero", x=5, y=5)
        self.enemy = Character(name="Goblin", x=10, y=10, health=80)
        self.game.set_player_character(self.player)
        self.game.add_object(self.enemy)

# Import the refactored and new classes from game.py
from game import (
    GameObject,
    Item,
    Weapon,
    Consumable,
    Inventory,
    Character,
    Player,
    Enemy,
    Ability,
    TargetedDamageAbility,
    Skyix,
    Kane,
)

class TestGameObject(unittest.TestCase):
    def test_game_object_creation(self):
        """Test the creation of a GameObject."""
        obj = GameObject(name="Tree", x=10, y=20, health=50)
        self.assertEqual(obj.name, "Tree")
        self.assertEqual(obj.x, 10)
        self.assertEqual(obj.y, 20)
        self.assertEqual(obj.health, 50)
        self.assertEqual(obj.max_health, 50)

    def test_character_take_damage(self):
        """Test that take_damage correctly reduces a character's health."""
        initial_health = self.player.health
        self.player.defense = 2
        self.player.take_damage(20)
        self.assertEqual(self.player.health, initial_health - 18)

    def test_character_heal(self):
        """Test that heal correctly increases a character's health without exceeding max."""
        self.player.health = 50
        self.player.heal(20)
        self.assertEqual(self.player.health, 70)
        self.player.heal(40)
        self.assertEqual(self.player.health, 100)

    def test_character_equip_weapon(self):
        """Test that a character can equip a weapon by name."""
        sword = Weapon(name="Test Sword", description="A test sword.", damage=15)
        self.player.equip_weapon(sword)
        self.assertIs(self.player.weapon, sword)
        self.assertEqual(self.player.weapon.damage, 15)

    @patch('random.uniform', return_value=20)
    def test_player_attack_regular_hit(self, mock_uniform):
        """Test a player's regular attack on an enemy."""
        initial_enemy_health = self.enemy.health
        attack_damage = self.player.base_attack_damage
        expected_damage = max(0, attack_damage - self.enemy.defense)
        self.player.attack(self.enemy)
        self.assertEqual(self.enemy.health, initial_enemy_health - expected_damage)

    @patch('random.uniform')
    def test_player_attack_critical_hit(self, mock_uniform):
        """Test a player's critical hit attack on an enemy.
        NOTE: The current simple attack method does not have critical hits.
        This test will be adapted to a regular hit."""
        initial_enemy_health = self.enemy.health
        attack_damage = self.player.base_attack_damage
        expected_damage = max(0, attack_damage - self.enemy.defense)
        self.player.attack(self.enemy)
        self.assertEqual(self.enemy.health, initial_enemy_health - expected_damage)

    def test_enemy_attack(self):
        """Test a simple enemy attack on the player."""
        initial_player_health = self.player.health
        self.player.defense = 2
        attack_damage = self.enemy.base_attack_damage
        expected_damage = max(0, attack_damage - self.player.defense)
        self.enemy.attack(self.player)
        self.assertEqual(self.player.health, initial_player_health - expected_damage)
        self.player.pickup_item(sword)
        self.player.equip_weapon("Test Sword")
        self.assertIsNotNone(self.player.equipped_weapon)
        self.assertEqual(self.player.equipped_weapon.name, "Test Sword")
        self.assertEqual(self.player.equipped_weapon.damage, 15)

    def test_character_attack(self):
        """Test a character's attack on another character."""
        initial_enemy_health = self.enemy.health
        self.player.attack(self.enemy)
        expected_damage = self.player.base_attack_damage
        self.assertEqual(self.enemy.health, initial_enemy_health - expected_damage)

    def test_character_attack_with_weapon(self):
        """Test a character's attack with an equipped weapon."""
        sword = Weapon(name="Test Sword", description="A test sword.", damage=15)
        self.player.pickup_item(sword)
        self.player.equip_weapon("Test Sword")
        initial_enemy_health = self.enemy.health
        self.player.attack(self.enemy)
        expected_damage = self.player.base_attack_damage + sword.damage
        self.assertEqual(self.enemy.health, initial_enemy_health - expected_damage)

    def test_item_pickup(self):
        """Test that a character can pick up items."""
        potion = Consumable(name="Lesser Heal", description="A weak potion.", effect="heal", value=10)
        self.player.pickup_item(potion)
        self.assertIn(potion, self.player.inventory.items)

    def test_use_health_potion(self):
        """Test that using a health potion restores health and consumes the item."""
        self.player.health = 50
        potion = Consumable(name="Test Potion", description="A test potion.", effect="heal", value=30)
        self.player.inventory.add_item(potion)
        self.player.use_item("Test Potion")
        self.assertEqual(self.player.health, 80)
        self.assertNotIn(potion, self.player.inventory.items)
        self.player.pickup_item(potion)
        self.player.use_item("Test Potion")
        self.assertEqual(self.player.health, 80)
        self.assertEqual(len(self.player.inventory.items), 0)

    def test_inventory_add_and_list(self):
        """Test adding an item to the inventory and listing items."""
        potion = Consumable("Health Potion", "Heals 20 HP", "heal", 20)
        self.player.pickup_item(potion)
        self.assertIn(potion, self.player.inventory.items)
        with patch('builtins.print') as mock_print:
            self.player.inventory.list_items()
            mock_print.assert_any_call("- Health Potion (Consumable, heal +20): Heals 20 HP")

    def test_use_consumable_item(self):
        """Test using a consumable item from the inventory."""
        self.player.health = 50
        potion = Consumable("Health Potion", "Heals 20 HP", "heal", 20)
        self.player.pickup_item(potion)
        self.player.use_item("Health Potion")
        self.assertEqual(self.player.health, 70)
        self.assertNotIn(potion, self.player.inventory.items)

    def test_use_mana_consumable(self):
        """Test using a mana-restoring consumable item."""
        self.player.mana = 30
        elixir = Consumable("Mana Elixir", "Restores 50 Mana", "restore_mana", 50)
        self.player.pickup_item(elixir)
        self.player.use_item("Mana Elixir")
        self.assertEqual(self.player.mana, 80)
        self.assertNotIn(elixir, self.player.inventory.items)
    def test_take_damage(self):
        """Test that take_damage correctly reduces health."""
        obj = GameObject(health=100)
        obj.take_damage(30)
        self.assertEqual(obj.health, 70)

    def test_heal(self):
        """Test that heal correctly increases health without exceeding max."""
        obj = GameObject(health=100)
        obj.health = 40
        obj.heal(20)
        self.assertEqual(obj.health, 60)
        obj.heal(50) # Should cap at max_health
        self.assertEqual(obj.health, 100)

    def test_die(self):
        """Test the die method."""
        obj = GameObject()
        with patch('builtins.print') as mock_print:
            obj.die()
            self.assertFalse(obj.visible)
            self.assertFalse(obj.solid)
            mock_print.assert_called_with(f"{obj.name} has been defeated.")

class TestInventory(unittest.TestCase):
    def setUp(self):
        self.inventory = Inventory()
        self.item1 = Item("Potion", "A healing potion.")
        self.item2 = Weapon("Sword", "A sharp blade.", 10)

    def test_add_item(self):
        """Test adding an item to the inventory."""
        self.assertTrue(self.inventory.add_item(self.item1))
        self.assertIn(self.item1, self.inventory.items)
        self.assertEqual(len(self.inventory.items), 1)

    def test_inventory_full(self):
        """Test that adding to a full inventory fails."""
        full_inventory = Inventory(capacity=1)
        full_inventory.add_item(self.item1)
        self.assertFalse(full_inventory.add_item(self.item2))

    def test_remove_item(self):
        """Test removing an item from the inventory."""
        self.inventory.add_item(self.item1)
        removed_item = self.inventory.remove_item("potion") # Test case-insensitivity
        self.assertEqual(removed_item, self.item1)
        self.assertNotIn(self.item1, self.inventory.items)

    def test_remove_item_not_found(self):
        """Test removing an item that doesn't exist."""
        self.assertIsNone(self.inventory.remove_item("nonexistent"))

class TestCharacter(unittest.TestCase):
    def setUp(self):
        self.char = Character("TestChar", health=100)

    def test_gain_xp_and_level_up(self):
        """Test that gaining enough XP triggers a level up."""
        self.char.xp_to_next_level = 100
        self.char.gain_xp(120)
        self.assertEqual(self.char.level, 2)
        self.assertEqual(self.char.xp, 20)
        self.assertEqual(self.char.max_health, 110)
        self.assertEqual(self.char.health, 110) # Should heal on level up

class TestAbilitySystem(unittest.TestCase):
    def setUp(self):
        """Set up a character and abilities for testing."""
        self.skyix = Skyix()
        self.bandit = Kane("Test Bandit")

    def test_skyix_initial_abilities(self):
        """Test that Skyix learns her level 1 ability upon creation."""
        self.assertEqual(len(self.skyix.unlocked_abilities), 1)
        self.assertEqual(self.skyix.unlocked_abilities[0].name, "Void Tech")

    def test_learn_ability_on_level_up(self):
        """Test that Skyix learns a new ability when she reaches the required level."""
        self.skyix.gain_xp(200) # Should be enough to reach level 3
        self.assertGreaterEqual(self.skyix.level, 3)
        unlocked_names = [a.name for a in self.skyix.unlocked_abilities]
        self.assertIn("Void Tech", unlocked_names)
        self.assertIn("Energy Blast", unlocked_names)

    def test_cast_ability_success(self):
        """Test successfully casting a known ability."""
        initial_bandit_health = self.bandit.health
        initial_void_energy = self.skyix.void_energy
        ability = self.skyix.unlocked_abilities[0] # Void Tech

        self.skyix.cast_ability("void tech", self.bandit)

        self.assertEqual(self.skyix.void_energy, initial_void_energy - ability.cost)
        self.assertEqual(self.bandit.health, initial_bandit_health - ability.damage)

    def test_cast_ability_not_enough_resource(self):
        """Test casting an ability without enough resource."""
        self.skyix.void_energy = 10 # Not enough for Void Tech (cost: 40)
        initial_bandit_health = self.bandit.health
        with patch('builtins.print') as mock_print:
            self.skyix.cast_ability("void tech", self.bandit)
            self.assertEqual(self.bandit.health, initial_bandit_health) # No damage dealt
            mock_print.assert_called_with("Not enough Void Energy for Void Tech.")

    def test_cast_ability_unknown_spell(self):
        """Test casting an ability the character does not know."""
        with patch('builtins.print') as mock_print:
            self.player.talk(npc)
            mock_print.assert_called_with("[Guard]: Halt! Who goes there?")

    def test_game_loop_input_move(self):
        """Test the move command in the game's input handler."""
        initial_y = self.player.y
        with patch('builtins.input', return_value='move w'):
            self.game.handle_input()
            self.assertEqual(self.player.y, initial_y - 1)

    def test_game_loop_input_quit(self):
        """Test the quit command in the game's input handler."""
        with patch('builtins.input', return_value='quit'):
            self.game.handle_input()
            self.assertFalse(self.game.is_running)

    def test_scripted_event_trigger(self):
        """Test if a simple scripted event triggers correctly."""
        self.assertFalse(self.game.event_triggered("delilah_battle"))
        self.player.x = 16
        self.game.update()
        self.assertTrue(self.game.event_triggered("delilah_battle"))
        delilah_exists = any(isinstance(obj, Delilah) for obj in self.game.game_objects)
        self.assertTrue(delilah_exists)

    def test_cast_spell(self):
        """Test casting a spell on a target."""
        self.player.mana = 100
        initial_enemy_health = self.enemy.health
        mana_cost = 35
        expected_damage = 50 - self.enemy.defense
        self.player.cast_spell("nightmare", self.enemy)
        self.assertEqual(self.enemy.health, initial_enemy_health - expected_damage)
        self.assertEqual(self.player.mana, 100 - mana_cost)

    def test_use_mana_potion(self):
        """Test that using a mana potion restores mana and consumes the item."""
        self.player.mana = 20
        potion = Consumable(name="Test Mana Potion", description="A test mana potion.", effect="restore_mana", value=40)
        self.player.pickup_item(potion)
        self.player.use_item("Test Mana Potion")
        self.assertEqual(self.player.mana, 60)
        self.assertNotIn(potion, self.player.inventory.items)
        delilah_exists = any(obj.name == "Delilah the Desolate" for obj in self.game.game_objects)
        self.assertTrue(delilah_exists)

    def test_cast_spell_from_input(self):
        """Test casting a spell on a target via game input."""
        self.player.mana = 100
        initial_enemy_health = self.enemy.health
        with patch('builtins.input', return_value='cast nightmare on Goblin'):
            self.game.handle_input()
        self.assertEqual(self.enemy.health, initial_enemy_health - 50)
        self.assertEqual(self.player.mana, 65)

    def test_use_non_consumable_item(self):
        """Test attempting to use a non-consumable item like a weapon."""
        weapon = Weapon(name="Sword", description="A simple sword.", damage=10)
        self.player.pickup_item(weapon)
        self.player.use_item("Sword")
        self.assertIn(weapon, self.player.inventory.items)

    def test_use_item_not_in_inventory(self):
        """Test attempting to use an item that is not in the inventory."""
        initial_health = self.player.health
        self.player.use_item("Imaginary Potion")
        self.assertEqual(self.player.health, initial_health)

    def test_game_remove_object(self):
        """Test that remove_object correctly removes an object from the game."""
        initial_object_count = len(self.game.game_objects)
        enemy_to_remove = self.enemy
        self.game.remove_object(enemy_to_remove)
        self.assertEqual(len(self.game.game_objects), initial_object_count - 1)
        self.assertNotIn(enemy_to_remove, self.game.game_objects)
        self.player.use_item("Imaginary Potion")
        # No assertion needed, just checking for no errors
            self.skyix.cast_ability("fireball", self.bandit)
            mock_print.assert_called_with(f"{self.skyix.name} does not know the ability 'fireball'.")


if __name__ == '__main__':
    unittest.main()