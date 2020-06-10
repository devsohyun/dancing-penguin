using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLight : MonoBehaviour
{
    public float maxIntensity = 100;
    public float minIntensity = 10;
    public float multiplier = 3;
    public bool useBuffer = true;
    public int targetIndex = 14;


    public MicrophoneInput _MicrophoneInput;
    public GameObject spotLight;
    public float[] freqBand = new float[64];
    public float[] bandBuffer = new float[64];



    // Update is called once per frame
    void FixedUpdate()
    {
        freqBand = _MicrophoneInput.freqBand;
        bandBuffer = _MicrophoneInput.bandBuffer;

        float result;
        if(useBuffer){
            result = Mathf.Abs(bandBuffer[targetIndex]) * multiplier * (maxIntensity - minIntensity) + minIntensity;
        }else{
            result = Mathf.Abs(freqBand[targetIndex]) * multiplier * (maxIntensity - minIntensity) + minIntensity;
        }
        spotLight.GetComponent<Light>().intensity = result;
}
}
