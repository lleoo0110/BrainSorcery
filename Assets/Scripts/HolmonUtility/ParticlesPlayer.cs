namespace HolmonUtility
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ParticlesPlayer : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> particles;

        public bool IsPlaying
        {
            get
            {
                foreach (var particle in particles)
                {
                    if (particle.isPlaying)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public void Play()
        {
            foreach (var particle in particles)
            {
                particle.Play();
            }
        }
    }
}
