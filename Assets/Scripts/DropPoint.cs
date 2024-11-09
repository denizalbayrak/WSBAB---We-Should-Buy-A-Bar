using UnityEngine;

public class DropPoint : MonoBehaviour
{
    public GameObject plane;
    public GameObject deliveredObject = null;
    public bool isEmpty = true;

    private void Awake()
    {
        plane = transform.GetChild(0).gameObject;
        plane.SetActive(false);
    }

    public void DeliverObject(GameObject obj)
    {
        if (!isEmpty)
        {
            Debug.LogWarning("This drop point is already occupied! Cannot place another object.");
            return;
        }
        isEmpty = false;
        deliveredObject = obj;
        obj.transform.SetParent(this.transform);
        obj.transform.localPosition = Vector3.zero;  // Place the object exactly at the drop point
    }

    public void RemoveObject()
    {
        if (deliveredObject != null)
        {
            deliveredObject.transform.SetParent(null); // Detach the object
            Destroy(deliveredObject);
            deliveredObject = null;
            isEmpty = true;

        }
        else
        {
            Debug.LogWarning("No object to remove from this drop point.");
        }
    }

    public void TogglePlane(bool state)
    {
        if (plane != null)
        {
            plane.SetActive(state);
        }
    }
}
