using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Game/Level")]
public class Level : ScriptableObject
{
    public string levelName;
    public float levelDuration; 
    public List<Order> availableOrders; 
    public GameObject levelMapPrefab;

    public int scoreFor1Star; 
    public int scoreFor2Stars; 
    public int scoreFor3Stars; 
}
