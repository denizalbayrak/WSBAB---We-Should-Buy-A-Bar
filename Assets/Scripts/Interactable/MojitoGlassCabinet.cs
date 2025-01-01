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
                if (GetAvailableMojitoGlassCount() <= 0)
                {
                    Debug.Log("The cabinet is empty. No more mojito glasses available.");
                    return;
                }

                for (int i = 0; i < mojitoGlassObjects.Length; i++)
                {
                    if (mojitoGlassObjects[i].activeSelf)
                    {
                        mojitoGlassObjects[i].SetActive(false);
                        break;
                    }
                }

                GameObject newMojitoGlass = Instantiate(mojitoGlassPrefab);
                MojitoGlass mojitoGlass = newMojitoGlass.GetComponent<MojitoGlass>();
                if (mojitoGlass != null)
                {
                    mojitoGlass.CurrentState = MojitoGlass.GlassState.CleanEmpty;

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
                MojitoGlass mojitoGlass = playerInteraction.CarriedObject.GetComponent<MojitoGlass>();
                if (mojitoGlass != null && mojitoGlass.CurrentState == MojitoGlass.GlassState.CleanEmpty)
                {
                    if (GetAvailableSlotsInCabinet() <= 0)
                    {
                        Debug.Log("The cabinet is full. Cannot place more mojitoGlass glasses.");
                        return;
                    }

                    Destroy(playerInteraction.CarriedObject);

                    playerInteraction.CarriedObject = null;

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
                return GetAvailableMojitoGlassCount() > 0;
            }
            else
            {
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

    public void RespawnGlass(float delay)
    {
        StartCoroutine(RespawnGlassCoroutine(delay));
    }
    private IEnumerator RespawnGlassCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (GameObject glass in mojitoGlassObjects)
        {
            if (!glass.activeSelf)
            {
                glass.SetActive(true);
                Debug.Log("Beer glass respawned in the cabinet.");
                yield break;
            }
        }

        if (mojitoGlassPrefab != null)
        {
            Instantiate(mojitoGlassPrefab, GetRespawnPosition(), Quaternion.identity);
            Debug.Log("mojito glass instantiated in the cabinet.");
        }
        else
        {
            Debug.LogError("mojitoGlassPrefab is not assigned in the mojitoGlassCabinet script.");
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