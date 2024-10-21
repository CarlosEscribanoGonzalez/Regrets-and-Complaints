using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Line
{
    public string text;
    public string voice;
    public GameObject speaker;
    public float typingDelay = 0.05f;
    public float speakingDelay = 0.25f;
    public int fontSize = 20;
}
