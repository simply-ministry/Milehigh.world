import unittest
from unittest.mock import patch

# Import the specific character classes to be tested
from game import Skyix, Kane, TargetedDamageAbility

class TestSkyix(unittest.TestCase):
    """
    Test suite for the specific implementation of the Skyix character.
    """
    def setUp(self):
        """Set up a fresh Skyix instance for each test."""
        self.skyix = Skyix()

    def test_skyix_initialization(self):
        """Test that Skyix initializes with her correct, unique stats."""
        self.assertEqual(self.skyix.name, "Sky.ix the Bionic Goddess")
        # Health is 120 base + 10 from the initial level up in __init__
        self.assertEqual(self.skyix.health, 130)
        self.assertEqual(self.skyix.max_health, 130)
        self.assertEqual(self.skyix.void_energy, 100)
        self.assertEqual(self.skyix.max_void_energy, 100)
        self.assertEqual(len(self.skyix.abilities), 2) # She should have two potential abilities

    def test_str_representation(self):
        """Test the custom __str__ method to ensure it includes void_energy."""
        # The __init__ method triggers a level up, so health becomes 130/130
        expected_str = "Sky.ix the Bionic Goddess (HP: 130/130, Void Energy: 100/100)"
        self.assertEqual(str(self.skyix), expected_str)

    def test_initial_level_up_and_ability_learning(self):
        """
        Test that the level_up call in __init__ correctly sets the initial state
        and learns the level 1 ability.
        """
        # The __init__ calls level_up, which bumps level to 2 and learns the first skill.
        # This is a bit of a quirk in the current implementation.
        # The test will validate this behavior.
        skyix_fresh = Skyix()
        self.assertEqual(skyix_fresh.level, 2)
        self.assertEqual(len(skyix_fresh.unlocked_abilities), 1)
        self.assertEqual(skyix_fresh.unlocked_abilities[0].name, "Void Tech")
        # Ensure the level up also correctly sets health
        self.assertEqual(skyix_fresh.health, skyix_fresh.max_health)
        self.assertEqual(skyix_fresh.max_health, 130) # 120 base + 10 from level up

class TestKane(unittest.TestCase):
    """
    Test suite for the specific implementation of the Kane character.
    """
    def setUp(self):
        """Set up a fresh Kane instance for each test."""
        self.kane = Kane(name="Test Kane", x=5, y=5)

    def test_kane_initialization(self):
        """Test that Kane initializes with the correct stats."""
        self.assertEqual(self.kane.name, "Test Kane")
        self.assertEqual(self.kane.health, 150)
        self.assertEqual(self.kane.max_health, 150)
        self.assertEqual(self.kane.damage, 20)
        self.assertIsInstance(self.kane, Kane)

    def test_kane_attack(self):
        """Test Kane's attack method."""
        target = Skyix() # Use another character as a target
        initial_target_health = target.health
        with patch('builtins.print') as mock_print:
            self.kane.attack(target)
            self.assertEqual(target.health, initial_target_health - self.kane.damage)
            mock_print.assert_any_call(f"{self.kane.name} attacks {target.name}!")
            mock_print.assert_any_call(f"{target.name} takes {self.kane.damage} damage.")


if __name__ == '__main__':
    unittest.main()