using UnityEngine;
using UnityEngine.SceneManagement;

public class InGame_UI_Manager : MonoBehaviour
{
    public void GoToMainMenu(){
        SceneManager.LoadScene(0);
    }
}
