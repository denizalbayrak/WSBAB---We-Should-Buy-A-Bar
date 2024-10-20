using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SaveSelectionPanel : MonoBehaviour
{
    public Transform saveSlotsParent; // Save slotlarýnýn yerleþtirileceði parent
    public GameObject saveSlotPrefab; // Save slot prefab'ý

    private List<SaveSlot> saveSlots = new List<SaveSlot>();

    private void OnEnable()
    {
        RefreshSaveSlots();
    }

    /// <summary>
    /// Tüm save slotlarýný günceller.
    /// </summary>
    public void RefreshSaveSlots()
    {
        // Mevcut slotlarý temizle
        foreach (Transform child in saveSlotsParent)
        {
            Destroy(child.gameObject);
        }
        saveSlots.Clear();

        // 3 slot oluþtur
        for (int i = 1; i <= 3; i++)
        {
            GameObject slotObj = Instantiate(saveSlotPrefab, saveSlotsParent);
            SaveSlot slotScript = slotObj.GetComponent<SaveSlot>();
            slotScript.slotNumber = i;
            slotScript.InitializeSlot();
            saveSlots.Add(slotScript);
        }
    }
}
