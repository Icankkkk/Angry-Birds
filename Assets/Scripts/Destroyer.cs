using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
   
    // pada method ini terdapat pengecekkan terhadap tag dari game object
    // Hanya game object  dengan tag Bird, Enemy, dan Obstacle yang akan hancur bila menyentuh collider game object ini.
    private void OnTriggerEnter2D(Collider2D col)
    {
        string tag = col.gameObject.tag;
        if (tag == "Bird" || tag == "Enemy" || tag == "Obstacle")
        {
            Destroy(col.gameObject);
        }
    }
}
