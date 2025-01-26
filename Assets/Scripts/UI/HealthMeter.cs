using GGJ.BubbleFall;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthMeter : MonoBehaviour
{
    [SerializeField] private RectTransform healthContainer;
    [SerializeField] private RawImage healthMeterNotchPrefab;

    private void OnEnable()
    {
        PlayerHealth.OnPlayerHealthChange += OnHealthChange;

    }
    private void OnDisable()
    {
        PlayerHealth.OnPlayerHealthChange -= OnHealthChange;

    }

    public void SetValue(int current, int max)
    {
        // Recreate max health
        if (healthContainer.childCount != max)
        {
            for (int i = 0; i < healthContainer.childCount; i++)
            {
                if (i >= max)
                {
                    Destroy(healthContainer.GetChild(i).gameObject);
                }
            }
            while (healthContainer.childCount < max)
            {
                Instantiate(healthMeterNotchPrefab, healthContainer.transform);
            }
        }

        // Turn notches on/off depending on value
        for (int i = 0; i < max; i++)
        {
            healthContainer.GetChild(i).GetComponent<RawImage>().enabled = i < current;
        }

    }

    private void OnHealthChange(int current, int max)
    {
        SetValue(current, max);
    }

}
