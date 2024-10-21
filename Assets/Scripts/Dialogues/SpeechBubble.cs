using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] private Animator bubbleAnimator;
    [SerializeField] private Animator exclamationAnimator;
    private bool dialogueDisplayed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bubbleAnimator.SetTrigger("FadeIn");
        if (!GetComponent<Dialogue>().IsLast()) exclamationAnimator.SetTrigger("FadeIn");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!dialogueDisplayed)
        {
            bubbleAnimator.SetTrigger("FadeOut");
            exclamationAnimator.SetTrigger("FadeOut");
        }
        else dialogueDisplayed = false;
    }

    public void RemoveBubble()
    {
        bubbleAnimator.SetTrigger("Remove");
        exclamationAnimator.SetTrigger("Remove");
        dialogueDisplayed = true;
    }
}

