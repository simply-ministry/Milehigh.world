using UnityEngine;
using System.Collections;

/// <summary>
/// Manages the discovery scene in the ruins of Lîŋq,
/// featuring Micah, Omega.one, and Cirrus. This script controls the dialogue
/// and the reveal of the Onalym Nexus.
/// </summary>
public class LinqDiscoveryScene : MonoBehaviour
{
    public Character micah;
    public Character omegaOne;
    public Character cirrus;
    public GameObject onalymNexus; // Assign the Nexus prefab in the Inspector

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
        // Dialogue sequence based on the narrative
        Debug.Log("Cirrus: This city... It was a beacon. What happened here, Micah?");
        yield return new WaitForSeconds(2.5f);

        Debug.Log("Micah: Lîŋq fell to the Void. They reached too far, and the darkness consumed them.");
        yield return new WaitForSeconds(2.5f);

        Debug.Log("Omega.one: Analysis: The energy signatures are unstable. Traces of Void and celestial power remain.");
        yield return new WaitForSeconds(2.5f);

        Debug.Log("Cirrus: Then it's true. The Nexus... it wasn't just a gateway. It was a weapon.");
        yield return new WaitForSeconds(2.5f);

        // The discovery
        if (onalymNexus != null)
        {
            onalymNexus.SetActive(true);
            Debug.Log("*The ground trembles, revealing a hidden chamber. Within, the Onalym Nexus hums with dangerous power.*");
        }
    }
}