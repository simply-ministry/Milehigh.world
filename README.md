# Milehigh.World: Into the Void

This repository contains the source code and assets for the science-fantasy RPG, "Milehigh.World: Into the Void." The project is being developed in Unity, with supplementary Python scripts for managing the asset pipeline.

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
```

### Key Directories:

* **`Assets/Scripts/`**: Contains all C# source code for the Unity project, organized by system.
* **`docs/`**: A directory for all Game Design Documents (GDDs), technical specifications, and narrative outlines.
* **Root Directory**: Contains Python scripts for asset validation, project configuration files, and this README.

## 🚀 Getting Started

### Prerequisites

* **Unity Hub** and a compatible **Unity Editor** version (e.g., 2022.3 LTS or later).
* **Python 3.8+** for running utility scripts.
* **Git** for version control.

### Installation

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/your-username/milehigh.world.git](https://github.com/your-username/milehigh.world.git)
    cd milehigh.world
    ```
2.  **Set up the Python environment:**
    ```bash
    pip install -r requirements.txt
    ```
3.  **Open the project in Unity:**
    * Open Unity Hub.
    * Click "Add" or "Open."
    * Navigate to the cloned repository folder and select it.
    * The project will open in the Unity Editor, ready for development.

## 🛠️ Asset Pipeline

This project uses a custom Python-based pipeline for validating **Universal Scene Description (USD)** assets.

* `usd_parser.py`: This script is used to parse and extract USD data snippets from design documents.
* `test_usd_validation.py`: This script contains unit tests to ensure that USD assets meet the project's technical requirements before being imported into Unity.

To run the validation tests, use the following command:
```bash
python test_usd_validation.py
```