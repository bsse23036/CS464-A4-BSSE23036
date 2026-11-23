using UnityEngine;

public class GoalZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LevelWon();
            }
            else
            {
                Debug.LogWarning("GameManager instance missing in scene!");
            }
        }
    }
}