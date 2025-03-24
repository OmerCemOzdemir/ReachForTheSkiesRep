using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnShip : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SaveManager.saveManager_Instance.SaveGame();
        SceneManager.LoadScene(1);
    }

}
