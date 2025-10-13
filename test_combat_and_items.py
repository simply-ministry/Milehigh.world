from unittest.mock import patch
import unittest
from rpg import Player, Enemy, Weapon, Armor, HealthPotion, Scene, SceneManager

class TestCombatAndItems(unittest.TestCase):

    def setUp(self):
        """Set up a fresh game state for each test."""
        self.player = Player(name="Test Player", x=0, y=0)
        self.enemy = Enemy(name="Test Enemy", x=1, y=0)
        self.enemy.health = 100 # Set a known health value

        # Mock a scene and scene manager to pass to update methods
        self.scene = Scene("Test Arena")
        self.scene.set_player(self.player)
        self.scene.add_object(self.enemy)

        # A minimal SceneManager that doesn't do anything on its own
        class MockSceneManager(SceneManager):
            def setup(self):
                pass
            def update(self):
                pass

        self.scene_manager = MockSceneManager(self.scene, game=None, setup_scene=False)

    def test_player_attack_bare_hands(self):
        """Tests a player's attack without a weapon."""
        self.player.strength = 10 # 10 strength = +5 damage bonus
        # Attack damage should be strength_bonus (5) + weapon_damage (0)
        self.player.attack(self.enemy)
        # 100 initial health - 5 damage
        self.assertEqual(self.enemy.health, 95)

    @patch('rpg.random.uniform', return_value=100)
    def test_player_attack_with_weapon(self, mock_uniform):
        """Tests a player's attack with an equipped weapon."""
        self.player.strength = 10 # +5 damage bonus
        weapon = Weapon("Test Sword", "A sword for testing.", damage=15)
        self.player.equipment.equip(weapon)
        # Attack damage should be strength_bonus (5) + weapon_damage (15) = 20
        self.player.attack(self.enemy)
        # 100 initial health - 20 damage
        self.assertEqual(self.enemy.health, 80)

    def test_enemy_take_damage_with_defense(self):
        """Tests that damage taken by an enemy is reduced by their defense."""
        self.enemy.defense = 5
        # Incoming damage is 20, but defense is 5
        self.enemy.take_damage(20)
        # 100 initial health - (20 - 5) damage
        self.assertEqual(self.enemy.health, 85)

    def test_enemy_attack(self):
        """Tests a basic enemy attack against the player."""
        self.player.health = 100 # Set a known health value
        self.enemy.attack_damage = 10
        self.enemy.attack(self.player)
        self.assertEqual(self.player.health, 90)

    def test_enemy_ai_moves_towards_player(self):
        """Tests that an enemy moves towards the player when in aggro range."""
        self.enemy.x = 5
        self.enemy.y = 5
        self.player.x = 10
        self.player.y = 5
        self.enemy.aggro_range = 10

        # The enemy should move 1 unit towards the player on the x-axis
        self.enemy.update(self.scene_manager)
        self.assertEqual(self.enemy.x, 6)
        self.assertEqual(self.enemy.y, 5)

    def test_player_pickup_item(self):
        """Tests that a player can pick up an item."""
        item = Weapon("Test Dagger", "A dagger for testing.", damage=5)
        self.player.pickup_item(item)
        self.assertIn(item, self.player.inventory)
        self.assertFalse(item.visible) # Item should become invisible after pickup
        self.assertFalse(item.solid) # Item should become non-solid after pickup

    def test_player_equip_weapon_and_armor(self):
        """Tests that a player can equip a weapon and armor."""
        weapon = Weapon("Steel Sword", "A basic sword.", damage=10)
        armor = Armor("Leather Armor", "Basic protection.", defense=5)
        self.player.inventory.extend([weapon, armor])

        self.player.equip_item("Steel Sword")
        self.assertEqual(self.player.equipment.slots["weapon"], weapon)

        self.player.equip_item("Leather Armor")
        self.assertEqual(self.player.equipment.slots["armor"], armor)

    def test_attack_with_equipped_gear(self):
        """Tests that equipped weapon and armor correctly affect combat."""
        self.player.strength = 10 # +5 damage bonus
        self.player.health = 100
        self.enemy.defense = 2
        self.enemy.attack_damage = 12

        # Equip gear
        weapon = Weapon("Mighty Axe", "An axe.", damage=20)
        armor = Armor("Iron Plate", "Good protection.", defense=8)
        self.player.inventory.extend([weapon, armor])
        self.player.equip_item("Mighty Axe")
        self.player.equip_item("Iron Plate")

        # Test player's attack on enemy
        # Damage = (weapon_damage(20) + strength(5)) - enemy_defense(2) = 23
        self.player.attack(self.enemy)
        self.assertEqual(self.enemy.health, 100 - 23)

        # Test enemy's attack on player
        # Damage = enemy_attack(12) - player_armor(8) = 4
        self.enemy.attack(self.player)
        self.assertEqual(self.player.health, 100 - 4)

    def test_use_health_potion(self):
        """Tests that using a health potion restores player's health."""
        self.player.health = 50
        self.player.max_health = 100
        potion = HealthPotion(amount=30)
        self.player.inventory.append(potion)

        self.player.use_item("Health Potion")
        self.assertEqual(self.player.health, 80)
        # The potion should be consumed
        self.assertNotIn(potion, self.player.inventory)

if __name__ == '__main__':
    unittest.main()