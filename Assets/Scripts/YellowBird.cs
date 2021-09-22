using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBird : Bird // membuat turunan (inheritance) dari class Bird
{
    [SerializeField]
    public float _boostForce = 100;
    public bool _hasBoost = false;

    // Fungsi Boost adalah untuk memberikan efek boost  ketika burung sedang terbang, dan efek ini hanya dapat digunakan 1 kali saja.
    public void Boost()
    {
        if (State == BirdState.Thrown && !_hasBoost)
        {
            Rigidbody.AddForce(Rigidbody.velocity * _boostForce);
            _hasBoost = true;
        }
    }

    public override void OnTap()
    {
        Boost();
    }

}
