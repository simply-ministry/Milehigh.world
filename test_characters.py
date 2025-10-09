import unittest
from game import Skyix, Anastasia, Micah, Player, Enemy

class TestNewCharacters(unittest.TestCase):

    def setUp(self):
        """Set up test fixtures before each test method."""
        self.skyix = Skyix()
        self.anastasia = Anastasia()
        self.micah = Micah()
        self.ally = Player(name="Ally")
        self.enemy = Enemy(name="Test Enemy")

    # --- Sky.ix Tests ---

    def test_skyix_initialization(self):
        """Test that Sky.ix initializes with the correct stats."""
        self.assertEqual(self.skyix.health, 120)
        self.assertEqual(self.skyix.mana, 150)
        self.assertIn("void_tech", self.skyix.spells)

    def test_skyix_deploy_drone_success(self):
        """Test that Sky.ix can deploy a drone with sufficient mana."""
        initial_mana = self.skyix.mana
        self.skyix.deploy_drone()
        self.assertEqual(self.skyix.active_drones, 1)
        self.assertLess(self.skyix.mana, initial_mana)

    def test_skyix_deploy_drone_insufficient_mana(self):
        """Test that Sky.ix cannot deploy a drone without enough mana."""
        self.skyix.mana = 10
        initial_drones = self.skyix.active_drones
        self.skyix.deploy_drone()
        self.assertEqual(self.skyix.active_drones, initial_drones)

    def test_skyix_cast_spell_void_tech(self):
        """Test that Sky.ix can cast her unique spell."""
        initial_enemy_health = self.enemy.health
        self.skyix.cast_spell("void_tech", self.enemy)
        self.assertLess(self.enemy.health, initial_enemy_health)

    # --- Anastasia Tests ---

    def test_anastasia_initialization(self):
        """Test that Anastasia initializes with the correct stats."""
        self.assertEqual(self.anastasia.health, 90)
        self.assertEqual(self.anastasia.mana, 200)

    def test_anastasia_weave_dream_heal(self):
        """Test Anastasia's healing ability."""
        self.ally.health = 50
        self.anastasia.weave_dream(self.ally, is_healing=True)
        self.assertEqual(self.ally.health, 75)

    def test_anastasia_weave_dream_harm(self):
        """Test Anastasia's harming ability."""
        initial_enemy_health = self.enemy.health
        self.anastasia.weave_dream(self.enemy, is_healing=False)
        self.assertLess(self.enemy.health, initial_enemy_health)

    def test_anastasia_glimpse_future(self):
        """Test Anastasia's utility skill."""
        initial_visions = len(self.anastasia.visions)
        self.anastasia.glimpse_future()
        self.assertGreater(len(self.anastasia.visions), initial_visions)

    # --- Micah Tests ---

    def test_micah_initialization(self):
        """Test that Micah initializes with the correct stats."""
        self.assertEqual(self.micah.health, 200)
        self.assertEqual(self.micah.fortitude, 0)
        self.assertEqual(self.micah.mana, 0)

    def test_micah_gain_fortitude_on_damage(self):
        """Test that Micah gains Fortitude when he takes damage."""
        self.micah.take_damage(50)
        self.assertEqual(self.micah.fortitude, 50)

    def test_micah_adamantine_skin_damage_reduction(self):
        """Test that Adamantine Skin reduces incoming damage."""
        self.micah.fortitude = 100
        self.micah.activate_adamantine_skin()
        initial_health = self.micah.health
        self.micah.take_damage(50)
        # Damage should be halved (25), so health should be initial_health - 25
        self.assertEqual(self.micah.health, initial_health - 25)

    def test_micah_earthen_smash(self):
        """Test that Earthen Smash deals damage and consumes Fortitude."""
        self.micah.fortitude = 50
        initial_enemy_health = self.enemy.health
        self.micah.earthen_smash(self.enemy)
        self.assertLess(self.enemy.health, initial_enemy_health)
        self.assertLess(self.micah.fortitude, 50)

if __name__ == '__main__':
    unittest.main()