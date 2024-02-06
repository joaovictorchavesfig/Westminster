using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class PauseMenu : MonoBehaviour
{
    [Header("Variáveis")]
    public AudioMixer audioMixer;
    public static bool gamePaused = false;
    public static bool noteActive = false;
    public static int deathCounter = 0;
    public static int currentSceneIndex = 2; //inicio do jogo
    private bool isFullscreen;

    [Header("Referências")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject painelOpcoes;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSFX;
    [SerializeField] private TextMeshProUGUI deathCounterText;
    

    private void Start()
    {
        float currentVolume;

        audioMixer.GetFloat("MusicVolume", out currentVolume);
        sliderMusic.value = currentVolume;

        audioMixer.GetFloat("SFXVolume", out currentVolume);
        sliderSFX.value = currentVolume;
    }

    private void Update()
    {
        if(Input.GetButtonDown("Interact"))
        {
            if(noteActive)
            {
                CollectNote.instance.Fechar();
            }
        }

        if(Input.GetButtonDown("Pause"))
        {
            if(noteActive)
            {
                CollectNote.instance.Fechar();
            }
            else
            {
                if(gamePaused)
                {
                    if(painelOpcoes.activeSelf == false)
                    {
                        Resume();
                    }
                    else
                    {
                        FecharOpcoes();
                    }
                }
                else
                {
                    Pause();
                    UpdateDeathCounter();
                }
            }
        }
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        gamePaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        gamePaused = false;
    }

    public void AbrirOpcoes()
    {
        pauseMenu.SetActive(false);
        painelOpcoes.SetActive(true);
    }

    public void FecharOpcoes()
    {
        painelOpcoes.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void Sair()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex; //armazena o numero da cena
        AudioManager.StopStatic(ChangeMusic.currentSong);
        FindObjectOfType<PlayerSceneManagement>().clearInstance();
        FindObjectOfType<LevelLoader>().ChangeSceneFixed(0);
        gamePaused = false;
    }

    public void CartasColetadas()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex; //armazena o numero da cena
        FindObjectOfType<PlayerSceneManagement>().clearInstance();
        FindObjectOfType<LevelLoader>().ChangeSceneFixed(1);
        gamePaused = false;
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    private void UpdateDeathCounter()
    {
        deathCounterText.text = deathCounter.ToString();
    }
}