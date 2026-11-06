using UnityEngine;
using RunnerCube.Core;

namespace RunnerCube.UI
{
    /// <summary>
    /// HUD забега: счёт, монеты и панели. Никакой логики, только реакция на события.
    /// </summary>
    public sealed class GameHud : MonoBehaviour
    {
        [SerializeField] private NumberLabel scoreLabel;
        [SerializeField] private NumberLabel coinsLabel;
        [SerializeField] private NumberLabel recordLabel;
        [SerializeField] private PanelToggle pausePanel;
        [SerializeField] private PanelToggle losePanel;
        [SerializeField] private PanelToggle pauseButton;

        private RunSession _session;
        private GameEvents _events;

        private void Start()
        {
            _session = SceneServices.Instance.Session;
            _events = SceneServices.Instance.Events;
            _events.StateChanged += OnState;
            _events.ScoreChanged += OnScore;
            _events.CoinsChanged += OnCoins;

            OnScore(_session.Score);
            OnCoins(0, _session.Progress.Coins);
            OnState(_session.State);
        }

        private void OnDestroy()
        {
            if (_events != null)
            {
                _events.StateChanged -= OnState;
                _events.ScoreChanged -= OnScore;
                _events.CoinsChanged -= OnCoins;
            }
        }

        private void OnState(GameState state)
        {
            if (pausePanel != null)
                pausePanel.Show(state == GameState.Paused);

            if (losePanel != null)
                losePanel.Show(state == GameState.GameOver);

            if (pauseButton != null)
                pauseButton.Show(state == GameState.Playing);

            if (state == GameState.GameOver && recordLabel != null)
                recordLabel.Set(_session.Progress.Record);
        }

        private void OnScore(int score)
        {
            if (scoreLabel != null)
                scoreLabel.Set(score);
        }

        private void OnCoins(int runCoins, int total)
        {
            if (coinsLabel != null)
                coinsLabel.Set(runCoins);
        }
    }
}
