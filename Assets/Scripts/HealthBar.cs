using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Image lagImage;
    [SerializeField] private TMP_Text healthText;

    private float targetFill = 1f;
    private float lagDelayTimer;
    private const float LagDelay = 0.4f;
    private const float LagDrainSpeed = 0.8f;

    public void Set(int current, int max)
    {
        float newFill = max > 0 ? Mathf.Clamp01((float)current / max) : 0f;
        if (newFill < targetFill)
        {
            lagDelayTimer = LagDelay;
        }
        else if (lagImage != null)
        {
            lagImage.fillAmount = newFill;
        }

        targetFill = newFill;
        if (fillImage != null)
        {
            fillImage.fillAmount = targetFill;
        }

        if (healthText != null)
        {
            healthText.text = $"HP: {Mathf.Max(0, current)}/{max}";
        }
    }

    private void Update()
    {
        if (lagImage == null) return;

        if (lagImage.fillAmount > targetFill)
        {
            if (lagDelayTimer > 0f)
            {
                lagDelayTimer -= Time.unscaledDeltaTime;
            }
            else
            {
                lagImage.fillAmount = Mathf.MoveTowards(
                    lagImage.fillAmount,
                    targetFill,
                    LagDrainSpeed * Time.unscaledDeltaTime
                );
            }
        }
    }
}