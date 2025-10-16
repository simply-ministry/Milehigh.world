using UnityEngine;
using System.Collections;

/// <summary>
/// Manages the discovery scene in the ruins of Lîŋq,
/// featuring Micah, Omega.one, and Cirrus.
/// </summary>
public class LinqDiscoveryScene : MonoBehaviour
{
    public Character micah;
    public Character omegaOne;
    public Character cirrus;
    public GameObject onalymNexus;

    public float dialoguePause = 2.5f;

    void Start()
    {
        if (onalymNexus != null)
        {
            onalymNexus.SetActive(false); // Ensure the Nexus is hidden initially
        }
        StartCoroutine(SceneSequence());
    }

    IEnumerator SceneSequence()
    {
        // Initial dialogue
        cirrus.Say("This city... It was a beacon. A center of knowledge... of power. What happened here, Micah?");
        yield return new WaitForSeconds(dialoguePause);

        micah.Say("Lîŋq fell to the Void, Cirrus. Its people sought to control its power, to unravel its secrets... They reached too far, and the darkness consumed them.");
        yield return new WaitForSeconds(dialoguePause);

        omegaOne.Say("Analysis: The energy signatures within these ruins are unstable. There are traces of both Void corruption and residual celestial power.");
        yield return new WaitForSeconds(dialoguePause);

        cirrus.Say("Then it's true. The Nexus... it wasn't just a gateway. It was a weapon.");
        yield return new WaitForSeconds(dialoguePause);

        // The discovery of the Nexus
        Debug.Log("*The ground trembles. A nearby tower collapses, revealing a hidden chamber.*");
        // In a real scene, this would be triggered by an animation or physics event.
        yield return new WaitForSeconds(1.5f);

        if (onalymNexus != null)
        {
            onalymNexus.SetActive(true);
            Debug.Log("*Within, a pulsating light emanates from the Onalym Nexus, still active, still humming with dangerous power.*");
        }
        else
        {
            Debug.LogWarning("Onalym Nexus prefab not assigned in the inspector.");
        }
    }
}