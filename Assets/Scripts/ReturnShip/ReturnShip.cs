using UnityEngine;

public class ReturnShip : MonoBehaviour
{
    private LoaderManager loaderManager;

    private void Start()
    {
        loaderManager = FindAnyObjectByType<LoaderManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //SaveManager.saveManager_Instance.SaveGame();
        // SceneManager.LoadScene(1);
        loaderManager.LoadNextLevel(1);
    }

}
