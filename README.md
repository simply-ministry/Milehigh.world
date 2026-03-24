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
Welcome to the official repository for the science-fantasy RPG, "Milehigh.World: Into the Void." This project is a narrative-driven RPG developed in Unity, supplemented by a suite of Python scripts for asset management, database control, and game logic prototyping.

## 🎮 Project Overview

**Mîlēhîgh.wørld** is a science-fantasy RPG that explores a universe defined by a central conflict between high-tech advancement and ancient mysticism. The narrative follows the **Ɲōvəmîŋāđ**, a group of ten preordained heroes, as they navigate a fragmented reality to either fulfill or prevent the Lost Prophecy of Lîŋq and achieve **Millenia**—an enduring era of peace.

The inciting event is an invasion by **King Cyrus** through the **Onalym Nexus**, a dimensional bridge, which shatters reality and kicks off the gathering of the Ɲōvəmîŋāđ. The primary antagonist is **The Void**, a corrupting "digital abyss," and its cunning manipulator, **Lucent the Lightweaver**. The game's design is built on the juxtaposition of technologies like cybernetics and quantum teleportation with mystical forces such as Phoenix and Dragon powers.

The project reinforces its digital, cyberpunk roots through a recurring binary motif:
- **milehigh.world**: `01101101 01101001 01101100 01100101 01101000 01101001 01100111 01101000 00101110 01110111 01101111 01110010 01101100 01100100`
- **Void**: `01110110 01101111 01101001 01100100`

For a complete narrative and world-building reference, see the [Game Design Document](docs/GDD.md).

## 🏛️ Project Architecture

This repository uses a hybrid architecture that combines a **Unity/C# frontend** for the main game with a **Python backend** for tooling, prototyping, and data management.

-   **Unity/C# (The Game)**: The core of the project, located in the `Assets/` directory. This is where all real-time game logic, character controllers, combat systems, and UI are implemented.
-   **Python (Tooling & Prototyping)**: A collection of scripts in the root directory used for tasks that support the main game, such as database management, asset validation, and rapid prototyping of game mechanics.

