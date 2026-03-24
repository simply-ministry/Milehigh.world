Mastering Programmatic Scene Management in Unity
1. Introduction to Programmatic Scene Management in Unity
Scenes are the foundational building blocks of any Unity project, serving as containers for game worlds, individual levels, user interface (UI) elements, and other distinct segments of an application. While the Unity Editor provides intuitive tools for manually creating and arranging scenes, the true power for crafting dynamic, complex, and scalable experiences lies in programmatic scene management. Scripting allows developers to control scene loading, unloading, creation, and modification at runtime and within the editor, enabling a vast range of functionalities from intricate level transitions to sophisticated custom development tools.
1.1. The Imperative of Scene Scripting
The necessity for programmatic control over scenes becomes apparent when developing games or applications that extend beyond simple, static structures. Dynamic game experiences, such as procedural level generation, adaptive environments that change based on player actions, or modular UI systems where different menus or HUD elements are loaded as separate scenes, all rely heavily on scripting. Furthermore, for efficient development workflows, especially in larger teams, programmatic scene management can automate repetitive tasks, ensure consistency in scene setup, and facilitate the creation of powerful custom editor tools.
Unity's architecture for scene management is bifurcated, providing distinct Application Programming Interfaces (APIs) tailored to the specific context of operation: one for managing scenes while the game is executing (runtime) and another for manipulating scenes within the Unity Editor environment (editor-time). This separation is a cornerstone of Unity's design, reflecting a deliberate effort to optimize performance and maintain a clear distinction between development-time utilities and runtime necessities. The runtime APIs are streamlined for efficiency, ensuring that no unnecessary editor-specific code is included in the final build of an application. Conversely, the editor-time APIs offer a richer set of functionalities tightly integrated with the editor's features, empowering developers to extend and customize their development environment. Adherence to this separation, by using appropriate mechanisms such as designated "Editor" folders for scripts or preprocessor directives, is not merely a best practice but a technical requirement to ensure projects are buildable, efficient, and free from runtime errors that could arise from attempting to invoke editor-specific code in a deployed game.
The limitations of manual scene handling become particularly evident in scenarios requiring dynamic content. For instance, if a game features levels that are assembled based on player progression or a UI that loads and unloads different modules (e.g., inventory, map, settings) without a full screen transition, static scene setups configured at build time are insufficient. These dynamic requirements necessitate scripting APIs that can manage scene lifecycles in response to in-game events or application logic, thereby enabling more fluid and responsive user experiences.
1.2. Core Distinction: Runtime vs. Editor-Time Scene Management
Understanding the fundamental difference between runtime and editor-time scene management is paramount for any Unity developer aiming to leverage scene scripting effectively.
Runtime Scene Management: This pertains to the manipulation of scenes while the game or application is actively running. Common runtime operations include loading a new game level, additively loading a UI scene on top of the current gameplay scene, or unloading scenes to free up resources. These operations are governed by the SceneManager class within the UnityEngine.SceneManagement namespace. This API is designed to be lightweight and efficient, suitable for inclusion in final game builds.
Editor-Time Scene Management: This involves manipulating scenes within the Unity Editor environment, primarily for development and content creation purposes. Examples include creating a new scene through a custom editor tool, batch-modifying properties across multiple scenes, or setting up specific scene configurations for Play Mode testing. These operations are handled by the EditorSceneManager class, located in the UnityEditor.SceneManagement namespace. The EditorSceneManager extends the functionality of the runtime SceneManager, providing additional features specific to the editor context. It is crucial to note that code using EditorSceneManager should be strictly confined to editor scripts, as these APIs are not available in standalone builds. Documentation explicitly states, "In Edit mode in the Editor, SceneManager cannot be used and a scene can only be loaded once (using EditorSceneManager)" , and for methods like EditorSceneManager.OpenScene, "Do not use it for loading Scenes at run time. To load Scenes at run time, see SceneManager.LoadScene".
A clear grasp of this runtime versus editor-time distinction is essential for writing code that is not only functional but also correct, efficient, and free from build errors that can arise from attempting to use editor-specific APIs in a runtime context.
1.3. Overview of Key Namespaces
Two primary namespaces are central to programmatic scene management in Unity:
UnityEngine.SceneManagement: This namespace contains the SceneManager class for runtime scene operations. It also houses the Scene struct, which represents a loaded scene instance, and enums like LoadSceneMode used in loading operations.
UnityEditor.SceneManagement: This namespace provides the EditorSceneManager class for editor-specific scene manipulations. It also includes auxiliary classes like SceneSetup for configuring scene loading in the editor and various editor-specific events related to scene changes.
2. Runtime Scene Operations with UnityEngine.SceneManagement.SceneManager
The UnityEngine.SceneManagement.SceneManager class is the cornerstone for all scene manipulations that occur while the game is running. It is a static class, meaning its methods and properties are accessed directly without needing an instance of the class. Its primary responsibilities include loading new scenes, unloading existing ones, creating empty scenes dynamically, and providing access to information about currently loaded scenes.
2.1. Overview of SceneManager
SceneManager provides a comprehensive suite of tools for managing the lifecycle of scenes during gameplay. This includes transitioning between different game levels, additively loading UI or other gameplay elements, and managing the overall scene composition of the application at runtime.
2.2. Loading Scenes
Loading scenes is one of the most frequent operations performed via SceneManager. Unity offers both synchronous and asynchronous methods for this purpose.
2.2.1. Synchronous Loading with LoadScene
The SceneManager.LoadScene method provides a way to load scenes synchronously. It can be called with either the scene's name (as a string) or its build index (an integer representing its order in the Build Settings). SceneManager.LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single) SceneManager.LoadScene(int sceneBuildIndex, LoadSceneMode mode = LoadSceneMode.Single)
The LoadSceneMode enum determines how the scene is loaded:
LoadSceneMode.Single: This is the default mode. When a scene is loaded in Single mode, all currently loaded scenes are automatically unloaded before the new scene is loaded. This is typically used for transitioning between distinct game levels or states.
LoadSceneMode.Additive: This mode loads the specified scene without unloading any currently active scenes. The new scene's contents are added to the existing hierarchy, allowing multiple scenes to be active and rendered simultaneously. This is useful for modular game design, such as loading a persistent UI scene, a player character scene, or additional environmental details on top of a base level scene.
It is important to note that "When using SceneManager.LoadScene, the scene loads in the next frame, that is it does not load immediately". While this might seem fast, for complex scenes, this can still introduce a noticeable stutter or pause in gameplay.
C# Example: Single and Additive Loading
using UnityEngine;
using UnityEngine.SceneManagement;

public class RuntimeSceneLoader : MonoBehaviour
{
    public string sceneToLoadByName = "Level01";
    public int sceneToLoadByIndex = 1;

    // Example of loading a scene in Single mode
    public void LoadSceneSingle()
    {
        SceneManager.LoadScene(sceneToLoadByName, LoadSceneMode.Single);
    }

    // Example of loading a scene additively
    public void LoadSceneAdditive()
    {
        // Assuming sceneToLoadByIndex is a valid index from Build Settings
        SceneManager.LoadScene(sceneToLoadByIndex, LoadSceneMode.Additive);
        Debug.Log(sceneToLoadByIndex + " additively loaded.");
    }
}


