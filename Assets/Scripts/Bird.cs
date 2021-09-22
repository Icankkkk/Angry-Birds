using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bird : MonoBehaviour
{
    // Atribut pada class Bird
    public enum BirdState { Idle, Thrown, HitSomething}
    public Rigidbody2D Rigidbody;
    public CircleCollider2D Collider;

    public UnityAction OnBirdDestroyed = delegate { };
    public UnityAction<Bird> onBirdShot = delegate { };

    public BirdState State { get { return _state; } }
    public GameObject efekMeledak;

    private BirdState _state;
    private float _minVelocity = 0.05f;
    private bool _flagDestroy = false;

    void OnDestroy()
    {
        if (_state == BirdState.Thrown || _state == BirdState.HitSomething)
        {
            OnBirdDestroyed();
           // Instantiate(efekMeledak, transform.position, transform.rotation);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        _state = BirdState.HitSomething;
         Instantiate(efekMeledak, transform.position, transform.rotation);
    }

    // Pada method Start, akan mematikan fungsi physics dan collider dari burung.
    void Start()
    {
        Rigidbody.bodyType = RigidbodyType2D.Kinematic; // mengatur tipe untuk rigidbody 2D
        Collider.enabled = false; // mematikan collider saat game di mulai
        _state = BirdState.Idle;
    }

    /*
     Method FixedUpdate merupakan method bawaan dari Monobehaviour, di mana method tersebut akan terus dipanggil pada setiap fixed frame.
     Dalam method tersebut, kita akan mengubah state dari burung menjadi Thrown, jika kondisi saat ini adalah Idle dan kecepatannya berubah menjadi lebih dari 0.5. 
     Jika kondisi burung pada saat ini adalah Thrown, dan kecepatan burung telah berada di bawah batas minimum (0.5), maka kita akan menghancurkan game object  burung tersebut setelah 2 detik.
    */
    private void FixedUpdate()
    {
        if (_state == BirdState.Idle && Rigidbody.velocity.sqrMagnitude >= _minVelocity)
        {
            _state = BirdState.Thrown;
        }

        if ((_state == BirdState.Thrown || _state == BirdState.HitSomething) && Rigidbody.velocity.sqrMagnitude < _minVelocity && !_flagDestroy)
        {
            // menghancurkan game object setelah 2 detik
            // jika kecepatannya sudah kurang dari batas minimum
            _flagDestroy = true;
            StartCoroutine(DestroyAfter(2));
        }
    }

    private IEnumerator DestroyAfter(float second)
    {
        yield return new WaitForSeconds(second);
        Destroy(gameObject);
    }

    // Method MoveTo berfungsi untuk menginisiasi posisi dan mengubah parent dari game object burung.
    public void MoveTo(Vector2 target, GameObject parent)
    {
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.position = target;
    }

    // Method Shoot berfungsi untuk melemparkan burung dengan arah, jarak tali yang ditarik, dan kecepatan awal. 
    public void Shoot(Vector2 velocity, float distance, float speed)
    {
        Collider.enabled = true; // menghidupkan physics (collider)
        Rigidbody.bodyType = RigidbodyType2D.Dynamic; // atur dengan type dynamic
        Rigidbody.velocity = velocity * speed * distance;
        onBirdShot(this);
    }

    public virtual void OnTap()
    {
        //Do nothing
    }
}
