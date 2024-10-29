using Unity.VisualScripting;
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
        isEmpty=false;
        deliveredObject = obj;
        obj.transform.SetParent(this.transform);
        obj.transform.localPosition = Vector3.zero;  // Obje tam drop point'e yerleþtirilir
    }

    public void TogglePlane(bool state)
    {
        if (plane != null)
        {
            plane.SetActive(state);  
        }
    }
}
