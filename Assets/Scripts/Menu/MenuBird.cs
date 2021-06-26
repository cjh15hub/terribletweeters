using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBird : MonoBehaviour
{
    public Vector2 velocity = new Vector2(2.0f, 0);

    // camera values
    private Camera mainCamera;
    private float offScreenLeft = 0;
    private float offScreenRight = 0;
    private float offScreenPadding = 6;

    public void Awake()
    {
        mainCamera = Camera.main;

        offScreenLeft = mainCamera.ScreenToWorldPoint(Vector3.zero).x - offScreenPadding;
        offScreenRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x + offScreenPadding;
    }

    public void Update()
    {
        if (transform.position.x > offScreenRight) transform.position = new Vector2(offScreenLeft, transform.position.y);
        else if (transform.position.x < offScreenLeft) transform.position = new Vector2(offScreenRight, transform.position.y);

        transform.Translate(velocity * Time.deltaTime);
    }
}
