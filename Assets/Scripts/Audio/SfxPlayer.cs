using UnityEngine;
using RunnerCube.Core;

namespace RunnerCube.Audio
{
    /// <summary>
    /// Все звуки игры из одного места. Никакой игровой логики, только воспроизведение.
    /// </summary>
    public sealed class SfxPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource source;
        [SerializeField] private AudioClip coin;
        [SerializeField] private AudioClip death;
        [SerializeField] private AudioClip shieldOn;
        [SerializeField] private AudioClip shieldBreak;
        [SerializeField] private AudioClip star;
        [SerializeField] private AudioClip dumbbell;

        public void PlayCoin() => Play(coin);
        public void PlayDeath() => Play(death);
        public void PlayShieldOn() => Play(shieldOn);
        public void PlayShieldBreak() => Play(shieldBreak);
        public void PlayDumbbell() => Play(dumbbell);

        public void PlayStar() => Play(star);

        private void Play(AudioClip clip)
        {
            if (clip != null)
                source.PlayOneShot(clip);
        }
    }
}
