using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    private PlayerController player;
    public bool gamePaused = false;
    public bool canPause = true;
    private static SettingsController instance; 

    [Header("Sections:")]
    [SerializeField] private GameObject pauseButtons; //Los tres botones de pausa (continuar, salir y ajustes)
    [SerializeField] private GameObject settings; //Las opciones de ajustes (v�deo/audio y controles)
    [SerializeField] private GameObject audioVideo; //Brillo y volumen
    [SerializeField] private GameObject controls; //Foto con los controles del juego

    [Header("Audio:")]
    [SerializeField] private AudioManager audioManager;

    [Header("Video:")]
    [SerializeField] private Scrollbar brightnessBar; //Permite regular el brillo
    [SerializeField] private Image panel; //Panel cuyo alpha se altera para cambiar el brillo del juego
    
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Awake() //Permite al objeto persistir entre escenas
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            TogglePause();
        }
    }

    public void TogglePause() //Al pausar el juego, el tiempo se ralentiza, junto con cualquier audio (los cuales bajan su intensidad). El rat�n se muestra
    {
        if(Time.timeScale == 1)
        {
            Time.timeScale = 0.25f;
            Cursor.lockState = CursorLockMode.None;
            gamePaused = true;
            pauseButtons.SetActive(true);
            audioManager.ChangePitch(0.5f);
            audioManager.ChangeMusicVolume();
            audioManager.ChangeSFXVolume();
            player.stopMovement = true;
        }
        else //Todo vuelve a la normalidad si se presiona ESC estando ya pausado el juego
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            gamePaused = false;
            pauseButtons.SetActive(false);
            settings.SetActive(false);
            audioManager.ChangePitch(1.0f);
            audioManager.ChangeMusicVolume(); //El volumen de la m�sica y efectos, al igual que el tiempo, vuelven a la normalidad
            audioManager.ChangeSFXVolume();
            player.stopMovement = false;
        }
    }

    public void ToggleSettings() //Habilita las opciones de audio/v�deo y controles
    {
        pauseButtons.SetActive(!pauseButtons.activeSelf);
        settings.SetActive(!settings.activeSelf);
        audioVideo.SetActive(false);
        controls.SetActive(false);
    }

    public void ToggleAudioVideo() //Despliega los scrollbars de brillo y volumen
    {
        audioVideo.SetActive(true);
        controls.SetActive(false);
    }

    public void ToggleControls() //Despliega una foto de los controles
    {
        controls.SetActive(true);
        audioVideo.SetActive(false);
    }

    public void ChangeBrightness() //Altera el brillo del juego
    {
        panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, 1 - Mathf.Clamp(brightnessBar.value, 0.1f, 1));
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