This separation allows for a clean and efficient workflow, where game logic can be quickly tested in a lightweight Python environment before being implemented in C#.

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
│   ├── Art/
│   ├── Audio/
│   ├── Prefabs/
│   ├── Scenes/
│   └── Scripts/          # All C# source code for the Unity project.
│       ├── Character/    # Character-specific logic and abilities.
│       ├── Combat/       # Combat systems, including damage and AI.
│       ├── Core/         # Core gameplay systems (inventory, quests, interaction).
│       ├── Physics/      # Custom physics components.
│       └── Story/        # Narrative scenes and dialogue management.
├── blender_scripts/      # Python scripts for Blender automation.
├── docs/                 # All design and technical documentation.
├── .gitignore
├── README.md             # This file.
├── requirements.txt      # Python dependencies for utility scripts.
├── database.py           # Manages the game's SQLite database.
├── game.py               # A Python-based prototype of the game's core mechanics.
├── rpg.py                # A more complex, data-driven RPG prototype.
├── test_*.py             # Pytest files for all Python scripts.
└── usd_parser.py         # Extracts USD snippets from Markdown files.
```

## 🛠️ Asset Pipeline

This project uses a custom Python-based pipeline for validating **Universal Scene Description (USD)** assets, ensuring they meet technical requirements before being imported into Unity.
### Key Directories & Files:

*   **`Assets/Scripts/`**: Contains all C# source code for the Unity project, organized by system. This is the heart of the game's real-time functionality.
*   **`blender_scripts/`**: A collection of Python scripts designed to be run within Blender to automate tasks like rendering image sequences to video.
*   **`docs/`**: A directory for all Game Design Documents (GDDs), technical specifications, and narrative outlines.
*   **Root Directory**: Contains Python scripts for asset validation, database management, and prototyping, along with project configuration files and this README.

## 🚀 Getting Started

### Prerequisites

*   **Unity Hub** and a compatible **Unity Editor** version (e.g., 2022.3 LTS or later).
*   **Python 3.8+** for running utility scripts.
*   **Blender** (optional, for running scripts in `blender_scripts/`).
*   **Git** for version control.

### Installation & Setup
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
1.  **Clone the repository:**
    ```bash
    git clone https://github.com/your-username/milehigh.world.git
    cd milehigh.world
    ```
2.  **Set up the Python environment:**
    It is highly recommended to use a virtual environment to manage Python dependencies.
    ```bash
    # Create and activate a virtual environment
    python -m venv venv
    source venv/bin/activate  # On Windows, use `venv\Scripts\activate`

    # Install the required packages
    pip install -r requirements.txt
    ```
3.  **Initialize the game database:**
    The `database.py` script creates and populates a SQLite database with initial game data.
    It is recommended to use a virtual environment to keep the project's dependencies isolated.
    ```bash
    python -m venv venv
    source venv/bin/activate  # On Windows, use `venv\Scripts\activate`
    pip install -r requirements.txt
    ```
3.  **Initialize the game database:**
    This command creates the `game_content.db` file and populates it with the necessary schema and initial data.
    ```bash
    python database.py
    ```
4.  **Open the project in Unity:**
    *   Open Unity Hub.
    *   Click "Open" and select the cloned repository folder.
    *   The project will open in the Unity Editor, ready for development.

## 🛠️ Python Scripts

This project uses a number of Python scripts for various purposes. Here's a brief overview:

*   **`database.py`**: Manages the game's SQLite database (`game_content.db`), which stores information about characters, items, quests, and more. Running this script directly will initialize the database with the required schema and some initial data.
*   **`game.py` & `rpg.py`**: These files are Python-based prototypes of the game's core mechanics. They are used for testing and iterating on game logic before implementing it in C#.
    *   `game.py`: A comprehensive prototype with a game loop, combat, dialogue, and a connection to `game_data.json`.
    *   `rpg.py`: A more advanced, data-driven prototype that loads character and item data directly from the SQLite database.
*   **`usd_parser.py`**: This script is used to parse and extract USD (Universal Scene Description) data snippets from design documents, which is useful for validating the game's assets.
*   **`test_*.py`**: These files contain unit tests for the Python scripts, written using the `pytest` framework.

### Running the Python Tests

To ensure the Python scripts are functioning correctly, run the test suite from the root of the repository:
*   **`game.py`**, **`rpg.py`**, **`simple_rpg.py`**: These files are Python-based prototypes of the game's core mechanics. They are used for testing and iterating on game logic before implementing it in C#.
    *   `simple_rpg.py`: A lightweight prototype for testing specific features like the level-up system. You can run it directly to see a demonstration of the level-up and ability learning mechanics:
        ```bash
        python simple_rpg.py
        ```
    *   `game.py`: A more complete prototype with a game loop, combat, and dialogue. It loads game data from `game_data.json` and can be run to play a simple, text-based version of the game:
        ```bash
        python game.py
        ```
    *   `rpg.py`: The most advanced prototype, featuring data-driven character and item loading from the database. It can be run to play a more complex, turn-based RPG experience:
        ```bash
        python rpg.py
        ```
*   **`usd_parser.py`**: This script is used to parse and extract USD (Universal Scene Description) data snippets from design documents, which is useful for validating the game's assets.
*   **`test_*.py`**: These files contain unit tests for the Python scripts, written using the `pytest` framework.

### Running the Tests

To ensure the Python scripts are working correctly, you can run the test suite using `pytest`.

```bash
python -m pytest
```
*Note: The `test_usd_validation.py` suite may be skipped if the required USD libraries are not installed. This is expected behavior.*

## C# Codebase Overview

The C# codebase is located in the `Assets/Scripts/` directory and is organized by feature. Here's a high-level overview:

*   **`Character/` & `Characters/`**: Contains the base `Character` class and all character-specific implementations, including their unique abilities and attributes.
*   **`Combat/`**: Holds the logic for the game's combat system, including abilities, damage calculation, and the `CombatManager`.
*   **`Core/`**: Contains the foundational scripts of the game, such as the `GameManager`, `PlayerController`, and `Interactor`.
*   **`Physics/`**: A collection of scripts for custom physics effects, such as buoyancy and custom gravity.
*   **`Story/`**: Contains scripts for managing narrative scenes and quests.
*   **`UI/`**: Holds the scripts for managing the game's user interface, including health bars, action buttons, and floating damage text.

## 📖 Story & Characters

"Milehigh.World: Into the Void" features a rich narrative and a diverse cast of characters. The story revolves around the Ɲōvəmîŋāđ, a group of ten individuals chosen to save their fragmented universe from the encroaching Void. The main characters include:

*   **Aeron**: A noble warrior and leader of the Ɲōvəmîŋāđ.
*   **Anastasia**: A powerful support mage who can shape reality through dreams.
*   **Cirrus**: The Dragon King, an elemental bruiser with immense power.
*   **Ingris**: The Phoenix Warrior, a self-sustaining bruiser who embodies rebirth and resilience.
*   **Kai**: A tactical seer who can reveal enemy weaknesses and provide support.
*   **Kane**: Aeron's rival brother and a formidable antagonist.
*   **Micah**: A powerful tank and defensive specialist.
*   **Reverie**: An unpredictable mage who challenges the status quo.
*   **Sky.ix**: The Bionic Goddess, a key figure in the prophecy.
*   **Zaia**: A rogue/assassin specializing in stealth and high-precision strikes.

The game's narrative is driven by the interactions between these characters and their struggle against the forces of the Void.

## 🤝 Contributing

We welcome contributions to "Milehigh.World: Into the Void"! If you'd like to contribute, please fork the repository and submit a pull request. We ask that you follow the existing coding conventions and document your code thoroughly.
