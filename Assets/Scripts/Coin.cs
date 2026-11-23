using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotateSpeed = 100f;

    [Header("Floating / Bobbing Motion")]
    public bool enableBobbing = true;
    public float bobSpeed = 2f;
    public float bobHeight = 0.15f;

    [Header("Audio & Visual Feedback")]
    [SerializeField] private GameObject pickupEffectPrefab;

    private Vector3 startPosition;
    private bool isCollected = false;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Continuous rotation around the Y-axis
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);

        // Gentle floating up-and-down motion
        if (enableBobbing)
        {
            float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;

        // Check both the collider and its parent in case the Player has child colliders
        if (other.CompareTag("Player") || (other.transform.parent != null && other.transform.parent.CompareTag("Player")))
        {
            isCollected = true;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddCoin();
            }

            if (SfxPlayer.Instance != null)
            {
                SfxPlayer.Instance.PlayPickup();
            }

            if (pickupEffectPrefab != null)
            {
                Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}