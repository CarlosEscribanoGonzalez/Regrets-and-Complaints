using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] music; //Array que guarda todas las canciones/m�sica
    [SerializeField] private Sound[] SFX; //Array que guarda todos los efectos de sonidos/voces
    [SerializeField] private AudioSource musicSource, SFXSource, voiceSource, ambientSource; //Cada tipo de audio tiene su propia fuente de sonido
    [SerializeField] private Scrollbar musicBar, SFXBar; //Permiten regular el volumen de cualquier tipo de audio
    [SerializeField] private GameObject pauseMenu;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (IterationController.numIteration)
        {
            case 0:
                PlayMusic("Song1");
                PlayAmbientSound("AmbientSound");
                break;
            case 4:
                PlayMusic("Song2");
                break;
            case 8:
                PlayMusic("Song3");
                break;
            case 11:
                PlayMusic("Song4");
                break;
            case 13:
                pauseMenu.SetActive(false);
                break;
        }
        StopAllCoroutines();
        StartCoroutine(MusicFadeIn());
        StartCoroutine(AmbientFadeIn());
    }

    IEnumerator MusicFadeIn()
    {
        musicSource.volume = 0;
        float maxMusicVolume = 0.5f - IterationController.numIteration * 0.04f;
        while (musicSource.volume < maxMusicVolume)
        {
            musicSource.volume += 0.2f * Time.deltaTime;
            yield return new WaitForSeconds(0.001f);
        }
    }

    IEnumerator AmbientFadeIn()
    {
        ambientSource.volume = 0;
        float maxMusicVolume = IterationController.numIteration * 0.04f;
        while (ambientSource.volume < maxMusicVolume)
        {
            ambientSource.volume += 0.2f * Time.deltaTime;
            yield return new WaitForSeconds(0.001f);
        }
    }

    public void FadeOut()
    {
        StartCoroutine(MusicFadeOut());
        StartCoroutine(AmbientFadeOut());
    }

    IEnumerator MusicFadeOut()
    {
        while (musicSource.volume > 0)
        {
            musicSource.volume -= 0.3f * Time.deltaTime;
            yield return new WaitForSeconds(0.001f);
        }
    }
    
    IEnumerator AmbientFadeOut()
    {
        while (ambientSource.volume > 0)
        {
            ambientSource.volume -= 0.3f * Time.deltaTime;
            yield return new WaitForSeconds(0.001f);
        }
    }



    public void PlayMusic(string name)
    {
        Sound m = Array.Find(music, x => x.name == name);
        if (m == null) Debug.Log("M�sica no encontrada");
        else
        {
            Debug.Log("Musica encontrada");
            musicSource.clip = m.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(SFX, x => x.name == name);
        if (s == null) Debug.Log("Audio no encontrado");
        else
        {
            SFXSource.PlayOneShot(s.clip);
        }
    }

    public void PlayVoice(string name)
    {
        Sound v = Array.Find(SFX, x => x.name == name);
        if (v == null) Debug.Log("Audio no encontrado");
        else
        {
            voiceSource.PlayOneShot(v.clip);
        }
    }

    public void PlayAmbientSound(string name)
    {
        Sound a = Array.Find(music, x => x.name == name);
        if (a == null) Debug.Log("Audio no encontrado");
        else
        {
            ambientSource.clip = a.clip;
            ambientSource.Play();
        }
    }

    public void ChangeMusicVolume() //Hay que tener en cuenta que el volumen es m�s bajo si la pantalla est� pausada
    {
        if (Time.timeScale == 1)
        {
            musicSource.volume = 0.5f - IterationController.numIteration * 0.04f;
            ambientSource.volume = IterationController.numIteration * 0.04f;
        }
        else
        {
            musicSource.volume = musicSource.volume / 3;
            ambientSource.volume = ambientSource.volume / 3; 
        }
    }

    public void ChangeSFXVolume()
    {
        if (Time.timeScale == 1) 
        {
            SFXSource.volume = SFXBar.value;
            voiceSource.volume = SFXBar.value;
        }
        else
        {
            SFXSource.volume = SFXBar.value / 3;
            voiceSource.volume = SFXBar.value / 3;
        }
    }

    public void ChangeVoicePitch(float value)
    {
        voiceSource.pitch = value;
    }

    public void ChangeVoiceVolume(float value)
    {
        if(SFXBar.value != 0) voiceSource.volume = SFXBar.value + value;
    }

    public void ChangePitch(float value) //Altera el pitch de todo para que, cuando el juego est� parado, todo se escuche m�s lento
    {
        musicSource.pitch = value;
        ambientSource.pitch = value;
        SFXSource.pitch = value;
    }
}
