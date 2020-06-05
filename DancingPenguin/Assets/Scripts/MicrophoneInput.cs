using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent (typeof(AudioSource))]
public class MicrophoneInput : MonoBehaviour
{
    
    AudioSource audioSource;
    
    //Microphone Input
    public AudioClip audioClip; 
    public bool useMicrophone;
    public string selectedDevice;
    public int audioSampleRate = 44100;
    public string microphone;
    public AudioMixerGroup mixerGroupMicrophone, mixerGroupMaster;   

    //Buffer
    
    public static float[] freqBand = new float[8];
    public static float[] bandBuffer = new float[8];
    private float[] bufferDecrease = new float[8]; 
    public bool useBuffer;
    public int band;


    

    public GameObject box;
    public GameObject penguin;

    [Header("Settings")]
    public float scaleFactor = -2.0f;
    
    public int spectrumIndex = 14;

    

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
                //GetComponent<Renderer>().material.color = Color.red;
            
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

    // Update is called once per frame
    void Update()
    {
        if(audioSource != null){
            
            float[] spectrum = new float[512];

            audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

            for (int i = 1; i < spectrum.Length - 1; i++)
            {
                // Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
                // Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
                // Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
                // Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.blue);
            
            }

            penguin.transform.localScale = new Vector3(2.0f, Mathf.Abs(Mathf.Log(spectrum[spectrumIndex]))*scaleFactor, 2.0f);
            
            /*
            if (useBuffer)  {
                penguin.transform.localScale = new Vector3(2.0f,  bandBuffer [band] * Mathf.Abs(Mathf.Log(spectrum[spectrumIndex]))*scaleFactor, 2.0f);   
            }

            if (!useBuffer)  {
                penguin.transform.localScale = new Vector3(2.0f,  freqBand [band] * Mathf.Abs(Mathf.Log(spectrum[spectrumIndex]))*scaleFactor, 2.0f);   
                //penguin.transform.localScale = new Vector3(2.0f,  bandBuffer * Mathf.Abs(Mathf.Log(spectrum[spectrumIndex]))*scaleFactor, 1.0f);
            }
            */

        }
        
        BandBuffer();

     
        
    }

    void BandBuffer()
    {
        for (int g = 0; g < 8; ++g) {
            if (freqBand [g] > bandBuffer [g]){
                bandBuffer [g] = freqBand [g] ;
                bufferDecrease [g] = 0.005f;
            }
            if (freqBand [g] < bandBuffer [g]){
                bandBuffer [g] -= bufferDecrease [g];
                bufferDecrease [g] *= 1.2f;
            }

        }
    }

    void ChangeColors()
    {
         if (Input.GetKeyDown(KeyCode.R))
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
    }
    
 }
