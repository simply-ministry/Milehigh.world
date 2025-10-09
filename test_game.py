import unittest
from unittest.mock import patch

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
            self.skyix.cast_ability("fireball", self.bandit)
            mock_print.assert_called_with(f"{self.skyix.name} does not know the ability 'fireball'.")


if __name__ == '__main__':
    unittest.main()