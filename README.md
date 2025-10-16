# Milehigh.World: Into the Void

Welcome to the official repository for the science-fantasy RPG, "Milehigh.World: Into the Void." This document serves as a comprehensive guide for developers, designers, and anyone interested in contributing to the project.

## 🎮 Project Overview

"Milehigh.World: Into the Void" is a narrative-driven RPG developed in Unity, set in a fragmented universe where technology and mysticism collide. The story follows the Ɲōvəmîŋāđ, ten chosen individuals whose actions will determine the fate of their world. The game combines deep storytelling with strategic combat and exploration.

## 🚀 Getting Started

Follow these steps to get the project up and running on your local machine.

### Prerequisites

*   **Unity Hub** and a compatible **Unity Editor** version (2022.3 LTS or later recommended).
*   **Python 3.8+** for running utility and asset pipeline scripts.
*   **Git** for version control.

### Installation

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/your-username/milehigh.world.git
    cd milehigh.world
    ```
2.  **Set up the Python environment:**
    It's recommended to use a virtual environment to manage dependencies.
    ```bash
    python -m venv venv
    source venv/bin/activate  # On Windows, use `venv\Scripts\activate`
    pip install -r requirements.txt
    ```
3.  **Open the project in Unity:**
    *   Open Unity Hub.
    *   Click "Add" or "Open."
    *   Navigate to the cloned repository folder and select it.
    *   The project will open in the Unity Editor, ready for development.

## 🏛️ Core Architecture

The project is built on a set of core architectural patterns designed for scalability and maintainability.

### Singleton Managers

Core systems like `GameManager`, `CombatManager`, and `UIManager` are implemented as persistent singletons. This pattern ensures that there is only one instance of each manager and provides a global access point (e.g., `GameManager.Instance`). These managers are crucial for managing game state, combat flow, and UI updates across different scenes.

### Event-Driven Character System

The `Character.cs` class forms the foundation of all living entities in the game. It uses a robust event-driven architecture. Key events include:
*   `OnHealthChanged`
*   `OnManaChanged`
*   `OnDamageTaken`
*   `OnDie`

Other systems (like `CharacterUI` or `CombatManager`) subscribe to these events to react to changes in a character's state without creating tight dependencies. For example, the UI listens to `OnHealthChanged` to update a character's health bar automatically.

### Data-Driven Design with ScriptableObjects

Game data for items, abilities, and quests are stored in `ScriptableObject` assets. This approach allows designers to create, modify, and balance game content in the Unity Editor without writing new code, accelerating the development workflow.

## 📂 Repository Structure

The repository is organized to separate Unity project files from external tools and documentation.

```
.
├── Assets/
│   ├── Scripts/
│   │   ├── Character/      # C# scripts for all characters, playable and NPC.
│   │   ├── Combat/         # C# scripts for combat mechanics (abilities, damage).
│   │   ├── Core/           # C# scripts for core systems (GameManager, PlayerController).
│   │   ├── UI/             # C# scripts for UI components and managers.
│   │   ├── Physics/        # C# scripts for custom physics logic.
│   │   └── Story/          # C# scripts for managing narrative cutscenes.
│   └── ...               # Other standard Unity asset folders (Scenes, Prefabs, etc.).
├── docs/                   # All design and technical documentation.
├── .gitignore
├── README.md               # This file.
├── requirements.txt        # Python dependencies for utility scripts.
├── test_usd_validation.py  # Tests for the USD asset pipeline.
└── usd_parser.py           # Python script to extract USD snippets from Markdown.
```

## 🛠️ Asset Pipeline

This project uses a custom Python-based pipeline for validating **Universal Scene Description (USD)** assets, ensuring they meet technical requirements before being imported into Unity.

*   `usd_parser.py`: Parses and extracts USD data snippets from design documents.
*   `test_usd_validation.py`: Contains unit tests for the USD assets.

To run the validation tests, execute the following command from the root directory:
```bash
python test_usd_validation.py
```

## 🤝 How to Contribute

We welcome contributions from the community! To contribute, please follow these steps:

1.  **Fork the repository.**
2.  **Create a new branch** for your feature or bug fix (`git checkout -b feature/your-feature-name`).
3.  **Make your changes.** Ensure your code adheres to the project's coding conventions.
4.  **Add XML documentation** to any new public classes, methods, or properties.
5.  **Submit a pull request** with a clear description of your changes.

Thank you for helping make "Milehigh.World: Into the Void" a reality!