This example demonstrates basic calls to LoadScene using both scene name and build index, illustrating single and additive loading modes.
The prevalence of LoadSceneMode.Additive in documentation and examples and its central role in multi-scene editing workflows point towards a design philosophy in Unity that favors modularity. This approach allows developers to construct complex game worlds by composing multiple, smaller, and potentially reusable scenes. For instance, a main game environment might be one scene, the player character and its logic another, the UI a third, and specific mission objectives or dynamic events loaded as further additive scenes. This not only promotes better organization and reusability of assets and logic but also facilitates collaborative development, as different team members can work on separate scene modules concurrently.
2.2.2. Asynchronous Loading with LoadSceneAsync
For most practical applications, especially when loading scenes that might take more than a fraction of a second, synchronous loading can lead to undesirable game freezes or performance hiccups. To address this, SceneManager provides LoadSceneAsync. As stated in the documentation, "In most cases, to avoid pauses or performance hiccups while loading, you should use the asynchronous version of this command which is: LoadSceneAsync". This method loads the scene in the background, allowing the game to continue running smoothly.
SceneManager.LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single) SceneManager.LoadSceneAsync(int sceneBuildIndex, LoadSceneMode mode = LoadSceneMode.Single) SceneManager.LoadSceneAsync(string sceneName, LoadSceneParameters parameters) SceneManager.LoadSceneAsync(int sceneBuildIndex, LoadSceneParameters parameters)
LoadSceneAsync returns an AsyncOperation object. This object provides information about the loading process and allows for some control over it:
progress: A float value between 0.0 and 1.0 indicating the progress of the loading operation. Note that this value typically reaches 0.9 when the loading is mostly done, and the final 0.1 is for the activation phase.
isDone: A boolean that becomes true when the scene has finished loading (including activation).
allowSceneActivation: A boolean property (defaults to true) that can be set to false to prevent the loaded scene from activating automatically. This allows the game to load a scene completely in the background and then activate it at an opportune moment (e.g., after a "Press any key to continue" prompt).
completed: An event that is triggered when the asynchronous operation has completed.
The existence and strong recommendation for LoadSceneAsync directly stem from the potential negative impact of LoadScene on user experience. By providing AsyncOperation with properties like progress and allowSceneActivation, and the completed event, Unity empowers developers to manage the loading process explicitly. This enables the implementation of loading screens, progress bars, or other feedback mechanisms, transforming a potentially jarring pause into a managed and more pleasant transition for the player.
C# Example: Asynchronous Loading with Progress and Activation Control
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AsyncSceneLoader : MonoBehaviour
{
    public string sceneToLoadAsync = "LargeLevel";

    public void StartAsyncLoad()
    {
        StartCoroutine(LoadSceneInBackground());
    }

    IEnumerator LoadSceneInBackground()
    {
        Debug.Log("Starting to load scene: " + sceneToLoadAsync);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoadAsync, LoadSceneMode.Additive);

        // Prevent the scene from activating until loading is almost complete
        asyncLoad.allowSceneActivation = false;

        // Wait until the asynchronous scene fully loads
        while (asyncLoad.progress < 0.9f) // Scene loading is ~90% complete
        {
            Debug.Log("Loading progress: " + (asyncLoad.progress * 100) + "%");
            yield return null;
        }

        Debug.Log("Scene is ready to activate.");
        // Here you could, for example, show a "Press Space to Continue" message
        // For this example, we'll just activate it after a small delay or key press.
        // For simplicity, activating immediately after loading is complete enough for this example.
        asyncLoad.allowSceneActivation = true;

        // Wait until the scene is fully activated
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Debug.Log("Scene " + sceneToLoadAsync + " loaded and activated.");
        Scene loadedScene = SceneManager.GetSceneByName(sceneToLoadAsync);
        if (loadedScene.IsValid())
        {
            Debug.Log(loadedScene.name + " has " + loadedScene.rootCount + " root GameObjects.");
        }
    }
}


This example demonstrates initiating an asynchronous load, monitoring its progress, and controlling its activation.
2.3. Unloading Scenes with UnloadSceneAsync
When scenes are loaded additively, they remain in memory until explicitly unloaded. The SceneManager.UnloadSceneAsync method is used for this purpose, allowing scenes to be removed from the game and their resources to be freed. This is crucial for managing memory and performance, especially in games with many dynamically loaded and unloaded parts.
SceneManager.UnloadSceneAsync(Scene scene) SceneManager.UnloadSceneAsync(string sceneName) SceneManager.UnloadSceneAsync(int sceneBuildIndex)
Like LoadSceneAsync, UnloadSceneAsync returns an AsyncOperation that can be monitored.
C# Example: Unloading an Additively Loaded Scene
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUnloader : MonoBehaviour
{
    public string sceneToUnloadName = "UIScene_Module";

    public void UnloadUIScene()
    {
        Scene sceneToUnload = SceneManager.GetSceneByName(sceneToUnloadName);
        if (sceneToUnload.IsValid() && sceneToUnload.isLoaded)
        {
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(sceneToUnload);
            if (unloadOperation == null)
            {
                Debug.LogError("Failed to start unloading scene: " + sceneToUnloadName);
                return;
            }
            unloadOperation.completed += OnUnloadCompleted;
            Debug.Log("Unloading scene: " + sceneToUnloadName);
        }
        else
        {
            Debug.LogWarning("Scene " + sceneToUnloadName + " is not loaded or is invalid.");
        }
    }

    private void OnUnloadCompleted(AsyncOperation operation)
    {
        Debug.Log("Scene " + sceneToUnloadName + " successfully unloaded.");
        // Remove the callback to prevent it from being called multiple times if this method is reused
        operation.completed -= OnUnloadCompleted;
    }
}


This example shows how to get a reference to a loaded scene by name and then initiate its asynchronous unloading, including handling the completed event.
2.4. Creating Scenes at Runtime with CreateScene
Unity allows for the creation of new, empty scenes at runtime using SceneManager.CreateScene(string sceneName). These dynamically created scenes exist only in memory and "Scenes cannot be saved at runtime". This means their content is transient unless explicitly moved to other, persistent scenes or managed through other means.
Potential use cases for CreateScene include:
Creating temporary staging areas for objects before they are introduced into the main game world.
Dynamically generating content within an isolated environment.
Setting up sandbox environments for AI testing or physics simulations.
The ability to create scenes programmatically at runtime, even if they cannot be saved directly, opens avenues for advanced development techniques. For instance, complex procedural content could be generated within a temporary scene. Once validated or finalized, the relevant GameObjects from this temporary scene can then be moved into a persistent game scene using SceneManager.MoveGameObjectToScene or merged using SceneManager.MergeScenes. This workflow can isolate complex instantiation processes, potentially improving performance by offloading them from the main game loop, and also helps in managing the lifecycle of temporary or procedurally generated objects more cleanly.
2.5. Accessing and Querying Loaded Scenes
SceneManager provides several properties and methods to query the state of loaded scenes:
GetActiveScene(): Returns a Scene struct representing the currently active scene. The active scene is the one into which new GameObjects are instantiated by default, and its lighting settings are typically used unless overridden.
SetActiveScene(Scene scene): Sets a given loaded scene as the active scene. This is particularly important when working with multiple additively loaded scenes, as it dictates which scene's RenderSettings, LightmapSettings, NavMesh data, and Occlusion Culling data are used.
sceneCount / loadedSceneCount: Returns the total number of scenes currently loaded in memory.
sceneCountInBuildSettings: Returns the number of scenes included in the project's Build Settings.
GetSceneAt(int index): Retrieves a Scene struct by its index in the internal list of loaded scenes (this is not the build index).
GetSceneByName(string name): Retrieves a Scene struct for a loaded scene by its name.
GetSceneByPath(string scenePath): Retrieves a Scene struct for a loaded scene using its asset path (e.g., "Assets/Scenes/MyLevel.unity").
GetSceneByBuildIndex(int buildIndex): Retrieves a Scene struct using its index in the Build Settings.
C# Example: Logging Scene State
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;

