import pytest
import os
import re
from usd_parser import extract_usd_from_markdown

# Define the path to the GDD, which contains the USD snippets to be validated.
GDD_PATH = "docs/GDD.md"

@pytest.fixture
def usd_snippets():
    """A pytest fixture that extracts all USD snippets from the GDD."""
    if not os.path.exists(GDD_PATH):
        pytest.fail(f"The GDD file was not found at the required path: {GDD_PATH}")
    return extract_usd_from_markdown(GDD_PATH)

def test_usd_snippets_exist(usd_snippets):
    """Tests that at least one USD snippet was found in the documentation."""
    assert usd_snippets, "No USD snippets were found in the GDD. The test cannot proceed."

def test_asset_paths_are_defined(usd_snippets):
    """
    Validates that each 'def' in the USD snippets has an asset path
    in its metadata, following the project's standards.
    """
    for snippet in usd_snippets:
        # A more robust check for an asset path within a prim definition
        prim_def_pattern = re.compile(r'def\s+\w+\s+"[^"]+"\s*\(([^)]+)\)', re.DOTALL)
        matches = prim_def_pattern.findall(snippet)
        for match in matches:
            assert 'asset aname = "' in match, f"Missing asset path metadata in prim definition: {match}"

def test_materials_are_bound(usd_snippets):
    """
    Validates that relevant prims have a material binding.
    This is a basic check and could be expanded to verify the material path.
    """
    for snippet in usd_snippets:
        lines = snippet.strip().split('\n')
        # Check for prims that should have materials
        if 'def Xform "CH_Skyix"' in snippet:
            assert 'rel material:binding = </materials/CH_Skyix_Material>' in snippet, "Skyix character prim is missing a material binding."
        if 'def Xform "WP_Aeron_Sword"' in snippet:
            assert 'rel material:binding = </materials/WP_Aeron_Sword_Material>' in snippet, "Aeron's sword prim is missing a material binding."

def test_no_invalid_prim_types(usd_snippets):
    """
    Checks for any prim types that are not allowed in the project's pipeline.
    This is an example of enforcing technical standards.
    """
    invalid_types = ["Cone", "Cylinder"] # Example of disallowed prim types
    for snippet in usd_snippets:
        for invalid_type in invalid_types:
            assert f'def {invalid_type} "' not in snippet, f"Found a disallowed prim type '{invalid_type}' in a USD snippet."