from playwright.sync_api import sync_playwright, Page, expect

def run(playwright):
    browser = playwright.chromium.launch(headless=True)
    page = browser.new_page()

    # Construct the file path to open the local index.html file.
    file_path = "file:///app/index.html"
    page.goto(file_path)

    # Set a desktop-like viewport to trigger the md: classes
    page.set_viewport_size({"width": 1280, "height": 800})

    # Locate the gameplay loop section
    gameplay_loop_section = page.locator("#gameplay-loop")

    # Take a screenshot of just this section
    gameplay_loop_section.screenshot(path="jules-scratch/verification/verification.png")

    browser.close()

with sync_playwright() as playwright:
    run(playwright)