public class SceneStateLogger : MonoBehaviour
{
    void Start()
    {
        LogCurrentSceneState();
    }

    public void LogCurrentSceneState()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("--- SceneManager State ---");

        Scene activeScene = SceneManager.GetActiveScene();
        sb.AppendLine($"Active Scene: {activeScene.name} (Path: {activeScene.path}, Build Index: {activeScene.buildIndex})");

        sb.AppendLine($"Total Scenes in Build Settings: {SceneManager.sceneCountInBuildSettings}");
        sb.AppendLine($"Currently Loaded Scenes: {SceneManager.sceneCount}");

        sb.AppendLine("Loaded Scenes Details:");
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene s = SceneManager.GetSceneAt(i);
            sb.AppendLine($"  - Index {i}: {s.name} (Path: {s.path}, Build Index: {s.buildIndex}, Is Loaded: {s.isLoaded})");
        }
        Debug.Log(sb.ToString());
    }
}


This script demonstrates how to use various SceneManager methods to gather and log information about the current scene setup, which can be invaluable for debugging multi-scene applications.
2.6. Scene Manipulation
SceneManager also offers methods to manipulate the contents of scenes at runtime:
MergeScenes(Scene sourceScene, Scene destinationScene): Moves all root GameObjects from the sourceScene to the destinationScene. The sourceScene is then removed. This can be useful for combining dynamically generated content into a main scene.
MoveGameObjectToScene(GameObject go, Scene scene): Moves a specific GameObject (and its entire hierarchy) from its current scene to the specified destinationScene. Both scenes must already be loaded.
MoveGameObjectsToScene(NativeArray<int> instanceIDs, Scene scene): Moves multiple GameObjects, identified by their instance IDs, to a target scene.
2.7. SceneManager Events
To allow for reactive programming and decoupled systems, SceneManager exposes several events:
activeSceneChanged(Scene current, Scene next): This event is invoked when the active scene changes, typically after a call to SceneManager.SetActiveScene or when a new scene loaded in Single mode becomes active. It provides references to the previously active scene (current) and the newly active scene (next).
sceneLoaded(Scene scene, LoadSceneMode mode): This event is fired when a scene has finished loading. It provides the Scene struct of the loaded scene and the LoadSceneMode used.
sceneUnloaded(Scene scene): This event is triggered after a scene has been successfully unloaded. It provides the Scene struct of the unloaded scene.
Subscribing to these events allows scripts to perform actions automatically when scene states change, such as initializing game systems when a level loads, saving player data when a scene unloads, or updating UI elements when the active scene changes.
The sceneLoaded event is particularly potent when combined with asynchronous additive loading (LoadSceneAsync with LoadSceneMode.Additive). Because LoadSceneAsync completes at an indeterminate time and LoadSceneMode.Additive introduces new GameObjects into an existing hierarchy, the sceneLoaded event serves as a precise signal that the new scene's content is available. Systems can subscribe to this event and, upon its firing, safely perform initialization, resolve dependencies, or activate logic for GameObjects within the newly loaded scene. This promotes an event-driven architecture, which is generally more robust and easier to maintain than systems that poll for scene readiness or rely on hard-coded delays or direct coupling to the loading invocation code.
C# Example: Handling sceneLoaded and activeSceneChanged Events
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEventHandler : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene Loaded: {scene.name} (Mode: {mode})");
        // Example: Initialize systems specific to this scene
        if (scene.name == "Level01_Gameplay")
        {
            // Find and initialize LevelManager for Level01
        }
    }

    void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
        Debug.Log($"Active Scene Changed From: {(previousScene.IsValid()? previousScene.name : "N/A")} To: {newScene.name}");
        // Example: Update lighting or audio based on the new active scene
    }

    void OnSceneUnloaded(Scene scene)
    {
        Debug.Log($"Scene Unloaded: {scene.name}");
        // Example: Clean up resources associated with the unloaded scene
    }
}


This example demonstrates how to subscribe to and unsubscribe from SceneManager events and implement handler methods.
2.8. SceneManager API Quick Reference
The following table provides a quick reference to the key members of the UnityEngine.SceneManagement.SceneManager class.
Member Type
Name
Brief Description
Key Parameters/Return Value
Method
LoadScene
Loads a scene synchronously.
string sceneName or int sceneBuildIndex, LoadSceneMode mode
Method
LoadSceneAsync
Loads a scene asynchronously in the background.
string sceneName or int sceneBuildIndex, LoadSceneMode mode or LoadSceneParameters parameters. Returns AsyncOperation.
Method
UnloadSceneAsync
Unloads a scene asynchronously.
Scene scene, string sceneName, or int sceneBuildIndex. Returns AsyncOperation.
Method
CreateScene
Creates a new empty scene at runtime (in memory).
string sceneName. Returns Scene.
Method
GetActiveScene
Gets the currently active Scene.
Returns Scene.
Method
SetActiveScene
Sets a loaded scene as the active scene.
Scene scene. Returns bool.
Property
sceneCount
The number of currently loaded scenes.
Returns int.
Property
loadedSceneCount
(Same as sceneCount) The number of loaded Scenes.
Returns int.
Property
sceneCountInBuildSettings
Number of scenes listed in Build Settings.
Returns int.
Method
GetSceneAt
Gets the Scene at the given index in the list of loaded scenes.
int index. Returns Scene.
Method
GetSceneByName
Searches loaded scenes for a Scene with the given name.
string name. Returns Scene.
Method
GetSceneByPath
Searches loaded scenes for a Scene by its asset path.
string scenePath. Returns Scene.
Method
GetSceneByBuildIndex
Gets a Scene struct from a build index.
int buildIndex. Returns Scene.
Method
MergeScenes
Merges the source Scene into the destination Scene.
Scene sourceScene, Scene destinationScene.
Method
MoveGameObjectToScene
Moves a GameObject from its current Scene to a new Scene.
GameObject go, Scene scene.
Event
activeSceneChanged
Fired when the active Scene has changed.
Delegate UnityAction<Scene, Scene> (current, next).
Event
sceneLoaded
Fired when a Scene has loaded.
Delegate UnityAction<Scene, LoadSceneMode> (scene, mode).
Event
sceneUnloaded
Fired when a Scene has unloaded.
Delegate UnityAction<Scene> (scene).

