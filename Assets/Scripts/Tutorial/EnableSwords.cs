﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{
    public class EnableSwords : MonoBehaviour
    {
        private Transform[] _swords;
        void Start()
        {
            _swords = GetComponentsInChildren<Transform>();
            
            foreach (var c in _swords)
            {
                if (c == transform) continue;
                c.gameObject.SetActive(false);
            }
        }
    
        private void OnEnable()
        {
            EventManager.StartListening("Intro3", EnableGameObjects);
        }
    
        private void OnDisable()
        {
            EventManager.StopListening("Intro3", EnableGameObjects);
        }
    
        private void EnableGameObjects(EventManager.EventMessage args)
        {
            foreach (var c in _swords)
            {
                c.gameObject.SetActive(true);
                foreach (var transform in c.GetComponentsInChildren<Transform>())
                {
                    transform.gameObject.SetActive(true);
                }
            }
        }
        
        public void EnableTools()
        {
            foreach (var c in _swords)
            {
                c.gameObject.SetActive(true);
            }
        }
    }
}
