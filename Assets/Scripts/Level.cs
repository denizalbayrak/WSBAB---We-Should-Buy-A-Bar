using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Levels/Level")]
public class Level : ScriptableObject
{
    public string levelName;
    public List<Order> orders; // Seviyeye ait sipariþ listesi
    public GameObject levelMapPrefab; // Seviyeye ait harita prefabý
}
