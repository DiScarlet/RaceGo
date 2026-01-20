using UnityEngine;
using UnityEngine.UI;

public class VolumeHandler : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;

    private void Start()
    {
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        OnMusicVolumeChanged(musicSlider.value);
    }

    public void OnMusicVolumeChanged(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
    }
}
