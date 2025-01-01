using UnityEngine;
using System.Collections.Generic;

public class Highlightable : MonoBehaviour
{
    public List<Renderer> objectRenderers = new List<Renderer>(); 
    public List<List<Color>> originalColors = new List<List<Color>>(); 

    private void Awake()
    {
        objectRenderers.AddRange(GetComponentsInChildren<Renderer>());

        foreach (var renderer in objectRenderers)
        {
            List<Color> colors = new List<Color>();
            foreach (var mat in renderer.materials)
            {
                colors.Add(mat.color); 
            }
            originalColors.Add(colors);
        }

        if (objectRenderers.Count == 0)
        {
            Debug.LogError("No Renderer components found in " + gameObject.name);
        }
    }

    public void SetHighlight(Color highlightColor)
    {
        for (int rendererIndex = 0; rendererIndex < objectRenderers.Count; rendererIndex++)
        {
            var renderer = objectRenderers[rendererIndex];
            Material[] mats = renderer.materials;

            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = new Material(mats[i]); 

                Color originalColor = originalColors[rendererIndex][i];
                Color newColor = originalColor * highlightColor;
                mats[i].color = newColor;
            }

            renderer.materials = mats;
        }
    }


    public void ResetHighlight()
    {
        for (int rendererIndex = 0; rendererIndex < objectRenderers.Count; rendererIndex++)
        {
            var renderer = objectRenderers[rendererIndex];
            Material[] mats = renderer.materials;

            for (int i = 0; i < mats.Length; i++)
            {
                mats[i].color = originalColors[rendererIndex][i];
            }

            renderer.materials = mats;
        }
    }
}
