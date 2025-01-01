using UnityEngine;
using UnityEngine.UI;

public class BeerStation : PlacableInteractable, IHoldInteractable
{
    private bool isFilling = false;
    private float fillProgress = 0f;
    private float fillDuration = 5f; 
    private BeerGlass glassBeingFilled;
    private Animator glassAnimator;

    public Image fillProgressUI; 
    private bool isClockVisible = false;
    private bool isFillStart = false;
    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject != null)
            {
                BeerGlass beerGlass = playerInteraction.CarriedObject.GetComponent<BeerGlass>();
                if (beerGlass != null)
                {
                    if (beerGlass.CurrentState == BeerGlass.GlassState.CleanEmpty)
                    {
                        if (placedObject == null)
                        {
                            base.Interact(player); 
                            Debug.Log("Placed a clean, empty beer glass on the beer station.");
                        }
                        else
                        {
                            Debug.Log("Cannot place object. Placement point is already occupied.");
                        }
                    }
                    else
                    {
                        Debug.Log("You can only place a clean, empty beer glass here.");
                    }
                }
                else
                {
                    Debug.Log("You need to carry a beer glass to place it here.");
                }
            }
            else if (playerInteraction.CarriedObject == null)
            {
                if (placedObject != null)
                {
                    if (!isFilling)
                    {
                        base.Interact(player);
                        Debug.Log("Picked up the beer glass from the beer station.");
                    }
                    else
                    {
                        Debug.Log("Cannot pick up the glass. Filling in progress.");
                    }
                }
                else
                {
                    Debug.Log("Nothing to pick up here.");
                }
            }
        }
    }

    public bool CanHoldInteract(GameObject player)
    {
        if (placedObject != null)
        {
            BeerGlass beerGlass = placedObject.GetComponent<BeerGlass>();

            if (isFilling)
            {
                return true;
            }
            else if (beerGlass != null && beerGlass.CurrentState == BeerGlass.GlassState.CleanEmpty)
            {
                return true;
            }
        }

        return false;
    }

    public void OnHoldInteract(GameObject player, float deltaTime)
    {
        var animationController = player.GetComponent<PlayerAnimator>();
        if (animationController == null)
        {
            Debug.LogError("PlayerAnimationController is missing on player!");
            return;
        }

        if (isFilling)
        {
            fillProgress += deltaTime;
            if (fillProgress > fillDuration)
            {
                fillProgress = fillDuration;
            }           
            UpdateFillProgressUI();

            if (glassAnimator != null)
            {
                float normalizedTime = fillProgress / fillDuration;
                glassAnimator.Play("BeerFill", 0, normalizedTime);
                glassAnimator.speed = 0f; 
                if (isFillStart)
                {
                    isFillStart = false;
                    animationController.SetFillingBeer(false);
                }
                player.GetComponent<Animator>().Play("FillBeer", 0, normalizedTime);
            }

            if (fillProgress >= fillDuration)
            {
                isFilling = false;
                glassBeingFilled.Fill();
                glassAnimator.Play("BeerFill", 0, 1f);
                glassAnimator.speed = 0f;
                if (isFillStart)
                {
                    isFillStart = false;
                    animationController.SetFillingBeer(false);
                }
                glassBeingFilled = null;
                Debug.Log("Finished filling the beer glass.");

                if (fillProgressUI != null)
                {
                    fillProgressUI.gameObject.SetActive(false);
                }
                isClockVisible = false;
            }
        }
        else
        {
            isFilling = true;
            fillProgress = 0f;
            glassBeingFilled = placedObject.GetComponent<BeerGlass>();

           
            glassAnimator = glassBeingFilled.GetComponent<Animator>();
            if (glassAnimator != null)
            {
                
                glassAnimator.Play("BeerFill", 0, 0f);
                glassAnimator.speed = 0f; 
            }
            if (!isFillStart)
            {
                isFillStart = true;
                animationController.SetFillingBeer(true);
                animationController.TriggerFillingBeer();
            }
            Debug.Log("Started filling the beer glass. Hold Ctrl for " + fillDuration + " seconds.");

            if (fillProgressUI != null)
            {
                fillProgressUI.gameObject.SetActive(true);
            }
            isClockVisible = true;
        }
    }

    private void UpdateFillProgressUI()
    {
        if (fillProgressUI != null)
        {
            float fillAmount = fillProgress / fillDuration;
            fillProgressUI.fillAmount = fillAmount;
        }
    }

    public override bool CanInteract(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject != null)
            {
                BeerGlass beerGlass = playerInteraction.CarriedObject.GetComponent<BeerGlass>();
                return beerGlass != null && beerGlass.CurrentState == BeerGlass.GlassState.CleanEmpty && placedObject == null;
            }
            else
            {
                if (placedObject != null)
                {
                    return !isFilling;
                }
            }
        }
        return false;
    }
}
