# Milehigh.World: Into the Void

This repository contains the source code and assets for the science-fantasy RPG, "Milehigh.World: Into the Void." The project is being developed in Unity, with supplementary Python scripts for managing the asset pipeline.

## 🎮 Project Overview

"Milehigh.World: Into the Void" is a narrative-driven RPG set in a fragmented universe known as The Verse. The story follows the Ɲōvəmîŋāđ, ten chosen individuals destined to fulfill or prevent a prophecy that will determine the fate of their world. The game blends advanced technology with mystical forces, featuring a diverse cast of characters and a deep, branching narrative.

## 📂 Repository Structure

This repository is organized to maintain a clean and scalable workflow between the Unity project and external tools.

```
.
├── Assets/
│   ├── Scripts/
│   │   ├── Character/      # C# scripts for all characters, playable and NPC.
│   │   ├── Combat/         # C# scripts for combat mechanics (abilities, damage).
│   │   ├── Core/           # C# scripts for core systems (interaction, scene management).
│   │   ├── Physics/        # C# scripts for custom physics (collisions, water effects).
│   │   └── Story/          # C# scripts for managing narrative cutscenes.
│   └── ...               # Other standard Unity asset folders (Scenes, Prefabs, etc.).
├── docs/                   # All design and technical documentation.
├── .gitignore
├── README.md               # This file.
├── requirements.txt        # Python dependencies for utility scripts.
├── test_usd_validation.py  # Tests for the USD asset pipeline.
└── usd_parser.py           # Python script to extract USD snippets from Markdown.
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