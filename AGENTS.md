# AGENTS.md - Jules Workflow and Coding Standards for MILEHIGH.WORLD

Welcome, Agent. This document provides instructions and standards for developing the **MILEHIGH.WORLD: Into the Void** codebase.

## 🤖 Jules Agent Workflows

Jules is an asynchronous coding agent used to scale development and automate tasks across the repository.

### 1. Parallel Task Execution
Multiple Jules agents can be deployed in parallel using the **Task Multiplier Script**.
- **Configuration:** Populate `tasks.txt` with individual task descriptions.
- **Execution:** Run the provided bash multiplier script to spin up independent Jules instances for each task.

### 2. Infrastructure & Environment
Jules typically operates within a Google Cloud VM environment.
- **Setup:** Use `setup_jules.sh` to configure system dependencies and run baseline tests.
- **Validation:** Ensure all environment variables and API keys are correctly set before starting remote sessions.

### 3. CLI Integration
Leverage the GitHub CLI to pipe issues directly into Jules sessions:
```bash
gh issue view <issue_number> --json body -q .body | jules remote push
```

### 4. Core Commands
- `jules login`: Authenticate your session.
- `jules remote pull`: Fetch cloud-generated changes to your local branch.
- `jules submit`: Commit and push finalized changes for review.

---

## 🛠 Coding Standards

### C# / Unity Standards
- **Namespace:** Use `Milehigh.World.Core` for foundational logic, `Milehigh.World.Entities` for characters/enemies, and `Milehigh.World.Engine` for core systems.
- **Documentation:** Every public class, method, and property MUST have XML documentation (`/// <summary>`).
- **Initialization:** Use `Awake()` for internal initialization and `Start()` for cross-object references. Avoid constructors in `MonoBehaviour` classes.
- **Singletons:** Use the established `Instance` pattern for managers (e.g., `GameManager.Instance`).
- **Licensing:** Every `.cs` file MUST start with:
  ```csharp
  // SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
  ```

### Python Standards
- **Docstrings:** Use Google Style Python Docstrings.
- **Type Hinting:** Use type hints for all function signatures.
- **Automation:** Blender scripts should be placed in `blender_scripts/` and designed to support headless execution.
- **Licensing:** Every `.py` file MUST start with:
  ```python
  # SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
  ```

### Data Management
- **ScriptableObjects:** Use `ScriptableObject` for static game data (stats, item definitions).
- **CharacterData:** The `CharacterData` struct/class should contain: `characterName`, `health`, `resonance`, `integrity`, and `vanguardMultiplier`.
- **Constants:** Maintain parity between `frontend/src/constants.tsx` and the Unity C# data classes.

---

## 🏛 Project Architecture

- **Hybrid Model:** Unity/C# handles real-time gameplay; Python handles data processing, asset validation, and Blender automation.
- **Server-Authoritative:** Combat logic should be designed with a server-authoritative mindset (refer to `ServerCombatManager.cs`).
- **Event-Driven:** Utilize the event system in `Character.cs` (`OnHealthChanged`, `OnDie`) to decouple systems.

## ✅ Verification and Testing

- **Python Tests:** Run `pytest` from the root directory to validate Python scripts and data integrity.
- **C# Combat Simulation:** Use `CombatSimulation.cs` to test battle logic in a standalone environment.
- **Frontend:** Verify TypeScript types in `frontend/src/types.ts` when modifying data structures.

---

*“Into the Void, we find our strength. In the Verse, we find our destiny.”*
