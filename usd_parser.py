import re

def extract_usd_snippets(filepath="document.md"):
    """
    Extracts USD code snippets from a Markdown file.

    Args:
        filepath (str): The path to the Markdown file.

    Returns:
        list[str]: A list of USD code snippets.
    """
    with open(filepath, 'r') as f:
        content = f.read()

    # Find all code blocks fenced with ```usd
    pattern = r"```usd(.*?)```"
    snippets = re.findall(pattern, content, re.DOTALL)

    return [s.strip() for s in snippets]

if __name__ == '__main__':
    snippets = extract_usd_snippets()
    if snippets:
        print(f"Found {len(snippets)} USD snippets.")
        for i, snippet in enumerate(snippets):
            print(f"--- Snippet {i+1} ---")
            print(snippet)
    else:
        print("No USD snippets found.")