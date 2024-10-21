using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Envejecimiento : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private Material materialSprites;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Camera camera;
    [SerializeField] private Volume volume;
    [SerializeField] private float ritmoDeDesaturacion = 2.0f;
    private Vector3 lastPosition = Vector3.negativeInfinity;

    void Awake()
    {
        if (playerTransform != null && material != null && camera != null)
        {
            lastPosition = playerTransform.position;
            material.SetFloat("_IntensidadV", IterationController.numIteration/12f * 3);
            float saturacion =Mathf.Clamp(1f - (IterationController.numIteration/12f * ritmoDeDesaturacion), 0.0f, 1f) ;
            materialSprites.SetFloat("_Saturacion", saturacion);
        }
        if(volume.profile.TryGet(out ColorAdjustments CA))
        {
            CA.saturation.value = Mathf.Clamp(-IterationController.numIteration/12f * ritmoDeDesaturacion * 100, -100, 0);
        }

        if (IterationController.numIteration > 12)
        {
            material.SetVector("_PosicionDelJugador", -new Vector3(0.5f,0.5f,0.0f));
        }

    }

    void Update()
    {
        if (lastPosition != Vector3.negativeInfinity)
        {
            lastPosition = playerTransform.position;
            Vector3 posicionEnCamara = camera.WorldToScreenPoint(lastPosition);
            Vector3 posicionNormalizada = new Vector3(posicionEnCamara.x/camera.pixelWidth , posicionEnCamara.y/camera.pixelHeight, 0);
            material.SetVector("_PosicionDelJugador", -posicionNormalizada);
        }
    }
}
