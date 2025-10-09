import unittest
from unittest.mock import patch
from game import GameObject, Player, Enemy, Weapon, Consumable, Game
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

    def test_player_equip_weapon(self):
        """Test that a player can equip a weapon."""
        sword = Weapon(name="Test Sword", description="A test sword.", damage=15)
        self.player.equip_weapon(sword)
        self.assertIs(self.player.weapon, sword)
        self.assertEqual(self.player.weapon.damage, 15)

    @patch('random.uniform', return_value=20)
    def test_player_attack_regular_hit(self, mock_uniform):
        """Test a player's regular attack on an enemy."""
        sword = Weapon(name="Test Sword", description="A test sword.", damage=20)
        self.player.equip_weapon(sword)
        self.player.strength = 10
        self.enemy.defense = 5
        initial_enemy_health = self.enemy.health
        expected_damage = (sword.damage + (self.player.strength // 2)) - self.enemy.defense

        self.player.attack(self.enemy)

        self.assertEqual(self.enemy.health, initial_enemy_health - expected_damage)

    @patch('random.uniform')
    def test_player_attack_critical_hit(self, mock_uniform):
        """Test a player's critical hit attack on an enemy."""
        # Ensure the miss chance roll is high (not a miss) and the crit chance roll is low (a crit)
        mock_uniform.side_effect = [10, 4]
        sword = Weapon(name="Test Sword", description="A test sword.", damage=20)
        self.player.equip_weapon(sword)
        self.player.strength = 10
        self.player.dexterity = 100 # Guarantees a crit
        self.enemy.defense = 5
        initial_enemy_health = self.enemy.health

        base_damage = sword.damage + (self.player.strength // 2)
        critical_damage = (base_damage * 2) - self.enemy.defense

        self.player.attack(self.enemy)

        self.assertEqual(self.enemy.health, initial_enemy_health - critical_damage)


    def test_enemy_attack(self):
        """Test a simple enemy attack on the player."""
        initial_player_health = self.player.health
        self.player.defense = 2
        self.enemy.attack_damage = 10

        self.enemy.attack(self.player)

        expected_health = initial_player_health - (self.enemy.attack_damage - self.player.defense)
        self.assertEqual(self.player.health, expected_health)

    def test_item_pickup(self):
        """Test that a player can pick up items."""
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
        self.player.inventory.add_item(potion)

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
        initial_enemy_health = self.enemy.health
        initial_player_mana = self.player.mana

        self.player.cast_spell("frostbolt", self.enemy)

        self.assertEqual(self.enemy.health, initial_enemy_health)
        self.assertEqual(self.player.mana, initial_player_mana)

    def test_use_mana_potion(self):
        """Test that using a mana potion restores mana and consumes the item."""
        self.player.mana = 20
        potion = Consumable(name="Test Mana Potion", description="A test mana potion.", effect="restore_mana", value=40)
        self.player.inventory.add_item(potion)

        self.player.use_item("Test Mana Potion")

        self.assertEqual(self.player.mana, 60)
        self.assertEqual(len(self.player.inventory.items), 0)

    def test_use_non_consumable_item(self):
        """Test attempting to use a non-consumable item like a weapon."""
        weapon = Weapon(name="Sword", description="A simple sword.", damage=10)
        self.player.inventory.add_item(weapon)
        self.assertFalse(self.player.use_item("Sword"))
        self.assertEqual(len(self.player.inventory.items), 1)

    def test_use_item_not_in_inventory(self):
        """Test attempting to use an item that is not in the inventory."""
        self.assertFalse(self.player.use_item("Imaginary Potion"))

    def test_game_remove_object(self):
        """Test that remove_object correctly removes an object from the game."""
        initial_object_count = len(self.game.objects)
        self.game.remove_object(self.enemy)
        self.assertEqual(len(self.game.objects), initial_object_count - 1)
        self.assertNotIn(self.enemy, self.game.objects)

        with patch('builtins.input', return_value='cast nightmare on goblin'):
            self.game.handle_input()

        self.assertEqual(enemy.health, 30) # 80 - 50 psychic damage
        self.assertEqual(self.player.mana, 65) # 100 - 35 mana cost

if __name__ == '__main__':
    unittest.main()