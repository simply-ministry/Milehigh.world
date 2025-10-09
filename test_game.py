import unittest
from unittest.mock import patch
from game import GameObject, Character, NPC, Item, Consumable, Weapon, Inventory, Game

class TestGame(unittest.TestCase):

    def setUp(self):
        """Set up test fixtures before each test method."""
        self.game = Game(width=20, height=10)
        self.player = Character(name="Hero", x=5, y=5)
        self.enemy = Character(name="Goblin", x=10, y=10, health=80)
        self.game.set_player_character(self.player)
        self.game.add_object(self.enemy)

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

    def test_character_equip_weapon(self):
        """Test that a character can equip a weapon by name."""
        sword = Weapon(name="Test Sword", description="A test sword.", damage=15)
        self.player.pickup_item(sword)
        self.player.equip_weapon("Test Sword")
        self.assertIsNotNone(self.player.equipped_weapon)
        self.assertEqual(self.player.equipped_weapon.name, "Test Sword")
        self.assertEqual(self.player.equipped_weapon.damage, 15)

    def test_character_attack(self):
        """Test a character's attack on another character."""
        initial_enemy_health = self.enemy.health
        self.player.attack(self.enemy)
        expected_damage = self.player.base_attack_damage
        self.assertEqual(self.enemy.health, initial_enemy_health - expected_damage)

    def test_character_attack_with_weapon(self):
        """Test a character's attack with an equipped weapon."""
        sword = Weapon(name="Test Sword", description="A test sword.", damage=15)
        self.player.pickup_item(sword)
        self.player.equip_weapon("Test Sword")
        initial_enemy_health = self.enemy.health
        self.player.attack(self.enemy)
        expected_damage = self.player.base_attack_damage + sword.damage
        self.assertEqual(self.enemy.health, initial_enemy_health - expected_damage)

    def test_item_pickup(self):
        """Test that a character can pick up items."""
        potion = Consumable(name="Lesser Heal", description="A weak potion.", effect="heal", value=10)
        weapon = Weapon("Axe", "A simple axe.", 5)

        self.player.pickup_item(potion)
        self.assertEqual(len(self.player.inventory.items), 1)

        self.player.pickup_item(weapon)
        self.assertEqual(len(self.player.inventory.items), 2)

    def test_use_health_potion(self):
        """Test that using a health potion restores health and consumes the item."""
        self.player.health = 50
        potion = Consumable(name="Test Potion", description="A test potion.", effect="heal", value=30)
        self.player.pickup_item(potion)
        self.player.use_item("Test Potion")
        self.assertEqual(self.player.health, 80)
        self.assertEqual(len(self.player.inventory.items), 0)

    def test_inventory_add_and_list(self):
        """Test adding an item to the inventory and listing items."""
        potion = Consumable("Health Potion", "Heals 20 HP", "heal", 20)
        self.player.pickup_item(potion)
        self.assertIn(potion, self.player.inventory.items)
        with patch('builtins.print') as mock_print:
            self.player.inventory.list_items()
            mock_print.assert_any_call("- Health Potion (Consumable, heal +20): Heals 20 HP")

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
        delilah_exists = any(obj.name == "Delilah the Desolate" for obj in self.game.game_objects)
        self.assertTrue(delilah_exists)

    def test_cast_spell_from_input(self):
        """Test casting a spell on a target via game input."""
        self.player.mana = 100
        initial_enemy_health = self.enemy.health
        with patch('builtins.input', return_value='cast nightmare on Goblin'):
            self.game.handle_input()
        self.assertEqual(self.enemy.health, initial_enemy_health - 50)
        self.assertEqual(self.player.mana, 65)

    def test_use_non_consumable_item(self):
        """Test attempting to use a non-consumable item like a weapon."""
        weapon = Weapon(name="Sword", description="A simple sword.", damage=10)
        self.player.pickup_item(weapon)
        self.player.use_item("Sword")
        self.assertIn(weapon, self.player.inventory.items)

    def test_use_item_not_in_inventory(self):
        """Test attempting to use an item that is not in the inventory."""
        self.player.use_item("Imaginary Potion")
        # No assertion needed, just checking for no errors

if __name__ == '__main__':
    unittest.main()