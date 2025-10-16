# usd_parser.py
import re

def extract_usd_from_markdown(file_path):
    """
    Parses a Markdown file to find and extract USD code snippets.

    Args:
        file_path (str): The path to the Markdown documentation file.

    Returns:
        list: A list of strings, where each string is a USD code block.
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
    # Example usage:
    snippets = extract_usd_from_markdown("document.md")
    if snippets:
        print(f"Found {len(snippets)} USD snippets.")
        for i, snippet in enumerate(snippets):
            print(f"--- Snippet {i+1} ---")
            print(snippet.strip())
    else:
        print("No USD snippets found.")