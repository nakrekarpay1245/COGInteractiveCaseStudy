using UnityEngine;
using System.Collections;

namespace _Game.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        private AudioSource _audioSource;
        private AudioManager _audioManager;
        private string _key;
        
        public bool IsPlaying => _audioSource.isPlaying;
        public string Key => _key;
        
        private void Awake()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        public void Initialize(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }
        
        public void Play(AudioClip clip, string key, float volume = 1f, float pitch = 1f)
        {
            _key = key;
            _audioSource.clip = clip;
            _audioSource.volume = volume;
            _audioSource.pitch = pitch;
            _audioSource.Play();
            StartCoroutine(ReturnToPoolWhenFinished());
        }
        
        private IEnumerator ReturnToPoolWhenFinished()
        {
            yield return new WaitWhile(() => _audioSource.isPlaying);
            _audioManager.ReturnToPool(this);
        }
    }
}