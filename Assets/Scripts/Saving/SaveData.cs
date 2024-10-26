using UnityEngine;
using Wsbab.Enums; // CharacterType enumýný kullanmak için

[System.Serializable]
public class SaveData
{
    public int level;
    public float playTime;
    public string playerName;

    // Karakter seçimi
    public CharacterType selectedCharacter = CharacterType.Female; // Varsayýlan olarak Female
}
[System.Serializable]
public class PlacedObjectData
{
    public string objectID;
    public Vector3 position;
    public Quaternion rotation;
    public string placementAreaID;
}