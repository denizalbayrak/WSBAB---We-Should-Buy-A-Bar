using UnityEngine;
using Wsbab.Enums; // CharacterType enum�n� kullanmak i�in
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int level;
    public float playTime;
    public string playerName;
    public CharacterType selectedCharacter = CharacterType.Female;


    // Recipe names that the player owns
    public List<string> ownedRecipeNames = new List<string>();
    public List<string> ownedInventoryItemNames = new List<string>();
    public List<string> selectedRecipeNames = new List<string>(); // Kullan�c�n�n se�ili tarifleri

}
