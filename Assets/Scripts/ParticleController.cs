using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particle;
    
    public void SetColor(Color color)
    {
        var main = _particle.main;
        main.startColor = new ParticleSystem.MinMaxGradient(color);
    }
}
