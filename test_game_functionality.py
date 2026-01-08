import unittest
from unittest.mock import MagicMock, patch

from game import (
    Consumable,
    DialogueManager,
    DialogueNode,
    Enemy,
    Game,
    Interactable,
    Item,
    Player,
    Scene,
    SceneManager,
)


class TestGameFunctionality(unittest.TestCase):
    def setUp(self):
        self.game = Game()
        self.scene = Scene("Test Scene")
        self.player = Player("Test Player", x=5, y=5)
        self.scene.set_player(self.player)
        self.scene_manager = SceneManager(self.scene, self.game, setup_scene=False)

    def test_player_drop_item(self):
        item = Item("Test Item")
        self.player.inventory.append(item)
        self.player.drop_item("Test Item", self.scene)
        self.assertNotIn(item, self.player.inventory)
        self.assertIn(item, self.scene.game_objects)

    def test_player_inventory_capacity(self):
        self.player.inventory_capacity = 1
        item1 = Item("Test Item 1")
        item2 = Item("Test Item 2")
        self.player.pickup_item(item1)
        self.player.pickup_item(item2)
        self.assertIn(item1, self.player.inventory)
        self.assertNotIn(item2, self.player.inventory)

    def test_examine_command(self):
        statue = Interactable(
            "Statue", "S", 5, 6, "An old stone statue."
        )
        self.scene.add_object(statue)
        with patch("builtins.input", return_value="examine Statue"):
            self.game.handle_input(self.scene_manager)
        self.assertIn(
            "Statue: An old stone statue.", self.game.message_log
        )

    def test_enemy_ai_state_transitions(self):
        enemy = Enemy("Goblin", x=10, y=10)
        self.scene.add_object(enemy)
        self.assertEqual(enemy.state, "idle")

        enemy.update(self.scene_manager)
        self.assertEqual(enemy.state, "chasing")

        enemy.x = 6
        enemy.y = 5
        enemy.update(self.scene_manager)
        self.assertEqual(enemy.state, "attacking")

        enemy.x = 10
        enemy.y = 10
        enemy.update(self.scene_manager)
        self.assertEqual(enemy.state, "chasing")

    def test_enemy_ai_stunned(self):
        enemy = Enemy("Goblin", x=6, y=5)
        self.scene.add_object(enemy)
        enemy.apply_status_effect("stun", 1)
        initial_health = self.player.health
        enemy.update(self.scene_manager)
        self.assertEqual(self.player.health, initial_health)

    def test_dialogue_system(self):
        dialogue = DialogueManager()
        dialogue.add_node(
            "start",
            DialogueNode(
                "Hello!",
                options={"Ask about the weather.": "weather"},
            ),
        )
        dialogue.add_node("weather", DialogueNode("It's sunny."))

        self.assertEqual(
            dialogue.get_current_node().text, "Hello!"
        )

        dialogue.select_option(0)
        self.assertEqual(
            dialogue.get_current_node().text, "It's sunny."
        )