This table serves as an at-a-glance summary, reinforcing the detailed explanations and aiding developers in quickly identifying the appropriate API for specific runtime scene management tasks.
3. Editor-Time Scene Control with UnityEditor.SceneManagement.EditorSceneManager
While SceneManager handles runtime scene operations, UnityEditor.SceneManagement.EditorSceneManager is its counterpart for operations within the Unity Editor during edit mode. This static class extends SceneManager, inheriting its runtime capabilities (though typically not used directly for runtime logic from editor scripts) and adding a wealth of editor-specific functionalities. Its use is strictly for editor scripts, custom tools, and automation within the development environment, not for runtime game logic. EditorSceneManager plays a crucial role in controlling which scenes are displayed and managed in the Hierarchy window and how scenes behave during editor-initiated Play Mode sessions.
The comprehensive API of EditorSceneManager empowers developers to go far beyond simple opening and saving of scenes. It provides the foundation for building sophisticated custom workflows, development tools, and automation scripts that can significantly enhance productivity and enforce consistency across projects. This includes capabilities for creating scenes from scratch or templates, managing their save state, handling preview scenes for custom inspectors, reordering scenes in the hierarchy, and even loading scenes specifically for play-mode testing without altering build settings. The extensive event system further allows custom scripts to react intelligently to nearly any scene-related action performed by the user or other editor scripts, enabling deep integration of custom logic into the editor's native scene handling.
3.1. Introduction to EditorSceneManager
EditorSceneManager provides the necessary tools to programmatically interact with scenes as assets and as loaded entities within the editor. This includes opening scenes for editing, creating new scenes, saving changes, and managing special scene types like preview scenes.
3.2. Opening and Closing Scenes in the Editor
Manipulating which scenes are open in the editor's Hierarchy window is a common requirement for editor tools.
OpenScene(string scenePath, OpenSceneMode mode = OpenSceneMode.Single): This method opens a scene specified by its asset path (e.g., "Assets/MyScenes/MyScene.unity"). The OpenSceneMode enum offers several options :
Single: Closes all currently open scenes and then opens the specified scene.
Additive: Opens the specified scene additively, keeping other scenes in the Hierarchy loaded.
AdditiveWithoutLoading: Adds the scene to the Hierarchy in an unloaded state. The scene's contents are not immediately loaded into memory.
CloseScene(Scene scene, bool removeScene): Closes the specified loaded scene. If removeScene is true, the scene is also removed from the Hierarchy window setup. If it's the last scene being closed, a new, empty, untitled scene is usually created to ensure the editor always has at least one scene open.
RestoreSceneManagerSetup(SceneSetup[] setups): This powerful method allows for opening multiple scenes simultaneously based on an array of SceneSetup objects. Each SceneSetup object defines the path to a scene asset, whether it should be loaded (isLoaded), and whether it should be the active scene (isActive). This is extremely useful for quickly restoring a specific working environment with multiple interdependent scenes.
C# Example: Opening a Scene and Restoring a Setup (from an Editor Window)
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Collections.Generic;

public class SceneControlWindow : EditorWindow
{

    public static void ShowWindow()
    {
        GetWindow<SceneControlWindow>("Scene Control");
    }

    private string scenePathToOpen = "Assets/Scenes/MySpecificScene.unity"; // Example path

    void OnGUI()
    {
        GUILayout.Label("Scene Operations", EditorStyles.boldLabel);
        scenePathToOpen = EditorGUILayout.TextField("Scene Path to Open", scenePathToOpen);

        if (GUILayout.Button("Open Scene (Single)"))
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(scenePathToOpen, OpenSceneMode.Single);
            }
        }

        if (GUILayout.Button("Open Scene (Additive)"))
        {
            // No need to save for additive, unless it's part of a workflow that implies it
            EditorSceneManager.OpenScene(scenePathToOpen, OpenSceneMode.Additive);
        }

        if (GUILayout.Button("Restore Multi-Scene Setup"))
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                SceneSetup[] setup = new SceneSetup[] {
                    new SceneSetup { path = "Assets/Scenes/CoreManagers.unity", isActive = true, isLoaded = true },
                    new SceneSetup { path = "Assets/Scenes/Level_01_Environment.unity", isActive = false, isLoaded = true },
                    new SceneSetup { path = "Assets/Scenes/PlayerCharacter.unity", isActive = false, isLoaded = true }
                };
                EditorSceneManager.RestoreSceneManagerSetup(setup);
            }
        }
    }
}
#endif


This example illustrates how OpenScene and RestoreSceneManagerSetup can be used within a custom editor window. The consistent use of SaveCurrentModifiedScenesIfUserWantsTo before operations that might discard changes (like opening a scene in Single mode or restoring a setup) is a critical safety measure. This method prompts the user if there are unsaved modifications, bridging programmatic control with user consent and preventing accidental data loss—a key principle in designing robust editor tools.
3.3. Creating New Scenes in the Editor
New scenes can be created programmatically using EditorSceneManager.NewScene.
NewScene(NewSceneSetup setup, NewSceneMode mode): Creates a new scene.
NewSceneSetup: An enum specifying the initial content of the new scene (e.g., EmptyScene for a completely blank scene, or DefaultGameObjects for a scene with a default camera and light).
NewSceneMode: An enum determining how the new scene is integrated into the Hierarchy (e.g., Single to replace current scenes, Additive to add alongside them). When a new scene is created, it initially appears in the Hierarchy as "Untitled" and has not yet been saved to disk. Only one "Untitled" scene can exist at a time.
C# Example: Creating a New Default Scene
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement; // For Scene struct

public class EditorSceneCreator
{

    public static void CreateDefaultSceneAdditive()
    {
        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Additive);
        EditorUtility.DisplayDialog("Scene Created", $"New scene '{newScene.name}' created additively.", "OK");
        // The scene is untitled and in memory. It needs to be saved to persist.
        // Example: EditorSceneManager.SaveScene(newScene, "Assets/New Scenes/MyNewDefaultScene.unity");
    }
}
#endif


