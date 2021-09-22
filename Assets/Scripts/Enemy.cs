using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public float Health = 50f;

    public UnityAction<GameObject> OnEnemeyDestroyed = delegate { }; // berfungsi untuk memberikan tanda pada saat game object tersebut di-destroy.

    private bool _isHit = false;

    private void OnDestroy()
    {
        if (_isHit)
        {
            OnEnemeyDestroyed(gameObject);
        }
    }

    /*  method OnCollisionEnter2D akan dipanggil, ketika ada collider lain di luar game object Enemy, bersentuhan dengan collider dari game object Enemy.
        Pertama, bila game object asing tersebut tidak memiliki komponen Rigidbody2D maka script ini akan mengabaikannya.
        Jika game object asing memiliki tag Bird, maka Enemy akan segara mati/hancur. Tetapi apabila tag yang dimiliki adalah Obstacle, script ini akan menghitung gaya dari obstacle tersebut pada saat mengenai enemy, 
        kemudian mengurangi health dari enemy. Bila health kurang dari 0, maka musuh akan mati.
    */
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<Rigidbody2D>() == null) return;

        if (col.gameObject.tag == "Bird")
        {
            _isHit = true;
        }
        else if (col.gameObject.tag == "Obstacle")
        {
            // hitung damage yang di peroleh
            float damage = col.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10;
            Health -= damage;

            if (Health <= 0)
            {
                _isHit = true;
                Destroy(gameObject);
            }
        }
    }
}
