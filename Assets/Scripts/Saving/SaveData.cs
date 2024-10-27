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
