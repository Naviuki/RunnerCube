using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public AudioSource _ButtonSound;

    public void PlayButtonSound()
    {
        _ButtonSound.Play();
    }
}
