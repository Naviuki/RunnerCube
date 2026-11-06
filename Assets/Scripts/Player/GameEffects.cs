using UnityEngine;

namespace RunnerCube.Player
{
    /// <summary>
    /// Проигрывает короткие эффекты изготовления частиц в мире.
    /// </summary>
    public sealed class GameEffects : MonoBehaviour
    {
        private const float Lifetime = 2f;

        [SerializeField] private GameObject coinEffect;

        public void Play(GameObject effect, Vector3 position)
        {
            if (effect == null)
                return;

            GameObject instance = Instantiate(effect, position, Quaternion.identity);
            Destroy(instance, Lifetime);
        }

        public void PlayCoin(Vector3 position) => Play(coinEffect, position);
    }
}
