using UnityEngine;

public class DropPoint : MonoBehaviour
{
    public GameObject plane; // Plane child objesi (görsel)
    public bool IsEmpty => deliveredObject == null;  // DropPoint'in boþ olup olmadýðýný kontrol eder
    private GameObject deliveredObject = null;

    public void DeliverObject(GameObject obj)
    {
        deliveredObject = obj;
        obj.transform.SetParent(this.transform);
        obj.transform.localPosition = Vector3.zero;  // Obje tam drop point'e yerleþtirilir
        TogglePlane(false);  // Objeyi yerleþtirince plane gizlenir
    }

    public void TogglePlane(bool state)
    {
        if (plane != null)
        {
            plane.SetActive(state);  // Plane açýlýp kapanýr
        }
    }
}
