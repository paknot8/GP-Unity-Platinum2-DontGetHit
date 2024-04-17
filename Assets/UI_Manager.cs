using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public AudioSource buttonSound;

    void Start()
    {
        // Ensure that the main menu panel is active and the settings panel is inactive at the start
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void PlayGame()
    {
        // Start the coroutine to load the scene with a transition effect
        StartCoroutine(LoadSceneWithTransition(1));
    }

    IEnumerator LoadSceneWithTransition(int sceneIndex)
    {
        // Wait for the transition effect to complete to make fade out effect
        yield return new WaitForSeconds(0.3f);

        // Load the scene
        SceneManager.LoadScene(1);
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
