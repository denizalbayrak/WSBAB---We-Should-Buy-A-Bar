using UnityEngine;

public class PlayerCarry : MonoBehaviour
{
    private CarryableObject carriedObject;

    public Transform carryPoint; // Point where the object is held

    // Remove surfaceLayerMask and TryGetSurfacePosition since we're handling surface detection in PlayerController

    public bool IsCarrying => carriedObject != null;

    public void PickUp(CarryableObject newObject)
    {
        if (IsCarrying)
        {
            Debug.Log("You are already carrying an object.");
            return;
        }

        carriedObject = newObject;
        carriedObject.OnPickUp(carryPoint);
    }

    // Parameterless Drop method for default behavior
    public void Drop()
    {
        if (!IsCarrying)
        {
            Debug.Log("You are not carrying any object.");
            return;
        }

        // Default drop behavior (e.g., drop at player's feet)
        Vector3 dropPosition = transform.position + transform.forward * 1f + Vector3.up * 0.5f;
        Quaternion dropRotation = Quaternion.identity;

        carriedObject.OnDrop(dropPosition, dropRotation);

        carriedObject = null;
    }

    // Overloaded Drop method with position and rotation parameters
    public void Drop(Vector3 dropPosition, Quaternion dropRotation)
    {
        if (!IsCarrying)
        {
            Debug.Log("You are not carrying any object.");
            return;
        }

        carriedObject.OnDrop(dropPosition, dropRotation);

        carriedObject = null;
    }

    // Getter for the carried object
    public CarryableObject GetCarriedObject()
    {
        return carriedObject;
    }
}
