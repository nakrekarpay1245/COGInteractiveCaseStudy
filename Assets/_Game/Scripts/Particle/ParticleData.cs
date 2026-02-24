using UnityEngine;

namespace _Game.Particle
{
    [System.Serializable]
    public class ParticleData
    {
        [SerializeField] private string _key;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _count = 5;
        [SerializeField] private float _duration = 2f;
        
        public string Key => _key;
        public GameObject Prefab => _prefab;
        public int Count => _count;
        public float Duration => _duration;
    }
}