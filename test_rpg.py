import unittest
from unittest.mock import patch, MagicMock
import os

# It's important to import the classes from the script we are testing
from rpg import Game, Scene, AethelgardBattle, Interactable, Aeron, Kane
from database import init_db

class TestRpgInteraction(unittest.TestCase):

    DB_FILE = "test_rpg.db"

    def setUp(self):
        """Set up a controlled game environment for each test."""
        # Initialize a temporary database for the test
        init_db(self.DB_FILE)

        self.game = Game(db_file=self.DB_FILE)
        self.scene = Scene("Test Scene", width=40, height=10)
        # The AethelgardBattle scene manager contains the setup logic we need
        self.scene_manager = AethelgardBattle(self.scene, self.game)
        # We need to run the setup to populate the scene with objects
        self.scene_manager.setup()
        # Mock the log_message to capture output without printing to console
        self.game.log_message = MagicMock()

    def tearDown(self):
        """Clean up the temporary database after each test."""
        os.remove(self.DB_FILE)

    @patch('builtins.input', return_value='examine')
    def test_examine_command_success(self, mock_input):
        """
        Tests that the 'examine' command correctly identifies a nearby
        interactable object and logs its description.
        """
        # The player starts at (5, 5) and the statue is at (5, 4), so they are adjacent.
        # Call the method that handles input
        self.game.handle_input(self.scene_manager)

        # The description of the statue in AethelgardBattle
        expected_description = "Ancient Statue: The statue depicts a forgotten king. A faint inscription reads: 'Only the worthy may pass.'"

        # Check that log_message was called with the correct description
        self.game.log_message.assert_called_once_with(expected_description)

    @patch('builtins.input', return_value='examine')
    def test_examine_command_no_object(self, mock_input):
        """
        Tests that the 'examine' command shows the correct message when
        no interactable object is nearby.
        """
        # Move the player far away from the statue
        self.scene.player_character.x = 20
        self.scene.player_character.y = 20

        # Call the method that handles input
        self.game.handle_input(self.scene_manager)

        # Check that log_message was called with the 'nothing nearby' message
        self.game.log_message.assert_called_once_with("There is nothing nearby to examine.")

if __name__ == '__main__':
    unittest.main()