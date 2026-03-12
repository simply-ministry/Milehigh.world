"""Parses Markdown files to extract Universal Scene Description (USD) snippets.

This script provides a utility function to find and extract USD code blocks
from Markdown files. It is used to validate the USD examples embedded in the
project's documentation.
"""
import re

def extract_usd_from_markdown(file_path):
    """Parses a Markdown file to find and extract USD code snippets.

    This function reads the content of a given Markdown file and uses a regular
    expression to find all code blocks that are explicitly tagged with the 'usd'
    language identifier.

    Args:
        file_path (str): The path to the Markdown documentation file.

    Returns:
        list: A list of strings, where each string is a USD code block.
              Returns an empty list if the file is not found or no snippets
              are found.
    """
    print(f"Parsing {file_path} for USD snippets...")
    try:
        with open(file_path, 'r') as f:
            content = f.read()
        # Regex to find all code blocks tagged as 'usd'
        usd_snippets = re.findall(r"```usd\n(.*?)```", content, re.DOTALL)
        return usd_snippets
    except FileNotFoundError:
        print(f"Error: File not found at {file_path}")
        return []

if __name__ == "__main__":
    """Main execution block for command-line usage.

    This block serves as an example of how to use the module. When the script
    is run directly, it will attempt to parse the 'docs/GDD.md' file and
    print any USD snippets it finds to the console.
    """
    # Example usage:
    snippets = extract_usd_from_markdown("docs/GDD.md")
    if snippets:
        print(f"Found {len(snippets)} USD snippets.")
        for i, snippet in enumerate(snippets):
            print(f"--- Snippet {i+1} ---")
            print(snippet.strip())
    else:
        print("No USD snippets found.")