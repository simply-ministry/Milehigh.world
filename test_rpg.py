import unittest
from unittest.mock import patch, MagicMock

# It's important to import the classes from the script we are testing
from rpg import Game, Scene, AethelgardBattle, Interactable, Aeron, Kane

class TestRpgInteraction(unittest.TestCase):

    def setUp(self):
        """Set up a controlled game environment for each test."""
        self.game = Game()
        self.scene = Scene("Test Scene", width=40, height=10)
        # The AethelgardBattle scene manager contains the setup logic we need
        self.scene_manager = AethelgardBattle(self.scene, self.game)
        # We need to run the setup to populate the scene with objects
        self.scene_manager.setup()
        # Mock the print to capture output without printing to console
        self.game.print = MagicMock()

    @patch('builtins.input', return_value='examine')
    @patch('builtins.print')
    def test_examine_command_success(self, mock_print, mock_input):
        """
        Tests that the 'examine' command correctly identifies a nearby
        interactable object and logs its description.
        """
        # The player starts at (5, 5) and the statue is at (5, 4), so they are adjacent.
        # Call the method that handles input
        self.game.handle_input(self.scene_manager)

        # The description of the statue in AethelgardBattle
        expected_description = "The statue depicts a forgotten king. A faint inscription reads: 'Only the worthy may pass.'"

        # Check that print was called with the correct description
        mock_print.assert_any_call(expected_description)

    @patch('builtins.input', return_value='examine')
    @patch('builtins.print')
    def test_examine_command_no_object(self, mock_print, mock_input):
        """
        Tests that the 'examine' command shows the correct message when
        no interactable object is nearby.
        """
        # Move the player far away from the statue
        self.scene.player_character.x = 20
        self.scene.player_character.y = 20

        # Call the method that handles input
        self.game.handle_input(self.scene_manager)

        # Check that print was called with the 'nothing nearby' message
        mock_print.assert_any_call("There is nothing nearby to examine.")

if __name__ == '__main__':
    unittest.main()