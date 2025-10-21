import unittest
from unittest.mock import patch, MagicMock
import os
import game
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

    @patch('game.AethelgardBattle.run')
    def test_new_game_starts_correct_scene(self, mock_run):
        """
        Tests that starting a new game (no 'load' argument)
        initializes and runs the AethelgardBattle scene.
        """
        # We pass an empty list to simulate no command-line arguments
        with patch('sys.argv', ['game.py']):
            game.main()

        # Check that the run method was called, indicating the game loop started
        mock_run.assert_called_once()

    @patch('database.load_game')
    @patch('game.SceneManager.run')
    def test_load_game_bypasses_new_game_creation(self, mock_run, mock_load_game):
        """
        Tests that if a saved game is successfully loaded, a new game
        is not created.
        """
        # Mock the load_game function to return a dummy SceneManager
        # This simulates a successful load
        mock_scene = MagicMock(spec=game.Scene)
        mock_scene.name = "Loaded Scene"
        mock_loaded_scene = MagicMock(spec=game.SceneManager)
        mock_loaded_scene.scene = mock_scene
        mock_load_game.return_value = mock_loaded_scene

        # Simulate running 'python game.py load my_save'
        with patch('sys.argv', ['game.py', 'load', 'my_save']):
            game.main()

        # Assert that load_game was called with the correct save name
        mock_load_game.assert_called_once_with('my_save')

        # Assert that the game runs the loaded scene manager
        mock_loaded_scene.run.assert_called_once()
        scene_manager.run.assert_called_once()
        self.assertIs(scene_manager, mock_loaded_scene,
                      "The game should run the scene manager returned by load_game.")

        # Ensure it's not an instance of the default new game scene
        self.assertNotIsInstance(scene_manager, rpg.TrollCaveScene,
                                 "A new game should not be created when one is loaded.")

if __name__ == '__main__':
    unittest.main()