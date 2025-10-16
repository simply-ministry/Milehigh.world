# Milehigh.World: Into the Void

Welcome to the official repository for the science-fantasy RPG, "Milehigh.World: Into the Void." This project is a narrative-driven RPG developed in Unity, supplemented by a suite of Python scripts for asset management, database control, and game logic prototyping.

## 🎮 Project Overview

**Mîlēhîgh.wørld** is a science-fantasy RPG that explores a universe defined by a central conflict between high-tech advancement and ancient mysticism. The narrative follows the **Ɲōvəmîŋāđ**, a group of ten preordained heroes, as they navigate a fragmented reality to either fulfill or prevent the Lost Prophecy of Lîŋq and achieve **Millenia**—an enduring era of peace.

The inciting event is an invasion by **King Cyrus** through the **Onalym Nexus**, a dimensional bridge, which shatters reality and kicks off the gathering of the Ɲōvəmîŋāđ. The primary antagonist is **The Void**, a corrupting "digital abyss," and its cunning manipulator, **Lucent the Lightweaver**. The game's design is built on the juxtaposition of technologies like cybernetics and quantum teleportation with mystical forces such as Phoenix and Dragon powers.

For a complete narrative and world-building reference, see the [Game Design Document](docs/GDD.md).

## 📂 Repository Structure

This repository is organized to maintain a clean and scalable workflow between the Unity project and external tools.

```
.
├── Assets/
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
└── ...                   # Other project files (tests, configs).
│   ├── Scripts/
│   │   ├── Character/      # C# scripts for all characters, playable and NPC.
│   │   ├── Combat/         # C# scripts for combat mechanics (abilities, damage).
│   │   ├── Core/           # C# scripts for core systems (interaction, scene management).
│   │   ├── Physics/        # C# scripts for custom physics (collisions, water effects).
│   │   └── Story/          # C# scripts for managing narrative cutscenes.
│   └── ...               # Other standard Unity asset folders (Scenes, Prefabs, etc.).
├── blender_scripts/        # Python scripts for automating tasks in Blender.
├── docs/                   # All design and technical documentation.
├── .gitignore
├── README.md               # This file.
├── requirements.txt        # Python dependencies for utility scripts.
├── database.py             # Python script for managing the game's SQLite database.
├── game.py                 # A Python-based prototype of the game's core mechanics.
├── rpg.py                  # A more complex Python-based RPG prototype.
├── simple_rpg.py           # A simplified Python RPG for testing specific features.
├── test_*.py               # Pytest files for all Python scripts.
└── usd_parser.py           # Python script to extract USD snippets from Markdown.
```

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

### Installation

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/your-username/milehigh.world.git
    cd milehigh.world
    ```
2.  **Set up the Python environment:**
    ```bash
    pip install -r requirements.txt
    ```
3.  **Open the project in Unity:**
    *   Open Unity Hub.
    *   Click "Add" or "Open."
    *   Navigate to the cloned repository folder and select it.
    *   The project will open in the Unity Editor, ready for development.

## 🛠️ Python Scripts

This project uses a number of Python scripts for various purposes. Here's a brief overview:

*   **`database.py`**: Manages the game's SQLite database, which stores information about characters, items, quests, and more.
*   **`game.py`**, **`rpg.py`**, **`simple_rpg.py`**: These files are Python-based prototypes of the game's core mechanics. They are used for testing and iterating on game logic before implementing it in C#.
*   **`usd_parser.py`**: This script is used to parse and extract USD data snippets from design documents.
*   **`test_*.py`**: These files contain unit tests for the Python scripts, written using the `pytest` framework.

To run the tests, use the following command:

```bash
pytest
```

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