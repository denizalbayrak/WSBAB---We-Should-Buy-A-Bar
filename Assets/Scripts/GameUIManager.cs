using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;

    public GameObject WarningPanel; // Reference to the warning panel
    public TextMeshProUGUI WarningPanelText; // Reference to the warning panel

    public List<Recipe> activeRecipes = new List<Recipe>();
    public GameObject recipeUIPrefab;
    public Transform recipeUIParent;
    public int maxActiveRecipes = 4;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
}
