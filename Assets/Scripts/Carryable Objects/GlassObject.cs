using UnityEngine;

public class GlassObject : CarryableObject
{
    public override void OnPickUp(Transform carrier)
    {
        Debug.Log($"{objectName} picked up.");

        if (instance != null)
        {
            instance.transform.SetParent(carrier);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;
            instance.GetComponent<Rigidbody>().isKinematic = true;

            // Disable collider
            Collider collider = instance.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
        }
    }

    public override void OnDrop(Vector3 dropPosition, Quaternion dropRotation)
    {
        Debug.Log($"{objectName} dropped.");

        if (instance != null)
        {
            instance.transform.SetParent(null);
            Rigidbody rb = instance.GetComponent<Rigidbody>();
            rb.isKinematic = false;

            // Enable collider
            Collider collider = instance.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = true;
            }

            // Place the object at the drop position and rotation
            instance.transform.position = dropPosition;
            instance.transform.rotation = dropRotation;

            // Reset physics velocities
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
