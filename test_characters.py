import unittest
from unittest.mock import patch, MagicMock

from rpg import (
    Enemy,
    Scene,
    SceneManager,
    Player,
)


class MockSceneManager(SceneManager):
    """A minimal SceneManager for testing purposes that does not perform any actions."""

    def setup(self):
        """Overrides the base setup method to do nothing."""
        pass

    def update(self):
        """Overrides the base update method to do nothing."""
        pass

if __name__ == "__main__":
    unittest.main()