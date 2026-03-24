import unittest
from game import Player, Scene, Consumable

class TestGameFix(unittest.TestCase):
    def test_stackable_item_pickup_removes_object_from_scene(self):
        """
        Tests that when a player picks up a stackable item that they already
        have, the item object is removed from the scene.
        """
        player = Player(name="Test Player")
        scene = Scene("Test Scene")

        # Add one potion to the player's inventory
        player.inventory.append(Consumable(name="Health Potion", description="Heals HP"))

        # Add a second potion to the scene
        potion_in_scene = Consumable(name="Health Potion", description="Heals HP")
        scene.add_object(potion_in_scene)

        # Player picks up the second potion
        player.pickup_item(potion_in_scene, scene)

        # Assert that the player has one stack of 2 potions
        self.assertEqual(len(player.inventory), 1)
        self.assertEqual(player.inventory[0].quantity, 2)

        # Assert that the second potion object was removed from the scene
        self.assertNotIn(potion_in_scene, scene.game_objects)

if __name__ == '__main__':
    unittest.main()
