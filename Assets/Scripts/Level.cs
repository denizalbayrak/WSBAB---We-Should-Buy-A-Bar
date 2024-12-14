using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Game/Level")]
public class Level : ScriptableObject
{
    public string levelName;
    public float levelDuration; // Level time (seconds)
    public List<Order> availableOrders; 
    public GameObject levelMapPrefab;

    public int scoreFor1Star; // one star score
    public int scoreFor2Stars; // two star score
    public int scoreFor3Stars; // three star score
}
