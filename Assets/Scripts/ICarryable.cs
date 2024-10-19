using UnityEngine;

public interface ICarryable
{
    void OnPickUp(Transform carrier);
    void OnDrop(Vector3 dropPosition, Quaternion dropRotation);
}
