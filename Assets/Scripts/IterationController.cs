using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Linq.Expressions;
using static UnityEngine.Rendering.DebugUI;

public class IterationController : MonoBehaviour
{
    public static int numIteration = 0;
    [Header("Room transition:")]
    [SerializeField] private Animator panelAnim;
    [Header("Audio variations:")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private float pitchVariation;
    [SerializeField] private float volumeVariation;
    [SerializeField] private float distortionVariation;
    [SerializeField] private float cutoffVariation;
    [Header("Player variations:")]
    private PlayerController player;
    [SerializeField] private float speedVariation;
    private AudioManager audioManager;

    private void Awake()
    {
        GameObject go = GameObject.FindWithTag("Player");
        if(go != null) player = go.GetComponent<PlayerController>();
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        if(numIteration == 0)
        {
            mixer.SetFloat("MasterPitch", 1);
            mixer.SetFloat("MasterVolume", 0);
            mixer.SetFloat("DistortionLevel", 0.5f);
            mixer.SetFloat("CutoffFreq", 5000);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(ChangeRoom());
    }

    public void GoToCredits()
    {
        StartCoroutine(ChangeRoom());
    }

    IEnumerator ChangeRoom()
    {
        if (player != null) player.stopMovement = true;
        panelAnim.SetBool("Fade", true);
        if(audioManager != null) audioManager.FadeOut();
        yield return new WaitForSeconds(2.0f);
        numIteration++;
        AgePlayer();
        LoadScene();
    }

    private void LoadScene()
    {
        if(numIteration < 12) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void AgePlayer()
    {
        AddAudioDistortion();
        if(player != null) player.DecreaseSpeed(speedVariation);
    }

    private void AddAudioDistortion()
    {
        if(numIteration == 12)
        {
            volumeVariation = 10.0f;
        }
        float value;
        mixer.GetFloat("MasterPitch", out value);
        mixer.SetFloat("MasterPitch", value - pitchVariation);
        mixer.GetFloat("MasterVolume", out value);
        mixer.SetFloat("MasterVolume", value - volumeVariation);
        mixer.GetFloat("DistortionLevel", out value);
        mixer.SetFloat("DistortionLevel", value + distortionVariation);
        mixer.GetFloat("CutoffFreq", out value);
        mixer.SetFloat("CutoffFreq", value - cutoffVariation);
    }
}
