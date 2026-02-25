using UnityEngine;
using System.Collections.Generic;
using _Game.Core;
using TriInspector;

namespace _Game.Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private List<AudioData> _audioDataList = new List<AudioData>();

        private Dictionary<string, Queue<AudioPlayer>> _audioPools = new Dictionary<string, Queue<AudioPlayer>>();
        private Dictionary<string, AudioData> _audioDataMap = new Dictionary<string, AudioData>();
        [SerializeField, ReadOnly] private bool _isMuted;

        private void Start()
        {
            InitializePools();
        }

        private void InitializePools()
        {
            foreach (AudioData audioData in _audioDataList)
            {
                Queue<AudioPlayer> pool = new Queue<AudioPlayer>();

                for (int i = 0; i < audioData.Count; i++)
                {
                    GameObject playerObject = new GameObject($"AudioPlayer_{audioData.Key}_{i}");
                    playerObject.transform.SetParent(transform);

                    AudioPlayer player = playerObject.AddComponent<AudioPlayer>();
                    player.Initialize(this);

                    pool.Enqueue(player);
                }

                _audioPools[audioData.Key] = pool;
                _audioDataMap[audioData.Key] = audioData;
            }
        }

        public void PlayAudio(string key)
        {
            if (_isMuted)
                return;

            if (_audioPools.TryGetValue(key, out Queue<AudioPlayer> pool) && pool.Count > 0 && _audioDataMap.TryGetValue(key, out AudioData audioData))
            {
                AudioClip clip = audioData.GetNextClip();
                if (clip != null)
                {
                    AudioPlayer player = pool.Dequeue();
                    float volume = audioData.GetRandomVolume();
                    float pitch = audioData.GetRandomPitch();
                    player.Play(clip, key, volume, pitch);
                }
            }
        }

        public void ReturnToPool(AudioPlayer player)
        {
            if (_audioPools.TryGetValue(player.Key, out Queue<AudioPlayer> pool))
            {
                pool.Enqueue(player);
            }
        }

        public void Mute()
        {
            _isMuted = true;
            AudioListener.volume = 0f;
        }

        public void UnMute()
        {
            _isMuted = false;
            AudioListener.volume = 1f;
        }
    }
}