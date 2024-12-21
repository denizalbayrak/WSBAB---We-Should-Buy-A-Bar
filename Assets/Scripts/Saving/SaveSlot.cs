using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    public int slotNumber; // Slot numaras� (1, 2, 3)

    // Paneller
    public GameObject emptyPanel;
    public GameObject occupiedPanel;

    // Buttons
    public Button addButton;
    public Button loadButton;
    public Button deleteButton;

    // UI Elements
    public TextMeshProUGUI slotNumberText;

    private void Start()
    {
        InitializeSlot();
    }

    /// <summary>
    /// Slotu ba�lat�r ve UI ��elerini g�nceller.
    /// </summary>
    public void InitializeSlot()
    {
        SaveData data = SaveSystem.LoadGame(slotNumber);
        if (data != null)
        {
            // Save var, OccupiedPanel'i a�
            emptyPanel.SetActive(false);
            occupiedPanel.SetActive(true);

            // Detaylar� g�ster
            slotNumberText.text = "Slot " + slotNumber;

            // Buton i�levlerini ekle
            loadButton.onClick.RemoveAllListeners();
            loadButton.onClick.AddListener(() => LoadGame());

            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(() => DeleteGame());
        }
        else
        {
            // Save yok, EmptyPanel'i a�
            emptyPanel.SetActive(true);
            occupiedPanel.SetActive(false);

            // Buton i�levlerini ekle
            addButton.onClick.RemoveAllListeners();
            addButton.onClick.AddListener(() => CreateNewSave());
        }
    }

    /// <summary>
    /// Yeni bir save olu�turur ve oyunu ba�lat�r.
    /// </summary>
    private void CreateNewSave()
    {
        // Yeni oyun ba�lat ve kaydet
        GameManager.Instance.NewGame(slotNumber);

        // Slotu g�ncelle
        InitializeSlot();
    }

    /// <summary>
    /// Oyunu y�kler.
    /// </summary>
    private void LoadGame()
    {
        GameManager.Instance.LoadGame(slotNumber);
    }

    /// <summary>
    /// Save dosyas�n� siler ve slotu g�nceller.
    /// </summary>
    private void DeleteGame()
    {
        SaveSystem.DeleteGame(slotNumber);
        InitializeSlot();
    }
}
