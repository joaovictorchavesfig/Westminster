using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int firstLevel;
    [SerializeField] private GameObject painelMenuPrincipal;
    [SerializeField] private GameObject painelOpcoes;
    [SerializeField] private GameObject painelTitulo;
    public AudioMixer audioMixer;

    void Start()
    {
        AudioManager.PlayStatic("Menu_Theme");
        ChangeMusic.currentSong = "Menu_Theme";
    }

    public void NovoJogo()
    {
        if(PauseMenu.currentSceneIndex != 2)
        {
            PauseMenu.currentSceneIndex = 2;

            PlayerMovement.firstTimeTravel = false;

            NoteMenu.carta1  = false;
            NoteMenu.carta2  = false;
            NoteMenu.carta3  = false;
            NoteMenu.carta4  = false;
            NoteMenu.carta5  = false;
            NoteMenu.carta6  = false;
            NoteMenu.carta7  = false;
            NoteMenu.carta8  = false;

            Key.pickedKey = false;

            PauseMenu.gamePaused = false;
            PauseMenu.noteActive = false;
            PauseMenu.deathCounter = 0;
        }

        FindObjectOfType<LevelLoader>().ChangeSceneFixed(PauseMenu.currentSceneIndex);
    }

    public void Continuar()
    {
        if(PauseMenu.currentSceneIndex != 2)
        {
            FindObjectOfType<LevelLoader>().ChangeSceneFixed(PauseMenu.currentSceneIndex);
        }
    }

    public void AbrirOpcoes()
    {
        painelMenuPrincipal.SetActive(false);
        painelTitulo.SetActive(false);
        painelOpcoes.SetActive(true);
    }

    public void FecharOpcoes()
    {
        painelOpcoes.SetActive(false);
        painelTitulo.SetActive(true);
        painelMenuPrincipal.SetActive(true);
    }

    public void SairJogo()
    {
        Application.Quit();
    }

    public void Creditos()
    {
        FindObjectOfType<LevelLoader>().ChangeSceneFixed(23);
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
}