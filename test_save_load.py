import unittest
import os
import database
from rpg import (
    Game,
    Scene,
    FirstMeetingScene,
    Anastasia,
    Reverie,
    get_class_by_name,
)

class TestSaveLoad(unittest.TestCase):

    def setUp(self):
        """Set up a clean game state and a save file path for each test."""
        self.save_filename = "test_savegame"
        database.init_db()
        database.set_class_loader(get_class_by_name)
        self.game = Game()
        self.scene = Scene("Test Meeting")
        self.scene_manager = FirstMeetingScene(self.scene, self.game)
        # The setup() method is called inside FirstMeetingScene's __init__

    def tearDown(self):
        """Clean up the database file after each test."""
        if os.path.exists(database.DB_FILE):
            os.remove(database.DB_FILE)

    def test_save_and_load_game_state(self):
        """
        Tests that the game state can be saved to a file and then loaded back,
        restoring the exact state of the game, scene, and characters.
        """
        # 1. Save the initial game state
        database.save_game(self.save_filename, self.scene_manager)
        self.assertTrue(os.path.exists(database.DB_FILE), "Save file was not created.")

        # 2. Load the game state into a new manager
        loaded_manager = database.load_game(self.save_filename)
        self.assertIsNotNone(loaded_manager, "Failed to load the game.")

        # 3. Assert that the loaded state matches the original state
        original_game = self.scene_manager.game
        loaded_game = loaded_manager.game
        original_scene = self.scene_manager.scene
        loaded_scene = loaded_manager.scene

        # Compare game properties
        self.assertEqual(original_game.width, loaded_game.width)
        self.assertEqual(len(original_game.message_log), len(loaded_game.message_log))

        # Compare scene properties
        self.assertEqual(original_scene.name, loaded_scene.name)
        self.assertEqual(len(original_scene.game_objects), len(loaded_scene.game_objects))

        # Compare player character's state
        original_player = original_scene.player_character
        loaded_player = loaded_scene.player_character
        self.assertIsNotNone(loaded_player)
        self.assertEqual(original_player.name, loaded_player.name)
        self.assertEqual(original_player.x, loaded_player.x)
        self.assertEqual(original_player.y, loaded_player.y)
        self.assertEqual(original_player.health, loaded_player.health)
        self.assertEqual(original_player.__class__, loaded_player.__class__)

        # Compare NPC's state and dialogue
        original_npc = next(o for o in original_scene.game_objects if isinstance(o, Reverie))
        loaded_npc = next(o for o in loaded_scene.game_objects if isinstance(o, Reverie))
        self.assertIsNotNone(loaded_npc)
        self.assertEqual(original_npc.name, loaded_npc.name)
        self.assertEqual(original_npc.x, loaded_npc.x)
        self.assertIsNotNone(loaded_npc.dialogue, "NPC dialogue failed to load.")

        # Check a specific dialogue node to ensure deep serialization worked
        original_dialogue_node = original_npc.dialogue.nodes["start"].text
        loaded_dialogue_node = loaded_npc.dialogue.nodes["start"].text
        self.assertEqual(original_dialogue_node, loaded_dialogue_node)

if __name__ == '__main__':
    unittest.main()