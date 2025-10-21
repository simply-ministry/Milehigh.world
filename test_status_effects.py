import unittest
from unittest.mock import MagicMock
import os
from game import Player, Enemy, PoisonDart, Game, Scene, AethelgardBattle
from database import init_db

class TestStatusEffects(unittest.TestCase):
    """Test suite for status effects."""

    DB_FILE = "test_status_effects.db"

    def setUp(self):
        """Set up a controlled game environment for each test."""
        init_db(self.DB_FILE)
        self.game = Game()
        self.scene = Scene("Test Scene")
        # The AethelgardBattle setup is complex, so we'll manually create objects
        self.player = Player(name="Test Player", x=0, y=0)
        self.enemy = Enemy(name="Test Dummy", x=1, y=0)
        self.scene.set_player(self.player)
        self.scene.add_object(self.enemy)
        # Mock the scene manager to avoid running the full game loop
        self.scene_manager = MagicMock()
        self.scene_manager.scene = self.scene
        self.game.log_message = MagicMock()

    def tearDown(self):
        """Clean up the temporary database after each test."""
        os.remove(self.DB_FILE)

    def test_poison_effect_damages_over_time(self):
        """Tests that the 'poison' status effect correctly deals damage each turn."""
        initial_health = self.enemy.health
        poison_potency = 5
        poison_duration = 3

        # Apply poison
        self.enemy.apply_status_effect('poison', duration=poison_duration, potency=poison_potency)
        self.assertEqual(len(self.enemy.status_effects), 1)

        # Simulate 1st turn
        self.enemy.update(self.scene_manager)
        self.assertEqual(self.enemy.health, initial_health - poison_potency)
        self.assertEqual(self.enemy.status_effects['poison']['duration'], poison_duration - 1)

        # Simulate 2nd turn
        self.enemy.update(self.scene_manager)
        self.assertEqual(self.enemy.health, initial_health - poison_potency * 2)
        self.assertEqual(self.enemy.status_effects['poison']['duration'], poison_duration - 2)

        # Simulate 3rd turn (effect wears off)
        self.enemy.update(self.scene_manager)
        self.assertEqual(self.enemy.health, initial_health - poison_potency * 3)
        self.assertNotIn('poison', self.enemy.status_effects)

    def test_stun_effect_prevents_action(self):
        """Tests that a 'stun' status effect prevents an enemy from acting."""
        # Give the enemy a stun effect for 1 turn
        self.enemy.apply_status_effect('stun', duration=1)

        # Mock the enemy's attack method to see if it gets called
        self.enemy.attack = MagicMock()

        # Run the enemy's update logic
        self.enemy.update(self.scene_manager)

        # The attack method should NOT have been called
        self.enemy.attack.assert_not_called()

        # After the update, the stun should have worn off
        self.assertNotIn('stun', self.enemy.status_effects)

        # On the next turn, the enemy should be able to attack again
        self.enemy.update(self.scene_manager)
        self.enemy.attack.assert_called_once_with(self.player)

    def test_use_poison_dart_item(self):
        """Tests that using a PoisonDart correctly applies the poison effect to a target."""
        dart = PoisonDart(potency=10, duration=2)
        self.player.inventory.append(dart)

        # Use the dart on the enemy
        self.player.use_item("Poison Dart", self.enemy)

        # Check that the enemy is poisoned
        self.assertIn('poison', self.enemy.status_effects)
        self.assertEqual(self.enemy.status_effects['poison']['potency'], 10)
        self.assertEqual(self.enemy.status_effects['poison']['duration'], 2)

        # Check that the dart was consumed from the player's inventory
        self.assertNotIn(dart, self.player.inventory)

if __name__ == '__main__':
    unittest.main()