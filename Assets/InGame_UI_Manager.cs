using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Manages in-game UI functions such as navigation and exiting the game.
public class InGame_UI_Manager : MonoBehaviour
{
    // Loads the main menu scene.
    public void GoToMainMenu() => StartCoroutine(LoadSceneWithTransition(0));

    // Restarts the game scene.
    public void PlayAgain() => StartCoroutine(LoadSceneWithTransition(1));

    // Coroutine to load a scene with transition.
    private IEnumerator LoadSceneWithTransition(int sceneIndex)
    {
        ResetValues();
        yield return new WaitForSeconds(0f);

        if(sceneIndex == 1)
        {
            Game_Manager.inGame = false;
            SceneManager.LoadScene(1); // Load game scene
        } 
        else if(sceneIndex == 0) 
        {
            SceneManager.LoadScene(0); // Load main menu scene
        }
    }

    // Exits the game.
    public void ExitGame() => StartCoroutine(ExitWithTransition());

    // Coroutine to exit the game with transition.
    private IEnumerator ExitWithTransition()
    {
        ResetValues();
        yield return new WaitForSeconds(0f);

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; // Quit editor play mode
        #else
                Application.Quit(); // Quit standalone build
        #endif
    }

    // Resets game values.
    private void ResetValues()
    {
        Game_Manager.isPaused = false;
        Time.timeScale = 1;
    }
}
