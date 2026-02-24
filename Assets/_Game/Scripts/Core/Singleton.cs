using UnityEngine;

namespace _Game.Core
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                _instance = FindFirstObjectByType<T>();

                if (_instance != null)
                    return _instance;

                Debug.LogError($"An instance of {typeof(T)} is needed in the scene, but there is none.");
                return null;
            }
            private set { _instance = value; }
        }
    }
}