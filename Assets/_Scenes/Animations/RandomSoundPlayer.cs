using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
    public AudioClip[] sounds;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomSound()
    {
        if (sounds.Length > 0)
        {
            // Выбираем случайный звук из массива sounds
            AudioClip randomSound = sounds[Random.Range(0, sounds.Length)];

            // Проигрываем выбранный звук
            audioSource.PlayOneShot(randomSound);
        }
        else
        {
            Debug.LogError("Sounds array is empty. Add audio clips to the array.");
        }
    }
}
