import unittest
from unittest.mock import patch, MagicMock
import os
import rpg
import database

class TestGameRunner(unittest.TestCase):

    def setUp(self):
        """Set up a clean environment for each test."""
        # Ensure there's no database file from previous runs
        if os.path.exists(database.DB_FILE):
            os.remove(database.DB_FILE)
        database.init_db()

    def tearDown(self):
        """Clean up the database file after each test."""
        if os.path.exists(database.DB_FILE):
            os.remove(database.DB_FILE)

    @patch('rpg.SceneManager.run')
    def test_new_game_starts_correct_scene(self, mock_run):
        """
        Tests that starting a new game (no 'load' argument)
        initializes and runs the AethelgardBattle scene.
        """
        # We pass an empty list to simulate no command-line arguments
        scene_manager = rpg.main([])

        # Check that the run method was called, indicating the game loop started
        mock_run.assert_called_once()

        # Check that the returned scene manager is of the correct type
        self.assertIsInstance(scene_manager, rpg.AethelgardBattle,
                              "Starting a new game should create an AethelgardBattle instance.")

        # Check that the scene name is correct
        self.assertEqual(scene_manager.scene.name, "Aethelgard",
                         "The default scene should be 'Aethelgard'.")

    @patch('database.load_game')
    def test_load_game_bypasses_new_game_creation(self, mock_load_game):
        """
        Tests that if a saved game is successfully loaded, a new game
        is not created.
        """
        # Mock the load_game function to return a dummy SceneManager
        # This simulates a successful load
        mock_scene = MagicMock(spec=rpg.Scene)
        mock_scene.name = "Loaded Scene"
        mock_loaded_scene = MagicMock(spec=rpg.SceneManager)
        mock_loaded_scene.scene = mock_scene
        mock_load_game.return_value = mock_loaded_scene

        # Simulate running 'python rpg.py load my_save'
        argv = ['rpg.py', 'load', 'my_save']
        scene_manager = rpg.main(argv)

        # Assert that load_game was called with the correct save name
        mock_load_game.assert_called_once_with('my_save')

        # Assert that the game runs the loaded scene manager
        scene_manager.run.assert_called_once()
        self.assertIs(scene_manager, mock_loaded_scene,
                      "The game should run the scene manager returned by load_game.")

        # Ensure it's not an instance of the default new game scene
        self.assertNotIsInstance(scene_manager, rpg.AethelgardBattle,
                                 "A new game should not be created when one is loaded.")

if __name__ == '__main__':
    unittest.main()