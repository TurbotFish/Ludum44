using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSound : MonoBehaviour
{
    public List<AudioClip> clips;
    public AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source.clip = clips[Random.Range(0, clips.Count)];
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
