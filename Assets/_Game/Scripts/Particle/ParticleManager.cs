using UnityEngine;
using System.Collections.Generic;
using _Game.Core;
using _Game.Utilities;

namespace _Game.Particle
{
    public class ParticleManager : Singleton<ParticleManager>
    {
        [SerializeField] private List<ParticleData> _particleDataList = new List<ParticleData>();

        private Dictionary<string, Queue<ParticlePlayer>> _particlePools = new Dictionary<string, Queue<ParticlePlayer>>();

        private void Start()
        {
            InitializePools();
        }

        private void InitializePools()
        {
            foreach (ParticleData particleData in _particleDataList)
            {
                Queue<ParticlePlayer> pool = new Queue<ParticlePlayer>();

                for (int i = 0; i < particleData.Count; i++)
                {
                    GameObject particleObject = Instantiate(particleData.Prefab, transform);
                    particleObject.name = $"{particleData.Key}_{i}";

                    ParticlePlayer player = particleObject.GetComponent<ParticlePlayer>();
                    if (player == null)
                        player = particleObject.AddComponent<ParticlePlayer>();

                    player.Initialize(this, particleData.Key, particleData.Duration);
                    particleObject.SetActive(false);

                    pool.Enqueue(player);
                }

                _particlePools[particleData.Key] = pool;
            }
        }

        public void Spawn(string key, Vector3 position)
        {
            Spawn(key, position, Quaternion.identity);
        }

        public void Spawn(string key, Vector3 position, Quaternion rotation)
        {
            Spawn(key, position, rotation, null);
        }

        public void Spawn(string key, Vector3 position, Transform parent)
        {
            Spawn(key, position, Quaternion.identity, parent);
        }

        public void Spawn(string key, Vector3 position, Quaternion rotation, Transform parent, float customDuration = -1f)
        {
            if (_particlePools.TryGetValue(key, out Queue<ParticlePlayer> pool) && pool.Count > 0)
            {
                ParticlePlayer player = pool.Dequeue();
                player.Play(position, rotation, parent, customDuration);
            }
        }

        public ParticlePlayer SpawnWithTexture(string key, Vector3 position, Texture texture)
        {
            if (_particlePools.TryGetValue(key, out Queue<ParticlePlayer> pool) && pool.Count > 0)
            {
                // RichLogger.Log($"Spawning particle with texture for key: {key}");
                ParticlePlayer player = pool.Dequeue();
                player.SetTexture(texture);
                player.Play(position, Quaternion.identity, null);
                return player;
            }
            return null;
        }

        public void ReturnToPool(ParticlePlayer player)
        {
            if (_particlePools.TryGetValue(player.Key, out Queue<ParticlePlayer> pool))
            {
                player.ResetParticle();
                pool.Enqueue(player);
            }
        }
    }
}