import unittest
from unittest.mock import patch

# Import all necessary classes from the refactored game.py
from game import (
    GameObject, Item, Weapon, Consumable, Character, Player,
    Enemy, Game, Scene, AethelgardBattle, HealthPotion
)

# --- Test Cases ---

class TestGameObject(unittest.TestCase):
    """Tests for the base GameObject class."""
    def test_game_object_creation(self):
        """Tests the initialization of a GameObject."""
        obj = GameObject(name="Tree", x=10, y=20, health=50)
        self.assertEqual(obj.name, "Tree")
        self.assertEqual(obj.x, 10)
        self.assertEqual(obj.y, 20)
        self.assertEqual(obj.health, 50)

    def test_take_damage(self):
        """Test that take_damage correctly reduces health."""
        obj = GameObject(health=100)
        obj.take_damage(30)
        self.assertEqual(obj.health, 70)


class TestCharacterAndPlayer(unittest.TestCase):
    """Tests for the Character and Player classes."""
    def setUp(self):
        """Sets up the test environment before each test."""
        self.player = Player(name="Hero")
        self.player.strength = 10
        self.player.dexterity = 10
        self.player.intelligence = 10
        self.enemy = Enemy(name="Goblin")

    def test_character_take_damage(self):
        """Test that take_damage correctly reduces a character's health."""
        initial_health = self.player.health
        self.player.take_damage(20)
        self.assertEqual(self.player.health, initial_health - 20)

    def test_equip_item(self):
        """Test that a character can equip a weapon."""
        sword = Weapon(name="Test Sword", description="A test sword.", damage=15)
        self.player.inventory.append(sword)
        self.player.equip_item(sword.name)
        self.assertIs(self.player.equipment.slots['weapon'], sword)

    @patch('random.uniform', return_value=20) # Ensures a hit
    def test_player_attack_regular_hit(self, mock_uniform):
        """Test a player's regular attack on an enemy."""
        sword = Weapon(name="Test Sword", description="A sword.", damage=10)
        self.player.inventory.append(sword)
        self.player.equip_item("Test Sword")
        initial_enemy_health = self.enemy.health
        self.player.attack(self.enemy)
        # Damage = weapon_damage + strength_bonus (10 // 2 = 5)
        self.assertEqual(self.enemy.health, initial_enemy_health - 15)

    @patch('random.uniform', return_value=5) # Ensures a critical hit (miss < 5 < crit)
    def test_player_attack_critical_hit(self, mock_uniform):
        """Test a player's critical hit attack on an enemy."""
        sword = Weapon(name="Test Sword", description="A sword.", damage=10)
        self.player.inventory.append(sword)
        self.player.equip_item("Test Sword")
        initial_enemy_health = self.enemy.health
        self.player.attack(self.enemy)
        # Damage = (weapon_damage + strength_bonus) * 2 = (10 + 5) * 2 = 30
        self.assertEqual(self.enemy.health, initial_enemy_health - 30)

    def test_item_pickup(self):
        """Test that a character can pick up items."""
        potion = Consumable(name="Lesser Heal", description="A weak potion.")
        scene = Scene("Test Scene")
        scene.add_object(potion)
        self.player.pickup_item(potion, scene)
        self.assertIn(potion, self.player.inventory)
        self.assertNotIn(potion, scene.game_objects)

    def test_use_health_potion(self):
        """Test that using a health potion restores health and consumes the item."""
        self.player.health = 50
        potion = HealthPotion(name="Test Potion", description="A test potion.", amount=30)
        self.player.inventory.append(potion)
        self.player.use_item("Test Potion", self.player)
        self.assertEqual(self.player.health, 80)
        self.assertEqual(len(self.player.inventory), 0)

    def test_gain_experience_and_level_up(self):
        """Test that gaining enough XP triggers a level up."""
        self.player.level = 1
        self.player.experience = 0
        self.player.gain_experience(100 * 1 * 1) # Exactly enough for level 2
        self.assertEqual(self.player.level, 2)
        self.assertEqual(self.player.max_health, 110) # 100 + 10
        self.assertEqual(self.player.health, 110) # Fully heal on level up

    def test_cast_spell_success(self):
        """Test casting a spell successfully."""
        self.player.mana = 100
        initial_enemy_health = self.enemy.health
        self.player.cast_spell("fireball", self.enemy)
        self.assertEqual(self.enemy.health, initial_enemy_health - (15 + int(self.player.intelligence * 1.5)))
        self.assertEqual(self.player.mana, 80) # 100 - 20

    def test_cast_spell_not_enough_mana(self):
        """Test casting a spell without enough mana."""
        self.player.mana = 10
        initial_enemy_health = self.enemy.health
        self.player.cast_spell("fireball", self.enemy)
        self.assertEqual(self.enemy.health, initial_enemy_health) # No damage
        self.assertEqual(self.player.mana, 10) # No mana cost

class TestGame(unittest.TestCase):
    """Tests for the main Game class and its loop."""
    def setUp(self):
        """Sets up the test environment before each test."""
        self.game = Game(width=20, height=10)
        self.scene = Scene("Test Scene", width=20, height=10)
        self.player = Player(name="TestPlayer", x=5, y=5)
        self.scene.set_player(self.player)
        self.scene_manager = AethelgardBattle(self.scene, self.game, setup_scene=False)

    def test_game_loop_input_move(self):
        """Test the move command in the game's input handler."""
        initial_y = self.player.y
        with patch('builtins.input', return_value='move w'):
            self.game.handle_input(self.scene_manager)
            self.assertEqual(self.player.y, initial_y - 1)

    def test_game_loop_input_quit(self):
        """Test the quit command in the game's input handler."""
        with patch('builtins.input', return_value='quit'):
            self.game.handle_input(self.scene_manager)
            self.assertTrue(self.game.game_over)

if __name__ == '__main__':
    unittest.main()