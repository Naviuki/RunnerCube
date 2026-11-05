using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarController : MonoBehaviour
{
    public int scinIndex = 0;
    public GameObject[] avatars;
    

    void Start()
    {
        scinIndex = PlayerPrefs.GetInt("SelectedScin", 0);
        foreach(GameObject sck in avatars)
            sck.SetActive(false);


        avatars[scinIndex].SetActive(true);
    }
}