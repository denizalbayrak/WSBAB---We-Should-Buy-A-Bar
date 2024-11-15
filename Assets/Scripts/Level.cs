using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Game/Level")]
public class Level : ScriptableObject
{
    public string levelName;
    public float levelDuration; // Level s�resi (saniye)
    public List<Order> availableOrders; // Bu level'da mevcut olacak tarifler
    public GameObject levelMapPrefab;
}
