using UnityEngine;

public abstract class CarryableObject : ScriptableObject, ICarryable
{
    public string objectName;
    public string description;

    protected GameObject instance;

    public GameObject Instance
    {
        get { return instance; }
    }

    public void SetInstance(GameObject obj)
    {
        instance = obj;
    }

    public abstract void OnPickUp(Transform carrier);
    public abstract void OnDrop(Vector3 dropPosition, Quaternion dropRotation);
}
