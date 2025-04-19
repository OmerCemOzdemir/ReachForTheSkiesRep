using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalUI : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Return()
    {
        SceneManager.LoadScene(0);

    }

}
