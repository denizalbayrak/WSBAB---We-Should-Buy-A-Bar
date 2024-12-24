using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Trash : PlacableInteractable
{
    [Header("Cabinet References")]
    [Tooltip("Assign the BeerGlassCabinet here.")]
    public BeerGlassCabinet beerCabinet;

    [Tooltip("Assign the WineGlassCabinet here.")]
    public WineGlassCabinet wineCabinet;

    [Tooltip("Assign the MojitoGlassCabinet here.")]
    public MojitoGlassCabinet mojitoCabinet;

    [Tooltip("Assign the MimosaGlassCabinet here.")]
    public MimosaGlassCabinet mimosaCabinet;

    [Tooltip("Assign the WhiskeyGlassCabinet here.")]
    public WhiskeyGlassCabinet whiskeyCabinet;

    [Tooltip("Respawn delay in seconds.")]
    public float respawnDelay = 8f;
    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            // Player is carrying something
            if (playerInteraction.CarriedObject != null)
            {
                // Attempt to get the IWashableGlass interface
                if (playerInteraction.CarriedObject.TryGetComponent<IWashableGlass>(out IWashableGlass washableGlass))
                {
                    // Store the glass type before destroying
                    GlassType trashedGlassType = washableGlass.Type;

                    // Destroy the carried glass
                    Destroy(playerInteraction.CarriedObject);

                    // Reset the player's carried object and state
                    playerInteraction.isCarrying = false;
                    playerInteraction.animator.SetBool("isCarry", false);
                    playerInteraction.CarriedObject = null;

                    Debug.Log($"Trashed a {trashedGlassType} glass.");

                    // Notify the appropriate cabinet to respawn the glass
                    RespawnGlassInCabinet(trashedGlassType);
                }
                else
                {
                    // The carried object is not a washable glass
                    Destroy(playerInteraction.CarriedObject);

                    // Reset the player's carried object and state
                    playerInteraction.isCarrying = false;
                    playerInteraction.animator.SetBool("isCarry", false);
                    playerInteraction.CarriedObject = null;

                    Debug.Log("Trashed a non-washable object.");
                }
            }
        }
        else
        {
            Debug.Log("Cannot interact with the trash cabinet.");
        }
    }


    public override bool CanInteract(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject != null)
            {
                return true;
            }
        }

        return false;
    }
    private void RespawnGlassInCabinet(GlassType glassType)
    {
        switch (glassType)
        {
            case GlassType.Beer:
                if (beerCabinet != null)
                {
                    beerCabinet.RespawnGlass(respawnDelay);
                }
                else
                {
                    Debug.LogError("BeerCabinet reference not set in Trash script.");
                }
                break;
            case GlassType.Wine:
                if (wineCabinet != null)
                {
                    wineCabinet.RespawnGlass(respawnDelay);
                }
                else
                {
                    Debug.LogError("WineCabinet reference not set in Trash script.");
                }
                break;
            case GlassType.Mojito:
                if (mojitoCabinet != null)
                {
                    mojitoCabinet.RespawnGlass(respawnDelay);
                }
                else
                {
                    Debug.LogError("MojitoCabinet reference not set in Trash script.");
                }
                break;
            case GlassType.Mimosa:
                if (mimosaCabinet != null)
                {
                    mimosaCabinet.RespawnGlass(respawnDelay);
                }
                else
                {
                    Debug.LogError("MimosaCabinet reference not set in Trash script.");
                }
                break;
            case GlassType.Whiskey:
                if (whiskeyCabinet != null)
                {
                    whiskeyCabinet.RespawnGlass(respawnDelay);
                }
                else
                {
                    Debug.LogError("WhiskeyCabinet reference not set in Trash script.");
                }
                break;
            default:
                Debug.LogWarning($"No cabinet found for glass type: {glassType}");
                break;
        }
    }

}