# Test Environment Setup Guide

This guide will walk you through setting up the `CharacterTest.unity` scene to verify the functionality of the foundational character and physics scripts.

## 1. Open the Test Scene
- Launch the Unity Editor and open the project.
- In the **Project** window, navigate to `Assets/Scenes/Test/`.
- Double-click on the `CharacterTest.unity` scene to open it.

## 2. Create the Ground
- In the **Hierarchy** window, right-click and select **3D Object > Plane**.
- Rename the newly created `Plane` object to `Ground`.
- In the **Inspector** for the `Ground` object, set its **Position** to `(0, 0, 0)` and **Scale** to `(10, 1, 10)` to create a large floor.

## 3. Create the Player Character (Aeron)
- In the **Hierarchy** window, right-click and select **3D Object > Capsule**.
- Rename the `Capsule` object to `Player`.
- In the **Inspector** for the `Player` object, set its **Position** to `(0, 1.5, 0)`.

## 4. Configure the Player Components
Select the `Player` object in the Hierarchy and add the following components using the **"Add Component"** button in the Inspector:

- **Rigidbody**:
  - This is Unity's core physics component. Leave its properties at their default values for now.

- **Aeron**:
  - Search for `Aeron` and add the script. This will automatically add the base `Character` script as a dependency.
  - You will see Aeron's stats (Health, Attack, etc.) in the Inspector.

- **Player Controller**:
  - Search for `PlayerController` and add the script. This will handle movement input.

- **Advanced Physics**:
  - Search for `AdvancedPhysics` and add the script.
  - You can adjust the `Friction` and `Air Resistance` values to see how they affect movement.

- **Collision Manager**:
  - Search for `CollisionManager` and add the script.
  - You can adjust the `Restitution` (bounciness) value.

## 5. Set Up the Main Camera
- In the **Hierarchy**, select the `Main Camera`.
- Set its **Position** to `(0, 5, -10)`.
- Set its **Rotation** to `(20, 0, 0)` to angle it down towards the player.

## 6. Create an Interactable Prop for Collision Testing
- In the **Hierarchy**, right-click and select **3D Object > Cube**.
- Rename the `Cube` object to `BouncyCube`.
- Set its **Position** to `(3, 1, 3)`.
- In the Inspector, add the following components to the `BouncyCube`:
  - **Rigidbody**: Allows it to be affected by physics.
  - **Collision Manager**: Search for and add the `CollisionManager` script. Set its **Restitution** to `1` to make it very bouncy.

## 7. Run the Test
- Press the **Play** button at the top of the Unity Editor.
- **Movement:** Use the **WASD** or **arrow keys** to move the `Player` capsule around the `Ground`. You should see the effects of the `AdvancedPhysics` script in how the character moves and slows down.
- **Collision:** Move the `Player` into the `BouncyCube`. You should see a dynamic, bouncy collision response, as calculated by the `CollisionManager` script.

This setup provides a basic but effective way to verify that the core character and physics systems are working together correctly. You can now use this scene as a sandbox to further develop and test new features.