using UnityEngine;
using UnityEngine.UI; 

public class Sink : PlacableInteractable, IHoldInteractable
{
    private bool isWashing = false;
    private bool isWashingAnimStarted = false;
    private float washProgress = 0f;
    private float washDuration = 4f; 
    private IWashableGlass glassBeingWashed;
    [SerializeField] private Animator animator;
    public Image washProgressUI; 
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null && playerInteraction.CarriedObject != null)
        {
            base.Interact(player);

            if (placedObject != null)
            {
                IWashableGlass washableGlass = placedObject.GetComponent<IWashableGlass>();
                if (washableGlass != null)
                {
                    if (washableGlass.IsDirty)
                    {
                        isWashing = true;
                        washProgress = 0f;
                        glassBeingWashed = washableGlass;
                        Debug.Log("Started washing the glass. Hold Ctrl for " + washDuration + " seconds.");

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

  
    public bool CanHoldInteract(GameObject player)
    {
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
            washProgress += deltaTime;
            if (washProgress > washDuration)
            {
                washProgress = washDuration;
            }

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
                isWashing = false;
                glassBeingWashed.Clean();
                glassBeingWashed = null;
                Debug.Log("Finished washing the beer glass.");
                if (isWashingAnimStarted)
                {
                    isWashingAnimStarted = false;
                    animationController.SetFillingBeer(false);
                }
                if (washProgressUI != null)
                {
                    washProgressUI.gameObject.SetActive(false);
                }
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
                IWashableGlass washableGlass = playerInteraction.CarriedObject.GetComponent<IWashableGlass>();
                return placedObject == null && washableGlass != null && washableGlass.IsDirty;
            }
            else
            {
                return placedObject != null && !isWashing;
            }
        }
        return false;
    }
}
