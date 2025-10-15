# Scripts

This directory contains all C# scripts for the game, organized by feature.

## Directory Structure

- **Character:** Scripts related to player characters and NPCs, including movement, abilities, and progression.
- **Combat:** Scripts for the combat system, including weapon handling, damage calculation, and enemy AI.
- **Core:** Core game systems, such as the game manager, scene loader, and event system.
- **Networking:** Scripts for multiplayer functionality.
- **UI:** Scripts for user interface elements, including menus, HUD, and interactable elements.

## Utility Classes

### SpriteEffects.cs
A static utility class for performing visual effects on SpriteRenderers:
- **Flash Effect:** Temporarily changes a sprite's color for a specified duration (useful for damage indicators)
- **Fade Effect:** Smoothly interpolates a sprite's alpha value over time (useful for fade-in/fade-out effects)

Example usage:
```csharp
// Flash the sprite white for 0.2 seconds
SpriteEffects.Flash(this, spriteRenderer, 0.2f, Color.white);

// Fade out the sprite over 1 second
SpriteEffects.Fade(this, spriteRenderer, 1f, 0f);
```

See `SpriteEffectsDemo.cs` for an interactive demonstration.

### SpriteManipulator.cs
A MonoBehaviour class that demonstrates programmatic control over GameObject transforms (position, rotation, scale). This is for technical demonstration purposes.

## Coding Conventions

- Follow the official C# coding conventions.
- Use namespaces to prevent naming conflicts.
- Comment code where necessary to explain complex logic.