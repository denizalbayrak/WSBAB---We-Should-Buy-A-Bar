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
                // Player wants to take a wine glass
                if (GetAvailableWineGlassCount() <= 0)
                {
                    Debug.Log("The cabinet is empty. No more wine glasses available.");
                    return;
                }

                // Disable one of the active wine glass objects
                for (int i = 0; i < wineGlassObjects.Length; i++)
                {
                    if (wineGlassObjects[i].activeSelf)
                    {
                        wineGlassObjects[i].SetActive(false);
                        break;
                    }
                }

                // Instantiate a new wine glass and give it to the player
                GameObject newWineGlass = Instantiate(wineGlassPrefab);
                WineGlass wineGlass = newWineGlass.GetComponent<WineGlass>();
                if (wineGlass != null)
                {
                    // Set the wine glass to be clean and empty
                    wineGlass.CurrentState = WineGlass.GlassState.CleanEmpty;

                    // Have the player pick up the wine glass
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
                // Player is carrying something
                // Check if it's a clean, empty wine glass
                WineGlass wineGlass = playerInteraction.CarriedObject.GetComponent<WineGlass>();
                if (wineGlass != null && wineGlass.CurrentState == WineGlass.GlassState.CleanEmpty)
                {
                    if (GetAvailableSlotsInCabinet() <= 0)
                    {
                        Debug.Log("The cabinet is full. Cannot place more wine glasses.");
                        return;
                    }

                    // Destroy the wine glass (remove it from the game)
                    Destroy(playerInteraction.CarriedObject);

                    // Reset the player's carried object
                    playerInteraction.CarriedObject = null;

                    // Enable one of the disabled wine glass objects
                    for (int i = 0; i < wineGlassObjects.Length; i++)
                    {
                        if (!wineGlassObjects[i].activeSelf)
                        {
                            wineGlassObjects[i].SetActive(true);
                            break;
                        }
                    }

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
                // Can interact if the cabinet has wine glasses to give
                return GetAvailableWineGlassCount() > 0;
            }
            else
            {
                // Can place a clean, empty wine glass if there's room in the cabinet
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
}