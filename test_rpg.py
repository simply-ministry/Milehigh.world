# SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
import unittest
from unittest.mock import patch, MagicMock
import os
import database

# It's important to import the classes from the script we are testing
from game import Game, Scene, AethelgardBattle, Interactable, Aeron, Kane, Player, Enemy
from database import init_db


class TestRpgInteraction(unittest.TestCase):
    """A suite of tests for player interaction within the RPG,
    focusing on commands like 'examine'.
    """

    DB_FILE = "test_rpg.db"

    def setUp(self):
        """Set up a controlled game environment for each test."""
        init_db(self.DB_FILE)
        self.game = Game()
        self.scene = Scene("Test Scene", width=40, height=10)
        self.scene_manager = AethelgardBattle(self.scene, self.game, setup_scene=False)
        self.scene_manager.setup()
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
        self.game.handle_input(self.scene_manager)
        expected_description = "Ancient Statue: The statue depicts a forgotten king. A faint inscription reads: 'Only the worthy may pass.'"
        self.game.log_message.assert_called_once_with(expected_description)

    @patch('builtins.input', return_value='examine')
    def test_examine_command_no_object(self, mock_input):
        """
        Tests that the 'examine' command shows the correct message when
        no interactable object is nearby.
        """
        self.scene.player_character.x = 20
        self.scene.player_character.y = 20
        self.game.handle_input(self.scene_manager)
        self.game.log_message.assert_called_once_with("There is nothing nearby to examine.")

if __name__ == '__main__':
    unittest.main()