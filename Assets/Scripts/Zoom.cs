using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{

    public float zoom = 0;

    private void Start()
    {
        //GetComponent<Rigidbody2D>().AddForce(new Vector2(zoom, 0));
        GetComponent<Rigidbody2D>().velocity = new Vector2(zoom, 0);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Summer Ground") return;
        var rigidbody = GetComponent<Rigidbody2D>();

        var otherRigidbody = collision.rigidbody;

    }
}
