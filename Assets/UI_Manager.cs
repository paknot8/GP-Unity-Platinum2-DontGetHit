using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    void Start()
    {
        // Ensure that the main menu panel is active and the settings panel is inactive at the start
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void PlayGame()
    {
        // Load scene 1
        SceneManager.LoadScene("Scene1");
    }

    public void EnableSettings()
    {
        // Enable settings panel and disable main menu panel
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void EnableMainMenu()
    {
        // Enable main menu panel and disable settings panel
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Quit editor play mode
#else
        Application.Quit(); // Quit standalone build
#endif
    }
}
