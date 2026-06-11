using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SoundPlayerScript : MonoBehaviour
{
    public GameObject temporarySoundObject;
    private List<GameObject> temporarySoundObjects = new List<GameObject>();
    public void playSoundAtPosition(AudioClip audio, float volume, float pitch, Vector3 position)
    {
        GameObject newTSO = Instantiate(temporarySoundObject);
        AudioSource AS = newTSO.GetComponent<AudioSource>();
        AS.clip = audio;
        newTSO.transform.position = position;
        AS.volume = volume;
        AS.pitch = pitch;
        AS.Play();
        temporarySoundObjects.Add(newTSO);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject TSO in temporarySoundObjects)
        {
            if (TSO.GetComponent<AudioSource>().isPlaying == false)
            {
                temporarySoundObjects.Remove(TSO);
                Destroy(TSO);
            }
        }
    }
}
