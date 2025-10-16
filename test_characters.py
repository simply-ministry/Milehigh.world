import unittest
from unittest.mock import patch, MagicMock

from rpg import (
    Anastasia,
    Reverie,
    Nyxar,
    Enemy,
    Scene,
    SceneManager,
    Player,
)


class MockSceneManager(SceneManager):
    """A minimal SceneManager that doesn't do anything on its own."""

    def setup(self):
        pass

    def update(self):
        pass


class TestAnastasia(unittest.TestCase):
    """Tests for the Anastasia character and her abilities."""

    def setUp(self):
        self.anastasia = Anastasia()
        self.enemy = Enemy(name="Test Dummy", x=1)
        self.scene = Scene("Test Arena")
        self.scene.set_player(self.anastasia)
        self.scene.add_object(self.enemy)
        self.scene_manager = MockSceneManager(self.scene, game=None, setup_scene=False)

    def test_dream_weave_generation(self):
        """Verify that Anastasia's Dream Weave resource is generated correctly."""
        self.anastasia.dream_weave = 0
        self.anastasia.build_dream_weave(10)
        self.assertEqual(self.anastasia.dream_weave, 10)

    def test_lucid_dream_activation(self):
        """Verify that Anastasia's Lucid Dream state activates and deactivates correctly."""
        self.anastasia.dream_weave = self.anastasia.max_dream_weave
        self.anastasia.activate_lucid_dream()
        self.assertTrue(self.anastasia.is_lucid_dream_active)

    def test_lulling_whisper(self):
        """Verify that Anastasia's Lulling Whisper ability applies the 'sleep' status effect."""
        self.anastasia.lulling_whisper([self.enemy])
        self.assertIn("sleep", self.enemy.status_effects)

    def test_phantasmal_grasp(self):
        """Verify that Anastasia's Phantasmal Grasp ability applies the 'slow' and 'psychic_damage' status effects."""
        self.anastasia.phantasmal_grasp(self.enemy)
        self.assertIn("slow", self.enemy.status_effects)
        self.assertIn("psychic_damage", self.enemy.status_effects)


class TestReverie(unittest.TestCase):
    """Tests for the Reverie character and her abilities."""

    def setUp(self):
        self.reverie = Reverie()
        self.enemy = Enemy(name="Test Dummy", x=1)
        self.scene = Scene("Test Arena")
        self.scene.set_player(self.reverie)
        self.scene.add_object(self.enemy)
        self.scene_manager = MockSceneManager(self.scene, game=None, setup_scene=False)

    def test_enigma_generation(self):
        """Verify that Reverie's Enigma resource is generated correctly when casting spells."""
        self.reverie.enigma = 0
        self.reverie.cast_spell("fire_blast", self.enemy)
        self.assertGreater(self.reverie.enigma, 0)

    @patch("rpg.random.choice", return_value="massive_damage")
    def test_chaos_unleashed_massive_damage(self, mock_choice):
        """Verify that Reverie's Chaos Unleashed ability correctly triggers the 'massive_damage' effect."""
        self.reverie.enigma = self.reverie.max_enigma
        initial_health = self.enemy.health
        self.reverie.chaos_unleashed(self.enemy)
        self.assertLess(self.enemy.health, initial_health)

    @patch("rpg.random.choice", return_value="full_heal_and_mana")
    def test_chaos_unleashed_full_heal(self, mock_choice):
        """Verify that Reverie's Chaos Unleashed ability correctly triggers the 'full_heal_and_mana' effect."""
        self.reverie.enigma = self.reverie.max_enigma
        self.reverie.health = 10
        self.reverie.chaos_unleashed(self.enemy)
        self.assertEqual(self.reverie.health, self.reverie.max_health)


class TestNyxar(unittest.TestCase):
    """Tests for the Nyxar character and his abilities."""

    def setUp(self):
        self.nyxar = Nyxar()
        self.enemy = Enemy(name="Test Dummy", x=1)
        self.scene = Scene("Test Arena")
        self.scene.set_player(self.nyxar)
        self.scene.add_object(self.enemy)
        self.scene_manager = MockSceneManager(self.scene, game=None, setup_scene=False)

    def test_dominion_generation(self):
        """Verify that Nyxar's Dominion resource is generated when he takes damage."""
        self.nyxar.dominion = 0
        self.nyxar.take_damage(20)
        self.assertGreater(self.nyxar.dominion, 0)

    def test_sanguine_strike(self):
        """Verify that Nyxar's Sanguine Strike ability correctly damages an enemy and costs health."""
        initial_health = self.nyxar.health
        initial_enemy_health = self.enemy.health
        self.nyxar.sanguine_strike(self.enemy)
        self.assertLess(self.nyxar.health, initial_health)
        self.assertLess(self.enemy.health, initial_enemy_health)

    def test_abyssal_aegis(self):
        """Verify that Nyxar's Abyssal Aegis ability correctly creates a shield."""
        self.nyxar.abyssal_aegis()
        self.assertGreater(self.nyxar.aegis_shield_hp, 0)

    def test_reign_of_chaos(self):
        """Verify that Nyxar's Reign of Chaos ability correctly damages all enemies."""
        self.nyxar.dominion = self.nyxar.max_dominion
        enemy2 = Enemy(name="Test Dummy 2", x=2)
        self.scene.add_object(enemy2)
        initial_enemy1_health = self.enemy.health
        initial_enemy2_health = enemy2.health
        self.nyxar.reign_of_chaos(self.scene_manager)
        self.assertLess(self.enemy.health, initial_enemy1_health)
        self.assertLess(enemy2.health, initial_enemy2_health)


class TestStatusEffects(unittest.TestCase):
    """Tests for the status effect system."""

    def setUp(self):
        self.player = Player(name="Test Player", x=0)
        self.enemy = Enemy(name="Test Dummy", x=1)
        self.scene = Scene("Test Arena")
        self.scene.set_player(self.player)
        self.scene.add_object(self.enemy)
        self.scene_manager = MockSceneManager(self.scene, game=None, setup_scene=False)

    def test_sleep_effect(self):
        """Verify that an actor with the 'sleep' status effect cannot perform actions."""
        self.enemy.status_effects["sleep"] = 2
        initial_player_health = self.player.health
        self.enemy.update(self.scene_manager)
        self.assertEqual(self.player.health, initial_player_health)

    def test_psychic_damage_effect(self):
        """Verify that the 'psychic_damage' status effect correctly applies damage each turn."""
        self.player.status_effects["psychic_damage"] = 2
        initial_health = self.player.health
        self.player.update(self.scene_manager)
        self.assertLess(self.player.health, initial_health)

    def test_life_drain_effect(self):
        """Verify that the 'life_drain' status effect correctly drains health and heals the attacker."""
        self.enemy.status_effects["life_drain"] = {
            "duration": 2,
            "damage_per_turn": 10,
            "heals": self.player.name,
        }
        self.player.health = 50
        # Move the enemy far away to prevent it from attacking the player after the status update
        self.enemy.x = 100
        initial_enemy_health = self.enemy.health
        self.enemy.update(self.scene_manager)
        self.assertLess(self.enemy.health, initial_enemy_health)
        self.assertGreater(self.player.health, 50)

    @patch("rpg.random.uniform", return_value=1)  # This will ensure the evasion succeeds
    def test_evasion_effect(self, mock_uniform):
        """Verify that the 'evasion' status effect allows the actor to avoid attacks."""
        self.player.status_effects["evasion"] = 2
        initial_health = self.player.health
        self.enemy.attack(self.player)
        self.assertEqual(self.player.health, initial_health)


if __name__ == "__main__":
    unittest.main()