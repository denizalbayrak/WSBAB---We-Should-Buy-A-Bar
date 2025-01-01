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
                if (GetAvailableMimosaGlassCount() <= 0)
                {
                    Debug.Log("The cabinet is empty. No more mojito glasses available.");
                    return;
                }

                for (int i = 0; i < mimosaGlassObjects.Length; i++)
                {
                    if (mimosaGlassObjects[i].activeSelf)
                    {
                        mimosaGlassObjects[i].SetActive(false);
                        break;
                    }
                }

                GameObject newMimosaGlass = Instantiate(mimosaGlassPrefab);
                MimosaGlass mimosaGlass = newMimosaGlass.GetComponent<MimosaGlass>();
                if (mimosaGlass != null)
                {
                    mimosaGlass.CurrentState = MimosaGlass.GlassState.CleanEmpty;

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
                MimosaGlass mimosaGlass = playerInteraction.CarriedObject.GetComponent<MimosaGlass>();
                if (mimosaGlass != null && mimosaGlass.CurrentState == MimosaGlass.GlassState.CleanEmpty)
                {
                    if (GetAvailableSlotsInCabinet() <= 0)
                    {
                        Debug.Log("The cabinet is full. Cannot place more mimosaGlass glasses.");
                        return;
                    }

                    Destroy(playerInteraction.CarriedObject);

                    playerInteraction.CarriedObject = null;

                    for (int i = 0; i < mimosaGlassObjects.Length; i++)
                    {
                        if (!mimosaGlassObjects[i].activeSelf)
                        {
                            mimosaGlassObjects[i].SetActive(true);
                            break;
                        }
                    }
                    playerInteraction.isCarrying = false;
                    playerInteraction.animator.SetBool("isCarry", false);
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
                return GetAvailableMimosaGlassCount() > 0;
            }
            else
            {
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

    public void RespawnGlass(float delay)
    {
        StartCoroutine(RespawnGlassCoroutine(delay));
    }
    private IEnumerator RespawnGlassCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (GameObject glass in mimosaGlassObjects)
        {
            if (!glass.activeSelf)
            {
                glass.SetActive(true);
                Debug.Log("Beer glass respawned in the cabinet.");
                yield break;
            }
        }

        if (mimosaGlassPrefab != null)
        {
            Instantiate(mimosaGlassPrefab, GetRespawnPosition(), Quaternion.identity);
            Debug.Log("mimosa glass instantiated in the cabinet.");
        }
        else
        {
            Debug.LogError("mimosaGlassPrefab is not assigned in the mimosaGlassCabinet script.");
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