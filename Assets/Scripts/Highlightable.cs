using UnityEngine;
using System.Collections.Generic;

public class Highlightable : MonoBehaviour
{
    public List<Renderer> objectRenderers = new List<Renderer>(); // Tüm Renderer'lar
    public List<List<Color>> originalColors = new List<List<Color>>(); // Her Renderer için orijinal renkler listesi

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
    /// <param name="highlightColor">Vurgu rengi. Orijinal renge çarpan olarak uygulanýr. Vurguyu kaldýrmak için Color.white kullanýlabilir.</param>
    public void SetHighlight(Color highlightColor)
    {

        for (int rendererIndex = 0; rendererIndex < objectRenderers.Count; rendererIndex++)
        {
            var renderer = objectRenderers[rendererIndex];
            Material[] mats = renderer.materials;

            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = new Material(mats[i]); // Her materyalin bir kopyasýný oluþtur

                // Orijinal rengi kullanarak highlight rengini uygula
                Color originalColor = originalColors[rendererIndex][i];
                Color newColor = originalColor * highlightColor;
                mats[i].color = newColor;

            }

            // Deðiþiklikleri Renderer'a geri ata
            renderer.materials = mats;
        }
    }

}
