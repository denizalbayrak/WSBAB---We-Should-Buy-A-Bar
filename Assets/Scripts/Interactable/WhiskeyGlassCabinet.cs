using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiskeyGlassCabinet : PlacableInteractable
{
    [Header("Cabinet Settings")]
    [Tooltip("References to the whiskey glass objects inside the cabinet.")]
    public GameObject[] whiskeyGlassObjects = new GameObject[2];

    [Tooltip("Prefab of the whiskey glass to give to the player.")]
    public GameObject whiskeyGlassPrefab;

    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject == null)
            {
                // Player wants to take a mojito glass
                if (GetAvailableWhiskeyGlassCount() <= 0)
                {
                    Debug.Log("The cabinet is empty. No more whiskey glasses available.");
                    return;
                }

                // Disable one of the active whiskey glass objects
                for (int i = 0; i < whiskeyGlassObjects.Length; i++)
                {
                    if (whiskeyGlassObjects[i].activeSelf)
                    {
                        whiskeyGlassObjects[i].SetActive(false);
                        break;
                    }
                }

                // Instantiate a new whiskeyGlass and give it to the player
                GameObject newWhiskeyGlass = Instantiate(whiskeyGlassPrefab);
                WhiskeyGlass whiskeyGlass = newWhiskeyGlass.GetComponent<WhiskeyGlass>();
                if (whiskeyGlass != null)
                {
                    // Set the whiskeyGlass to be clean and empty
                    whiskeyGlass.CurrentState = WhiskeyGlass.GlassState.CleanEmpty;

                    // Have the player pick up the whiskeyGlass
                    playerInteraction.PickUpObject(newWhiskeyGlass);

                    Debug.Log("Picked up a whiskey glass from the cabinet.");
                }
                else
                {
                    Debug.LogError("The whiskeyGlassPrefab does not have a whiskeyGlass component.");
                    Destroy(newWhiskeyGlass);
                }
            }
            else
            {
                // Player is carrying something
                // Check if it's a clean, empty mimosaGlass
                WhiskeyGlass whiskeyGlass = playerInteraction.CarriedObject.GetComponent<WhiskeyGlass>();
                if (whiskeyGlass != null && whiskeyGlass.CurrentState == WhiskeyGlass.GlassState.CleanEmpty)
                {
                    if (GetAvailableSlotsInCabinet() <= 0)
                    {
                        Debug.Log("The cabinet is full. Cannot place more whiskeyGlass glasses.");
                        return;
                    }

                    // Destroy the whiskeyGlass glass (remove it from the game)
                    Destroy(playerInteraction.CarriedObject);

                    // Reset the player's carried object
                    playerInteraction.CarriedObject = null;

                    // Enable one of the disabled whiskeyGlass glass objects
                    for (int i = 0; i < whiskeyGlassObjects.Length; i++)
                    {
                        if (!whiskeyGlassObjects[i].activeSelf)
                        {
                            whiskeyGlassObjects[i].SetActive(true);
                            break;
                        }
                    }

                    Debug.Log("Placed an empty whiskeyGlass into the cabinet.");
                }
                else
                {
                    Debug.Log("You can only place clean, empty whiskeyGlass into the cabinet.");
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
                // Can interact if the cabinet has whiskeyGlass to give
                return GetAvailableWhiskeyGlassCount() > 0;
            }
            else
            {
                // Can place a clean, empty whiskeyGlass if there's room in the cabinet
                WhiskeyGlass whiskeyGlass = playerInteraction.CarriedObject.GetComponent<WhiskeyGlass>();
                return whiskeyGlass != null &&
                       whiskeyGlass.CurrentState == WhiskeyGlass.GlassState.CleanEmpty &&
                       GetAvailableSlotsInCabinet() > 0;
            }
        }
        return false;
    }

    private int GetAvailableWhiskeyGlassCount()
    {
        int count = 0;
        foreach (GameObject glass in whiskeyGlassObjects)
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
        foreach (GameObject glass in whiskeyGlassObjects)
        {
            if (!glass.activeSelf)
            {
                count++;
            }
        }
        return count;
    }
}