using UnityEngine;

namespace RunnerCube.Data
{
    /// <summary>
    /// Настройки забега. Если ссылка в сцене не задана, значения берутся из полей по умолчанию.
    /// </summary>
    [CreateAssetMenu(fileName = "RunSettings", menuName = "RunnerCube/Run Settings")]
    public sealed class RunSettings : ScriptableObject
    {
        [Header("Скорость")]
        [SerializeField, Min(0f)] private float startSpeed = 35f;
        [SerializeField, Min(0f)] private float maxSpeed = 110f;
        [SerializeField, Min(0f)] private float speedIncreasePerSecond = 1f;
        [SerializeField, Min(0f)] private float dumbbellPenalty = 15f;
        [SerializeField, Min(1f)] private float speedCheatLimit = 1000f;

        [Header("Прыжок и гравитация")]
        [SerializeField] private float jumpForce = 13f;
        [SerializeField] private float gravity = -20f;

        [Header("Полосы")]
        [SerializeField, Min(0.1f)] private float lineWidth = 6.5f;
        [SerializeField, Min(0.1f)] private float sidestepSpeed = 25f;

        [Header("Подкат и смерть")]
        [SerializeField, Min(0.1f)] private float slideDuration = 1f;
        [SerializeField, Min(0f)] private float deathDelay = 0.5f;

        [Header("Бонусы")]
        [SerializeField, Min(0.1f)] private float shieldDuration = 5f;
        [SerializeField, Min(0.1f)] private float starDuration = 5f;

        public float StartSpeed => startSpeed;
        public float MaxSpeed => maxSpeed;
        public float SpeedIncreasePerSecond => speedIncreasePerSecond;
        public float DumbbellPenalty => dumbbellPenalty;
        public float SpeedCheatLimit => speedCheatLimit;
        public float JumpForce => jumpForce;
        public float Gravity => gravity;
        public float LineWidth => lineWidth;
        public float SidestepSpeed => sidestepSpeed;
        public float SlideDuration => slideDuration;
        public float DeathDelay => deathDelay;
        public float ShieldDuration => shieldDuration;
        public float StarDuration => starDuration;

        private static RunSettings _fallback;

        /// <summary>
        /// Настройки по умолчанию для тестов и случаев, когда ассет не подключён.
        /// </summary>
        public static RunSettings Default
        {
            get
            {
                if (_fallback == null)
                    _fallback = CreateInstance<RunSettings>();

                return _fallback;
            }
        }
    }
}
