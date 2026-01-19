using UnityEngine;
using TMPro; // for TMP_Dropdown

public class QualityHandler : MonoBehaviour
{
    public TMP_Dropdown graphicsDropdown;

    void Start()
    {
        // Detect platform and set default dropdown value
        int defaultQuality = 1; // fallback

#if UNITY_ANDROID || UNITY_IOS
            defaultQuality = 0; // Mobile -> Low
#else
        defaultQuality = 2; // PC -> High
#endif

        // Clamp to available quality levels
        defaultQuality = Mathf.Clamp(defaultQuality, 0, QualitySettings.names.Length - 1);

        graphicsDropdown.value = defaultQuality;
        QualitySettings.SetQualityLevel(defaultQuality);

        // Add listener for user changes
        graphicsDropdown.onValueChanged.AddListener(OnGraphicsChange);
    }

    public void OnGraphicsChange(int value)
    {
        int mappedValue = MapDropdownToQuality(value);
        QualitySettings.SetQualityLevel(mappedValue);

        Debug.Log("Applied graphics quality: " + QualitySettings.names[mappedValue]);
    }

    int MapDropdownToQuality(int dropdownIndex)
    {
        int maxIndex = QualitySettings.names.Length - 1;

#if UNITY_ANDROID || UNITY_IOS
            switch(dropdownIndex)
            {
                case 0: return Mathf.Min(0, maxIndex); // Low
                case 1: return Mathf.Min(0, maxIndex); // Medium -> Low
                case 2: return Mathf.Min(1, maxIndex); // High -> Medium
            }
#else // PC
        switch (dropdownIndex)
        {
            case 0: return Mathf.Min(1, maxIndex); // Low -> Medium
            case 1: return Mathf.Min(2, maxIndex); // Medium -> High
            case 2: return Mathf.Min(3, maxIndex); // High -> Ultra (if exists)
        }
#endif
        return Mathf.Min(1, maxIndex);
    }
}
