using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCsImages : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    private void Awake()
    {
        if (IterationController.numIteration >= sprites.Length) Destroy(gameObject);
        else
        {
            GetComponent<SpriteRenderer>().sprite = sprites[IterationController.numIteration];
        }
    }
}
