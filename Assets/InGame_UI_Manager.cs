using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGame_UI_Manager : MonoBehaviour
{
    public void GoToMainMenu() => StartCoroutine(LoadSceneWithTransition(0));
    public void PlayAgain() => StartCoroutine(LoadSceneWithTransition(1));

    private IEnumerator LoadSceneWithTransition(int sceneIndex)
    {
        ResetValues();
        yield return new WaitForSeconds(0f);
        if(sceneIndex == 1)
        {
            SceneManager.LoadScene(1);
        } 
        else if(sceneIndex == 0) 
        {
            SceneManager.LoadScene(0);
        }
        Debug.Log("Failed Nothing Selected Index: " + sceneIndex);
    }

    public void ExitGame() => StartCoroutine(ExitWithTransition());

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

    private void ResetValues()
    {
        Game_Manager.isPaused = false;
        Time.timeScale = 1;
    }
}
