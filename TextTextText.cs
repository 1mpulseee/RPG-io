using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTextText : MonoBehaviour
{
    public ParticleSystem ParticleSystem;
    public AudioSource AudioSource;
    public GameObject[] gameObjects;
    private int Count;

    public void CallPS()
    {
        ParticleSystem.Play();
        AudioSource.Play();
    }
}
