using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SaveSelectionPanel : MonoBehaviour
{
    public Transform saveSlotsParent; 
    public GameObject saveSlotPrefab; 

    private List<SaveSlot> saveSlots = new List<SaveSlot>();

    private void OnEnable()
    {
        RefreshSaveSlots();
    }

    public void RefreshSaveSlots()
    {
        foreach (Transform child in saveSlotsParent)
        {
            Destroy(child.gameObject);
        }
        saveSlots.Clear();

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
