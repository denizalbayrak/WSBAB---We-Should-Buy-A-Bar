using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public enum OrderType
{
    DeliverBeerGlass,
    DeliverWineGlass,
    DeliverWaterGlass,
    // Diðer sipariþ türlerini ekleyin
}
public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance { get; private set; }

    [Header("Level Settings")]
    public Level currentLevel;

    [Header("UI Settings")]
    public OrderUI orderUIPrefab; // Sipariþ UI prefabý
    public Transform orderUIContainer; // Sipariþlerin gösterileceði UI container'ý

    [Header("Scoring Settings")]
    public int scorePerSuccess = 10;
    public int scorePerFailure = -5;
    public TextMeshProUGUI scoreText;

    [Header("Order Spawn Settings")]
    public int maxActiveOrders = 4;
    public float interOrderDelay = 2f; // Her sipariþ arasýndaki gecikme süresi

    private float spawnTimer = 0f; // Spawn zamanlayýcýsý
    private Queue<Order> orderQueue = new Queue<Order>();
    private List<ActiveOrder> activeOrders = new List<ActiveOrder>();
    private int currentScore = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        LoadLevel(currentLevel);
        // Baþlangýçta spawnTimer'ý interOrderDelay olarak ayarlayýn
        spawnTimer = interOrderDelay;
    }

    /// <summary>
    /// Yüklenen seviyedeki sipariþleri sýraya alýr.
    /// </summary>
    /// <param name="level">Yüklenecek seviye.</param>
    public void LoadLevel(Level level)
    {
        currentLevel = level;
        orderQueue.Clear();
        foreach (var order in currentLevel.orders)
        {
            orderQueue.Enqueue(order);
        }
    }

    /// <summary>
    /// Yeni bir sipariþ UI elemaný oluþturur.
    /// </summary>
    /// <param name="order">Oluþturulacak sipariþ.</param>
    private void CreateOrderUI(Order order)
    {
        GameObject orderUIObj = Instantiate(orderUIPrefab.gameObject, orderUIContainer);
        OrderUI orderUI = orderUIObj.GetComponent<OrderUI>();
        orderUI.Setup(order, this);
        ActiveOrder activeOrder = new ActiveOrder(order, orderUI);
        activeOrders.Add(activeOrder);
        Debug.Log($"Spawned Order: {order.orderType} at {Time.time} seconds");
    }

    /// <summary>
    /// Sipariþ tamamlandýðýnda veya süresi bittiðinde çaðrýlýr.
    /// </summary>
    /// <param name="order">Ýþlem yapýlan sipariþ.</param>
    /// <param name="isSuccess">Baþarýlý mý?</param>
    public void ProcessOrder(Order order, bool isSuccess)
    {
        if (order == null)
        {
            Debug.LogError("ProcessOrder called with null Order.");
            return;
        }

        ActiveOrder activeOrder = activeOrders.Find(o => o.order == order);
        if (activeOrder != null)
        {
            activeOrder.orderUI.RemoveUI();
            activeOrders.Remove(activeOrder);

            if (isSuccess)
            {
                currentScore += scorePerSuccess;
                Debug.Log("Order Successful! Score: " + currentScore);
            }
            else
            {
                currentScore += scorePerFailure;
                Debug.Log("Order Failed! Score: " + currentScore);
            }

            UpdateScoreUI();

            // Sipariþ tamamlandýktan sonra gecikme süresini resetle
            spawnTimer = interOrderDelay;
        }
        else
        {
            Debug.LogWarning("ActiveOrder not found for the given order.");
        }
    }

    /// <summary>
    /// Teslim edilen objeye karþýlýk gelen sipariþi bulur.
    /// </summary>
    /// <param name="deliveredObject">Teslim edilen obje.</param>
    /// <returns>Karþýlýk gelen sipariþ.</returns>
    public Order FindMatchingOrder(GameObject deliveredObject)
    {
        if (deliveredObject == null)
        {
            Debug.LogError("FindMatchingOrder called with null deliveredObject.");
            return null;
        }

        BeerGlass beerGlass = deliveredObject.GetComponent<BeerGlass>();
        if (beerGlass == null)
        {
            Debug.LogError("Delivered object does not have a BeerGlass component.");
            return null;
        }

        Debug.Log($"Delivered object has BeerGlass component with state: {beerGlass.CurrentState}");

        if (currentLevel == null)
        {
            Debug.LogError("currentLevel is not assigned in OrderManager.");
            return null;
        }

        if (currentLevel.orders == null)
        {
            Debug.LogError("currentLevel.orders is null.");
            return null;
        }

        switch (beerGlass.CurrentState)
        {
            case BeerGlass.GlassState.Filled:
                Order foundOrder = currentLevel.orders.Find(o => o.orderType == OrderType.DeliverBeerGlass);
                if (foundOrder == null)
                {
                    Debug.LogWarning("No matching order found for OrderType.DeliverBeerGlass.");
                }
                return foundOrder;

            // Diðer durumlar için ek case'ler ekleyin
            default:
                Debug.LogWarning($"No matching case for BeerGlass state: {beerGlass.CurrentState}");
                return null;
        }
    }

    /// <summary>
    /// Puaný UI üzerinde günceller.
    /// </summary>
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }

    /// <summary>
    /// Aktif sipariþlerin zamanýný kontrol eder ve yeni sipariþler spawn eder.
    /// </summary>
    private void Update()
    {
        // Spawn zamanlayýcýsýný güncelle
        spawnTimer -= Time.deltaTime;

        // Eðer zamanlayýcý sýfýrlandýysa, aktif sipariþ sayýsý maksimumun altýnda ise ve sipariþ kuyruðunda sipariþ varsa yeni bir sipariþ spawn et
        if (spawnTimer <= 0f && activeOrders.Count < maxActiveOrders && orderQueue.Count > 0)
        {
            Order newOrder = orderQueue.Dequeue();
            CreateOrderUI(newOrder);
            spawnTimer = interOrderDelay; // Gecikmeyi resetle
        }

        // Aktif sipariþlerin zamanýný kontrol et
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
            ProcessOrder(order.order, false); // Süresi doldu, baþarýsýz sayýlýr
        }
    }

    /// <summary>
    /// Aktif sipariþleri temsil eden sýnýf.
    /// </summary>
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
}