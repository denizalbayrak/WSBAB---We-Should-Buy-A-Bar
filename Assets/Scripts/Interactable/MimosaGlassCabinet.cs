using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimosaGlassCabinet : PlacableInteractable
{
    [Header("Cabinet Settings")]
    [Tooltip("References to the mojito glass objects inside the cabinet.")]
    public GameObject[] mimosaGlassObjects = new GameObject[2];

    [Tooltip("Prefab of the mojito glass to give to the player.")]
    public GameObject mimosaGlassPrefab;

    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject == null)
            {
                // Player wants to take a mojito glass
                if (GetAvailableMimosaGlassCount() <= 0)
                {
                    Debug.Log("The cabinet is empty. No more mojito glasses available.");
                    return;
                }

                // Disable one of the active mimosa glass objects
                for (int i = 0; i < mimosaGlassObjects.Length; i++)
                {
                    if (mimosaGlassObjects[i].activeSelf)
                    {
                        mimosaGlassObjects[i].SetActive(false);
                        break;
                    }
                }

                // Instantiate a new mimosaGlass and give it to the player
                GameObject newMimosaGlass = Instantiate(mimosaGlassPrefab);
                MimosaGlass mimosaGlass = newMimosaGlass.GetComponent<MimosaGlass>();
                if (mimosaGlass != null)
                {
                    // Set the mmimosaGlass to be clean and empty
                    mimosaGlass.CurrentState = MimosaGlass.GlassState.CleanEmpty;

                    // Have the player pick up the mimosaGlass
                    playerInteraction.PickUpObject(newMimosaGlass);

                    Debug.Log("Picked up a mojito glass from the cabinet.");
                }
                else
                {
                    Debug.LogError("The mojitoGlassPrefab does not have a MojitoGlass component.");
                    Destroy(newMimosaGlass);
                }
            }
            else
            {
                // Player is carrying something
                // Check if it's a clean, empty mimosaGlass
                MimosaGlass mimosaGlass = playerInteraction.CarriedObject.GetComponent<MimosaGlass>();
                if (mimosaGlass != null && mimosaGlass.CurrentState == MimosaGlass.GlassState.CleanEmpty)
                {
                    if (GetAvailableSlotsInCabinet() <= 0)
                    {
                        Debug.Log("The cabinet is full. Cannot place more mimosaGlass glasses.");
                        return;
                    }

                    // Destroy the mimosaGlass glass (remove it from the game)
                    Destroy(playerInteraction.CarriedObject);

                    // Reset the player's carried object
                    playerInteraction.CarriedObject = null;

                    // Enable one of the disabled mimosaGlass glass objects
                    for (int i = 0; i < mimosaGlassObjects.Length; i++)
                    {
                        if (!mimosaGlassObjects[i].activeSelf)
                        {
                            mimosaGlassObjects[i].SetActive(true);
                            break;
                        }
                    }

                    Debug.Log("Placed an empty mimosaGlass into the cabinet.");
                }
                else
                {
                    Debug.Log("You can only place clean, empty mimosaGlass into the cabinet.");
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
                // Can interact if the cabinet has mimosaGlass to give
                return GetAvailableMimosaGlassCount() > 0;
            }
            else
            {
                // Can place a clean, empty mimosaGlass if there's room in the cabinet
                MimosaGlass mimosaGlass = playerInteraction.CarriedObject.GetComponent<MimosaGlass>();
                return mimosaGlass != null &&
                       mimosaGlass.CurrentState == MimosaGlass.GlassState.CleanEmpty &&
                       GetAvailableSlotsInCabinet() > 0;
            }
        }
        return false;
    }

    private int GetAvailableMimosaGlassCount()
    {
        int count = 0;
        foreach (GameObject glass in mimosaGlassObjects)
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
        foreach (GameObject glass in mimosaGlassObjects)
        {
            if (!glass.activeSelf)
            {
                count++;
            }
        }
        return count;
    }
}