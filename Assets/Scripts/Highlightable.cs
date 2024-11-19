using UnityEngine;
using System.Collections.Generic;

public class Highlightable : MonoBehaviour
{
    public List<Renderer> objectRenderers = new List<Renderer>(); // T�m Renderer'lar
    public List<List<Color>> originalColors = new List<List<Color>>(); // Her Renderer i�in orijinal renkler listesi

    private void Awake()
    {
        // T�m child objelerdeki Renderer bile�enlerini al
        objectRenderers.AddRange(GetComponentsInChildren<Renderer>());

        // T�m materyallerin orijinal renklerini sakla
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
    /// Objeye vurgu yapar veya vurguyu kald�r�r.
    /// </summary>
    /// <param name="highlightColor">Vurgu rengi. Orijinal renge �arpan olarak uygulan�r. Vurguyu kald�rmak i�in Color.white kullan�labilir.</param>
    public void SetHighlight(Color highlightColor)
    {

        for (int rendererIndex = 0; rendererIndex < objectRenderers.Count; rendererIndex++)
        {
            var renderer = objectRenderers[rendererIndex];
            Material[] mats = renderer.materials;

            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = new Material(mats[i]); // Her materyalin bir kopyas�n� olu�tur

                // Orijinal rengi kullanarak highlight rengini uygula
                Color originalColor = originalColors[rendererIndex][i];
                Color newColor = originalColor * highlightColor;
                mats[i].color = newColor;

            }

            // De�i�iklikleri Renderer'a geri ata
            renderer.materials = mats;
        }
    }

}
