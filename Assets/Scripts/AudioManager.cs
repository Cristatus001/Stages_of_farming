using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public Toggle voiceOverToggle;

    private AudioSource[] allAudioSources;

    private void Awake()
    {
        // Implement the Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Find the voice-over toggle if it's not assigned
        if (voiceOverToggle != null)
        {
            voiceOverToggle.onValueChanged.AddListener(delegate { ApplyVoiceOverSettings(); });
        }

        // Collect all AudioSources at the start
        UpdateVoiceOverSources();

        // Apply the initial state
        ApplyVoiceOverSettings();
    }

    // Method to update the list of all voice-over AudioSources
    public void UpdateVoiceOverSources()
    {
        allAudioSources = FindObjectsOfType<AudioSource>();
    }

    private void ApplyVoiceOverSettings()
    {
        if (allAudioSources != null)
        {
            foreach (var source in allAudioSources)
            {
                if (voiceOverToggle != null && voiceOverToggle.isOn)
                {
                    if (!source.isPlaying && source.clip != null)
                    {
                        source.Play();
                    }
                }
                else
                {
                    source.Stop();
                }
            }
        }
    }

    // Public method to handle cases where panels or scenes change
    public void OnSceneOrPanelChanged()
    {
        UpdateVoiceOverSources();
        ApplyVoiceOverSettings();
    }
}
