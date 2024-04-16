using System.Collections;
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
        // Start the coroutine to load the scene with a transition effect
        StartCoroutine(LoadSceneWithTransition(1));
    }

    IEnumerator LoadSceneWithTransition(int sceneIndex)
    {
        // Add transition effect here (e.g., fade out)
        // Example: FadeOutEffect.StartFade(1.0f); // Assuming you have a script for fading

        // Wait for the transition effect to complete
        yield return new WaitForSeconds(1.0f); // Adjust the duration according to your transition effect

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
