using UnityEngine;
using Wsbab.Enums;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int level;
    public float playTime;
    public string playerName;
    public CharacterType selectedCharacter = CharacterType.Female;


    public List<string> ownedRecipeNames = new List<string>();
    public List<string> ownedInventoryItemNames = new List<string>();
    public List<string> selectedRecipeNames = new List<string>(); 

}
