using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.Rendering.DebugUI;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        //Singledon

        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetMusicVolume(float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);
        audioMixer.SetFloat("music", Mathf.Log10(value) * 20);
    }
}
