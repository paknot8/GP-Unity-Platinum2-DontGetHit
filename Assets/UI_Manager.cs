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
        StartCoroutine(LoadSceneWithTransition(1)); // Start the coroutine to load the scene with a transition effect
    }

    // Wait for the transition effect to complete to make fade out effect
    IEnumerator LoadSceneWithTransition(int sceneIndex)
    {
        yield return new WaitForSeconds(0.3f);
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
        StartCoroutine(ExitWithTransition());
    }

    IEnumerator ExitWithTransition()
    {
        yield return new WaitForSeconds(0.3f);
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; // Quit editor play mode
        #else
                Application.Quit(); // Quit standalone build
        #endif
    }
}
