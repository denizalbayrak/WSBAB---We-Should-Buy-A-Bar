using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int level;
    public float playTime;
    public string playerName;

    // Character selection
    public CharacterType selectedCharacter = CharacterType.Female; // Default to Female
}
