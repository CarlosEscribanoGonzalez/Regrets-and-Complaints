using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class Dialogue : MonoBehaviour
{
    [Header("Texto")]
    [SerializeField] private TextMeshProUGUI dialogueText; //GameObject de texto en el que se produce el di�logo
    [SerializeField] private TextMeshProUGUI dialogueName; //GameObject de texto en el que se produce el di�logo
    [SerializeField] private Line[] dialogueLines; //Array que guarda todas las l�enas de un mismo di�logo con sus propiedades
    private Line currentLine; //L�nea actual del di�logo a ser mostrada
    private string currentText = ""; //Fragmento de texto que est� siendo mostrado de la l�nea actual
    private bool canSkip = false; //Indica si se puede pasar a la siguiente l�nea (es decir, ya se ha mostrado la l�nea actual completa)
    private int index = 0; //�ndice de l�nea de di�logo actual
    private static bool dialogueActive; //Indica si el di�logo ha empezado o no
    private bool canInteract = false; //Indica si el jugador est� en �rea de comenzar el di�logo
    private bool dialogoTerminado = false; //Indica si un dialogo ha terminado
    [SerializeField] private bool dialogoRepetible = true; //Indica si un dialogo es repetible
    private bool dialogoActivable = true; //Indica si un dialogo puede activarse
    private bool lastDisplayed = false; //Indica si el último diálogo ha sido reproducido y el personaje no tiene nada nuevo que decir

    [Header("UI")]
    [SerializeField] private Image textBackground; //Imagen de fondo para que se lea bien el texto
    private GameObject previousSpeaker; //Almacena qui�n fue el �ltimo personaje que habl� para retirarlo de la escena en caso de que deje de hablar

    [Header("Varios")]
    [SerializeField] private GameObject nextDialogue; //Algunos di�logos hacen que se activen otros di�logos
    private AudioManager audioManager; //Fuente de audio para reproducir la voz de los personajes
    private PlayerController player;
    private SettingsController settingsController; //Cuando el juego est� pausado el tiempo se ralentiza. Hay que hacer que los di�logos sean m�s lentos
    private Coroutine _startDialogue; //Corrutina de inicio de dialogo

    void Awake()
    {
        currentLine = dialogueLines[0];
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        settingsController = GameObject.Find("PauseMenu").GetComponent<SettingsController>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        dialogueActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canInteract && !dialogueActive && dialogoActivable)
        {
            dialogoActivable = false;
            if(_startDialogue == null) 
                _startDialogue = StartCoroutine(StartDialogue());
            else
            {
                StopCoroutine(_startDialogue);
                _startDialogue = StartCoroutine(StartDialogue());
            }
        }

        CheckInput();
    }

    IEnumerator StartDialogue()
    {
        player.stopMovement = true;
        GetComponent<SpeechBubble>().RemoveBubble(); //Se quita el cuadro de diálogo si se habla
        yield return new WaitForSeconds(0.2f);
        canSkip = false;
        if (!dialogueActive)
        {
            dialogueActive = true;
            textBackground.GetComponent<Animator>().SetBool("Shown", true);
            DisplayText();
            Debug.Log("Empieza");
        }
    }

    private void CheckInput()
    {
        if (canSkip && (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.E)) && Time.timeScale == 1) //Si se pulsa el click y el di�logo ya se ha desplegado se salta a la siguiente l�nea
        {
            index++;
            if (index < dialogueLines.Length)
            {
                currentLine = dialogueLines[index];
                canSkip = false;
                DisplayText();
                Debug.Log(index);
            }
            else //Si no hay m�s l�neas se destruye el di�logo 
            {
                StartCoroutine(RemoveDialogue());
                dialogoTerminado = true;
            }
        }
        else if (canInteract && !canSkip && (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.E)) && dialogueActive && !settingsController.gamePaused && Time.timeScale == 1 && !dialogoTerminado) //Si la l�nea se est� reproduciendo por pantalla se puede saltar la animaci�n
        {
            StopAllCoroutines();
            dialogueText.text = "";
            dialogueText.fontSize = currentLine.fontSize;
            dialogueText.text = currentLine.text;
            currentLine.speaker.GetComponent<Animator>().Play("Shown");
            textBackground.GetComponent<Animator>().Play("Shown");
            canSkip = true;
        }
    }

    private void DisplayText()
    {
        StartCoroutine(TypeText());
        if (currentLine.voice != null) StartCoroutine(PlayVoice());
        if (currentLine.speaker != previousSpeaker && previousSpeaker != null) UnshowSpeaker(); //Si el personaje que habla ha cambiado se elimina la imagen del que estaba hablando
        if (currentLine.speaker != null) ShowSpeaker();
        if (dialogueName != null)
        {
            if (currentLine.voice == "Saul") dialogueName.text = "Saúl"; //La tilde no la pilla bien
            else if (currentLine.voice == "Ramon") dialogueName.text = "Ramón";
            else dialogueName.text = currentLine.voice;
        }
    }

    private void ShowSpeaker()
    {
        Animator anim = currentLine.speaker.GetComponent<Animator>();
        anim.SetBool("Shown", true);
        previousSpeaker = currentLine.speaker;
    }

    private void UnshowSpeaker()
    {
        Animator anim = previousSpeaker.GetComponent<Animator>();
        anim.SetBool("Shown", false);
    }

    private void OnTriggerEnter2D(Collider2D collider) //La conversaci�n empieza al entrar en contacto con el collider
    {
        canInteract = true;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        canInteract = false;
    }

    IEnumerator RemoveDialogue()
    {
        player.stopMovement = false;
        textBackground.GetComponent<Animator>().SetBool("Shown", false);
        UnshowSpeaker();
        dialogueText.text = ""; //El texto se elimina antes que el resto para coincidir con la animaci�n de retire de cuadro de texto
        yield return new WaitForSeconds(0.5f); //Se deja tiempo para las animaciones
        if (nextDialogue != null)
        {
            nextDialogue.SetActive(true);
            this.gameObject.SetActive(false); //Se desactiva el di�logo, pero no se elimina
        }
        else lastDisplayed = true;
        dialogueActive = false;
        if (dialogoRepetible)
        {
            canSkip = false;
            index = 0;
            currentLine = dialogueLines[index];
            dialogoTerminado = false;
            dialogoActivable = true;
            if (_startDialogue != null)
            {
                StopCoroutine(_startDialogue);
                _startDialogue = null;
            }
        }
    }

    IEnumerator TypeText()
    {
        yield return new WaitForSeconds(0.1f); //Peque�o delay para que el texto y el audio se reproduzcan una vez ha aparecido al imagen del que habla
        dialogueText.fontSize = currentLine.fontSize;
        for (int i = 0; i <= currentLine.text.Length; i++)
        {
            currentText = currentLine.text.Substring(0, i);
            dialogueText.text = currentText;
            yield return new WaitForSeconds(currentLine.typingDelay);
        }
        canSkip = true;
    }

    IEnumerator PlayVoice()
    {
        yield return new WaitForSeconds(0.1f); //Peque�o delay para que el texto y el audio se reproduzcan una vez ha aparecido al imagen del que habla
        while (!canSkip)
        {
            audioManager.PlayVoice(currentLine.voice);
            yield return new WaitForSeconds(currentLine.speakingDelay);
            float newPitch = Random.Range(0.75f, 1.2f);
            float newVolume = Random.Range(0.0f, 0.13f);

            if (settingsController.gamePaused) //Cuando el juego se pausa los personajes hablan m�s lento, pues el tiempo sigue pasando
            {
                newPitch -= 0.5f;
                newVolume = Mathf.Clamp(newVolume - 0.7f, 0.01f, 1);
            }

            audioManager.ChangeVoicePitch(newPitch);
            audioManager.ChangeVoiceVolume(newVolume);
        }
    }

    public bool IsLast()
    {
        return lastDisplayed;
    }
}
