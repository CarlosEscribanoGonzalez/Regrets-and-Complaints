using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Dialogue[] dialogues;
    private void Awake()
    {
        for (int i = 0; i < dialogues.Length; i++)
        {
            if (i != IterationController.numIteration) dialogues[i]?.gameObject.SetActive(false);
        }
    }
}
