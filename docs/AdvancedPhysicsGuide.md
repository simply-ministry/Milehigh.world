# Guide: Using the Advanced Physics System

**Author:** Milehigh.World: Into the Void Project Documentation

This document provides instructions on how to use the custom physics components created for the "Milehigh.World: Into the Void" project. These scripts are designed to extend Unity's built-in physics and provide more control over friction, air resistance, and collision responses.

---

## Table of Contents
1.  [AdvancedPhysics Component](#1-advancedphysics-component)
    *   [Purpose](#purpose)
    *   [How to Use](#how-to-use)
    *   [Properties](#properties)
2.  [CollisionManager Component](#2-collisionmanager-component)
    *   [Purpose](#purpose-1)
    *   [How to Use](#how-to-use-1)
    *   [Properties](#properties-1)
    *   [Important Considerations](#important-considerations)

---

## 1. AdvancedPhysics Component

The `AdvancedPhysics.cs` script allows you to add more nuanced physical behaviors like air resistance and surface friction to your GameObjects.

### Purpose
To simulate more realistic movement by applying forces that oppose an object's motion, such as drag through the air and friction when sliding on a surface. This provides more control than Unity's standard `Physic Material`.

### How to Use
1.  Select the GameObject in your Unity scene that you want to apply these effects to.
2.  Ensure the GameObject has a `Rigidbody` component attached. If not, add one via `Component > Physics > Rigidbody`.
3.  Click the **"Add Component"** button in the Inspector.
4.  Search for **"AdvancedPhysics"** and add it to the GameObject.
5.  Adjust the `Friction` and `Air Resistance` properties in the Inspector to achieve the desired behavior.

### Properties

*   **Friction** (`float`):
    *   **Description:** This value determines how much an object slows down when sliding along a surface. It is only applied when the component detects that the object is "grounded."
    *   **Range:** 0 to 2. A value of 0 means no friction, while higher values will cause the object to slow down more quickly.
    *   **Default:** 0.2

*   **Air Resistance** (`float`):
    *   **Description:** This value simulates the drag an object experiences as it moves through the air. It is always active and is proportional to the square of the object's velocity, meaning it has a much stronger effect at high speeds.
    *   **Default:** 0.1

---

## 2. CollisionManager Component

The `CollisionManager.cs` script provides a more physically realistic collision response by manually calculating and applying impulses based on the conservation of momentum.

### Purpose
To create more dynamic and "bouncy" interactions between objects. This script overrides Unity's default collision handling to use a custom calculation that factors in the object's mass and a "restitution" (bounciness) coefficient.

### How to Use
1.  Select the GameObject in your Unity scene that you want to have this advanced collision response.
2.  Ensure the GameObject has a `Rigidbody` component.
3.  Click the **"Add Component"** button in the Inspector.
4.  Search for **"CollisionManager"** and add it to the GameObject.
5.  For the best results, any other object it collides with should also have the `CollisionManager` component.
6.  Adjust the `Restitution` property in the Inspector.

### Properties

*   **Restitution** (`float`):
    *   **Description:** This determines the "bounciness" of the object during a collision.
    *   **Range:** 0 to 1. A value of 0 represents a perfectly *inelastic* collision (the objects lose all bounce), while a value of 1 represents a perfectly *elastic* collision (no kinetic energy is lost to bounce).
    *   **Default:** 0.8

### Important Considerations

*   **Manual Calculation:** This script performs its own physics calculations for collision response. To avoid conflicts with Unity's internal physics engine, it's best used when you want a specific, non-standard collision behavior.
*   **One-Sided Calculation:** The script is designed so that only one of the two colliding objects calculates the response. This prevents the impulse from being applied twice. This is handled automatically using the objects' instance IDs.
*   **Performance:** For scenes with a very high number of collisions, be mindful that these custom calculations might have a performance impact compared to Unity's highly optimized native physics. It is best suited for key objects where the collision behavior is visually important.