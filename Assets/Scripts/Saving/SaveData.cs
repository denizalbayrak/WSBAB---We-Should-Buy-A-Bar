using UnityEngine;
using Wsbab.Enums; // CharacterType enum�n� kullanmak i�in

[System.Serializable]
public class SaveData
{
    public int level;
    public float playTime;
    public string playerName;

    // Karakter se�imi
    public CharacterType selectedCharacter = CharacterType.Female; // Varsay�lan olarak Female
}
