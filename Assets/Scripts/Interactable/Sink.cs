using UnityEngine;
using UnityEngine.UI; // E�er UI ��eleri kullanacaksan�z

public class Sink : PlacableInteractable, IHoldInteractable
{
    private bool isWashing = false;
    private bool isWashingAnimStarted = false;
    private float washProgress = 0f;
    private float washDuration = 4f; // Sabit y�kama s�resi (�rne�in 5 saniye)
    private IWashableGlass glassBeingWashed;
    [SerializeField] private Animator animator;
    public Image washProgressUI; // Y�kama ilerlemesini g�stermek i�in
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // UI Elements

    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null && playerInteraction.CarriedObject != null)
        {
            // �lk olarak, nesneyi yerle�tirin
            base.Interact(player);

            // Yerle�tirilen nesneyi al�n
            if (placedObject != null)
            {
                IWashableGlass washableGlass = placedObject.GetComponent<IWashableGlass>();
                if (washableGlass != null)
                {
                    if (washableGlass.IsDirty)
                    {
                        // Y�kama i�lemini ba�lat
                        isWashing = true;
                        washProgress = 0f;
                        glassBeingWashed = washableGlass;
                        Debug.Log("Started washing the glass. Hold Ctrl for " + washDuration + " seconds.");

                        // Y�kama ilerleme UI'sini g�ster
                        if (washProgressUI != null)
                        {
                            washProgressUI.gameObject.SetActive(true);
                            washProgressUI.fillAmount = 0;
                        }
                    }
                    else
                    {
                        Debug.Log("Placed a glass on the sink, but it's not dirty.");
                    }
                }
                else
                {
                    Debug.Log("Placed object is not a washable glass.");
                }
            }
        }
        else if (playerInteraction != null && playerInteraction.CarriedObject == null && placedObject != null)
        {
            // Oyuncu bir �ey ta��m�yorsa ve y�kama yap�lm�yorsa, barda�� alabilir
            if (!isWashing)
            {
                base.Interact(player);
                Debug.Log("Picked up the glass from the sink.");
                isWashingAnimStarted = false;
                animator.SetTrigger("WaterOff");
            }
            else
            {
                Debug.Log("Cannot pick up the glass. Washing in progress.");
            }
        }
        else
        {
            Debug.Log("Cannot interact with the sink.");
        }
    }

    // Implement IHoldInteractable
    public bool CanHoldInteract(GameObject player)
    {
        // Check if there is a glass on the sink and washing is in progress
        return isWashing;
    }

    public void OnHoldInteract(GameObject player, float deltaTime)
    {
        var animationController = player.GetComponent<PlayerAnimator>();
        if (animationController == null)
        {
            Debug.LogError("PlayerAnimationController is missing on player!");
            return;
        }
        if (isWashing)
        {         
            // Washing process in progress
            washProgress += deltaTime;
            if (washProgress > washDuration)
            {
                washProgress = washDuration;
            }

            // Update the wash progress UI
            UpdateWashProgressUI();

                float normalizedTime = washProgress / washDuration;
            if (!isWashingAnimStarted)
            {
                isWashingAnimStarted = true;
                animator.SetTrigger("WaterOn"); 
                animationController.SetFillingBeer(false);
            }
            player.GetComponent<Animator>().Play("FillBeer", 0, normalizedTime);
           

            if (washProgress >= washDuration)
            {
                // Washing completed
                isWashing = false;
                glassBeingWashed.Clean();
                glassBeingWashed = null;
                Debug.Log("Finished washing the beer glass.");
                if (isWashingAnimStarted)
                {
                    isWashingAnimStarted = false;
                    animationController.SetFillingBeer(false);
                }
                // Y�kama ilerleme UI'sini gizle
                if (washProgressUI != null)
                {
                    washProgressUI.gameObject.SetActive(false);
                }
                // Hide the fill progress UI
                if (washProgressUI != null)
                {
                    washProgressUI.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            isWashing = true;
            washProgress = 0f;
            if (!isWashing)
            {
                isWashing = true;
                animationController.SetFillingBeer(true);
                animationController.TriggerFillingBeer();
            }
            if (washProgressUI != null)
            {
                washProgressUI.gameObject.SetActive(true);
            }
        }
    }

    private void UpdateWashProgressUI()
    {
        if (washProgressUI != null)
        {
            float fillAmount = washProgress / washDuration;
            washProgressUI.fillAmount = fillAmount;
        }
    }

    public override bool CanInteract(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject != null)
            {
                // E�er oyuncu bir �ey ta��yorsa ve yerle�tirme noktas� bo�sa, kirli bir bardak yerle�tirebilir
                IWashableGlass washableGlass = playerInteraction.CarriedObject.GetComponent<IWashableGlass>();
                return placedObject == null && washableGlass != null && washableGlass.IsDirty;
            }
            else
            {
                // E�er oyuncu bir �ey ta��m�yorsa ve y�kama yap�lm�yorsa, barda�� alabilir
                return placedObject != null && !isWashing;
            }
        }
        return false;
    }
}