This demonstrates creating a new scene with default game objects and adding it to the current hierarchy.
3.4. Saving Scenes Programmatically
EditorSceneManager provides several methods for saving scenes:
SaveScene(Scene scene, string dstPath = ""): Saves the specified Scene object. If dstPath is provided and is different from the scene's current path, it performs a "Save As" operation. If dstPath is empty or same as current, it saves to its existing file.
SaveOpenScenes(): Saves all scenes currently open in the Hierarchy window.
SaveCurrentModifiedScenesIfUserWantsTo(): Checks if any open scenes in the Hierarchy are "dirty" (modified). If so, it displays a dialog asking the user if they want to save these changes. Returns true if scenes were saved or no save was needed, false if the user cancelled. This is essential for preventing data loss.
SaveModifiedScenesIfUserWantsTo(Scene[] scenes): Similar to the above, but prompts for a specific array of input scenes.
EnsureUntitledSceneHasBeenSaved(): If an "Untitled" scene exists, this method shows a save dialog to the user.
3.5. Managing Scene "Dirty" State
The "dirty" state of a scene indicates whether it has been modified since its last save. The editor uses this state to prompt users to save changes (e.g., by showing an asterisk next to the scene name in the Hierarchy ).
MarkSceneDirty(Scene scene): Programmatically marks the specified scene as modified. This is crucial if your editor tool makes changes to a scene's content, ensuring Unity's save mechanisms are aware of these changes.
MarkAllScenesDirty(): Marks all currently loaded scenes as modified.
To check if a scene is dirty, one would use the Scene.isDirty property (discussed in Section 4). While EditorSceneManager itself does not have a direct IsDirty(Scene scene) static method, the Scene.isDirty property serves this purpose for individual scene instances. Additionally, the EditorSceneManager.sceneDirtied event is fired whenever a scene's dirty state changes from unmodified to modified.
3.6. Play Mode Scene Management in Editor
For testing and editor tool development, it's sometimes necessary to load scenes during Play Mode that are not part of the regular Build Settings. EditorSceneManager provides methods for this:
LoadSceneInPlayMode(string sceneName, LoadSceneParameters parameters): Loads a scene by name synchronously during Play Mode in the editor, even if it's not in Build Settings.
LoadSceneAsyncInPlayMode(SceneAsset sceneAsset, LoadSceneParameters parameters): Loads a scene from a SceneAsset asynchronously during Play Mode in the editor, also without requiring it to be in Build Settings. LoadSceneParameters allows specifying LoadSceneMode and other options.
These methods are valuable for specialized testing scenarios or for editor extensions that need to interact with or set up specific scene configurations when Play Mode is entered.
3.7. Preview Scenes
Preview scenes are temporary, isolated scenes created in the editor, primarily for rendering previews of assets or game objects without affecting the main scenes open in the Hierarchy. Objects in a preview scene are only rendered within that scene's context.
NewPreviewScene(): Creates a new, empty preview scene.
OpenPreviewScene(string scenePath): Opens an existing scene asset as a preview scene.
ClosePreviewScene(Scene previewScene): Closes a given preview scene and cleans up its resources.
IsPreviewScene(Scene scene): Checks if a given Scene object is a preview scene.
IsPreviewSceneObject(Object obj): Checks if a given Object (e.g., a GameObject) is part of a preview scene.
Preview scenes are instrumental for developing rich editor extensions, such as custom inspectors that display a 3D model in an isolated environment, tools for previewing procedural content generation, or custom material editors that show real-time changes on a sample mesh. They provide a dedicated rendering and interaction context without cluttering the user's main workspace or requiring the editor to enter Play Mode.
3.8. Other Utility Methods
EditorSceneManager includes various other utility methods:
GetSceneManagerSetup(): Returns an array of SceneSetup objects representing the current configuration of scenes in the Hierarchy (which scenes are loaded, active, etc.).
MoveSceneAfter(Scene src, Scene dst) / MoveSceneBefore(Scene src, Scene dst): Allows reordering of scenes as they appear in the Hierarchy window.
DetectCrossSceneReferences(Scene scene): Scans a scene for GameObjects or components that reference assets or objects in other scenes.
Scene Culling Mask Methods: CalculateAvailableSceneCullingMask(), GetSceneCullingMask(Scene scene), SetSceneCullingMask(Scene scene, int cullingMask) are used to manage scene-specific culling masks, affecting which cameras render which scenes.
3.9. EditorSceneManager Events
EditorSceneManager exposes a comprehensive set of events that allow editor scripts to react to various scene-related activities within the editor:
activeSceneChangedInEditMode(Scene previousActiveScene, Scene newActiveScene): Fired when the active scene changes while in Edit Mode.
newSceneCreated(Scene scene, NewSceneSetup setup, NewSceneMode mode): Called after a new scene has been created.
sceneOpened(Scene scene, OpenSceneMode mode): Called after a scene has been opened in the editor.
sceneClosed(Scene scene): Called after a scene has been closed in the editor.
sceneClosing(Scene scene, bool removingScene): Called just before a scene is closed.
sceneDirtied(Scene scene): Called after a scene has been modified (becomes "dirty").
sceneSaved(Scene scene): Called after a scene has been saved.
sceneSaving(Scene scene, string path): Called just before a scene is saved to disk.
sceneManagerSetupRestored(SceneSetup[] scenes): Called after RestoreSceneManagerSetup has completed, providing the array of scenes that were just opened.
Subscribing to these events enables the creation of highly responsive and context-aware editor tools that can automate tasks, validate scene states, or update custom UI based on the editor's scene management activities.
3.10. EditorSceneManager API Quick Reference
The following table summarizes key members of the UnityEditor.SceneManagement.EditorSceneManager class, highlighting their common editor use cases.
Member Type
Name
Brief Description
Common Editor Use Cases
Method
OpenScene
Opens a scene in the editor.
Custom tools for level selection, opening specific utility scenes.
Method
CloseScene
Closes a specified open scene.
Cleaning up temporary scenes, managing multi-scene setups.
Method
NewScene
Creates a new scene in the editor.
Tools for quick scene creation with predefined setups, project initialization scripts.
Method
SaveScene
Saves a specific scene, optionally to a new path.
Automating save processes, custom "Save As" functionality.
Method
SaveOpenScenes
Saves all currently open scenes.
Batch saving operations, pre-build steps.
Method
SaveCurrentModifiedScenesIfUserWantsTo
Prompts user to save any modified open scenes.
Preventing data loss before critical operations (e.g., closing scenes, version control).
Method
MarkSceneDirty
Marks a scene as modified.
Ensuring editor recognizes programmatic changes for save prompts.
Method
RestoreSceneManagerSetup
Opens a specific set of scenes based on SceneSetup array.
Quickly switching between predefined multi-scene working environments.
Method
LoadSceneInPlayMode
Loads a scene during Play Mode (editor only), not requiring it in Build Settings.
Testing specific scenes or configurations without modifying build list.
Method
LoadSceneAsyncInPlayMode
Asynchronously loads a scene during Play Mode (editor only).
Similar to LoadSceneInPlayMode but for smoother transitions or larger test scenes.
Method
NewPreviewScene
Creates a new, temporary preview scene.
Custom asset inspectors, model viewers, material previewers.
Method
OpenPreviewScene
Opens an existing scene asset as a preview scene.
Displaying complex scene assets in isolated preview windows.
Method
ClosePreviewScene
Closes a preview scene.
Cleaning up resources used by preview windows.
Event
activeSceneChangedInEditMode
Fired when the active scene changes in Edit Mode.
Updating tool UI based on active scene, context-sensitive operations.
Event
newSceneCreated
Fired after a new scene is created.
Automatically populating new scenes with required objects, applying default settings.
Event
sceneOpened
Fired after a scene is opened.
Performing validation checks on opened scenes, initializing tool data.
Event
sceneDirtied
Fired when a scene becomes modified.
Triggering auto-save mechanisms, logging changes, UI feedback for unsaved changes.
Event
sceneSaved
Fired after a scene is saved.
Post-save processing (e.g., asset postprocessing, updating metadata).

