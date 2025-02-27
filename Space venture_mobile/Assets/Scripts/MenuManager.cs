using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using System.Collections;
public class MenuManager : MonoBehaviour
{
    [Header("Button reference's")]
    [SerializeField] private GameObject entryImage;
    [SerializeField] private Slider     entrySlider;
    [SerializeField] private GameObject Button_1;
    [SerializeField] private GameObject Button_2;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject tutorialMenu;
    [SerializeField] private GameObject gameOverMenu;

   

    public static MenuManager instance;

    public TextMeshProUGUI gameOverScoreText;
    private bool isGameStart;
    private bool isStartOver;
    private void Awake()
    {
        if (instance == null)
        {

            if (instance == null)
                instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        
    }
    private void Start()
    {
        FindAnyObjectByType<AudioManager>().Play("StartMenuBackGroundTheme");
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        entryImage.SetActive(true);
        SceneManager.LoadScene(0);
        entrySlider.value = 0;
        isStartOver = false;
    }
    private void Update()
    {
        entrySlider.value += 1 *Time.deltaTime;
        if (!isStartOver)
        {
            StartManager();
        }
    }
    public void StartGame()
    {
        startMenu.SetActive(false);
        if (!isGameStart)
        {
            tutorialMenu.SetActive(true); 
            isGameStart=true;
            return;
        }
       if(isGameStart)
        {
            FindAnyObjectByType<AudioManager>().Play("BackGroundTheme");
            FindAnyObjectByType<AudioManager>().Stop("StartMenuBackGroundTheme");
            //SceneManager.LoadScene(1);
            Time.timeScale = 1f;
            FindAnyObjectByType<AudioManager>().UnMuteSound("BackGroundTheme");//unmuting the Sound because in AudioManager We muted all sound when Mute Button Pressed
        }
       
        
    }
    
    public void PlayGame()
    {

        SceneManager.LoadScene(1);
        tutorialMenu.SetActive(false);
        Time.timeScale = 1f;
        FindAnyObjectByType<AudioManager>().Play("BackGroundTheme");
        FindAnyObjectByType<AudioManager>().Stop("StartMenuBackGroundTheme");

    }
    public void Restart()
    {
        FindAnyObjectByType<AudioManager>().UnPauseAllGameSound();
        FindAnyObjectByType<AudioManager>().Stop("GameOver");
        FindAnyObjectByType<AudioManager>().Play("BackGroundTheme");
        pauseMenu.SetActive(false);
        startMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
    public void Pause()
    {
        //SceneManager.LoadScene(0);
        FindAnyObjectByType<AudioManager>().Play("PauseButtonSound");
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void MainMenu()
    {
        FindAnyObjectByType<AudioManager>().MuteSound("BackGroundTheme");
        FindAnyObjectByType<AudioManager>().Play("StartMenuBackGroundTheme");//Playing the sound
        startMenu.SetActive(true);
        pauseMenu.SetActive(false);
        
        FindAnyObjectByType<AudioManager>().UnMuteSound("StartMenuBackGroundTheme");//unmuting the Sound because in AudioManager We muted all sound when Mute Button Pressed
    }
    public void Quite()
    {
        Application.Quit();
    }
    public void PlaySound()
    {
        Button_1.SetActive(true);
        Button_2.SetActive(false);
        //audioManager.Stop();
        FindAnyObjectByType<AudioManager>().UnPauseAllGameSound();
        FindAnyObjectByType<AudioManager>().Play("StartMenuBackGroundTheme");

    }
    public void MuteSound()
    {
        Button_2.SetActive(true);
        Button_1.SetActive(false);
        FindAnyObjectByType<AudioManager>().PauseAllGameSound();
        FindAnyObjectByType<AudioManager>().Stop("StartMenuBackGroundTheme");

    }
    public void EndGame()
    {
        gameOverMenu.SetActive(true);
        FindAnyObjectByType<AudioManager>().PauseAllGameSound();
    }

    private void StartManager()
    {

        if(entrySlider.value == 1) {
        entryImage.SetActive(false);
        startMenu.SetActive(true);
       
        isGameStart = false;
        Time.timeScale = 0f;
         isStartOver = true;
        }
    }
}
