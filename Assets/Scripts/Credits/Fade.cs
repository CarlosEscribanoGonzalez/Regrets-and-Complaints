using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    private bool fading = false;
    public Animator fade;

    private void Awake()
    {
        Invoke("desactivarFade", 2.0f);
    }

    public void volverAlMenu()
    {
        Debug.Log("Pulsado");
        if (!fading)
        {
            fading = true;
            gameObject.SetActive(true);
            fade?.Play("FadeOut");
            Invoke("cambiarEscena", 2.0f);
        }
    }

    private void desactivarFade()
    {
        gameObject.SetActive(false);
    }

    private void cambiarEscena()
    {
        SceneManager.LoadScene(0);
    }
}
