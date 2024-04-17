using UnityEngine;
using UnityEngine.SceneManagement;

public class InGame_UI_Manager : MonoBehaviour
{
    public void GoToMainMenu(){
        SceneManager.LoadScene(0);
    }

    public void PlayAgain(){
        SceneManager.LoadScene(1);
    }

    public void ExitGame(){
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; // Quit editor play mode
        #else
                Application.Quit(); // Quit standalone build
        #endif
    }
}
