import unittest
from simple_rpg import Character, Player, TargetedDamageAbility

class TestLevelUpSystem(unittest.TestCase):
    """
    Test suite for the consolidated level-up and ability learning system.
    """

    def setUp(self):
        """Set up a fresh player character for each test."""
        self.player = Player(name="Test Hero")

    def test_initial_state(self):
        """Tests that a new character has the correct initial attributes."""
        self.assertEqual(self.player.level, 1)
        self.assertEqual(self.player.xp, 0)
        self.assertEqual(self.player.xp_to_next_level, 100)
        self.assertEqual(self.player.max_health, 100)
        self.assertEqual(len(self.player.unlocked_abilities), 0)

    def test_gain_xp_no_level_up(self):
        """Test gaining XP without leveling up."""
        self.player.gain_xp(50)
        self.assertEqual(self.player.level, 1)
        self.assertEqual(self.player.xp, 50)

    def test_single_level_up(self):
        """Test leveling up once."""
        self.player.gain_xp(100)
        self.assertEqual(self.player.level, 2)
        self.assertEqual(self.player.xp, 0)
        self.assertEqual(self.player.xp_to_next_level, 150)
        self.assertEqual(self.player.max_health, 110)
        self.assertEqual(self.player.health, 110) # Should be fully healed

    def test_multiple_level_ups(self):
        """Test gaining enough XP to level up multiple times at once."""
        # XP needed for Lvl 2: 100
        # XP needed for Lvl 3: 150 (Total: 250)
        # XP needed for Lvl 4: 225 (Total: 475)
        self.player.gain_xp(500)
        self.assertEqual(self.player.level, 4)
        self.assertEqual(self.player.xp, 25) # 500 - 475
        self.assertEqual(self.player.max_health, 130) # 100 + 10*3
        self.assertEqual(self.player.health, 130)
        self.assertEqual(self.player.xp_to_next_level, int(225 * 1.5)) # XP for Lvl 5

    def test_ability_learning_on_level_up(self):
        """Test that abilities are learned at the correct levels."""
        # Level 3 is required for "Power Strike"
        self.player.gain_xp(250) # Gain enough XP for level 3
        self.assertEqual(self.player.level, 3)
        unlocked_names = [a.name for a in self.player.unlocked_abilities]
        self.assertIn("Power Strike", unlocked_names)
        self.assertNotIn("Fireball", unlocked_names)

    def test_learning_multiple_abilities(self):
        """Test learning all defined abilities after sufficient level-ups."""
        # Level 5 is required for "Fireball"
        self.player.gain_xp(820) # Should be enough for level 5
        self.assertGreaterEqual(self.player.level, 5)
        unlocked_names = [a.name for a in self.player.unlocked_abilities]
        self.assertIn("Power Strike", unlocked_names)
        self.assertIn("Fireball", unlocked_names)

    def test_no_duplicate_abilities(self):
        """Test that an ability is not learned more than once."""
        # Level up to 3 to learn "Power Strike"
        self.player.gain_xp(250)
        self.assertEqual(len(self.player.unlocked_abilities), 1)
        # Gain more XP, but not enough to reach the next ability threshold
        self.player.gain_xp(100)
        self.assertEqual(len(self.player.unlocked_abilities), 1)

if __name__ == '__main__':
    unittest.main()