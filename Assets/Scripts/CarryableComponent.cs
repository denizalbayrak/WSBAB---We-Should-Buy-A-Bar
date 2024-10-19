using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CarryableComponent : MonoBehaviour, IInteractable
{
    public CarryableObject carryableObject;

    private void Start()
    {
        if (carryableObject != null)
        {
            carryableObject.SetInstance(gameObject);
        }
    }

    public void Interact(GameObject interactor)
    {
        PlayerCarry playerCarry = interactor.GetComponent<PlayerCarry>();
        if (playerCarry != null)
        {
            playerCarry.PickUp(carryableObject);
        }
        else
        {
            Debug.LogWarning("PlayerCarry bileþeni bulunamadý.");
        }
    }
}
