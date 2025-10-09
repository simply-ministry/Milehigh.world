# Milehigh.World: Into the Void

## 1. Overview

**Milehigh.World: Into the Void** is a prototype for a Sci-Fi Action RPG developed on the Unity platform, designed for the Meta Quest. This repository contains all source code, design documents, and utilities related to the project.

The game features a rich narrative, a diverse cast of characters known as the Ɲōvəmîŋāđ, and a unique, server-authoritative architecture.

## 2. Table of Contents

- [Overview](#1-overview)
- [Core Concepts](#3-core-concepts)
- [Repository Structure](#4-repository-structure)
- [Setup and Installation](#5-setup-and-installation)
- [How to Run](#6-how-to-run)
- [Documentation](#7-documentation)

## 3. Core Concepts

This project is built on several key technical and design pillars:

- **Engine**: The project is developed in **Unity 2022.3.20f1** with C# as the primary scripting language.
- **Server-Authoritative Architecture**: As outlined in `SECURITY.md`, the game is designed with a server-first security model. The `ServerCombatManager.cs` script is a reference implementation of this, where the server is the source of truth for all combat validation to prevent cheating.
- **Modular Interaction System**: Player interactions are handled by a flexible system consisting of an `Interactor.cs` script on the player and an abstract `Interactable.cs` base class. This allows for easy creation of new interactable objects like NPCs, teleporters, and launchpads. The `AllianceTowerManager.cs` serves as a central hub for managing these interactions in a scene.
- **USD-Based Asset Pipeline**: The project utilizes a unique pipeline for defining and importing 3D assets. USD (Universal Scene Description) code snippets are embedded within Markdown documents (like `document.md`), which are then parsed by a Python script (`usd_parser.py`) and can be loaded into Unity at runtime, as demonstrated in `UsdImportExample.cs`.

## 4. Repository Structure

The repository is organized as follows:

```
.
├── Assets/
│   ├── Scripts/
│   │   ├── Character/      # C# scripts for all characters, playable and NPC.
│   │   ├── Combat/         # C# scripts for combat mechanics (abilities, damage).
│   │   ├── Core/           # C# scripts for core systems (interaction, scene management).
│   │   ├── Physics/        # C# scripts for custom physics (collisions, water effects).
│   │   └── Story/          # C# scripts for managing narrative cutscenes.
│   └── ...                 # Other standard Unity asset folders (Scenes, Prefabs, etc.).
├── docs/                   # All design and technical documentation.
├── .gitignore
├── README.md               # This file.
├── requirements.txt        # Python dependencies for utility scripts.
├── test_usd_validation.py  # Tests for the USD asset pipeline.
└── usd_parser.py           # Python script to extract USD snippets from Markdown.
```

## 5. Setup and Installation

### Prerequisites

- **Unity Hub** with **Unity 2022.3.20f1** installed.
- **Python 3.x**
- **Git**

### Steps

1.  **Clone the repository:**
    ```bash
    git clone <repository_url>
    cd <repository_directory>
    ```

2.  **Set up the Python environment:**
    It is recommended to use a virtual environment.
    ```bash
    python -m venv venv
    source venv/bin/activate  # On Windows, use `venv\Scripts\activate`
    pip install -r requirements.txt
    ```

3.  **Open the Unity Project:**
    - Open Unity Hub.
    - Click "Open" -> "Add project from disk".
    - Navigate to the cloned repository's root directory and select it.
    - The project will now be available in your Unity Hub project list.

## 6. How to Run

### Running the Game in the Editor

1.  Open the project in Unity.
2.  In the `Project` window, navigate to the `Assets/Scenes` directory.
3.  Double-click on a scene file (e.g., a test scene or main menu) to open it.
4.  Press the **Play** button at the top of the editor to run the scene.

### Running the Python Scripts

Ensure your Python virtual environment is activated.

-   **To run the USD parser:**
    ```bash
    python usd_parser.py
    ```
-   **To run the Python unit tests:**
    ```bash
    python -m unittest discover
    ```

## 7. Documentation

The primary design document for the game is the **Game Design Document (GDD)**. It provides a comprehensive overview of the game's concept, mechanics, narrative, and technical specifications.

- [**Read the full Game Design Document here](./docs/GDD.md)**

Additional technical and design documents can also be found in the `/docs` directory. The source code itself is now fully documented with XML and Google-style docstrings.