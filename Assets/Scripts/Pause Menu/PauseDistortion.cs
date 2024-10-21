using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseDistortion : MonoBehaviour
{
    [SerializeField] private Image distortion; 
    [SerializeField] private SettingsController settingsController; 
    public float minAlpha, maxAlpha, variationSpeed, delay;
    private bool canVariate = true;

    // Update is called once per frame
    void Update()
    {
        EnableDistortion();
        if (canVariate) StartVariation();
    }

    private void EnableDistortion()
    {
        if (settingsController.gamePaused) distortion.gameObject.SetActive(true);
        else distortion.gameObject.SetActive(false);
    }

    private void StartVariation()
    {
        canVariate = false;
        float newIntensity = Random.Range(minAlpha, maxAlpha);
        StartCoroutine(VariateIntensity(newIntensity));
    }

    IEnumerator VariateIntensity(float value)
    {
        if(distortion.color.a > value)
        {
            while(distortion.color.a > value)
            {
                distortion.color = new Color(distortion.color.r, distortion.color.g, distortion.color.b, distortion.color.a - (variationSpeed*Time.deltaTime));
                yield return new WaitForSeconds(delay);
            }
        }
        else
        {
            while (distortion.color.a < value)
            {
                distortion.color = new Color(distortion.color.r, distortion.color.g, distortion.color.b, distortion.color.a + (variationSpeed*Time.deltaTime));
                yield return new WaitForSeconds(delay);
            }
        }
        yield return new WaitForSeconds(delay);
        canVariate = true;
    }
}
