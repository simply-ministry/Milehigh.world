import re

def extract_usd_from_markdown(file_path):
    """
    Parses a Markdown file to find and extract USD code snippets.

    Args:
        file_path (str): The path to the Markdown documentation file.

    Returns:
        list: A list of strings, where each string is a USD code block.
    """
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
    except FileNotFoundError:
        print(f"Error: File not found at {file_path}")
        return []

    # Regex to find code blocks fenced with ```usd
    usd_pattern = re.compile(r'```usd\n(.*?)\n```', re.DOTALL)
    snippets = usd_pattern.findall(content)
    return snippets

if __name__ == "__main__":
    # Example usage:
    # This assumes you have a 'docs/GDD.md' file with USD snippets.
    file_to_parse = 'docs/GDD.md'
    usd_snippets = extract_usd_from_markdown(file_to_parse)

    if usd_snippets:
        print(f"Found {len(usd_snippets)} USD snippets in {file_to_parse}:")
        for i, snippet in enumerate(usd_snippets, 1):
            print(f"\n--- Snippet {i} ---")
            print(snippet.strip())
            print("--------------------")
    else:
        print(f"No USD snippets found in {file_to_parse}.")