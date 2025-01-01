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
    public GameObject orderUIPrefab; 
    public Transform orderUIContainer; 

    [Header("Scoring Settings")]
    public TextMeshProUGUI scoreText;

    [Header("Order Spawn Settings")]
    public int maxActiveOrders = 4;
    public float minOrderDelay = 15f; 
    public float maxOrderDelay = 25f; 

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

        StartCoroutine(SpawnOrders());
    }

    private void Update()
    {
        if (!isLevelActive)
            return;

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
            activeOrders.Remove(order);
            order.orderUI.RemoveUI();
            currentScore += order.order.scorePerFailure; 
            Debug.Log($"Order Failed! Score: {currentScore}");
            UpdateScoreUI();
        }
    }

    public void StopLevel()
    {
        isLevelActive = false; 
        StopAllCoroutines(); 
        ClearActiveOrders();
    }

    private void ClearActiveOrders()
    {
        foreach (var activeOrder in activeOrders)
        {
            if (activeOrder.orderUI != null)
            {
                activeOrder.orderUI.RemoveUI(); 
            }
        }
        activeOrders.Clear(); 
    }

    private IEnumerator SpawnOrders()
    {
        while (isLevelActive)
        {
            if (activeOrders.Count < maxActiveOrders)
            {
                CreateRandomOrder();
            }

       
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

       
        Order randomOrder = currentLevel.availableOrders[Random.Range(0, currentLevel.availableOrders.Count)];
        CreateOrderUI(randomOrder);
    }

    private void CreateOrderUI(Order orderTemplate)
    {
        
        Order newOrder = new Order
        {
            orderID = orderTemplate.orderID,
            orderType = orderTemplate.orderType,
            description = orderTemplate.description,
            orderImage = orderTemplate.orderImage,
            timeLimit = orderTemplate.timeLimit,
            quickTimeLimit = orderTemplate.quickTimeLimit, 
            scorePerSuccess = orderTemplate.scorePerSuccess,
            scorePerSuccessQuick = orderTemplate.scorePerSuccessQuick, 
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
            float deliveryTime = Time.time - matchingActiveOrder.spawnTime;

            activeOrders.Remove(matchingActiveOrder);
            matchingActiveOrder.orderUI.RemoveUI();

            if (deliveryTime <= matchingActiveOrder.order.quickTimeLimit)
            {
                currentScore += matchingActiveOrder.order.scorePerSuccessQuick;
                Debug.Log($"Quick Order Successful! +{matchingActiveOrder.order.scorePerSuccessQuick} points. Total Score: {currentScore}");
            }
            else
            {
                currentScore += matchingActiveOrder.order.scorePerSuccess;
                Debug.Log($"Order Successful! +{matchingActiveOrder.order.scorePerSuccess} points. Total Score: {currentScore}");
            }

            UpdateScoreUI();
        }
        else
        {
            Debug.Log($"Order Failed! No matching order.");
            UpdateScoreUI();
        }
    }

    private bool DoesDeliveredObjectMatchOrder(GameObject deliveredObject, Order order)
    {

        if (order.orderType == OrderType.DeliverBeerGlass)
        {
            BeerGlass beerGlass = deliveredObject.GetComponent<BeerGlass>();
            if (beerGlass != null && beerGlass.CurrentState == BeerGlass.GlassState.Filled)
            {
                return true;
            }
        }
        else if (order.orderType == OrderType.DeliverWineGlass)
        {
            WineGlass wineGlass = deliveredObject.GetComponent<WineGlass>();
            if (wineGlass != null && wineGlass.CurrentState == WineGlass.GlassState.Filled)
            {
                return true;
            }
        }
        else if (order.orderType == OrderType.DeliverMojitoGlass)
        {
            MojitoGlass mojitoGlass = deliveredObject.GetComponent<MojitoGlass>();
            if (mojitoGlass != null && mojitoGlass.CurrentState == MojitoGlass.GlassState.Filled)
            {
                return true;
            }
        }
        else if (order.orderType == OrderType.DeliverMimosaGlass)
        {
            MimosaGlass mimosaGlass = deliveredObject.GetComponent<MimosaGlass>();
            if (mimosaGlass != null && mimosaGlass.CurrentState == MimosaGlass.GlassState.Filled)
            {
                return true;
            }
        }
        else if (order.orderType == OrderType.DeliverWhiskeyGlass)
        {
            WhiskeyGlass whiskeyGlass = deliveredObject.GetComponent<WhiskeyGlass>();
            if (whiskeyGlass != null && whiskeyGlass.CurrentState == WhiskeyGlass.GlassState.Filled)
            {
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
        public float spawnTime; 

        public ActiveOrder(Order order, OrderUI orderUI)
        {
            this.order = order;
            this.orderUI = orderUI;
            this.timeLeft = order.timeLimit;
            this.spawnTime = Time.time; 
        }
    }

    public void ResetOrderManager()
    {
        StopLevel();
        currentScore = 0;
        UpdateScoreUI();
    }
}
