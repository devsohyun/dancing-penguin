using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer : MonoBehaviour
{
    public int band;
    public float startScale, scaleMultiplier;
    public bool useBuffer;
    public GameObject penguin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (useBuffer)  {
                penguin.transform.localScale = new Vector3(penguin.transform.localScale.x, (MicrophoneInput.bandBuffer [band] * scaleMultiplier) + startScale, penguin.transform.localScale.z);   
            }

            if (!useBuffer)  {
                penguin.transform.localScale = new Vector3(penguin.transform.localScale.x, (MicrophoneInput.freqBand [band] * scaleMultiplier) + startScale, penguin.transform.localScale.z);   
                //penguin.transform.localScale = new Vector3(2.0f,  bandBuffer * Mathf.Abs(Mathf.Log(spectrum[spectrumIndex]))*scaleFactor, 1.0f);
            }
        
    }
}
