using UnityEngine;

[RequireComponent(typeof(Character))]
public class PlayerController : MonoBehaviour
{
    private Character character;
    private AbilitySystem abilitySystem;
    private Character currentTarget; // The enemy you are currently targeting

    void Awake()
    {
        character = GetComponent<Character>();
        abilitySystem = GetComponent<AbilitySystem>();
    }

    void Update()
    {
        // Don't process input if the game is paused or in a cutscene
        // if (GameManager.Instance.GetCurrentState() != GameManager.GameState.Playing) return;

        // --- Real-Time Movement ---
        // Reads input every frame for smooth movement
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D keys
        float moveVertical = Input.GetAxis("Vertical");   // W/S keys
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        // This would be connected to a character motor or Unity's CharacterController
        // character.Move(movement * Time.deltaTime * speed);

        // --- Real-Time Targeting ---
        // Example: Use raycasting to select a target with the mouse
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    currentTarget = hit.collider.GetComponent<Character>();
                    Debug.Log($"Target set to: {currentTarget.characterName}");
                }
            }
        }

        // --- Real-Time Ability Activation ---
        // Trigger abilities with key presses (e.g., '1', '2', '3')
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Assumes the first ability is a basic attack
            if (currentTarget != null && abilitySystem != null)
            {
                abilitySystem.UseAbility(0, currentTarget); // Use the first ability in the list
            }
        }
    }
}