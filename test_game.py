import unittest
from unittest.mock import patch
from game import GameObject, Player, Enemy, Weapon, Consumable, Game

class TestGameLogic(unittest.TestCase):

    def setUp(self):
        """Set up test fixtures before each test method."""
        self.player = Player(name="Test Player")
        self.enemy = Enemy(name="Test Enemy")
        self.game = Game()
        self.game.add_object(self.player)
        self.game.add_object(self.enemy)


    def test_game_object_creation(self):
        """Test the creation of a GameObject with specific attributes."""
        obj = GameObject(name="Test Object", x=10, y=20, z=5, health=150, speed=2, defense=10)
        self.assertEqual(obj.name, "Test Object")
        self.assertEqual(obj.x, 10)
        self.assertEqual(obj.y, 20)
        self.assertEqual(obj.z, 5)
        self.assertEqual(obj.health, 150)
        self.assertEqual(obj.speed, 2)
        self.assertEqual(obj.defense, 10)
        self.assertTrue(obj.visible)
        self.assertTrue(obj.solid)

    def test_take_damage_reduces_health(self):
        """Test that take_damage correctly reduces health based on defense."""
        initial_health = self.player.health
        self.player.defense = 5
        damage = 20
        self.player.take_damage(damage)
        self.assertEqual(self.player.health, initial_health - (damage - self.player.defense))

    def test_take_damage_is_non_negative(self):
        """Test that damage taken is never negative, even with high defense."""
        initial_health = self.player.health
        self.player.defense = 30
        damage = 20
        self.player.take_damage(damage)
        self.assertEqual(self.player.health, initial_health)

    def test_heal_increases_health(self):
        """Test that heal correctly increases health."""
        self.player.health = 50
        self.player.heal(20)
        self.assertEqual(self.player.health, 70)

    def test_heal_does_not_exceed_max_health(self):
        """Test that healing does not increase health beyond the maximum."""
        self.player.health = 90
        self.player.heal(30)
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

    def test_level_up_mechanics(self):
        """Test that the player levels up and stats increase after gaining enough experience."""
        self.player.level = 1
        self.player.experience = 0
        exp_to_level = 100 * self.player.level * self.player.level

        self.player.gain_experience(exp_to_level)

        self.assertEqual(self.player.level, 2)
        self.assertEqual(self.player.max_health, 110)
        self.assertNotEqual(self.player.speed, 5)

    def test_cast_fireball_spell(self):
        """Test casting a fireball spell and its effect on mana and target health."""
        self.player.intelligence = 10
        self.player.mana = 50
        initial_enemy_health = self.enemy.health
        self.enemy.defense = 5

        self.player.cast_spell("fireball", self.enemy)

        self.assertEqual(self.player.mana, 30)
        expected_damage = (15 + int(self.player.intelligence * 1.5)) - self.enemy.defense
        self.assertEqual(self.enemy.health, initial_enemy_health - expected_damage)

    def test_cast_heal_spell(self):
        """Test casting a heal spell and its effect on mana and player health."""
        self.player.health = 50
        self.player.mana = 50
        self.player.intelligence = 10

        self.player.cast_spell("heal", self.player)

        self.assertEqual(self.player.mana, 40)
        expected_heal_amount = 10 + self.player.intelligence
        self.assertEqual(self.player.health, 50 + expected_heal_amount)

    def test_game_over_condition(self):
        """Test that the game ends when the player's health reaches zero."""
        self.player.health = 10
        self.enemy.attack_damage = 15
        self.player.defense = 0
        self.enemy.attack(self.player)
        self.assertLessEqual(self.player.health, 0)

    def test_win_condition(self):
        """Test that the game ends when all enemies are defeated."""
        self.enemy.health = 5
        self.enemy.defense = 0
        self.player.strength = 20
        self.player.attack(self.enemy)
        self.assertFalse(self.enemy.visible)

    def test_enemy_ai_outside_aggro_range(self):
        """Test enemy does not react when player is outside aggro range."""
        self.player.x = 20  # Default enemy aggro_range is 10
        self.player.y = 20
        initial_enemy_x = self.enemy.x
        initial_enemy_y = self.enemy.y
        initial_player_health = self.player.health

        self.enemy.update(delta_time=0.1, player=self.player)

        self.assertEqual(self.enemy.x, initial_enemy_x)
        self.assertEqual(self.enemy.y, initial_enemy_y)
        self.assertEqual(self.player.health, initial_player_health)

    def test_enemy_ai_inside_aggro_range_moves_but_not_attacks(self):
        """Test enemy moves towards player when inside aggro range but not attack range."""
        self.player.x = 5  # Inside aggro_range of 10, but outside attack range of 1
        self.player.y = 0
        initial_enemy_x = self.enemy.x
        initial_player_health = self.player.health

        self.enemy.update(delta_time=0.1, player=self.player)

        self.assertNotEqual(self.enemy.x, initial_enemy_x, "Enemy should move on the x-axis")
        self.assertEqual(self.player.health, initial_player_health, "Enemy should not attack")

    def test_enemy_ai_inside_attack_range_attacks(self):
        """Test enemy attacks player when in attack range."""
        self.player.x = 0.5  # Inside attack range of 1
        self.player.y = 0
        initial_player_health = self.player.health

        self.enemy.update(delta_time=0.1, player=self.player)

        self.assertLess(self.player.health, initial_player_health, "Player health should decrease")

    @patch('random.uniform', return_value=1)
    def test_player_attack_miss(self, mock_uniform):
        """Test that a player's attack can miss."""
        self.player.dexterity = 0  # Guarantees a miss
        initial_enemy_health = self.enemy.health
        self.player.attack(self.enemy)
        self.assertEqual(self.enemy.health, initial_enemy_health)

    @patch('random.uniform', return_value=20)
    def test_player_attack_bare_handed(self, mock_uniform):
        """Test player's attack damage with no weapon."""
        self.player.weapon = None
        self.player.strength = 10
        self.enemy.defense = 2
        initial_enemy_health = self.enemy.health
        expected_damage = (2 + (self.player.strength // 2)) - self.enemy.defense

        self.player.attack(self.enemy)

        self.assertEqual(self.enemy.health, initial_enemy_health - expected_damage)

    def test_cast_spell_insufficient_mana(self):
        """Test casting a spell with not enough mana."""
        self.player.mana = 10  # Fireball costs 20
        initial_enemy_health = self.enemy.health
        initial_player_mana = self.player.mana

        self.player.cast_spell("fireball", self.enemy)

        self.assertEqual(self.enemy.health, initial_enemy_health)
        self.assertEqual(self.player.mana, initial_player_mana)

    def test_cast_unknown_spell(self):
        """Test casting a spell that doesn't exist."""
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

    @patch('builtins.print')
    def test_game_start_without_player(self, mock_print):
        """Test that the game cannot start without a player."""
        game = Game()
        game.start()
        mock_print.assert_called_with("Cannot start game without a Player.")
        self.assertFalse(game.running)


if __name__ == '__main__':
    unittest.main()