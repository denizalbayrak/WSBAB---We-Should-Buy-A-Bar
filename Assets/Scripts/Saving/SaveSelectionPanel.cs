using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SaveSelectionPanel : MonoBehaviour
{
    public Transform saveSlotsParent; // Save slotlar�n�n yerle�tirilece�i parent
    public GameObject saveSlotPrefab; // Save slot prefab'�

    private List<SaveSlot> saveSlots = new List<SaveSlot>();

    private void OnEnable()
    {
        RefreshSaveSlots();
    }

    /// <summary>
    /// T�m save slotlar�n� g�nceller.
    /// </summary>
    public void RefreshSaveSlots()
    {
        // Mevcut slotlar� temizle
        foreach (Transform child in saveSlotsParent)
        {
            Destroy(child.gameObject);
        }
        saveSlots.Clear();

        // 3 slot olu�tur
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
