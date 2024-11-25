using UnityEngine;

public class GameSceneController : MonoBehaviour
{
    private void Start()
    {
        if (GameManager.Instance != null)
        {
          //  GameManager.Instance.SpawnPlayerCharacter();
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }
    }
}
