using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BMGcontroller : MonoBehaviour
{
    public static BMGcontroller Instance { get; private set; }
    private AudioSource audioSource;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }

    public void StartBGM()
    {
        audioSource.Play();
    }
}
