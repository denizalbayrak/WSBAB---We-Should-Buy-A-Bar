using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Game/Level")]
public class Level : ScriptableObject
{
    public string levelName;
    public float levelDuration; // Level time (seconds)
    public List<Order> availableOrders; 
    public GameObject levelMapPrefab;

    public int targetScore; // one star score
    public int twoStarScore; // two star score
    public int threeStarScore; // three star score
}
