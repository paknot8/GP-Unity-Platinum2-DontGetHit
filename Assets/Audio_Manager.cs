using UnityEngine;
using UnityEngine.UI;

// --- Notes --- //
/* Textmeshpro Sliders
Drag this Script in the button and select ChangeMusicVolume or ChangeSoundVolume on the OnButtonClick
To Load it have a GameObject with this Script in it, the start function will load the sound and music levels.
*/
public class Audio_Manager : MonoBehaviour
{
    [SerializeField] private Slider musicSlider; // Slider for controlling music volume
    [SerializeField] private Slider soundSlider; // Slider for controlling sound effects volume

    void Start() {
        LoadVolumeLevels();
    }

    public void LoadVolumeLevels()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
        }
        if (!PlayerPrefs.HasKey("soundVolume"))
        {
            PlayerPrefs.SetFloat("soundVolume", 1);
        }
        LoadAllAudioSettings();
    }

    // Method to change the volume of all AudioSources tagged as "Music" based on the music slider value
    public void ChangeMusicVolume()
    {
        foreach (var audioSource in FindObjectsOfType<AudioSource>())
        {
            if (audioSource.CompareTag("Music"))
            {
                audioSource.volume = musicSlider.value;
            }
        }
        SaveMusic();
    }

    // Method to change the volume of all AudioSources tagged as "Sound" based on the sound slider value
    public void ChangeSoundVolume()
    {
        foreach (var audioSource in FindObjectsOfType<AudioSource>())
        {
            if (audioSource.CompareTag("Sound"))
            {
                audioSource.volume = soundSlider.value;
            }
        }
        // Save the changes made to the sound volume
        SaveSound();
    }

    #region Load & Save (Music and Sound Effects)
        // Load volume settings from PlayerPrefs
        private void LoadAllAudioSettings()
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            soundSlider.value = PlayerPrefs.GetFloat("soundVolume");
        }

        // Save volume settings to PlayerPrefs
        private void SaveMusic() => PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        private void SaveSound() => PlayerPrefs.SetFloat("soundVolume", soundSlider.value);
    #endregion
}
