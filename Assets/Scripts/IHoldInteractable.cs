using UnityEngine;

public interface IHoldInteractable
{
    bool CanHoldInteract(GameObject player);
    void OnHoldInteract(GameObject player, float deltaTime);
}
