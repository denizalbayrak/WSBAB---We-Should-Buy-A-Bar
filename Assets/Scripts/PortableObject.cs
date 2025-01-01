using UnityEngine;
using System.Collections.Generic;

public enum HighlightState
{
    None,       
    CanCarry,   
    Carried     
}

public class PortableObject : MonoBehaviour
{
    public string itemName;             
    public bool isMandatory;            
    public float dropYOffset = 0f;

    public List<Renderer> objectRenderers = new List<Renderer>(); 
    private List<List<Color>> originalColors = new List<List<Color>>(); 


    private Color canCarryColorMultiplier = new Color(0.6f, 0.6f, 0.6f);

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

    /// <summary>
    /// Objeye vurgu yapar veya vurguyu kald�r�r.
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
                mats[i] = new Material(mats[i]); 

               
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

            
            renderer.materials = mats;
        }
    }
}
