using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class SaveSystem
{
    private static string GetSavePath(int slot)
    {
        return Path.Combine(Application.persistentDataPath, "save" + slot + ".json");
    }

    public static void SaveGame(SaveData data, int slot)
    {
        string path = GetSavePath(slot);
        string json = JsonUtility.ToJson(data, true);
        Debug.Log("SaveSystem - Saving data to path: " + path);
        Debug.Log("SaveSystem - JSON Data: " + json);
        File.WriteAllText(path, json);
    }


    public static SaveData LoadGame(int slot)
    {
        string path = GetSavePath(slot);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            return data;
        }
        else
        {
            return null;
        }
    }

    public static void DeleteGame(int slot)
    {
        string path = GetSavePath(slot);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static List<int> GetExistingSaveSlots()
    {
        List<int> slots = new List<int>();
        for (int i = 1; i <= 3; i++) 
        {
            if (File.Exists(GetSavePath(i)))
            {
                slots.Add(i);
            }
        }
        return slots;
    }

    public static bool HasAnySave()
    {
        for (int i = 1; i <= 3; i++)
        {
            if (File.Exists(GetSavePath(i)))
            {
                return true;
            }
        }
        return false;
    }


    public static int GetNextAvailableSlot()
    {
        for (int i = 1; i <= 3; i++)
        {
            if (!File.Exists(GetSavePath(i)))
            {
                return i;
            }
        }
        return -1; 
    }
}
