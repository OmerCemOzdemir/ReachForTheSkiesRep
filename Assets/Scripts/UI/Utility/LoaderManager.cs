using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoaderManager : MonoBehaviour
{
    [SerializeField] private Slider progressSlider;
    [SerializeField] private GameObject loaderUI;
    [SerializeField] private GameObject gearIconBig;
    [SerializeField] private GameObject gearIconSmall;

    public void LoadNextLevel(int level)
    {
        SaveManager.saveManager_Instance.SaveGame();
        loaderUI.SetActive(true);
        SceneManager.LoadSceneAsync(level);
        //StartCoroutine(PlayGearAnimation());
    }

    private void Update()
    {
        gearIconBig.transform.Rotate(0, 0, 0.01f * Time.deltaTime);
        gearIconSmall.transform.Rotate(0, 0, 0.01f * Time.deltaTime);
    }



    private IEnumerator LoadScene(int level)
    {
        progressSlider.value = 0;
        loaderUI.SetActive(true);
        //Time.timeScale = 0;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(level);
        asyncOperation.allowSceneActivation = false;
        float progress = 0;
        float timer = 0;
        float endTimer = 10;
        while (!asyncOperation.isDone)
        {

            timer = Mathf.MoveTowards(timer, endTimer, Time.deltaTime);

            if (timer >= 9)
            {
                progressSlider.value = 1;
                asyncOperation.allowSceneActivation = true;
                Debug.Log("Active New Level " + asyncOperation.allowSceneActivation);
                break;
            }
            else
            {
                Debug.Log("Level Before progress: " + timer);

            }

            if (progress >= 0.9f)
            {
                progressSlider.value = 1;
                asyncOperation.allowSceneActivation = true;
            }
            else
            {
                progress = Mathf.MoveTowards(progress, asyncOperation.progress, Time.deltaTime);
                progressSlider.value = progress;
            }
            yield return null;

        }





        // Debug.Log("asyncOperation.allowSceneActivation: " + asyncOperation.allowSceneActivation);
        // Debug.Log("asyncOperation.isDone: " + asyncOperation.isDone);

        //loaderUI.SetActive(false);
    }

}
/*
       private void Awake()
    {
        if (loaderInstance == null)
        {
            loaderInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (loaderInstance != this)
        {
            Destroy(gameObject);
        }
    }
 
                //Debug.Log("Level progress: " + progress);
               //Debug.Log("Level : " + level);
               //Debug.Log("Level : " + asyncOperation.isDone);


    private static LoaderManager loaderInstance;

    public static LoaderManager instance
    {
        get
        {
            if (loaderInstance == null)
            {
                loaderInstance = FindFirstObjectByType<LoaderManager>();

                if (loaderInstance == null)
                {
                    GameObject obj = new GameObject("LoaderManager");
                    loaderInstance = obj.AddComponent<LoaderManager>();
                }
            }
            return loaderInstance;
        }
    }



        while (!asyncOperation.isDone)
        {
            Debug.Log("Level Before progress: " + progress);

            progress = Mathf.MoveTowards(progress, asyncOperation.progress, Time.deltaTime);
            progressSlider.value = progress;
            if (progress >= 0.9f)
            {
                progressSlider.value = 1;
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;

        }




 */
