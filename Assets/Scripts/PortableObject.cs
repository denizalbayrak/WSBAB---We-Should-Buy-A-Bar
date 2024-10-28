using UnityEngine;
using System.Collections.Generic;

public enum HighlightState
{
    None,       // Orijinal renk
    CanCarry,   // Ta��ma ihtimali durumu
    Carried     // Ta��ma durumu
}

public class PortableObject : MonoBehaviour
{
    public float dropYOffset = 0f;

    private Renderer objectRenderer;
    private List<Color> originalColors = new List<Color>();

    // Renk �arpanlar�
    private Color canCarryColorMultiplier = new Color(0.6f, 0.6f, 0.6f); // Ta��ma ihtimali durumu
    private Color carriedColorMultiplier = new Color(0.3f, 0.3f, 0.3f);  // Ta��ma durumu

    private void Awake()
    {
        // Child objelerdeki Renderer bile�enini al
        objectRenderer = GetComponentInChildren<Renderer>();
        if (objectRenderer != null)
        {
            // T�m materyallerin orijinal renklerini sakla
            foreach (var mat in objectRenderer.materials)
            {
                originalColors.Add(mat.color);
            }
        }
        else
        {
            Debug.LogError("Renderer component is missing on " + gameObject.name);
        }
    }

    /// <summary>
    /// Objeye vurgu yapar veya vurguyu kald�r�r.
    /// </summary>
    /// <param name="state">Vurgu durumu (None, CanCarry, Carried).</param>
    public void SetHighlight(HighlightState state)
    {
        if (objectRenderer != null)
        {
            Material[] mats = objectRenderer.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = new Material(mats[i]); // Her materyalin bir kopyas�n� olu�tur

                switch (state)
                {
                    case HighlightState.None:
                        mats[i].color = originalColors[i];
                        break;
                    case HighlightState.CanCarry:
                        mats[i].color = originalColors[i] * canCarryColorMultiplier;
                        break;
                    case HighlightState.Carried:
                        mats[i].color = originalColors[i] * carriedColorMultiplier;
                        break;
                }
            }

            // De�i�iklikleri Renderer'a geri ata
            objectRenderer.materials = mats;
        }
    }
}
