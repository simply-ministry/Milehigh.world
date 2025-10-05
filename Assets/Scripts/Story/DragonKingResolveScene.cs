using System;
using System.Collections.Generic;
using UnityEngine;

public class DragonKingResolveScene : MonoBehaviour
{
    // Characters
    public GameObject cirrus;
    public GameObject ingris;

    // Dialogue System
    public delegate void DialogueAction(string text);
    public static event DialogueAction OnDialogue;

    void Start()
    {
        BeginScene();
    }

    void BeginScene()
    {
        StartCoroutine(SceneSequence());
    }

    System.Collections.IEnumerator SceneSequence()
    {
        ShowDialogue("Within the vast, crystalline halls of his private study in ƁÅČ̣ĤÎŘØN̈, Cirrus, the Dragon King, stood before a swirling holographic projection of the Verse.");
        yield return new WaitForSeconds(4f);

        ShowDialogue("His form was that of a man, regal and imposing, with eyes that held the wisdom of ages and the deep, unsettling glint of recent revelations.");
        yield return new WaitForSeconds(4f);

        ShowDialogue("His dark, ornate robes, woven with threads that seemed to capture the very starlight of his realm, accentuated his commanding presence.");
        yield return new WaitForSeconds(4f);

        ShowDialogue("Gone was the gentle hum of meditation; his aura now thrummed with a focused, almost fierce energy.");
        yield return new WaitForSeconds(3f);

        ShowDialogue("He traced a shimmering line across the holographic map, following the fragmented path of Lîŋq, a stark contrast to the once-whole expanse of reality.");
        yield return new WaitForSeconds(4f);

        ShowDialogue("The ideological clashes with King Cyrus, the invasion that had fractured the Verse through the Onalym Nexus—these were fresh wounds, not just on the fabric of existence, but on his very soul.");
        yield return new WaitForSeconds(5f);

        ShowDialogue("He had always known the threat was grave, but the chilling immediacy of Sky.ix's plea in his vision had crystallized the true scope of the impending doom.");
        yield return new WaitForSeconds(5f);

        ShowDialogue("The image of her desperate, bionic gaze, her plea for the Ɲōvəmîŋāđ to save humanity, burned in his mind.");
        yield return new WaitForSeconds(4f);

        ShowDialogue("A soft click echoed in the immense space as the door to his study slid open. Ingris, the Phoenix Warrior, entered.");
        yield return new WaitForSeconds(4f);

        ShowDialogue("Her armor, usually vibrant with the hues of flame and dawn, seemed muted in the subdued light of the chamber, though the strength in her bearing was undeniable.");
        yield return new WaitForSeconds(5f);

        ShowDialogue("Her movements were fluid, powerful, yet she carried a subtle concern in her eyes as she approached him.");
        yield return new WaitForSeconds(4f);

        ShowDialogue("Ingris: Cirrus, Your meditations were... restless. Is the Void's corruption pressing closer to ƁÅČ̣ĤÎŘØN̈?");
        yield return new WaitForSeconds(5f);

        ShowDialogue("Cirrus: More than that, my love. I have communed with a spark of the future... with Sky.ix, the Bionic Goddess.");
        yield return new WaitForSeconds(5f);

        ShowDialogue("He extended a hand, and a smaller, more focused holographic image of Sky.ix, defiant amidst the crumbling tech of Lîŋq, appeared between them.");
        yield return new WaitForSeconds(5f);

        ShowDialogue("Cirrus: She reached into my slumber, showing me the depths to which the Void has twisted Mîlēhîgh.wørld. It manifests differently in each layer – from the robotic husks of ŁĪƝĈ, to the corrupted mystics of Old Lîŋq, and the ancient horrors of Āɲč̣ịəŋṭ^Łīɲč̣.");
        yield return new WaitForSeconds(10f);

        ShowDialogue("Ingris: A premonition, then? Or a cry for help across time?");
        yield return new WaitForSeconds(4f);

        ShowDialogue("Cirrus: Both. She spoke of the Ɲōvəmîŋāđ. The ten preordained individuals. Their unification is the only hope to avert the ultimate unraveling of the Verse, to save humanity from Era's complete dominion.");
        yield return new WaitForSeconds(10f);

        ShowDialogue("Cirrus: You, my dearest Ingris, are one of them. Your flame is needed more than ever.");
        yield return new WaitForSeconds(5f);

        ShowDialogue("Ingris met his gaze, her hand tightening on her sword. The path ahead was perilous, fraught with the danger of her own potential transformation into Delilah the Desolate, a fate she fought with every fiber of her being. But the call was clear.");
        yield return new WaitForSeconds(8f);

        ShowDialogue("Ingris: Then we must act. Who first, my King? Where do we begin to gather these scattered lights?");
        yield return new WaitForSeconds(5f);

        ShowDialogue("Cirrus turned back to the holographic map, his finger hovering over a region far from ƁÅČ̣ĤÎŘØN̈, a land of soaring mountains and fierce winds.");
        yield return new WaitForSeconds(5f);

        ShowDialogue("Cirrus: We begin where courage takes wing, my Phoenix. We seek out Aeron the Brave, the winged lion of ÆṬĤŸŁĞÅŘÐ. His spirit, unyielding and true, will be a beacon.");
        yield return new WaitForSeconds(8f);

        ShowDialogue("The first step on their desperate quest to unite the Ɲōvəmîŋāđ had been chosen, a new purpose forged from a prophetic dream and the deepening shadow of the Void.");
        yield return new WaitForSeconds(6f);

        EndScene();
    }

    void ShowDialogue(string text)
    {
        if (OnDialogue != null)
        {
            OnDialogue(text);
        }
        else
        {
            Debug.Log(text); // Basic fallback
        }
    }

    void EndScene()
    {
        Debug.Log("Dragon King's Resolve Scene Ended.");
    }
}