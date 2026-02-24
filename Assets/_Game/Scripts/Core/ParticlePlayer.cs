using UnityEngine;
using System.Collections;

namespace RKNTemplate.Core
{
    public class ParticlePlayer : MonoBehaviour
    {
        private ParticleManager _particleManager;
        private string _key;
        private float _defaultDuration;
        private Coroutine _returnCoroutine;
        
        public string Key => _key;
        
        public void Initialize(ParticleManager particleManager, string key, float defaultDuration)
        {
            _particleManager = particleManager;
            _key = key;
            _defaultDuration = defaultDuration;
        }
        
        public void Play(Vector3 position, Quaternion rotation, Transform parent = null, float customDuration = -1f)
        {
            transform.position = position;
            transform.rotation = rotation;
            transform.SetParent(parent);
            
            gameObject.SetActive(true);
            
            float duration = customDuration > 0 ? customDuration : _defaultDuration;
            
            if (_returnCoroutine != null)
                StopCoroutine(_returnCoroutine);
                
            _returnCoroutine = StartCoroutine(ReturnToPoolAfterDelay(duration));
        }
        
        private IEnumerator ReturnToPoolAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            _particleManager.ReturnToPool(this);
        }
        
        public void ResetParticle()
        {
            if (_returnCoroutine != null)
            {
                StopCoroutine(_returnCoroutine);
                _returnCoroutine = null;
            }
            
            transform.SetParent(_particleManager.transform);
            gameObject.SetActive(false);
        }
    }
}