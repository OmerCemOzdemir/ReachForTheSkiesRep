using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private GameObject audioManager;
    [SerializeField] private Slider masterAudioSlider;
    [SerializeField] private Slider musicAudioSlider;
    [SerializeField] private Slider SFXAudioSlider;

    //[SerializeField] private TextMeshProUGUI settingsText;
    private float masterVolume;
    private float musicVolume;
    private float SFXVolume;


    private void Start()
    {
        if (SaveData.instance.newGame)
        {
            masterVolume = 0;
            musicVolume = 0;
            SFXVolume = 0;

            masterAudioSlider.onValueChanged.AddListener((masterAudioSliderValue) =>
            {
                //settingsText.text = "Slider Value Changed" + masterAudioSliderValue;
                audioMixer.SetFloat("Master", masterAudioSliderValue);
                SaveManager.saveManager_Instance.SaveGame();
                Debug.Log("Game Sound Changed");

            });

            musicAudioSlider.onValueChanged.AddListener((musicAudioSliderValue) =>
            {
                //settingsText.text = "Slider Value Changed" + musicAudioSliderValue;
                audioMixer.SetFloat("Music", musicAudioSliderValue);
                SaveManager.saveManager_Instance.SaveGame();
                Debug.Log("Game Sound Changed");


            });

            SFXAudioSlider.onValueChanged.AddListener((SFXAudioSliderValue) =>
            {
                // settingsText.text = "Slider Value Changed" + SFXAudioSliderValue;
                audioMixer.SetFloat("SFX", SFXAudioSliderValue);
                SaveManager.saveManager_Instance.SaveGame();
                Debug.Log("Game Sound Changed");


            });


        }
        else
        {
            masterAudioSlider.onValueChanged.AddListener((masterAudioSliderValue) =>
            {
                //settingsText.text = "Slider Value Changed" + masterAudioSliderValue;
                audioMixer.SetFloat("Master", masterAudioSliderValue);
                SaveManager.saveManager_Instance.SaveGame();
                Debug.Log("Game Sound Changed");

            });

            musicAudioSlider.onValueChanged.AddListener((musicAudioSliderValue) =>
            {
                //settingsText.text = "Slider Value Changed" + musicAudioSliderValue;
                audioMixer.SetFloat("Music", musicAudioSliderValue);
                SaveManager.saveManager_Instance.SaveGame();
                Debug.Log("Game Sound Changed");


            });

            SFXAudioSlider.onValueChanged.AddListener((SFXAudioSliderValue) =>
            {
                // settingsText.text = "Slider Value Changed" + SFXAudioSliderValue;
                audioMixer.SetFloat("SFX", SFXAudioSliderValue);
                SaveManager.saveManager_Instance.SaveGame();
                Debug.Log("Game Sound Changed");


            });



            masterVolume = SaveData.instance.gameMasterVolume;
            musicVolume = SaveData.instance.gameMusicVolume;
            SFXVolume = SaveData.instance.gameSFXVolume;

            audioMixer.SetFloat("Master", SaveData.instance.gameMasterVolume);
            audioMixer.SetFloat("Music", SaveData.instance.gameMusicVolume);
            audioMixer.SetFloat("SFX", SaveData.instance.gameSFXVolume);
        }

        masterAudioSlider.value = SaveData.instance.gameMasterVolume;
        musicAudioSlider.value = SaveData.instance.gameMusicVolume;
        SFXAudioSlider.value = SaveData.instance.gameSFXVolume;





        //InvokeRepeating("TestScriptForSlider", 3, 4);

        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            if (audioManager != null)
            {
                audioManager.transform.GetChild(0).GetChild(0).GetComponent<AudioSource>().Play();
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (audioManager != null)
            {
                audioManager.transform.GetChild(0).GetChild(1).GetComponent<AudioSource>().Play();
            }

        }

    }

    private void TestScriptForSlider()
    {
        //settingsText.text = "Slider Value Reset";

    }



}


// Debug.Log("Coooo");
// audioMixer.SetFloat("Master", Mathf.Log10(10) * 20);