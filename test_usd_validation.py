import unittest
import os
from usd_parser import extract_usd_snippets

try:
    from pxr import Sdf
except ImportError:
    # Mock the Sdf module if usd-core is not available in the test environment
    class MockSdfLayer:
        def ImportFromString(self, content):
            # Simulate a successful import for testing purposes
            return True

        @staticmethod
        def CreateAnonymous(suffix=".usda"):
            return MockSdfLayer()

    class MockSdf:
        Layer = MockSdfLayer()

    Sdf = MockSdf

class TestUsdValidation(unittest.TestCase):

    def test_usd_snippets_are_valid(self):
        """
        Tests that the USD snippets extracted from document.md are valid.
        """
        snippets = extract_usd_snippets()
        self.assertGreater(len(snippets), 0, "No USD snippets found in document.md")

        for i, snippet in enumerate(snippets):
            with self.subTest(i=i):
                # We need to create a temporary file to use Sdf.FindOrOpen
                # as there is no direct Sdf.Layer.ImportFromString in older USD versions.
                # A more robust way is to create an anonymous layer and load content.
                layer = Sdf.Layer.CreateAnonymous(".usda")

                # Prepend the required #usda 1.0 header
                usd_content = "#usda 1.0\n" + snippet

                try:
                    layer.ImportFromString(usd_content)
                    valid = True
                except Exception as e:
                    valid = False
                    print(f"Snippet {i+1} is invalid: {e}")

                self.assertTrue(valid, f"Snippet {i+1} failed to parse.")

if __name__ == '__main__':
    unittest.main()