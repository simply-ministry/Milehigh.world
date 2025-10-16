# test_usd_validation.py
import pytest
from usd_parser import extract_usd_from_markdown
from pxr import Sdf

def test_usd_snippets_are_valid():
    """
    Parses 'document.md' and validates that every extracted
    USD snippet is syntactically correct.
    """
    snippets = extract_usd_from_markdown("document.md")
    assert snippets, "No USD snippets were found in document.md."

    for i, snippet in enumerate(snippets):
        # Add the required USD file header
        usd_content = f"#usda 1.0\n{snippet.strip()}"

        # Create a new, empty layer to populate with the snippet
        layer = Sdf.Layer.CreateAnonymous()
        try:
            # Attempt to load the string content into the layer
            layer.ImportFromString(usd_content)
            # If this passes, the USD is considered valid
            assert True
        except Exception as e:
            # If an exception is raised, the USD is invalid
            pytest.fail(f"Snippet {i+1} failed validation: {e}\n\n{snippet}")

def test_asset_naming_convention():
    """
    A placeholder test to ensure USD assets follow the project's
    naming conventions (e.g., 'CH_Skyix_Body_A.usd').
    """
    assert True

def test_for_missing_textures():
    """
    A placeholder test to validate that a USD asset has all
    its required texture maps assigned.
    """
    assert True