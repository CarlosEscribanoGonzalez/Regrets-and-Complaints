using UnityEngine;

public class Reset : MonoBehaviour
{
    [SerializeField] private Material material;
    void Awake()
    {
        IterationController.numIteration = 0;
        material.SetFloat("_IntensidadV", 3);
    }
}
