using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    public int slotNumber; // Slot numarasý (1, 2, 3)

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
    /// Slotu baþlatýr ve UI öðelerini günceller.
    /// </summary>
    public void InitializeSlot()
    {
        SaveData data = SaveSystem.LoadGame(slotNumber);
        if (data != null)
        {
            // Save var, OccupiedPanel'i aç
            emptyPanel.SetActive(false);
            occupiedPanel.SetActive(true);

            // Detaylarý göster
            slotNumberText.text = "Slot " + slotNumber;

            // Buton iþlevlerini ekle
            loadButton.onClick.RemoveAllListeners();
            loadButton.onClick.AddListener(() => LoadGame());

            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(() => DeleteGame());
        }
        else
        {
            // Save yok, EmptyPanel'i aç
            emptyPanel.SetActive(true);
            occupiedPanel.SetActive(false);

            // Buton iþlevlerini ekle
            addButton.onClick.RemoveAllListeners();
            addButton.onClick.AddListener(() => CreateNewSave());
        }
    }

    /// <summary>
    /// Yeni bir save oluþturur ve oyunu baþlatýr.
    /// </summary>
    private void CreateNewSave()
    {
        // Yeni oyun baþlat ve kaydet
        GameManager.Instance.NewGame(slotNumber);

        // Slotu güncelle
        InitializeSlot();
    }

    /// <summary>
    /// Oyunu yükler.
    /// </summary>
    private void LoadGame()
    {
        GameManager.Instance.LoadGame(slotNumber);
    }

    /// <summary>
    /// Save dosyasýný siler ve slotu günceller.
    /// </summary>
    private void DeleteGame()
    {
        SaveSystem.DeleteGame(slotNumber);
        InitializeSlot();
    }
}
