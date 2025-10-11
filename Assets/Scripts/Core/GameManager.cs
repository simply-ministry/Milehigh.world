using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

/// <summary>
/// A persistent singleton that manages the overall game state,
/// including scene loading, pausing, and references to other key managers.
/// </summary>
public class GameManager : MonoBehaviour
{
    // --- Singleton Pattern ---
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        InCutscene
    }

    [Header("Game State")]
    [SerializeField] private GameState currentState;

    // --- System References (for other managers to register themselves) ---
    // public UIManager uiManager;
    // public QuestManager questManager;
    // public CombatManager combatManager;


    private void Awake()
    {
        // --- Singleton Implementation ---
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy this new instance if one already exists
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Make this object persist between scenes

        // --- Initial State ---
        // In a real game, you would start at a main menu.
        // For now, we'll assume we load directly into a playable scene.
        currentState = GameState.Playing;
    }

    /// <summary>
    /// Changes the current state of the game.
    /// </summary>
    /// <param name="newState">The state to switch to.</param>
    public void SetGameState(GameState newState)
    {
        currentState = newState;
        Debug.Log($"Game state changed to: {newState}");

        // Handle state-specific logic
        switch (currentState)
        {
            case GameState.Paused:
                Time.timeScale = 0f; // Pause all physics and time-based operations
                break;
            case GameState.Playing:
                Time.timeScale = 1f; // Resume normal time
                break;
            case GameState.InCutscene:
                Time.timeScale = 1f; // Ensure cutscenes play at normal speed
                // You might also disable player input here
                break;
        }
    }

    public GameState GetCurrentState()
    {
        return currentState;
    }

    /// <summary>
    /// Loads a new scene by its name.
    /// </summary>
    /// <param name="sceneName">The name of the scene to load.</param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}