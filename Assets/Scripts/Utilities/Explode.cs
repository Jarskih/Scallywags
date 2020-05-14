﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace ScallyWags
{
    public class Explode : MonoBehaviour
    {
        public GameObject particles;
        public SimpleAudioEvent _audio;
        private AudioSourcePoolManager _audioPool;
        private float _hitForce = 20f;
        [SerializeField] private GameObject splashEffect;

        private void Start()
        {
            _audioPool = FindObjectOfType<AudioSourcePoolManager>();
        }

        private void OnCollisionEnter(Collision other)
        {
            var damageable = other.gameObject.GetComponent<IDamageable>();
            damageable?.TakeDamage(transform.position, _hitForce);

            var ship = other.gameObject.GetComponentInParent<ShipCondition>();

            if (ship == null) return;

            if (ship.ShipType == ShipType.Enemy)
            {
                ship.TakeDamage();
            }

            var particleSystem = Instantiate(particles, transform.position, Quaternion.identity);
            if (particleSystem == null)
            {
                Debug.LogError("Missing particle system prefab");
            }
            else
            {
                var systems = particleSystem.GetComponentsInChildren<ParticleSystem>();
                foreach (var s in systems)
                {
                    s.Play();
                }
            }
            _audioPool.PlayAudioEvent(_audio, transform.position);
            
            Destroy(gameObject);
        }
    }
}