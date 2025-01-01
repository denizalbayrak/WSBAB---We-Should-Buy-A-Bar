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

        return transform.position;
    }
    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject == null)
            {
                if (GetAvailableBeerGlassCount() <= 0)
                {
                    Debug.Log("The cabinet is empty. No more beer glasses available.");
                    return;
                }

                for (int i = 0; i < beerGlassObjects.Length; i++)
                {
                    if (beerGlassObjects[i].activeSelf)
                    {
                        beerGlassObjects[i].SetActive(false);
                        break;
                    }
                }

                GameObject newBeerGlass = Instantiate(beerGlassPrefab);
                BeerGlass beerGlass = newBeerGlass.GetComponent<BeerGlass>();
                if (beerGlass != null)
                {
                    beerGlass.CurrentState = BeerGlass.GlassState.CleanEmpty;

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
                BeerGlass beerGlass = playerInteraction.CarriedObject.GetComponent<BeerGlass>();
                if (beerGlass != null && beerGlass.CurrentState == BeerGlass.GlassState.CleanEmpty)
                {
                    if (GetAvailableSlotsInCabinet() <= 0)
                    {
                        Debug.Log("The cabinet is full. Cannot place more beer glasses.");
                        return;
                    }

                    Destroy(playerInteraction.CarriedObject);

                    playerInteraction.CarriedObject = null;

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
                return GetAvailableBeerGlassCount() > 0;
            }
            else
            {
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
