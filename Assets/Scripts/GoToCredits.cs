using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToCredits : MonoBehaviour
{
    [SerializeField] private IterationController ic;
    
    void Awake()
    {
        Invoke("DisplayCredits", 10.0f);
    }

    private void DisplayCredits()
    {
        ic.GoToCredits();
    }
}
