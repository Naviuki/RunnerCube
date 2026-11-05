using UnityEngine;

namespace RunnerCube.Player
{
    /// <summary>
    /// Переводит позу игрока в анимации. Аниматор тут только исполнитель.
    /// </summary>
    public sealed class PlayerView : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string runningBool = "isRunning";
        [SerializeField] private string dyingTrigger = "isDying";
        [SerializeField] private string slidingTrigger = "isSliding";
        [SerializeField] private string swallowTrigger = "isSwallow";
        [SerializeField] private string[] jumpStates = new string[0];

        private int _runningHash;
        private int _dyingHash;
        private int _slidingHash;
        private int _swallowHash;

        private void Awake()
        {
            _runningHash = Animator.StringToHash(runningBool);
            _dyingHash = Animator.StringToHash(dyingTrigger);
            _slidingHash = Animator.StringToHash(slidingTrigger);
            _swallowHash = Animator.StringToHash(swallowTrigger);
        }

        public void Run(bool running) => animator.SetBool(_runningHash, running);

        public void Jump()
        {
            animator.SetBool(_runningHash, false);
            if (jumpStates.Length > 0)
                animator.Play(jumpStates[Random.Range(0, jumpStates.Length)], 0, 0f);
        }

        public void Slide() => animator.SetTrigger(_slidingHash);

        public void Swallow() => animator.SetTrigger(_swallowHash);

        public void Die()
        {
            animator.SetBool(_runningHash, false);
            animator.SetTrigger(_dyingHash);
        }
    }
}
