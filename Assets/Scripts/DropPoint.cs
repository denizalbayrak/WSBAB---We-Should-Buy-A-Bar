using UnityEngine;

public class DropPoint : MonoBehaviour
{
    public GameObject plane; // Plane child objesi (g�rsel)
    public bool IsEmpty => deliveredObject == null;  // DropPoint'in bo� olup olmad���n� kontrol eder
    private GameObject deliveredObject = null;

    public void DeliverObject(GameObject obj)
    {
        deliveredObject = obj;
        obj.transform.SetParent(this.transform);
        obj.transform.localPosition = Vector3.zero;  // Obje tam drop point'e yerle�tirilir
        TogglePlane(false);  // Objeyi yerle�tirince plane gizlenir
    }

    public void TogglePlane(bool state)
    {
        if (plane != null)
        {
            plane.SetActive(state);  // Plane a��l�p kapan�r
        }
    }
}
