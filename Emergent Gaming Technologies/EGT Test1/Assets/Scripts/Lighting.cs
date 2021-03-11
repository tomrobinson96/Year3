using UnityEngine;
using System.Collections;

public enum enColorchannels
{
    all = 0
}
public enum enWaveFunctions
{   
    noise = 5
}

public class Lighting : MonoBehaviour {

    public enColorchannels colorChannel = enColorchannels.all;
    public enWaveFunctions waveFunction = enWaveFunctions.noise;
    public float offset = 0.0f; // constant offset
    public float amplitude = 1.0f; // amplitude of the wave
    public float phase = 0.0f; // start point inside on wave cycle
    public float frequency = 0.5f; // cycle frequency per second
    bool affectsIntensity = true;

    // Keep a copy of the original values
    private Color originalColor;
    private float originalIntensity;


    // Use this for initialization
    void Start()
    {
        originalColor = GetComponent<Light>().color;
        originalIntensity = GetComponent<Light>().intensity;
    }

    // Update is called once per frame
    void Update()
    {
        Light light = GetComponent<Light>();
        if (affectsIntensity)
            light.intensity = originalIntensity * EvalWave();        

        if (colorChannel == enColorchannels.all)
            light.color = originalColor * EvalWave();       
    }

    private float EvalWave()
    {
        float x = (Time.time + phase) * frequency;
        float y;
        x = x - Mathf.Floor(x); // normalized value (0..1)
        
        if (waveFunction == enWaveFunctions.noise)
        {
            y = 1f - (Random.value * 2f);
        }
        else {
            y = 1.0f;

        }
        return (y * amplitude) + offset;

    }

}
