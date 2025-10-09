using UnityEngine;

public class UnderwaterEffects : MonoBehaviour
{
    public GameObject underwaterPostProcess;
    public AudioSource underwaterAudio;
    public Color underwaterAmbientColor = new Color(0.0f, 0.2f, 0.35f);

    private Color defaultAmbientColor;

    void Start()
    {
        defaultAmbientColor = RenderSettings.ambientLight;
        if (underwaterPostProcess != null)
            underwaterPostProcess.SetActive(false);
        if (underwaterAudio != null)
            underwaterAudio.Stop();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
            EnableUnderwaterEffects();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
            DisableUnderwaterEffects();
    }

    void EnableUnderwaterEffects()
    {
        if (underwaterPostProcess != null)
            underwaterPostProcess.SetActive(true);
        if (underwaterAudio != null)
            underwaterAudio.Play();
        RenderSettings.ambientLight = underwaterAmbientColor;
    }

    void DisableUnderwaterEffects()
    {
        if (underwaterPostProcess != null)
            underwaterPostProcess.SetActive(false);
        if (underwaterAudio != null)
            underwaterAudio.Stop();
        RenderSettings.ambientLight = defaultAmbientColor;
    }
}