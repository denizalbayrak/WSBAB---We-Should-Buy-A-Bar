using System.Collections;
using UnityEngine;

public class BeerGlassCabinet : PlacableInteractable
{
    [Header("Cabinet Settings")]
    [Tooltip("References to the beer glass objects inside the cabinet.")]
    public GameObject[] beerGlassObjects = new GameObject[2];

    [Tooltip("Prefab of the beer glass to give to the player.")]
    public GameObject beerGlassPrefab;
    public void RespawnGlass(float delay)
    {
        StartCoroutine(RespawnGlassCoroutine(delay));
    }
    private IEnumerator RespawnGlassCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Find an inactive glass slot
        foreach (GameObject glass in beerGlassObjects)
        {
            if (!glass.activeSelf)
            {
                glass.SetActive(true);
                Debug.Log("Beer glass respawned in the cabinet.");
                yield break;
            }
        }

        if (beerGlassPrefab != null)
        {
            Instantiate(beerGlassPrefab, GetRespawnPosition(), Quaternion.identity);
            Debug.Log("Beer glass instantiated in the cabinet.");
        }
        else
        {
            Debug.LogError("BeerGlassPrefab is not assigned in the BeerGlassCabinet script.");
        }
    }
    private Vector3 GetRespawnPosition()
    {
        foreach (Transform child in transform)
        {
            if (!child.gameObject.activeSelf)
            {
                return child.position;
            }
        }

        // Default respawn position (modify as needed)
        return transform.position;
    }
    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject == null)
            {
                // Player wants to take a beer glass
                if (GetAvailableBeerGlassCount() <= 0)
                {
                    Debug.Log("The cabinet is empty. No more beer glasses available.");
                    return;
                }

                // Disable one of the active beer glass objects
                for (int i = 0; i < beerGlassObjects.Length; i++)
                {
                    if (beerGlassObjects[i].activeSelf)
                    {
                        beerGlassObjects[i].SetActive(false);
                        break;
                    }
                }

                // Instantiate a new beer glass and give it to the player
                GameObject newBeerGlass = Instantiate(beerGlassPrefab);
                BeerGlass beerGlass = newBeerGlass.GetComponent<BeerGlass>();
                if (beerGlass != null)
                {
                    // Set the beer glass to be clean and empty
                    beerGlass.CurrentState = BeerGlass.GlassState.CleanEmpty;

                    // Have the player pick up the beer glass
                    playerInteraction.PickUpObject(newBeerGlass);

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
                // Player is carrying something
                // Check if it's a clean, empty beer glass
                BeerGlass beerGlass = playerInteraction.CarriedObject.GetComponent<BeerGlass>();
                if (beerGlass != null && beerGlass.CurrentState == BeerGlass.GlassState.CleanEmpty)
                {
                    if (GetAvailableSlotsInCabinet() <= 0)
                    {
                        Debug.Log("The cabinet is full. Cannot place more beer glasses.");
                        return;
                    }

                    // Destroy the beer glass (remove it from the game)
                    Destroy(playerInteraction.CarriedObject);

                    // Reset the player's carried object
                    playerInteraction.CarriedObject = null;

                    // Enable one of the disabled beer glass objects
                    for (int i = 0; i < beerGlassObjects.Length; i++)
                    {
                        if (!beerGlassObjects[i].activeSelf)
                        {
                            beerGlassObjects[i].SetActive(true);
                            break;
                        }
                    }
                    playerInteraction.isCarrying = false;
                    playerInteraction.animator.SetBool("isCarry", false);
                    Debug.Log("Placed an empty beer glass into the cabinet.");
                }
                else
                {
                    Debug.Log("You can only place clean, empty beer glasses into the cabinet.");
                }
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
                // Can interact if the cabinet has beer glasses to give
                return GetAvailableBeerGlassCount() > 0;
            }
            else
            {
                // Can place a clean, empty beer glass if there's room in the cabinet
                BeerGlass beerGlass = playerInteraction.CarriedObject.GetComponent<BeerGlass>();
                return beerGlass != null &&
                       beerGlass.CurrentState == BeerGlass.GlassState.CleanEmpty &&
                       GetAvailableSlotsInCabinet() > 0;
            }
        }
        return false;
    }

    private int GetAvailableBeerGlassCount()
    {
        int count = 0;
        foreach (GameObject glass in beerGlassObjects)
        {
            if (glass.activeSelf)
            {
                count++;
            }
        }
        return count;
    }

    private int GetAvailableSlotsInCabinet()
    {
        int count = 0;
        foreach (GameObject glass in beerGlassObjects)
        {
            if (!glass.activeSelf)
            {
                count++;
            }
        }
        return count;
    }
}
