using UnityEngine;

namespace _Game.SaveSystem
{
    public class SaveManager
    {
        private const string SAVE_KEY = "GameSaveData";

        public static void Save(int levelIndex)
        {
            SaveData saveData = new SaveData(levelIndex);

            string json = JsonUtility.ToJson(saveData);
            PlayerPrefs.SetString(SAVE_KEY, json);
            PlayerPrefs.Save();
        }

        public static int Load()
        {
            string json = PlayerPrefs.GetString(SAVE_KEY, string.Empty);
            
            if (string.IsNullOrEmpty(json))
            {
                return 0;
            }

            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            return saveData.CurrentLevelIndex;
        }
    }
}
