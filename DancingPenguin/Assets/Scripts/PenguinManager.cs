using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinManager : MonoBehaviour
{
    public MicrophoneInput _MicrophoneInput;
    [Header("Settings")]
    public int howManyPenguins = 64;
    
    public bool useBuffer;
    public float scaleFactor = 2.0f;
    public float minScale = 2.0f;
    
    public int[] targetIndex = new int[64];

    public float surroundingRadius = 100.0f;
    public float rotationSpeed = 0.05f;

    [Header("References")]
    public GameObject penguinPrefab;
    public GameObject penguinContainer;
    GameObject[] penguins = new GameObject[64];
    public float[] freqBand = new float[64];
    public float[] bandBuffer = new float[64];

    void Start()
    {
        InstantiatePenguins();
    }
    
    void InstantiatePenguins(){
        for (int i = 0; i < penguins.Length; i++) {
            GameObject penguinInstance = (GameObject) Instantiate (penguinPrefab);
            penguinInstance.transform.position = penguinContainer.transform.position;
            penguinInstance.transform.parent = penguinContainer.transform;
            penguinInstance.name = "Penguin" + i;
            penguinContainer.transform.eulerAngles = new Vector3 (0, 360/penguins.Length * i, 0);
            penguinInstance.transform.position = new Vector3(penguinContainer.transform.position.x, penguinContainer.transform.position.y, surroundingRadius);
            penguins[i] = penguinInstance;
        }
    }

    void FixedUpdate()
    {
        freqBand = _MicrophoneInput.freqBand;
        bandBuffer = _MicrophoneInput.bandBuffer;

        dancePenguin();

        penguinContainer.transform.localEulerAngles = new Vector3(penguinContainer.transform.localEulerAngles.x, penguinContainer.transform.localEulerAngles.y + rotationSpeed, penguinContainer.transform.localEulerAngles.z);
    }

    void dancePenguin(){
        float result;
        for(int i = 0; i < penguins.Length; i++)
        {
            // Debug.Log("buffer : " + bandBuffer[targetIndex[0]]);
            // Debug.Log("raw : " + freqBand[targetIndex[0]]);
            if(useBuffer){
                result = Mathf.Abs(bandBuffer[i])*scaleFactor + minScale;
            }else{
                result = Mathf.Abs(freqBand[i])*scaleFactor + minScale;
            }
            penguins[i].transform.localScale = new Vector3(1.0f, 1.0f, result);
        }
    }
}
