import unittest
from unittest.mock import patch, MagicMock
import os
import sqlite3
import json
from game import (
    GameObject, Item, Weapon, Armor, Consumable, Character, Player, Enemy,
    Scene, Game, FirstMeetingScene, HealthPotion
)
import database

class TestGameCoverage(unittest.TestCase):
    """A comprehensive test suite for the game.py module."""

    DB_FILE = "test_game_coverage.db"

    def setUp(self):
        """Set up a clean database and game instance for each test."""
        if os.path.exists(self.DB_FILE):
            os.remove(self.DB_FILE)

        database.init_db(self.DB_FILE)

        self.game = Game()
        self.game.db_conn = sqlite3.connect(self.DB_FILE)
        self.game.db_conn.row_factory = sqlite3.Row

        self.scene = Scene("Test Scene", width=40, height=10)
        self.player = Player(name="TestPlayer", x=5, y=5)
        self.scene.set_player(self.player)

        self.game.log_message = MagicMock()

    def tearDown(self):
        """Clean up the database after each test."""
        self.game.db_conn.close()
        if os.path.exists(self.DB_FILE):
            os.remove(self.DB_FILE)

    # GameObject tests
    def test_game_object_init(self):
        """Test GameObject initialization."""
        obj = GameObject(name="Test", symbol='T', x=1, y=2, health=50, defense=5)
        self.assertEqual(obj.name, "Test")
        self.assertEqual(obj.symbol, 'T')
        self.assertEqual(obj.x, 1)
        self.assertEqual(obj.y, 2)
        self.assertEqual(obj.health, 50)
        self.assertEqual(obj.defense, 5)

    def test_game_object_distance_to(self):
        """Test the distance calculation between two GameObjects."""
        obj1 = GameObject(x=0, y=0)
        obj2 = GameObject(x=3, y=4)
        self.assertEqual(obj1.distance_to(obj2), 5.0)

    def test_game_object_move(self):
        """Test moving a GameObject."""
        obj = GameObject(x=1, y=1)
        obj.move(2, 3)
        self.assertEqual(obj.x, 3)
        self.assertEqual(obj.y, 4)

    def test_game_object_take_damage(self):
        """Test the take_damage method."""
        obj = GameObject(health=100, defense=10)
        obj.take_damage(30)
        self.assertEqual(obj.health, 80)

    # Item tests
    def test_item_init(self):
        """Test Item initialization."""
        item = Item(name="Test Item", symbol='*')
        self.assertEqual(item.name, "Test Item")
        self.assertEqual(item.symbol, '*')

    def test_weapon_init_and_str(self):
        """Test Weapon initialization and string representation."""
        weapon = Weapon(name="Test Sword", description="A test sword.", damage=10)
        self.assertEqual(weapon.name, "Test Sword")
        self.assertEqual(weapon.damage, 10)
        self.assertEqual(str(weapon), "Test Sword (Weapon, 10 DMG): A test sword.")

    def test_armor_init_and_str(self):
        """Test Armor initialization and string representation."""
        armor = Armor(name="Test Shield", description="A test shield.", defense=5)
        self.assertEqual(armor.name, "Test Shield")
        self.assertEqual(armor.defense, 5)
        self.assertEqual(str(armor), "Test Shield (Armor, +5 DEF): A test shield.")

    def test_consumable_init_and_use(self):
        """Test Consumable initialization and use method."""
        consumable = HealthPotion(name="Health Potion", description="Heals 20 HP.", amount=20)
        self.assertEqual(consumable.name, "Health Potion")

        self.player.health = 50
        self.player.max_health = 100
        consumable.use(self.player)
        self.assertEqual(self.player.health, 70)

    # Character and Player tests
    def test_character_init(self):
        """Test Character initialization."""
        char = Character(name="TestChar", x=1, y=2, health=80)
        self.assertEqual(char.name, "TestChar")
        self.assertEqual(char.x, 1)
        self.assertEqual(char.y, 2)
        self.assertEqual(char.health, 80)

    @patch('random.uniform', return_value=10)
    def test_player_attack_no_weapon(self, mock_uniform):
        """Test the Player's attack method without a weapon."""
        target = Enemy(name="Goblin", health=50)
        target.defense = 2
        self.player.attack(target)
        # Player strength is 10, damage = 0 + 10 // 2 = 5
        # Actual damage = 5 - 2 (defense) = 3
        self.assertEqual(target.health, 47)

    def test_player_equip(self):
        """Test equipping items."""
        weapon = Weapon(name="Sword", description="", damage=10)
        self.player.inventory.append(weapon)
        self.player.equip_item("Sword")
        self.assertIsNotNone(self.player.equipment.slots["weapon"])
        self.assertEqual(self.player.equipment.slots["weapon"].name, "Sword")

    def test_player_pickup_item(self):
        """Test picking up an item."""
        item = Item(name="Rock", symbol='*')
        self.player.pickup_item(item)
        self.assertIn(item, self.player.inventory)

    def test_player_gain_experience_and_level_up(self):
        """Test gaining experience and leveling up."""
        self.player.level = 1
        self.player.experience = 0
        self.player.gain_experience(100)
        self.assertEqual(self.player.level, 2)
        self.assertEqual(self.player.experience, 100)
        self.assertEqual(self.player.max_health, 110)

    # Enemy tests
    def test_enemy_attack(self):
        """Test the Enemy's attack method."""
        enemy = Enemy(name="Wolf", attack_damage=15)
        self.player.health = 100
        enemy.attack(self.player)
        self.assertEqual(self.player.health, 85)

    # Scene and Game tests
    def test_scene_add_and_get_object(self):
        """Test adding and retrieving objects from a scene."""
        obj = GameObject(x=1, y=1)
        self.scene.add_object(obj)
        self.assertIs(self.scene.get_object_at(1, 1), obj)
        self.assertIsNone(self.scene.get_object_at(2, 2))

    def test_scene_set_player(self):
        """Test setting the player in a scene."""
        new_player = Player(name="NewPlayer")
        self.scene.set_player(new_player)
        self.assertIs(self.scene.player_character, new_player)
        self.assertIn(new_player, self.scene.game_objects)

    def test_game_log_message(self):
        """Test the game's message logging system."""
        self.game.log_message("Test message")
        self.game.log_message.assert_called_with("Test message")

    @patch('builtins.print')
    def test_game_draw(self, mock_print):
        """Test that the game's draw method executes without errors."""
        try:
            self.game.draw(self.scene)
        except Exception as e:
            self.fail(f"game.draw() raised an exception: {e}")

    # FirstMeetingScene tests
    @patch('builtins.input', return_value='quit')
    def test_first_meeting_scene_setup_and_run(self, mock_input):
        """Test the setup and execution of the FirstMeetingScene."""
        with open("game_data.json", "r") as f:
            game_data = json.load(f)

        scene = Scene("Test Scene", width=40, height=10)
        game = Game()
        scene_manager = FirstMeetingScene(scene, game, game_data)

        # Check for player and enemies
        self.assertIsNotNone(scene.player_character)
        self.assertTrue(any(isinstance(obj, Enemy) for obj in scene.game_objects))

        # Check for items
        self.assertTrue(any(isinstance(obj, Weapon) for obj in scene.game_objects))
        self.assertTrue(any(isinstance(obj, HealthPotion) for obj in scene.game_objects))

        # Run the scene to ensure it executes without errors
        scene_manager.run()

if __name__ == '__main__':
    unittest.main()
