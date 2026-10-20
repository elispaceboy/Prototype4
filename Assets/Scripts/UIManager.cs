using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private PlayerController playerControllerScript;
    private SpawnManager spawnManagerScript;
    public bool gameActive = false;
    public bool paused = true;
    public GameObject titleScreen;
    public GameObject pauseScreen;    
    public GameObject settingsScreen;

    public GameObject gameActiveUI;
    public TextMeshProUGUI currentWaveText;

    public float volume;
    public TextMeshProUGUI volumeText;
    public Scrollbar volumeSlider;
    private AudioSource backgroundMusic;

    public GameObject highscoreBox;
    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI lastscoreText;

    // Start is called before the first frame update
    private void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        spawnManagerScript = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        backgroundMusic = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.5f);
        onVolumeChange();
        setHighscoreText();
    }

    public void onStartPress()
    {
        spawnManagerScript.waveNumber = 1;
        gameStart();
    }

    public void onLoadGamePress()
    {
        spawnManagerScript.waveNumber = PlayerPrefs.GetInt("CurrentWave", 1);
        if (PlayerPrefs.GetInt("ResetSave, 0") == 1)
        {
            spawnManagerScript.waveNumber = 1;
        }
        gameStart();
    }

    public void gameStart()
    {
        gameActive = true;
        paused = false;
        GameObject.Find("Player").transform.position = new Vector3(0, 0.5f, 0);
        spawnManagerScript.gameStart();
        titleScreen.SetActive(false);
        gameActiveUI.SetActive(true);
        highscoreBox.SetActive(false);
        Time.timeScale = 1;
        UpdateScore();
    }

    public void onPausePress()
    {
        paused = true;
        pauseScreen.SetActive(true);
        gameActiveUI.SetActive(false);
        highscoreBox.SetActive(true);
        Time.timeScale = 0;
    }

   public void onResumePress()
   {
        gameActive = true;
        paused = false;
        pauseScreen.SetActive(false);
        gameActiveUI.SetActive(true);
        highscoreBox.SetActive(false);
        Time.timeScale = 1;
    }

    public void onGameExitPress()
    {
        Application.Quit();
    }

    public void onMenuPress()
    {
        gameActive = false;
        paused = true;
        titleScreen.SetActive(true);
        pauseScreen.SetActive(false);
        settingsScreen.SetActive(false);
        highscoreBox.SetActive(true);
        Time.timeScale = 0;
        UpdateScore();
    }

    public void onVolumeChange()
    {
        volume = volumeSlider.value;
        volumeText.text = "Volume: " + (volume*100) +"%";
        backgroundMusic.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void onSettingsPress()
    {
        titleScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }

    public void onSaveGamePress()
    {
        PlayerPrefs.Save();
        
    }


    public void UpdateScore()
    {
        int wave = spawnManagerScript.waveNumber;
        PlayerPrefs.SetInt("CurrentWave", wave);
        if (wave > PlayerPrefs.GetInt("HighestWave", 1))
        {
            PlayerPrefs.SetInt("HighestWave", wave);
        }
        setHighscoreText();
        currentWaveText.text = "Wave " + wave;
    }

    void setHighscoreText()
    {
        highscoreText.text = PlayerPrefs.GetInt("HighestWave", 1)+"";
        lastscoreText.text = PlayerPrefs.GetInt("CurrentWave", 1)+"";
        
    }

    public void resetOnLoad(bool reset)
    {
        if (reset)
        {
            PlayerPrefs.SetInt("ResetSave", 1);
        } else
        {
            PlayerPrefs.SetInt("ResetSave", 0);
        }
    }
}
