using UnityEngine;

namespace _Game.Audio
{
    public enum PlayMode
    {
        Random,
        Sequential
    }
    
    [System.Serializable]
    public class AudioData
    {
        [SerializeField] private string _key;
        [SerializeField] private AudioClip[] _audioClips;
        [SerializeField] private PlayMode _playMode = PlayMode.Random;
        [SerializeField] private int _count = 1;
        [SerializeField] private float _minVolume = 1f;
        [SerializeField] private float _maxVolume = 1f;
        [SerializeField] private float _minPitch = 1f;
        [SerializeField] private float _maxPitch = 1f;
        
        private int _currentIndex = 0;
        
        public string Key => _key;
        public AudioClip[] AudioClips => _audioClips;
        public PlayMode PlayMode => _playMode;
        public int Count => _count;
        public float MinVolume => _minVolume;
        public float MaxVolume => _maxVolume;
        public float MinPitch => _minPitch;
        public float MaxPitch => _maxPitch;
        
        public AudioClip GetNextClip()
        {
            if (_audioClips == null || _audioClips.Length == 0) return null;
            
            AudioClip clip = _playMode == PlayMode.Random 
                ? _audioClips[Random.Range(0, _audioClips.Length)]
                : _audioClips[_currentIndex % _audioClips.Length];
                
            if (_playMode == PlayMode.Sequential)
                _currentIndex++;
                
            return clip;
        }
        
        public float GetRandomVolume()
        {
            return Random.Range(_minVolume, _maxVolume);
        }
        
        public float GetRandomPitch()
        {
            return Random.Range(_minPitch, _maxPitch);
        }
    }
}