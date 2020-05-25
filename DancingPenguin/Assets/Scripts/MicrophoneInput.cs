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

    public GameObject box;
    public GameObject penguin;

    [Header("Settings")]
    public float scaleFactor = 2.0f;
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
        //UpdateMicrophone();

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
        }
    
     audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(audioSource != null){
            float[] spectrum = new float[64];

            audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

            for (int i = 1; i < spectrum.Length - 1; i++)
            {
                // Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
               // Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
                // Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
                // Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.blue);
            }
            
            penguin.transform.localScale = new Vector3(4.0f,  Mathf.Abs(Mathf.Log(spectrum[spectrumIndex]))*scaleFactor, 4);
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
