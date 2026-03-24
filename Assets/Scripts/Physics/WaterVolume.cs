using UnityEngine;

/// <summary>
/// Manages a trigger volume that represents a body of water.
/// When a GameObject with the "Player" tag enters or exits this volume,
/// it notifies the PlayerUnderwaterEffects script on the player to
/// enable or disable the appropriate underwater effects.
/// </summary>
[RequireComponent(typeof(Collider))]
public class WaterVolume : MonoBehaviour
{
    /// <summary>
    /// Initializes the component, ensuring its collider is set to be a trigger.
    /// </summary>
    private void Start()
    {
        // Ensure the collider is set to be a trigger.
        Collider col = GetComponent<Collider>();
        if (!col.isTrigger)
        {
            Debug.LogWarning("WaterVolume's collider was not set to 'isTrigger'. Forcing it to true.", this);
            col.isTrigger = true;
        }
    }

    /// <summary>
    /// Called when another collider enters this object's trigger.
    /// </summary>
    /// <param name="other">The other collider involved in this event.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerUnderwaterEffects effects = other.GetComponent<PlayerUnderwaterEffects>();
            if (effects != null)
            {
                effects.EnterWater();
            }
            else
            {
                Debug.LogWarning("Player entered water volume, but no PlayerUnderwaterEffects script was found on the player object.", other);
            }
        }
    }

    /// <summary>
    * Called when another collider exits this object's trigger.
    /// </summary>
    /// <param name="other">The other collider involved in this event.</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerUnderwaterEffects effects = other.GetComponent<PlayerUnderwaterEffects>();
            if (effects != null)
            {
                effects.ExitWater();
            }
        }
    }
}
