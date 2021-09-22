using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShooter : MonoBehaviour
{
    public CircleCollider2D Collider;
    public LineRenderer Trajectory;

    // Atribut _startPos digunakan untuk menyimpan titik awal sebelum karet ketapel ditarik.
    private Vector2 _startPos; 

    [SerializeField]
    // Atribut _radius merupakan radius/panjang maksimal dari tali ditarik.
    private float _radius = 0.75f;

    [SerializeField]
    // Atribut _throwSpeed adalah kecepatan awal yang diberikan ketapel pada saat melontarkan burung nantinya
    private float _throwSpeed = 30f;

    private void Start()
    {
        _startPos = transform.position; // mengatur posisi awal
    }

    // membuat object dari class Bird
    private Bird _bird;

    // Pada method OnMouseUp, burung akan kita lemparkan dengan arah (velocity) dan panjangan tarikan ketapel beserta dengan kecepatan awal.
    // Lalu kembalikan posisi tali pelontar ke posisi awal.  Dengan mematikan collider, maka method OnMouseDrag dan OnMouseUp tidak akan dipanggil ketika mouse di klik pada area collider.
    void OnMouseUp()
    {
        Collider.enabled = false;
        Vector2 velocity = _startPos - (Vector2)transform.position;
        float distance = Vector2.Distance(_startPos, transform.position);

        _bird.Shoot(velocity, distance, _throwSpeed);

        // mengembalikan ketapel ke posisi awal
        gameObject.transform.position = _startPos;
        Trajectory.enabled = false;
    }

    public void InitiateBird(Bird bird)
    {
        _bird = bird;
        _bird.MoveTo(gameObject.transform.position, gameObject);
        Collider.enabled = true;
    }

    // Pada method OnMouseDrag, kita akan mengubah posisi dari game object ShooterArea agar mengikuti gerakan mouse,
    // tetapi gerakan tersebut akan terbatas pada radius yang kita tetapkan.
    private void OnMouseDrag()
    {
        // mengubah posisi mouse ke world position
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // hitung supaya 'karet' ketapel berada dalam radius yang ditentukan
        Vector2 dir = pos - _startPos;
        if (dir.sqrMagnitude > _radius)
            dir = dir.normalized * _radius;
        transform.position = _startPos + dir;

        float distance = Vector2.Distance(_startPos, transform.position);

        if (!Trajectory.enabled)
        {
            Trajectory.enabled = true;
        }

        DisplayTrajectory(distance);
    }

    /*
     Method DisplayTrajectory merupakan method yang berfungsi untuk memprediksikan posisi burung
     kemudian menggambarkannya dengan menggunakan LineRenderer. 
         */
    void DisplayTrajectory(float distance)
    {
        if (_bird == null)
        {
            return;
        }

        Vector2 velocity = _startPos - (Vector2)transform.position;
        int segmenCount = 5;
        Vector2[] segments = new Vector2[segmenCount];

        // posisi awal trajectory merupakan posisi mouse dari player saat ini
        segments[0] = transform.position;

        // velocity awal
        Vector2 segVelocity = velocity * _throwSpeed * distance;

        for (int i = 0; i < segmenCount; i++)
        {
            float elapsedTime = i * Time.fixedDeltaTime * 5;
            segments[i] = segments[0] + segVelocity * elapsedTime + 0.5f * Physics2D.gravity * Mathf.Pow(elapsedTime, 2);
        }

        Trajectory.positionCount = segmenCount;
        for (int i = 0; i < segmenCount; i++)
        {
            Trajectory.SetPosition(i, segments[i]);
        }
    }
}
