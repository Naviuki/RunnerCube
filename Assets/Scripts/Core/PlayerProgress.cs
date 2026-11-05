using UnityEngine;

namespace RunnerCube.Core
{
    public static class PlayerPrefsKeys
    {
        public const string Coins = "Coins";
        public const string SelectedSkin = "SelectedScin";
        public const string Language = "language";
        public const string RecordScore = "recordScore";
        public const string LastRunScore = "lastRunScore";
    }

    /// <summary>
    /// Прогресс между запусками: монеты, рекорд, скин и язык.
    /// </summary>
    public sealed class PlayerProgress
    {
        public int Coins
        {
            get => PlayerPrefs.GetInt(PlayerPrefsKeys.Coins, 0);
            set
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.Coins, Mathf.Max(0, value));
                PlayerPrefs.Save();
            }
        }

        public int SelectedSkin
        {
            get => PlayerPrefs.GetInt(PlayerPrefsKeys.SelectedSkin, 0);
            set
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.SelectedSkin, value);
                PlayerPrefs.Save();
            }
        }

        public int Language => PlayerPrefs.GetInt(PlayerPrefsKeys.Language, 0);

        public int Record => PlayerPrefs.GetInt(PlayerPrefsKeys.RecordScore, 0);

        public int LastRun => PlayerPrefs.GetInt(PlayerPrefsKeys.LastRunScore, 0);

        public void SetLanguage(int index)
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.Language, index);
            PlayerPrefs.Save();
        }

        public void AddCoins(int amount) => Coins = Coins + amount;

        public bool TrySpend(int price)
        {
            if (Coins < price)
                return false;

            Coins = Coins - price;
            return true;
        }

        public bool IsSkinUnlocked(string skinName, int price)
        {
            if (price <= 0)
                return true;

            return PlayerPrefs.GetInt(skinName, 0) != 0;
        }

        public void UnlockSkin(string skinName)
        {
            PlayerPrefs.SetInt(skinName, 1);
            PlayerPrefs.Save();
        }

        public void RegisterRun(int score)
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.LastRunScore, score);
            if (score > Record)
                PlayerPrefs.SetInt(PlayerPrefsKeys.RecordScore, score);

            PlayerPrefs.Save();
        }
    }
}
