using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicOnAllScenes : MonoBehaviour
{
    public GameObject BGMusic1;
    private AudioSource audioSrc1;
    public GameObject[] _objects;

    void Awake()
    {
        _objects = GameObject.FindGameObjectsWithTag("Sound");
        if (_objects.Length == 0)
        {
            BGMusic1 = Instantiate(BGMusic1);
            BGMusic1.name = "BGMusic1";
            DontDestroyOnLoad(BGMusic1.gameObject);
        }
        else
        {
            BGMusic1 = GameObject.Find("BGMusic1");
        }
    }

    void Start()
    {
        audioSrc1 = BGMusic1.GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }
}
