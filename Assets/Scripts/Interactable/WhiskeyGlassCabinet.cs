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
                if (GetAvailableWhiskeyGlassCount() <= 0)
                {
                    Debug.Log("The cabinet is empty. No more whiskey glasses available.");
                    return;
                }

                for (int i = 0; i < whiskeyGlassObjects.Length; i++)
                {
                    if (whiskeyGlassObjects[i].activeSelf)
                    {
                        whiskeyGlassObjects[i].SetActive(false);
                        break;
                    }
                }

                GameObject newWhiskeyGlass = Instantiate(whiskeyGlassPrefab);
                WhiskeyGlass whiskeyGlass = newWhiskeyGlass.GetComponent<WhiskeyGlass>();
                if (whiskeyGlass != null)
                {
                    
                    whiskeyGlass.CurrentState = WhiskeyGlass.GlassState.CleanEmpty;

                 
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
                
                WhiskeyGlass whiskeyGlass = playerInteraction.CarriedObject.GetComponent<WhiskeyGlass>();
                if (whiskeyGlass != null && whiskeyGlass.CurrentState == WhiskeyGlass.GlassState.CleanEmpty)
                {
                    if (GetAvailableSlotsInCabinet() <= 0)
                    {
                        Debug.Log("The cabinet is full. Cannot place more whiskeyGlass glasses.");
                        return;
                    }

                    
                    Destroy(playerInteraction.CarriedObject);

                
                    playerInteraction.CarriedObject = null;

                  
                    for (int i = 0; i < whiskeyGlassObjects.Length; i++)
                    {
                        if (!whiskeyGlassObjects[i].activeSelf)
                        {
                            whiskeyGlassObjects[i].SetActive(true);
                            break;
                        }
                    }
                    playerInteraction.isCarrying = false;
                    playerInteraction.animator.SetBool("isCarry", false);
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
                return GetAvailableWhiskeyGlassCount() > 0;
            }
            else
            {
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

    public void RespawnGlass(float delay)
    {
        StartCoroutine(RespawnGlassCoroutine(delay));
    }
    private IEnumerator RespawnGlassCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (GameObject glass in whiskeyGlassObjects)
        {
            if (!glass.activeSelf)
            {
                glass.SetActive(true);
                Debug.Log("whiskey glass respawned in the cabinet.");
                yield break;
            }
        }

        if (whiskeyGlassPrefab != null)
        {
            Instantiate(whiskeyGlassPrefab, GetRespawnPosition(), Quaternion.identity);
            Debug.Log("whiskey glass instantiated in the cabinet.");
        }
        else
        {
            Debug.LogError("whiskeyGlassPrefab is not assigned in the whiskeyGlassCabinet script.");
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