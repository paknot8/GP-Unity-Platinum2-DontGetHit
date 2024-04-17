using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGame_UI_Manager : MonoBehaviour
{
    public void GoToMainMenu(){
        StartCoroutine(LoadSceneWithTransition(0));
    }

    public void PlayAgain(){
        StartCoroutine(LoadSceneWithTransition(1));
    }

    IEnumerator LoadSceneWithTransition(int sceneIndex)
    {
        yield return new WaitForSeconds(0.3f);
        if(sceneIndex == 1){
            SceneManager.LoadScene(1);
        } else {
            SceneManager.LoadScene(0);
        } 
    }

    public void ExitGame(){
        StartCoroutine(ExitWithTransition());
    }

    IEnumerator ExitWithTransition()
    {
        yield return new WaitForSeconds(0.3f);
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
