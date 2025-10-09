import unittest
from unittest.mock import patch, call
from game import GameObject, Character, NPC, Item, Consumable, Weapon, Inventory, Game

class TestGame(unittest.TestCase):

    def setUp(self):
        """Set up test fixtures before each test method."""
        self.game = Game(width=20, height=10)
        self.player = Character(name="Hero", x=5, y=5)
        self.game.set_player_character(self.player)

    def test_game_object_creation(self):
        """Test the creation of a GameObject."""
        obj = GameObject(name="Tree", x=1, y=2, health=50)
        self.assertEqual(obj.name, "Tree")
        self.assertEqual(obj.x, 1)
        self.assertEqual(obj.y, 2)
        self.assertEqual(obj.health, 50)
        self.assertEqual(obj.max_health, 50)

    def test_character_take_damage(self):
        """Test that take_damage correctly reduces a character's health."""
        initial_health = self.player.health
        self.player.take_damage(20)
        self.assertEqual(self.player.health, initial_health - 20)

    def test_character_heal(self):
        """Test that heal correctly increases a character's health without exceeding max."""
        self.player.health = 50
        self.player.heal(20)
        self.assertEqual(self.player.health, 70)
        self.player.heal(40) # Should cap at max_health (100)
        self.assertEqual(self.player.health, 100)

    def test_inventory_add_and_list(self):
        """Test adding an item to the inventory and listing items."""
        potion = Consumable("Health Potion", "Heals 20 HP", "heal", 20)
        self.player.pickup_item(potion)
        self.assertIn(potion, self.player.inventory.items)
        with patch('builtins.print') as mock_print:
            self.player.inventory.list_items()
            mock_print.assert_any_call("- Health Potion (Consumable, heal +20): Heals 20 HP")

    def test_use_consumable_item(self):
        """Test using a consumable item from the inventory."""
        self.player.health = 50
        potion = Consumable("Health Potion", "Heals 20 HP", "heal", 20)
        self.player.pickup_item(potion)

        self.player.use_item("Health Potion")

        self.assertEqual(self.player.health, 70)
        self.assertNotIn(potion, self.player.inventory.items)

    def test_use_mana_consumable(self):
        """Test using a mana-restoring consumable item."""
        self.player.mana = 30
        elixir = Consumable("Mana Elixir", "Restores 50 Mana", "restore_mana", 50)
        self.player.pickup_item(elixir)

        self.player.use_item("Mana Elixir")

        self.assertEqual(self.player.mana, 80)
        self.assertNotIn(elixir, self.player.inventory.items)

    def test_talk_to_npc(self):
        """Test the dialogue interaction between a character and an NPC."""
        npc = NPC(name="Guard", x=6, y=5, dialogue="Halt! Who goes there?")
        self.game.add_object(npc)
        with patch('builtins.print') as mock_print:
            self.player.talk(npc)
            mock_print.assert_called_with("[Guard]: Halt! Who goes there?")

    def test_game_loop_input_move(self):
        """Test the move command in the game's input handler."""
        initial_x = self.player.x
        with patch('builtins.input', return_value='move d'):
            self.game.handle_input()
            self.assertEqual(self.player.x, initial_x + 1)

    def test_game_loop_input_quit(self):
        """Test the quit command in the game's input handler."""
        with patch('builtins.input', return_value='quit'):
            self.game.handle_input()
            self.assertFalse(self.game.is_running)

    def test_scripted_event_trigger(self):
        """Test if a simple scripted event triggers correctly."""
        self.assertFalse(self.game.event_triggered("delilah_battle"))
        self.player.x = 16 # Move player past the trigger coordinate (15)
        self.game.update()
        self.assertTrue(self.game.event_triggered("delilah_battle"))
        # Check if Delilah was actually spawned
        delilah_exists = any(obj.name == "Delilah the Desolate" for obj in self.game.game_objects)
        self.assertTrue(delilah_exists)

    def test_cast_spell(self):
        """Test casting a spell on a target."""
        enemy = Character(name="Goblin", x=6, y=5, health=80)
        self.game.add_object(enemy)
        self.player.mana = 100

        with patch('builtins.input', return_value='cast nightmare on goblin'):
            self.game.handle_input()

        self.assertEqual(enemy.health, 30) # 80 - 50 psychic damage
        self.assertEqual(self.player.mana, 65) # 100 - 35 mana cost

if __name__ == '__main__':
    unittest.main()