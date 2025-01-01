using UnityEngine;

public class GameSceneController : MonoBehaviour
{
    private void Start()
    {
        if (GameManager.Instance != null)
        {
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }
    }
}
