using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WineGlassCabinet : PlacableInteractable
{
    [Header("Cabinet Settings")]
    [Tooltip("References to the wine glass objects inside the cabinet.")]
    public GameObject[] wineGlassObjects = new GameObject[2];

    [Tooltip("Prefab of the wine glass to give to the player.")]
    public GameObject wineGlassPrefab;

    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject == null)
            {
                if (GetAvailableWineGlassCount() <= 0)
                {
                    Debug.Log("The cabinet is empty. No more wine glasses available.");
                    return;
                }

                for (int i = 0; i < wineGlassObjects.Length; i++)
                {
                    if (wineGlassObjects[i].activeSelf)
                    {
                        wineGlassObjects[i].SetActive(false);
                        break;
                    }
                }

                GameObject newWineGlass = Instantiate(wineGlassPrefab);
                WineGlass wineGlass = newWineGlass.GetComponent<WineGlass>();
                if (wineGlass != null)
                {
                   
                    wineGlass.CurrentState = WineGlass.GlassState.CleanEmpty;

                
                    playerInteraction.PickUpObject(newWineGlass);

                    Debug.Log("Picked up a wine glass from the cabinet.");
                }
                else
                {
                    Debug.LogError("The wineGlassPrefab does not have a WineGlass component.");
                    Destroy(newWineGlass);
                }
            }
            else
            {
                WineGlass wineGlass = playerInteraction.CarriedObject.GetComponent<WineGlass>();
                if (wineGlass != null && wineGlass.CurrentState == WineGlass.GlassState.CleanEmpty)
                {
                    if (GetAvailableSlotsInCabinet() <= 0)
                    {
                        Debug.Log("The cabinet is full. Cannot place more wine glasses.");
                        return;
                    }

                    
                    Destroy(playerInteraction.CarriedObject);

                    playerInteraction.CarriedObject = null;

                    
                    for (int i = 0; i < wineGlassObjects.Length; i++)
                    {
                        if (!wineGlassObjects[i].activeSelf)
                        {
                            wineGlassObjects[i].SetActive(true);
                            break;
                        }
                    }
                    playerInteraction.isCarrying = false;
                    playerInteraction.animator.SetBool("isCarry", false);
                    Debug.Log("Placed an empty wine glass into the cabinet.");
                }
                else
                {
                    Debug.Log("You can only place clean, empty wine glasses into the cabinet.");
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
                return GetAvailableWineGlassCount() > 0;
            }
            else
            {
                WineGlass wineGlass = playerInteraction.CarriedObject.GetComponent<WineGlass>();
                return wineGlass != null &&
                       wineGlass.CurrentState == WineGlass.GlassState.CleanEmpty &&
                       GetAvailableSlotsInCabinet() > 0;
            }
        }
        return false;
    }

    private int GetAvailableWineGlassCount()
    {
        int count = 0;
        foreach (GameObject glass in wineGlassObjects)
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
        foreach (GameObject glass in wineGlassObjects)
        {
            if (!glass.activeSelf)
            {
                count++;
            }
        }
        return count;
    }
    public void RespawnGlass(float delay)
    {
        StartCoroutine(RespawnGlassCoroutine(delay));
    }
    private IEnumerator RespawnGlassCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (GameObject glass in wineGlassObjects)
        {
            if (!glass.activeSelf)
            {
                glass.SetActive(true);
                Debug.Log("Beer glass respawned in the cabinet.");
                yield break;
            }
        }

        if (wineGlassPrefab != null)
        {
            Instantiate(wineGlassPrefab, GetRespawnPosition(), Quaternion.identity);
            Debug.Log("wine glass instantiated in the cabinet.");
        }
        else
        {
            Debug.LogError("WineGlassPrefab is not assigned in the WineGlassCabinet script.");
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
}