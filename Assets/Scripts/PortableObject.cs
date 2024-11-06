using UnityEngine;
using System.Collections.Generic;

public enum HighlightState
{
    None,       // Orijinal renk
    CanCarry,   // Taþýma ihtimali durumu
    Carried     // Taþýma durumu
}

public class PortableObject : MonoBehaviour
{
    public string itemName;             // Obje adý, örneðin: "Çöp Kutusu"
    public bool isMandatory;            // Görev için zorunlu mu?
    public float dropYOffset = 0f;

    public List<Renderer> objectRenderers = new List<Renderer>(); // Tüm Renderer'lar
    private List<List<Color>> originalColors = new List<List<Color>>(); // Her Renderer için orijinal renkler listesi

    // Renk çarpanlarý
    private Color canCarryColorMultiplier = new Color(0.6f, 0.6f, 0.6f);

    private void Awake()
    {
        // Tüm child objelerdeki Renderer bileþenlerini al
        objectRenderers.AddRange(GetComponentsInChildren<Renderer>());

        // Tüm materyallerin orijinal renklerini sakla
        foreach (var renderer in objectRenderers)
        {
            List<Color> colors = new List<Color>();
            foreach (var mat in renderer.materials)
            {
                colors.Add(mat.color); // Orijinal rengi kaydet
            }
            originalColors.Add(colors);
        }

        if (objectRenderers.Count == 0)
        {
            Debug.LogError("No Renderer components found in " + gameObject.name);
        }
    }

    /// <summary>
    /// Objeye vurgu yapar veya vurguyu kaldýrýr.
    /// </summary>
    /// <param name="state">Vurgu durumu (None, CanCarry, Carried).</param>
    public void SetHighlight(HighlightState state)
    {
        for (int rendererIndex = 0; rendererIndex < objectRenderers.Count; rendererIndex++)
        {
            var renderer = objectRenderers[rendererIndex];
            Material[] mats = renderer.materials;

            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = new Material(mats[i]); // Her materyalin bir kopyasýný oluþtur

                // Orijinal rengi kullanarak highlight durumuna göre çarpan uygula
                switch (state)
                {
                    case HighlightState.None:
                        mats[i].color = originalColors[rendererIndex][i];
                        break;
                    case HighlightState.CanCarry:
                        mats[i].color = originalColors[rendererIndex][i] * canCarryColorMultiplier;
                        break;
                    case HighlightState.Carried:
                        mats[i].color = originalColors[rendererIndex][i] * canCarryColorMultiplier;
                        break;
                }
            }

            // Deðiþiklikleri Renderer'a geri ata
            renderer.materials = mats;
        }
    }
}
