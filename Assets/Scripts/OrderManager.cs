using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance { get; private set; }

    [Header("Level Settings")]
    public Level currentLevel;

    [Header("UI Settings")]
    public GameObject orderUIPrefab; // Sipari� UI prefab�
    public Transform orderUIContainer; // Sipari�lerin g�sterilece�i UI container'�

    [Header("Scoring Settings")]
    // public int scorePerSuccess = 10; // Global skor ayar�n� kald�r�n
    // public int scorePerFailure = -5;
    public TextMeshProUGUI scoreText;

    [Header("Order Spawn Settings")]
    public int maxActiveOrders = 4;
    public float minOrderDelay = 15f; // Minimum sipari�ler aras� gecikme s�resi
    public float maxOrderDelay = 25f; // Maksimum sipari�ler aras� gecikme s�resi

    private List<ActiveOrder> activeOrders = new List<ActiveOrder>();
    public int currentScore = 0;

    private float levelTimer;
    private bool isLevelActive = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public void LoadLevel(Level level)
    {
        if (level == null)
        {
            Debug.LogError("No level assigned in OrderManager.");
            return;
        }
        StopLevel();

        currentLevel = level;
        levelTimer = currentLevel.levelDuration;
        isLevelActive = true;
        currentScore = 0;
        UpdateScoreUI();

        // Sipari� olu�turma coroutine'ini ba�lat
        StartCoroutine(SpawnOrders());
    }

    private void Update()
    {
        if (!isLevelActive)
            return;

        // Aktif sipari�lerin zaman�n� kontrol et
        List<ActiveOrder> ordersToFail = new List<ActiveOrder>();

        foreach (var activeOrder in activeOrders)
        {
            activeOrder.timeLeft -= Time.deltaTime;
            activeOrder.orderUI.UpdateTimer(activeOrder.timeLeft / activeOrder.order.timeLimit);

            if (activeOrder.timeLeft <= 0f)
            {
                ordersToFail.Add(activeOrder);
            }
        }

        foreach (var order in ordersToFail)
        {
            // Sipari�i ba�ar�s�z olarak i�le
            activeOrders.Remove(order);
            order.orderUI.RemoveUI();
            currentScore += order.order.scorePerFailure; // Her sipari�in kendi ba�ar�s�z puan�n� ekleyin
            Debug.Log($"Order Failed! Score: {currentScore}");
            UpdateScoreUI();
        }
    }

    public void StopLevel()
    {
        isLevelActive = false; // Yeni sipari� olu�turmay� durdur
        StopAllCoroutines(); // Coroutine'leri durdur
        ClearActiveOrders(); // Aktif sipari�leri temizle
    }

    private void ClearActiveOrders()
    {
        // Aktif sipari� listesini temizle
        foreach (var activeOrder in activeOrders)
        {
            if (activeOrder.orderUI != null)
            {
                activeOrder.orderUI.RemoveUI(); // Sipari� UI'lar�n� kald�r
            }
        }
        activeOrders.Clear(); // Listeyi tamamen temizle
    }

    private IEnumerator SpawnOrders()
    {
        while (isLevelActive)
        {
            if (activeOrders.Count < maxActiveOrders)
            {
                CreateRandomOrder();
            }

            // Sipari�ler aras�ndaki gecikme
            float delay = Random.Range(minOrderDelay, maxOrderDelay);
            yield return new WaitForSeconds(delay);
        }
    }

    private void CreateRandomOrder()
    {
        if (currentLevel.availableOrders.Count == 0)
        {
            Debug.LogError("No available orders in current level.");
            return;
        }

        // Rastgele bir sipari� se�
        Order randomOrder = currentLevel.availableOrders[Random.Range(0, currentLevel.availableOrders.Count)];
        CreateOrderUI(randomOrder);
    }

    private void CreateOrderUI(Order orderTemplate)
    {
        // Yeni bir Order nesnesi olu�tur
        Order newOrder = new Order
        {
            orderID = orderTemplate.orderID,
            orderType = orderTemplate.orderType,
            description = orderTemplate.description,
            orderImage = orderTemplate.orderImage,
            timeLimit = orderTemplate.timeLimit,
            scorePerSuccess = orderTemplate.scorePerSuccess,
            scorePerFailure = orderTemplate.scorePerFailure
        };

        GameObject orderUIObj = Instantiate(orderUIPrefab, orderUIContainer);
        OrderUI orderUI = orderUIObj.GetComponent<OrderUI>();
        orderUI.Setup(newOrder, this);
        ActiveOrder activeOrder = new ActiveOrder(newOrder, orderUI);
        activeOrders.Add(activeOrder);
        Debug.Log($"Spawned Order: {newOrder.orderType} at {Time.time} seconds");
    }

    public void ProcessDeliveredItem(GameObject deliveredObject)
    {
        if (deliveredObject == null)
        {
            Debug.LogError("ProcessDeliveredItem called with null deliveredObject.");
            return;
        }
        // Aktif sipari�ler aras�nda, teslim edilen objeye uygun sipari�i bul
        ActiveOrder matchingActiveOrder = null;
        foreach (var activeOrder in activeOrders)
        {
            if (DoesDeliveredObjectMatchOrder(deliveredObject, activeOrder.order))
            {
                matchingActiveOrder = activeOrder;
                break;
            }
        }

        if (matchingActiveOrder != null)
        {
            // Sipari�i ba�ar�l� olarak i�le
            activeOrders.Remove(matchingActiveOrder);
            matchingActiveOrder.orderUI.RemoveUI();
            currentScore += matchingActiveOrder.order.scorePerSuccess; // Her sipari�in kendi ba�ar� puan�n� ekleyin
            Debug.Log($"Order Successful! Score: {currentScore}");
            UpdateScoreUI();
        }
        else
        {
            // E�le�en sipari� bulunamad�, ba�ar�s�z say
            // E�er teslim edilen objenin bir sipari�le e�le�medi�ini d���n�yorsan�z, bu durumu nas�l ele alaca��n�z� belirleyin
            // �rne�in, global bir ba�ar�s�z puan ekleyebilirsiniz veya hi�bir �ey yapmayabilirsiniz
            Debug.Log($"Order Failed! No matching order.");
            // Optionally, you can have a global failure score
            // currentScore += scorePerFailure;
            UpdateScoreUI();
        }
    }

    private bool DoesDeliveredObjectMatchOrder(GameObject deliveredObject, Order order)
    {
        // Teslim edilen objenin �zelliklerine g�re sipari�le e�le�ip e�le�medi�ini kontrol edin

        if (order.orderType == OrderType.DeliverBeerGlass)
        {
            BeerGlass beerGlass = deliveredObject.GetComponent<BeerGlass>();
            if (beerGlass != null && beerGlass.CurrentState == BeerGlass.GlassState.Filled)
            {
                // Teslim edilen obje dolu bir bira barda��, sipari�le e�le�iyor
                return true;
            }
        }
        else if (order.orderType == OrderType.DeliverWineGlass)
        {
            WineGlass wineGlass = deliveredObject.GetComponent<WineGlass>();
            if (wineGlass != null && wineGlass.CurrentState == WineGlass.GlassState.Filled)
            {
                // Teslim edilen obje dolu bir �arap barda��, sipari�le e�le�iyor
                return true;
            }
        }
        else if (order.orderType == OrderType.DeliverMojitoGlass)
        {
            MojitoGlass mojitoGlass = deliveredObject.GetComponent<MojitoGlass>();
            if (mojitoGlass != null && mojitoGlass.CurrentState == MojitoGlass.GlassState.Filled)
            {
                // Teslim edilen obje dolu bir mojito barda��, sipari�le e�le�iyor
                return true;
            }
        }
        else if (order.orderType == OrderType.DeliverMimosaGlass)
        {
            MimosaGlass mimosaGlass = deliveredObject.GetComponent<MimosaGlass>();
            if (mimosaGlass != null && mimosaGlass.CurrentState == MimosaGlass.GlassState.Filled)
            {
                // Teslim edilen obje dolu bir mimosa barda��, sipari�le e�le�iyor
                return true;
            }
        }
        else if (order.orderType == OrderType.DeliverWhiskeyGlass)
        {
            WhiskeyGlass whiskeyGlass = deliveredObject.GetComponent<WhiskeyGlass>();
            if (whiskeyGlass != null && whiskeyGlass.CurrentState == WhiskeyGlass.GlassState.Filled)
            {
                // Teslim edilen obje dolu bir mimosa barda��, sipari�le e�le�iyor
                return true;
            }
        }


        return false;
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }

    private class ActiveOrder
    {
        public Order order;
        public OrderUI orderUI;
        public float timeLeft;

        public ActiveOrder(Order order, OrderUI orderUI)
        {
            this.order = order;
            this.orderUI = orderUI;
            this.timeLeft = order.timeLimit;
        }
    }

    public void ResetOrderManager()
    {
        StopLevel();
        currentScore = 0;
        UpdateScoreUI();
    }
}

