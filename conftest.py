from rpg import Enemy, Scene, SceneManager


class MockSceneManager(SceneManager):
    """A minimal SceneManager that doesn't do anything on its own."""

    def setup(self):
        pass

    def update(self):
        pass


def make_test_scene(player, enemy_name="Test Dummy"):
    """Creates a standard test scene with a player and a named enemy.

    Returns:
        tuple: (enemy, scene, scene_manager)
    """
    enemy = Enemy(name=enemy_name, x=1)
    scene = Scene("Test Arena")
    scene.set_player(player)
    scene.add_object(enemy)
    scene_manager = MockSceneManager(scene, game=None, setup_scene=False)
    return enemy, scene, scene_manager
