using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent (typeof(AudioSource))]
public class MicrophoneInput : MonoBehaviour
{
    [Header("Audio")]
    AudioSource audioSource;
    
    //Microphone Input
    public AudioClip audioClip; 
    public bool useMicrophone;
    public string selectedDevice;
    public int audioSampleRate = 44100;
    public string microphone;
    public AudioMixerGroup mixerGroupMicrophone, mixerGroupMaster;   

    [Header("Buffer")]
    public float[] freqBand = new float[64];
    public float[] bandBuffer = new float[64];
    private float[] bufferDecrease = new float[64]; 

    [Header("Settings")]
    public float bufferDecreaseValue = 0.005f;
    public float bufferDecreaseMultiplier = 1.05f;
    

    // Start is called before the first frame update
    void Start()
    {
        //get component I need
        audioSource = GetComponent<AudioSource> ();
        
        //get all available microphones
        foreach (string device in Microphone.devices) {
            if (microphone == null) {
                //set defalt mic to first mic found
                microphone = device;
            }
        }

        //find devices microphone name 
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }

        //when microphone gets sounds
        if(useMicrophone)
        {
            if (Microphone.devices.Length > 0 )
            {
                Debug.Log ("microphoneInput");
                audioSource.outputAudioMixerGroup = mixerGroupMicrophone; //when sounds makes no eco from me
                //get microphone at auidoClip
                audioSource.clip = Microphone.Start(Microphone.devices[0].ToString(), true, 10, audioSampleRate);
            }
            else
            {
                useMicrophone = false;
            }
        }
        //when no sound, music starts again            
        if (!useMicrophone)
        {
            audioSource.clip = audioClip;
            audioSource.outputAudioMixerGroup = mixerGroupMaster; //when sounds makes no eco from me
        }
        audioSource.Play();
    }

    void FixedUpdate()
    {
        getAudioData();
        BandBuffer();
    }

    void getAudioData(){
        if(audioSource != null){
            audioSource.GetSpectrumData(freqBand, 0, FFTWindow.Rectangular);
            for (int i = 1; i < freqBand.Length - 1; i++)
            {
                // Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
                // Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
                // Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
                // Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.blue);
            }
        }
    }

    void BandBuffer()
    {
        for (int index = 0; index < 64; ++index) {
            // if value is higher than buffer value, increase buffer value
            if (freqBand [index] > bandBuffer [index]){
                bandBuffer [index] = freqBand [index];
                bufferDecrease [index] = bufferDecreaseValue;
            }
            // if value if lower than buffer value, decrease bufer value by bufferDecrease value.
            if (freqBand [index] < bandBuffer [index]){
                bandBuffer [index] -= bufferDecrease [index];
                bufferDecrease [index] *= bufferDecreaseMultiplier;
            }
        }
    }
}
