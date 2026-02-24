using System;

namespace _Game.SaveSystem
{
    [Serializable]
    public class SaveData
    {
        public int CurrentLevelIndex;

        public SaveData(int currentLevelIndex)
        {
            CurrentLevelIndex = currentLevelIndex;
        }
    }
}
