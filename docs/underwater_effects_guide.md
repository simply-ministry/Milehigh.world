# Underwater Visual and Audio Effects Guide

This guide explains how to set up the trigger system for underwater visual and audio effects in Unity. The system is decoupled into two main components: a trigger volume for the water (`WaterVolume.cs`) and an effects manager on the player (`PlayerUnderwaterEffects.cs`).

## 1. Setup Overview

The system works as follows:
1. A `WaterVolume` script is attached to a GameObject with a trigger collider that defines the water area.
2. A `PlayerUnderwaterEffects` script is attached to the player GameObject.
3. When the player enters the `WaterVolume` trigger, the volume's script finds the `PlayerUnderwaterEffects` script on the player and calls its `EnterWater()` method.
4. When the player exits the trigger, the `ExitWater()` method is called.
5. The `PlayerUnderwaterEffects` script is responsible for enabling and disabling all the actual effects (post-processing, audio snapshots, particles, etc.).

---

## 2. Component Setup

### Water Volume GameObject

1.  **Create the Volume**: Create an empty GameObject in your scene (e.g., "WaterZone").
2.  **Add a Collider**: Add a `BoxCollider` (or any other collider shape) to it.
    *   Set the collider's size and position to match your water area.
    *   **Crucially, enable `Is Trigger`** on the collider component.
3.  **Attach the Script**: Add the `Assets/Scripts/Physics/WaterVolume.cs` script to this GameObject.

Your Water Volume GameObject's inspector should look like this:
![Water Volume Inspector](https://i.imgur.com/your_image_url_here.png) <!--- Placeholder for an actual image if one were available -->

### Player GameObject

1.  **Select the Player**: Select your main player GameObject. It **must** have the "Player" tag for the `WaterVolume` to detect it.
2.  **Attach the Effects Script**: Add the `Assets/Scripts/Physics/PlayerUnderwaterEffects.cs` script to the player.
3.  **Configure the Script**: You will need to assign several assets to the script's fields in the Inspector:
    *   **Underwater Post Processing Volume**: A `Volume` component (from Unity's post-processing system) that contains your desired underwater visual effects (e.g., blue fog, lens distortion, caustics). This volume should be **disabled** by default, as the script will enable it.
    *   **Underwater Snapshot**: An `AudioMixerSnapshot` from your game's `AudioMixer`. This snapshot should be configured with underwater audio effects (e.g., a low-pass filter, reverb).
    *   **Above Water Snapshot**: The default `AudioMixerSnapshot` for when the player is not in the water.
    *   **Bubble Particle System**: A `ParticleSystem` GameObject (likely a child of the player) that creates bubble effects. The script will play and stop this system.

Your Player's `PlayerUnderwaterEffects` component inspector should look like this:
![Player Underwater Effects Inspector](https://i.imgur.com/your_other_image_url_here.png) <!--- Placeholder for an actual image if one were available -->

---

## 3. Creating the Effects

### Post-Processing Volume

-   Using the Volume framework (URP or HDRP), create a new Volume profile for underwater effects.
-   Add overrides for effects like:
    -   **Fog**: To give the water a murky, colored look.
    -   **Lens Distortion**: To simulate the way light refracts.
    -   **Bloom / Chromatic Aberration**: For subtle visual enhancements.
-   Assign this profile to a `Volume` component in your scene and link that component to the `PlayerUnderwaterEffects` script.

### Audio Mixer Snapshots

-   Open your main `AudioMixer` (`Window > Audio > Audio Mixer`).
-   In the "Snapshots" list, create two snapshots: "AboveWater" and "Underwater".
-   Select the "Underwater" snapshot. While it is active, adjust the properties of your audio groups. A common technique is to add a **Low-pass Filter** to your master group to muffle sounds.
-   Drag these two snapshots into the corresponding fields on the `PlayerUnderwaterEffects` script. The script will handle smoothly transitioning between them.