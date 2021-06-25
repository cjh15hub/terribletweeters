using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBird : MonoBehaviour
{
    public Vector2 velocity = new Vector2(2.0f, 0);

    private float screenEnds = 20;


    public void Update()
    {
        if (transform.position.x > screenEnds) transform.position = new Vector2(-1 * screenEnds, transform.position.y);
        if (transform.position.x < -1 * screenEnds) transform.position = new Vector2(screenEnds, transform.position.y);

        transform.Translate(velocity * Time.deltaTime);
    }
}
