using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject difficultyPanel;

    void Start()
    {
        // Ensure that the main menu panel is active and the settings panel is inactive at the start
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    // Initiates the game start sequence.
    public void PlayGame()
    {
        difficultyPanel.SetActive(true);
    }

    // Coroutine to load the scene with a transition effect.
    private IEnumerator LoadSceneWithTransition(int sceneIndex)
    {
        yield return new WaitForSeconds(0.1f);
        if(Game_Manager.hardMode && sceneIndex == 1)
        {
            Game_Manager.inGame = true;
            SceneManager.LoadScene(sceneIndex);
        } 
        else if (!Game_Manager.hardMode && sceneIndex == 1)
        {
            Game_Manager.inGame = true;
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Game_Manager.inGame = false;
            Debug.Log("Something is wrong, check UI_Manager...");
        }
    }

    public void NormalDifficultyMode(){
        Game_Manager.hardMode = false;
        StartCoroutine(LoadSceneWithTransition(1));
    }

    public void HardDifficultyMode(){
        Game_Manager.hardMode = true;
        StartCoroutine(LoadSceneWithTransition(1));
    }

    // Activates the settings panel and deactivates the main menu panel.
    public void EnableSettings()
    {
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    // Activates the main menu panel and deactivates the settings panel.
    public void EnableMainMenu()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    // Initiates the exit game sequence.
    public void ExitGame()
    {
        StartCoroutine(ExitWithTransition());
    }

    // Coroutine to exit the game with a transition effect.
    private IEnumerator ExitWithTransition()
    {
        yield return new WaitForSeconds(0.1f);
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; // Quit editor play mode
        #else
                Application.Quit(); // Quit standalone build
        #endif
    }
}