This table acts as a practical guide for developers building editor tools, helping them quickly identify the relevant EditorSceneManager API for their specific needs.
4. The UnityEngine.SceneManagement.Scene Struct: Anatomy of a Loaded Scene
When a scene is loaded into memory, either at runtime via SceneManager or in the editor via EditorSceneManager, it is represented by the UnityEngine.SceneManagement.Scene struct. This struct acts as a handle or reference to the scene's runtime data and properties. It is returned by various methods like SceneManager.GetActiveScene(), SceneManager.GetSceneByName(), or as a parameter in SceneManager events like sceneLoaded.
4.1. Introduction to the Scene Struct
The Scene struct provides access to information about a specific loaded scene instance. It does not contain the scene's GameObjects directly but offers properties to query its state and methods to access its root-level content.
4.2. Key Properties of Scene
The Scene struct exposes several important properties that describe the loaded scene:
name: A string representing the name of the scene file (e.g., "MyLevel01").
path: A string containing the relative asset path of the scene file within the project (e.g., "Assets/MyScenes/MyLevel01.unity").
buildIndex: An integer representing the scene's index in the Build Settings. If the scene is not included in the Build Settings (e.g., a scene loaded additively in the editor that's not part of the build, or a scene from an AssetBundle not in the main build list), this property will return -1. The buildIndex being -1 is a key indicator that a scene, though loaded, is not part of the formal build process defined in Build Settings. This has direct implications for how such a scene can be reloaded by index or whether it will be included in a final player build if not managed through other mechanisms like AssetBundles. Applications relying on scenes outside the Build Settings must use alternative identification methods (like name or path) and appropriate management strategies.
isLoaded: A boolean value that is true if the scene has finished its loading process and all its GameObjects have been initialized and enabled. This property is particularly crucial when dealing with asynchronously loaded scenes. While an AsyncOperation.isDone flag from SceneManager.LoadSceneAsync indicates that the loading operation itself has completed, Scene.isLoaded (once the Scene struct is obtained, perhaps via the sceneLoaded event or GetSceneBy... methods) provides a more definitive confirmation that the scene's content is fully processed and accessible. Relying on Scene.isLoaded is safer for immediate interaction with a scene's GameObjects, helping to avoid race conditions.
isDirty: A boolean value that is true if the scene has been modified since it was last saved. This property is primarily relevant within the Unity Editor. It is directly linked to editor functionalities like EditorSceneManager.MarkSceneDirty(). When an editor tool programmatically modifies a scene and calls MarkSceneDirty(), the corresponding Scene object's isDirty flag will become true. This signals to the editor to display visual cues (like an asterisk next to the scene name in the hierarchy ) and to include this scene in prompts to save changes. Custom editor tools that alter scene content must call MarkSceneDirty on the affected scene to ensure proper integration with Unity's standard save workflow and to provide accurate feedback to the user.
rootCount: An integer indicating the number of root GameObjects in the scene's hierarchy. Root GameObjects are those that do not have a parent within that scene.
IsValid(): This is a public method that acts like a property, returning true if the Scene struct handle is valid and refers to an actual loaded scene. A Scene struct might be invalid if, for example, it was returned from an attempt to get a scene by a name or path that does not exist or if a scene loading operation failed.
4.3. Accessing Scene Contents
The primary way to access the GameObjects within a Scene is through its root objects:
GetRootGameObjects(): This method returns an array of GameObject references, where each element is a root GameObject in the scene. Once these root GameObjects are obtained, standard GameObject traversal methods (e.g., transform.GetChild(), GetComponentInChildren()) can be used to find specific objects or components within the scene's hierarchy.
C# Example: Iterating Through Root GameObjects
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneContentExplorer : MonoBehaviour
{
    public void ExploreActiveSceneContents()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene.IsValid() && activeScene.isLoaded)
        {
            Debug.Log($"Exploring contents of scene: {activeScene.name}");
            GameObject[] rootObjects = activeScene.GetRootGameObjects();
            Debug.Log($"Found {rootObjects.Length} root GameObjects.");

            foreach (GameObject rootObject in rootObjects)
            {
                Debug.Log($" - Root Object: {rootObject.name}");
                // Further exploration can be done here, e.g., iterating children
                // or searching for specific components.
                PrintHierarchy(rootObject.transform, "  ");
            }
        }
        else
        {
            Debug.LogWarning("Active scene is not valid or not loaded.");
        }
    }

    void PrintHierarchy(Transform t, string prefix)
    {
        Debug.Log($"{prefix}{t.name}");
        foreach (Transform child in t)
        {
            PrintHierarchy(child, prefix + "  ");
        }
    }
}


4.4. Operators
The Scene struct overloads equality operators:
operator == (Scene lhs, Scene rhs): Returns true if the two Scene structs refer to the same loaded scene instance.
operator!= (Scene lhs, Scene rhs): Returns true if the two Scene structs refer to different loaded scene instances.
These operators are useful for comparing scene handles, for example, to check if a newly loaded scene is the one expected.
4.5. Scene Struct Member Reference
The following table provides a reference for the members of the UnityEngine.SceneManagement.Scene struct.
Member Type
Name
Return Type/Parameters
Description
Property
name
string
The name of the scene file.
Property
path
string
The relative asset path of the scene file.
Property
buildIndex
int
The scene's index in the Build Settings (-1 if not in Build Settings).
Property
isLoaded
bool
True if the scene has finished loading and its objects are enabled.
Property
isDirty
bool
True if the scene has been modified since its last save (primarily relevant in the Editor).
Property
rootCount
int
The number of root GameObjects in the scene.
Method
IsValid()
bool
Returns true if the Scene struct handle is valid.
Method
GetRootGameObjects()
GameObject[]
Returns an array of all root GameObjects in the scene.
Operator
==
Scene lhs, Scene rhs
Returns true if the Scene structs are equal (refer to the same loaded scene).
Operator
!=
Scene lhs, Scene rhs
Returns true if the Scene structs are not equal.

