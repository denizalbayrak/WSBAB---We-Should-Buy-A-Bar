using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    public int slotNumber;


    public GameObject emptyPanel;
    public GameObject occupiedPanel;


    public Button addButton;
    public Button loadButton;
    public Button deleteButton;

    public TextMeshProUGUI slotNumberText;

    private void Start()
    {
        InitializeSlot();
    }


    public void InitializeSlot()
    {
        SaveData data = SaveSystem.LoadGame(slotNumber);
        if (data != null)
        {
           
            emptyPanel.SetActive(false);
            occupiedPanel.SetActive(true);

         
            slotNumberText.text = "Slot " + slotNumber;

          
            loadButton.onClick.RemoveAllListeners();
            loadButton.onClick.AddListener(() => LoadGame());

            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(() => DeleteGame());
        }
        else
        {
            
            emptyPanel.SetActive(true);
            occupiedPanel.SetActive(false);

            
            addButton.onClick.RemoveAllListeners();
            addButton.onClick.AddListener(() => CreateNewSave());
        }
    }

    private void CreateNewSave()
    {
        GameManager.Instance.NewGame(slotNumber);

        InitializeSlot();
    }

    private void LoadGame()
    {
        GameManager.Instance.LoadGame(slotNumber);
    }

    private void DeleteGame()
    {
        SaveSystem.DeleteGame(slotNumber);
        InitializeSlot();
    }
}
