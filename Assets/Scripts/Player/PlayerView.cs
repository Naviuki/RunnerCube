using UnityEngine;

namespace RunnerCube.Player
{
    /// <summary>
    /// Переводит позу игрока в анимации. Без аниматора просто ничего не делает.
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
            if (animator == null)
                animator = GetComponentInChildren<Animator>(true);

            _runningHash = Animator.StringToHash(runningBool);
            _dyingHash = Animator.StringToHash(dyingTrigger);
            _slidingHash = Animator.StringToHash(slidingTrigger);
            _swallowHash = Animator.StringToHash(swallowTrigger);
        }

        public void Run(bool running)
        {
            if (animator != null)
                animator.SetBool(_runningHash, running);
        }

        public void Jump()
        {
            if (animator == null)
                return;

            animator.SetBool(_runningHash, false);
            if (jumpStates != null && jumpStates.Length > 0)
                animator.Play(jumpStates[Random.Range(0, jumpStates.Length)], 0, 0f);
        }

        public void Slide()
        {
            if (animator != null)
                animator.SetTrigger(_slidingHash);
        }

        public void Swallow()
        {
            if (animator != null)
                animator.SetTrigger(_swallowHash);
        }

        public void Die()
        {
            if (animator == null)
                return;

            animator.SetBool(_runningHash, false);
            animator.SetTrigger(_dyingHash);
        }
    }
}
