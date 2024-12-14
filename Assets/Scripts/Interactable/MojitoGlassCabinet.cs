using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MojitoGlassCabinet : PlacableInteractable
{
    [Header("Cabinet Settings")]
    [Tooltip("References to the mojito glass objects inside the cabinet.")]
    public GameObject[] mojitoGlassObjects = new GameObject[2];

    [Tooltip("Prefab of the mojito glass to give to the player.")]
    public GameObject mojitoGlassPrefab;

    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject == null)
            {
                // Player wants to take a mojito glass
                if (GetAvailableMojitoGlassCount() <= 0)
                {
                    Debug.Log("The cabinet is empty. No more mojito glasses available.");
                    return;
                }

                // Disable one of the active mojito glass objects
                for (int i = 0; i < mojitoGlassObjects.Length; i++)
                {
                    if (mojitoGlassObjects[i].activeSelf)
                    {
                        mojitoGlassObjects[i].SetActive(false);
                        break;
                    }
                }

                // Instantiate a new mojito glass and give it to the player
                GameObject newMojitoGlass = Instantiate(mojitoGlassPrefab);
                MojitoGlass mojitoGlass = newMojitoGlass.GetComponent<MojitoGlass>();
                if (mojitoGlass != null)
                {
                    // Set the mojito glass to be clean and empty
                    mojitoGlass.CurrentState = MojitoGlass.GlassState.CleanEmpty;

                    // Have the player pick up the mojito glass
                    playerInteraction.PickUpObject(newMojitoGlass);

                    Debug.Log("Picked up a mojito glass from the cabinet.");
                }
                else
                {
                    Debug.LogError("The mojitoGlassPrefab does not have a MojitoGlass component.");
                    Destroy(newMojitoGlass);
                }
            }
            else
            {
                // Player is carrying something
                // Check if it's a clean, empty mojito glass
                MojitoGlass mojitoGlass = playerInteraction.CarriedObject.GetComponent<MojitoGlass>();
                if (mojitoGlass != null && mojitoGlass.CurrentState == MojitoGlass.GlassState.CleanEmpty)
                {
                    if (GetAvailableSlotsInCabinet() <= 0)
                    {
                        Debug.Log("The cabinet is full. Cannot place more mojitoGlass glasses.");
                        return;
                    }

                    // Destroy the mojitoGlass glass (remove it from the game)
                    Destroy(playerInteraction.CarriedObject);

                    // Reset the player's carried object
                    playerInteraction.CarriedObject = null;

                    // Enable one of the disabled mojitoGlass glass objects
                    for (int i = 0; i < mojitoGlassObjects.Length; i++)
                    {
                        if (!mojitoGlassObjects[i].activeSelf)
                        {
                            mojitoGlassObjects[i].SetActive(true);
                            break;
                        }
                    }
                    playerInteraction.isCarrying = false;
                    playerInteraction.animator.SetBool("isCarry", false);
                    Debug.Log("Placed an empty mojito glass into the cabinet.");
                }
                else
                {
                    Debug.Log("You can only place clean, empty mojito glasses into the cabinet.");
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
                // Can interact if the cabinet has mojito glasses to give
                return GetAvailableMojitoGlassCount() > 0;
            }
            else
            {
                // Can place a clean, empty mojito glass if there's room in the cabinet
                MojitoGlass mojitoGlass = playerInteraction.CarriedObject.GetComponent<MojitoGlass>();
                return mojitoGlass != null &&
                       mojitoGlass.CurrentState == MojitoGlass.GlassState.CleanEmpty &&
                       GetAvailableSlotsInCabinet() > 0;
            }
        }
        return false;
    }

    private int GetAvailableMojitoGlassCount()
    {
        int count = 0;
        foreach (GameObject glass in mojitoGlassObjects)
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
        foreach (GameObject glass in mojitoGlassObjects)
        {
            if (!glass.activeSelf)
            {
                count++;
            }
        }
        return count;
    }
}