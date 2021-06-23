using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBird : MonoBehaviour
{
    public float speed = 2.0f;
    public int direction = 1;

    private float screenEnds = 20;

    public void Start()
    {
        
    }

    public void Update()
    {
        if(direction >= 1)
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);

            if (transform.position.x >= screenEnds) transform.position = new Vector2(-1 * screenEnds, transform.position.y);
        }
        else
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);

            if (transform.position.x <= -1 * screenEnds) transform.position = new Vector2(screenEnds, transform.position.y);
        }
    }
}
