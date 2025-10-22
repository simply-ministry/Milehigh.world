using UnityEngine;

/// <summary>
/// Manages the storage and retrieval of narrative intel, such as comms logs.
/// </summary>
public class IntelManager : MonoBehaviour
{
    // --- Singleton Pattern ---
    public static IntelManager Instance { get; private set; }

    // --- Comms Log Data ---
    [TextArea(15, 20)]
    public string commsLog;

    void Awake()
    {
        // Enforce singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadInitialCommsLog();
        }
    }

    /// <summary>
    /// Loads the initial comms log into the manager.
    /// In a real game, this might be loaded from a file.
    /// </summary>
    private void LoadInitialCommsLog()
    {
        commsLog = @"
== INTEL LOG: OPERATION AETHELGARD ==
DATE: 2025-10-22
TRANSMITTER: OTIS THE SKYWANDERER
ENCRYPTION: LEVEL 7 (DECODED)

Cirrus,

My search for Aeron the Brave has led me to the floating continent of ÆṬĤŸŁĞÅŘÐ. The place is a fortress, teeming with ancient magic and formidable guardians. I've uncovered a complex social structure and a series of trials one must pass to gain access to the inner sanctums where Aeron is rumored to be held.

**Key Factions:**
1.  **The Valkyries:** A proud warrior clan who guard the skies. To earn their trust, one must prove their worth in combat trials. Reputation must be at least 75 (Honored) to proceed.
2.  **The Centaurs:** Keepers of the old ways and masters of the land. They demand respect and a deep understanding of their traditions. Reputation must be at least 75 (Honored) to gain their assistance.

**The Path Forward:**
*   Gaining the trust of both the Valkyries and the Centaurs is paramount. They hold the two keys to the 'Labyrinth of ÆṬĤŸŁĞÅŘÐ'.
*   The Labyrinth is a magical maze, a final trial. I've heard tales of a fearsome Minotaur Guardian at its center. Defeating it is the only way through.
*   My contact, a mysterious cartographer named Lyra, has hinted that she can guide me once I've passed the trials. She awaits at the Labyrinth's exit.

This is a multi-stage operation. I'll need to work with the Novamingad Alliance to build our reputation here. The fate of Aeron, and perhaps the entire Shattered Zenith conflict, depends on it.

Otis out.
";
    }
}
