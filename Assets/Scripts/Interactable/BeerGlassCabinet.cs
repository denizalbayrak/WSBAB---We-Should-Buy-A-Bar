using UnityEngine;

public class BeerGlassCabinet : PlacableInteractable
{
    [Header("Cabinet Settings")]
    [Tooltip("Maximum number of beer glasses the cabinet can hold.")]
    public int maxBeerGlasses = 10;

    [Tooltip("Current number of beer glasses available.")]
    public int currentBeerGlasses = 10;

    [Tooltip("Prefab of the beer glass to give to the player.")]
    public GameObject beerGlassPrefab;

    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject == null)
            {
                if (currentBeerGlasses <= 0)
                {
                    Debug.Log("The cabinet is empty. No more beer glasses available.");
                    return;
                }

                // Instantiate a new beer glass and give it to the player
                GameObject newBeerGlass = Instantiate(beerGlassPrefab);
                BeerGlass beerGlass = newBeerGlass.GetComponent<BeerGlass>();
                if (beerGlass != null)
                {
                    // Set the beer glass to be clean and empty
                    beerGlass.IsDirty = false;
                    beerGlass.IsFilled = false;

                    // Have the player pick up the beer glass
                    playerInteraction.PickUpObject(newBeerGlass);

                    // Decrease the number of beer glasses in the cabinet
                    currentBeerGlasses--;

                    Debug.Log("Picked up a beer glass from the cabinet.");
                }
                else
                {
                    Debug.LogError("The beerGlassPrefab does not have a BeerGlass component.");
                    Destroy(newBeerGlass);
                }
            }
            else
            {
                // If the player is carrying something, we can place it on the cabinet
                base.Interact(player);
            }
        }
        else
        {
            Debug.Log("Cannot interact with the cabinet.");
        }
    }

    public override bool CanInteract(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject == null)
            {
                // Can interact if the cabinet has beer glasses
                return currentBeerGlasses > 0;
            }
            else
            {
                // Can place an object
                return base.CanInteract(player);
            }
        }
        return false;
    }

    public void Restock(int amount)
    {
        currentBeerGlasses += amount;
        if (currentBeerGlasses > maxBeerGlasses)
        {
            currentBeerGlasses = maxBeerGlasses;
        }
        Debug.Log("Cabinet restocked. Current beer glasses: " + currentBeerGlasses);
    }
}