This table summarizes the means by which developers can query information about a loaded scene and access its top-level content.
5. Advanced Scene Scripting Techniques
Beyond basic loading, unloading, and querying, Unity's scene scripting APIs enable more advanced techniques, such as creating scenes from templates and managing complex game states across multiple simultaneously loaded scenes.
5.1. Programmatic Scene Creation from Templates
Scene Templates in Unity provide a way to create new scenes based on a predefined structure, GameObjects, and component configurations, much like prefabs do for GameObjects. This feature is particularly powerful when combined with C# scripting for automating scene creation workflows. The SceneTemplate class and related APIs allow for the instantiation of new scenes from SceneTemplateAsset files programmatically. This approach represents a significant step towards more robust and customizable "prefabs for scenes," enabling more consistent and efficient level design, especially in projects with numerous similar scenes (e.g., puzzle levels, challenge rooms, standardized environment modules).
The core method for this is SceneTemplate.Instantiate: Tuple<Scene, SceneAsset> SceneTemplate.Instantiate(SceneTemplateAsset sceneTemplateAsset, bool loadAdditively, string newSceneOutputPath = null). This method takes a SceneTemplateAsset (the asset file representing the template), a boolean indicating whether to load the new scene additively, and an optional output path. It returns a tuple containing the Scene handle of the newly instantiated scene and its corresponding SceneAsset.
A crucial aspect of scene templates is the management of "cloneable dependencies." If the template scene or its assets are configured to be cloned upon instantiation (e.g., a material that should be unique to each new scene, or a ScriptableObject holding instance-specific data), an newSceneOutputPath must be provided. Unity will then create a folder at this path (or use the path if it's for the scene file itself) and place the cloned assets within it, updating references in the new scene to point to these clones. This careful handling of dependencies is vital for template reusability and maintaining project organization, ensuring that instances derived from a template can be modified independently without affecting the template itself or other instances.
The SceneTemplateAsset class itself exposes properties like templateScene (the actual scene asset used as the template), dependencies (a list of assets used by the template and whether they should be cloned or referenced), and templatePipeline (a script for custom instantiation logic).
Unity also provides events related to template instantiation, such as SceneTemplate.newSceneTemplateInstantiated, which is triggered after a template has been instantiated and its assets processed.
For even deeper customization, developers can create a script that implements the ISceneTemplatePipeline interface (or derives from SceneTemplatePipelineAdapter). This script can be assigned to a SceneTemplateAsset's templatePipeline property. The pipeline script can then implement methods like BeforeTemplateInstantiation and AfterTemplateInstantiation to execute custom C# code at specific points in the instantiation sequence. This allows for procedural adjustments to the scene, selection of asset variants, or any other custom logic to be injected into the template instantiation process, significantly enhancing the power and flexibility of scene templates.
SceneTemplateAssets themselves can also be created programmatically:
SceneTemplate.CreateSceneTemplate(string sceneTemplatePath): Creates a new, empty scene template asset at the specified path.
SceneTemplate.CreateTemplateFromScene(SceneAsset sourceSceneAsset, string sceneTemplatePath): Creates a new scene template asset from an existing scene asset, automatically associating the scene and extracting its dependencies.
C# Example: Instantiating a Scene from a Template
#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneTemplate; // Required for SceneTemplate and SceneTemplateAsset
using System; // Required for Tuple

public class SceneFromTemplateCreator
{

    public static void CreateSceneFromTemplate()
    {
        // Assume "Assets/SceneTemplates/MyLevelTemplate.scenetemplate" exists
        string templatePath = "Assets/SceneTemplates/MyLevelTemplate.scenetemplate";
        SceneTemplateAsset sceneTemplate = AssetDatabase.LoadAssetAtPath<SceneTemplateAsset>(templatePath);

        if (sceneTemplate == null)
        {
            Debug.LogError($"Scene Template not found at path: {templatePath}");
            return;
        }

        string newSceneDirectory = "Assets/GeneratedScenes";
        if (!AssetDatabase.IsValidFolder(newSceneDirectory))
        {
            AssetDatabase.CreateFolder("Assets", "GeneratedScenes");
        }

        // If the template has cloneable dependencies, an output path for the scene is needed.
        // The cloned assets will be placed in a folder with the same name as the scene,
        // in the same directory as the scene.
        string newScenePath = $"{newSceneDirectory}/NewSceneFromTemplate_{System.Guid.NewGuid().ToString().Substring(0, 8)}.unity";

        try
        {
            // Instantiate the template. Load additively for this example.
            // Provide newScenePath because templates often have cloneable dependencies.
            (Scene newScene, SceneAsset newSceneAsset) result = SceneTemplate.Instantiate(sceneTemplate, true, newScenePath);

            if (result.newScene.IsValid())
            {
                Debug.Log($"Successfully instantiated scene '{result.newScene.name}' from template '{sceneTemplate.name}' at path '{result.newSceneAsset.name}'.");
                EditorSceneManager.SetActiveScene(result.newScene); // Optionally make it active
            }
            else
            {
                Debug.LogError("Failed to instantiate scene from template.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error instantiating scene from template: {e.Message}");
        }
    }
}
#endif


This example demonstrates loading a SceneTemplateAsset and using SceneTemplate.Instantiate to create and load a new scene based on it, including specifying an output path.
5.2. Strategies for Managing Game State Across Multiple Loaded Scenes
When multiple scenes are loaded additively (a common pattern for modular game design, as discussed with LoadSceneMode.Additive ), managing shared game state and ensuring communication between systems in different scenes becomes a critical architectural challenge. Since GameObjects in one scene cannot directly reference GameObjects in another scene via the Inspector before both are loaded, and since global state (like player score, inventory, or overall game progression) or manager systems need to be accessible regardless of which specific gameplay or UI scenes are currently loaded or unloaded, robust patterns are required.
Common strategies include:
Singleton GameManagers in a Persistent Scene: A widely used pattern involves creating a "manager" GameObject (e.g., GameManager, AudioManager, InputManager) that implements the Singleton pattern. This GameObject is typically placed in an initial scene that is loaded first. The script attached to this manager calls DontDestroyOnLoad(this.gameObject); in its Awake method. This ensures that the manager GameObject persists across all subsequent scene loads (when using LoadSceneMode.Single for main level transitions) and resides in a special DontDestroyOnLoad scene that appears in the Hierarchy during Play Mode when multiple scenes are involved. Other scripts can then access this Singleton instance to get or set shared data or call global functions.
ScriptableObjects for Shared Data and Events: ScriptableObjects are asset files that can store shared data independently of scene hierarchies. They can be used to hold game settings, player profiles, or even act as channels for a simple event system. Systems in different scenes can all reference the same ScriptableObject assets to read shared data or to raise/listen to events, providing a decoupled way to manage state and communication.
Dependency Injection Frameworks: For more complex projects, dedicated dependency injection frameworks can manage the creation and provision of shared services and data objects to various parts of the application across different scenes.
Dedicated "Manager" Scene: A common architectural approach is to have a base "manager" or "bootstrap" scene that is always loaded first. This scene contains all persistent managers (often Singletons using DontDestroyOnLoad). This manager scene then additively loads and unloads other scenes (e.g., main menu, game levels, UI panels) as needed. This clearly separates persistent systems from transient scene content.
The choice of strategy depends on the complexity of the game and the specific needs for state management and inter-scene communication. The key is to establish a clear and consistent architecture early in development to avoid chaotic data flow and dependencies as the project grows.
6. Best Practices and Key Considerations
Effective programmatic scene management requires adherence to best practices and an understanding of key technical considerations to ensure robust, efficient, and error-free applications.
6.1. Choosing Between SceneManager and EditorSceneManager (Revisited)
The fundamental distinction remains:
UnityEngine.SceneManagement.SceneManager: For runtime game logic. Use its APIs in scripts that will be part of the final game build to control scene flow during gameplay (e.g., loading levels, managing additive UI scenes).
UnityEditor.SceneManagement.EditorSceneManager: For editor tooling and workflows. Use its APIs exclusively within editor scripts (typically in "Editor" folders or wrapped in #if UNITY_EDITOR blocks) to automate scene manipulation, create custom editor windows, or enhance the development environment.
Attempting to use EditorSceneManager APIs in build code will result in compilation errors because the UnityEditor assembly is not included in builds. This separation is not merely a guideline but a hard technical constraint imposed by Unity's architecture to keep builds lean and prevent runtime failures. The rigorous segregation of these two APIs is arguably the single most critical best practice derived from analyzing Unity's scene management systems.
6.2. Proper Handling of Editor-Only Code
To prevent build errors and keep game builds optimized, code that uses UnityEditor namespaces (including UnityEditor.SceneManagement) must be isolated:
"Editor" Folders: The primary and recommended method is to place all editor scripts inside a folder named "Editor". Such folders can exist at the root of the Assets directory or within any of its subdirectories. Unity's compilation pipeline automatically excludes scripts within "Editor" folders from game builds; they are only compiled and available within the Unity Editor environment.
Preprocessor Directives (#if UNITY_EDITOR... #endif): For situations where a small piece of editor-specific logic needs to reside within a script that is otherwise part of the runtime build (e.g., custom gizmo drawing for a MonoBehaviour), preprocessor directives can be used. Code wrapped in an #if UNITY_EDITOR block will only be compiled when the script is processed by the editor and will be stripped out during the build process for a player.
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor; // OK to use UnityEditor here
#endif

public class MyComponent : MonoBehaviour
{
    public int runtimeValue;

#if UNITY_EDITOR
    // This function is only available in the editor
    [ContextMenu("Editor Only Action")]
    void DoSomethingInEditor()
    {
        if (Selection.activeGameObject == this.gameObject)
        {
            Debug.Log("Performing editor action on selected MyComponent.");
        }
    }

    // Example: Custom gizmo drawing
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 1.0f);
    }
#endif

    void Update()
    {
        // Runtime logic
    }
}


Failure to correctly isolate editor-only code is a common source of errors when attempting to build a Unity project.
6.3. Performance Implications of Scene Loading
Scene loading can be a performance-intensive operation. Several factors influence load times and runtime performance:
Synchronous (LoadScene) vs. Asynchronous (LoadSceneAsync): As repeatedly emphasized, LoadSceneAsync is strongly preferred for loading any non-trivial scene to avoid freezing the game and maintain a smooth user experience. The consistent advice towards asynchronous loading points to a broader design emphasis in game development on responsiveness and minimizing player frustration from unexplained pauses. Unity provides the tools for smooth transitions, but their adoption is a conscious development choice.
Scene Complexity: The number and complexity of GameObjects, the size and resolution of textures, the complexity of materials and shaders, and the amount of script initialization logic all contribute to scene load times.
Additive Loading: While beneficial for modularity, additively loading many complex scenes can increase the overall number of GameObjects to manage, potentially leading to higher draw calls, physics overhead, and script updates if not carefully profiled and optimized.
Optimization Tips:
Optimize assets (textures, models, audio) to reduce their memory footprint and loading impact.
Use Unity's Addressable Asset System for more fine-grained control over asset loading and unloading, which can work in conjunction with scene management.
Implement level streaming patterns, where parts of a large world are loaded/unloaded additively based on player proximity or game events.
Profile scene loading and runtime performance using the Unity Profiler to identify bottlenecks.
6.4. Error Handling and Debugging
Robust scene management includes proper error handling and debugging techniques:
Always check the Scene.IsValid() method on a Scene struct returned by a loading or querying operation before attempting to use it, especially if there's a chance the operation might fail (e.g., scene path typo).
When using LoadSceneAsync or UnloadSceneAsync, subscribe to the AsyncOperation.completed event to execute post-load/unload logic and to check if the operation was successful (though isDone is the primary indicator of completion, the event is useful for chaining actions).
Utilize comprehensive logging, such as the scene state logging example provided earlier , to debug issues in multi-scene setups or dynamic loading scenarios.
Be aware that EditorSceneManager.OpenScene will throw an ArgumentException if it fails (e.g., due to an incorrect scene path), so use try-catch blocks in editor scripts if necessary.
6.5. SceneManager vs. EditorSceneManager - A Comparative Summary
The following table provides a direct side-by-side comparison to reinforce the distinctions between the runtime and editor-time scene management APIs.
Feature/Context
UnityEngine.SceneManagement.SceneManager
UnityEditor.SceneManagement.EditorSceneManager
Primary Use
Runtime Game Logic
Editor Tooling & Workflows
Availability
In Game Builds & Editor Play Mode
Editor Edit Mode Only (Not in Builds)
Typical Operations
Loading levels, additive game content, runtime scene creation/unloading.
Opening scenes for editing, saving, creating via tools, preview scenes, build setup.
Key Classes/Structs
SceneManager, Scene, LoadSceneMode, AsyncOperation
EditorSceneManager, SceneSetup, NewSceneSetup, OpenSceneMode, SceneAsset
Code Isolation
Standard C# scripts (MonoBehaviours, etc.)
Scripts in "Editor" folders or code within #if UNITY_EDITOR preprocessor blocks.

This comparative summary serves as a definitive quick guide for developers to decide which API is appropriate for their current task, thereby helping to prevent common errors related to misusing editor APIs at runtime or vice-versa.
7. Conclusion and Future Outlook
Unity's programmatic scene management APIs, encompassing both UnityEngine.SceneManagement.SceneManager for runtime operations and UnityEditor.SceneManagement.EditorSceneManager for editor-time control, offer a powerful and flexible toolkit for developers. Mastering these APIs allows for the creation of dynamic and complex game experiences, efficient multi-scene workflows, and sophisticated custom editor tools that can significantly streamline development.
The paramount takeaway is the critical importance of understanding and respecting the distinction between runtime and editor-time contexts. Adherence to best practices, such as isolating editor-specific code using "Editor" folders or preprocessor directives, is not merely a suggestion but a technical necessity for producing functional and optimized builds. Similarly, the preference for asynchronous scene loading (LoadSceneAsync) to ensure a smooth user experience reflects a broader commitment to quality in modern game development.
The Scene struct provides essential information about loaded scenes, while events from both SceneManager and EditorSceneManager enable reactive and decoupled system design. Advanced features like Scene Templates further push the boundaries of efficiency and consistency in scene creation, allowing for "prefabs for scenes" that can be customized through scripting pipelines.
Looking ahead, Unity continues to evolve its scene management capabilities. The ongoing development of the Data-Oriented Technology Stack (DOTS) introduces concepts like subscenes, which offer a different paradigm for managing and streaming parts of a game world. The Addressable Asset System also plays an increasingly integral role, providing more granular control over asset and scene loading, often working in tandem with the traditional SceneManager APIs. As projects grow in scale and complexity, a deep understanding of these foundational scene scripting principles will remain indispensable for Unity developers aiming to build high-quality, performant, and maintainable applications.
Works cited
1. Unity: What Is a Scene - Wayline, https://www.wayline.io/blog/unity-what-is-a-scene 2. c# - When trying to build the game I'm getting error/s in the editor ..., https://stackoverflow.com/questions/60357957/when-trying-to-build-the-game-im-getting-error-s-in-the-editor-console-but-not 3. Scripting API: SceneManager - Unity, https://docs.unity3d.com/6000.1/Documentation/ScriptReference/SceneManagement.SceneManager.html 4. Scripting API: EditorSceneManager - Unity, https://docs.unity3d.com/ScriptReference/SceneManagement.EditorSceneManager.html 5. Scripting API: SceneManagement.EditorSceneManager.OpenScene - Unity, https://docs.unity.cn/ScriptReference//SceneManagement.EditorSceneManager.OpenScene.html 6. Scripting API: EditorSceneManager - Unity - Manual, https://docs.unity3d.com/6000.1/Documentation/ScriptReference/SceneManagement.EditorSceneManager.html 7. Scripting API: SceneManagement.SceneManager.LoadScene - Unity, https://docs.unity3d.com/6000.1/Documentation/ScriptReference/SceneManagement.SceneManager.LoadScene.html 8. Scripting API: SceneManagement.LoadSceneMode.Additive - Unity, https://docs.unity3d.com/6000.1/Documentation/ScriptReference/SceneManagement.LoadSceneMode.Additive.html 9. Set up multiple scenes - Unity - Manual, https://docs.unity3d.com/6000.1/Documentation/Manual/setupmultiplescenes.html 10. Manual: Work with multiple scenes in Unity, https://docs.unity3d.com/6000.1/Documentation/Manual/MultiSceneEditing.html 11. Scripting API: SceneManagement.SceneManager ... - Unity, https://docs.unity3d.com/6000.1/Documentation/ScriptReference/SceneManagement.SceneManager-activeSceneChanged.html 12. Scene - Scripting API - Unity - Manual, https://docs.unity3d.com/6000.0/Documentation/ScriptReference/SceneManagement.Scene.html 13. Creating new Scenes from templates - Unity - Manual, https://docs.unity3d.com/Packages/com.unity.scene-template@1.0/manual/creating-scenes-from-templates.html 14. Creating scene templates - Unity - Manual, https://docs.unity3d.com/Manual/scene-templates-creating.html 15. Scripting API: SceneTemplateAsset - Unity - Manual, https://docs.unity3d.com/6000.1/Documentation/ScriptReference/SceneTemplate.SceneTemplateAsset.html 16. Class SceneTemplateAsset | Scene Template | 1.0.0-preview.11, https://docs.unity.cn/Packages/com.unity.scene-template@1.0/api/UnityEditor.SceneTemplate.SceneTemplateAsset.html 17. Customizing new scene creation - Unity - Manual, https://docs.unity3d.com/2020.2/Documentation/Manual/scene-templates-customizing-scene-instantiation.html