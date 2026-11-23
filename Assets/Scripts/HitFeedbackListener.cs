using UnityEngine;

[RequireComponent(typeof(Health))]
public class HitFeedbackListener : MonoBehaviour
{
    [SerializeField] private ParticleSystem hitSparks;
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        if (health != null) health.Damaged += OnHit;
    }

    private void OnDisable()
    {
        if (health != null) health.Damaged -= OnHit;
    }

    private void OnHit(int current, int max)
    {
        if (SfxPlayer.Instance != null)
        {
            SfxPlayer.Instance.PlayHit();
        }

        if (hitSparks != null)
        {
            hitSparks.Play();
        }
    }
}