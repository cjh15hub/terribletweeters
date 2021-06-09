using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Bird activeBird;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        activeBird = FindObjectOfType<Bird>();
    }

    private void Start()
    {
        lineRenderer.SetPosition(0, new Vector3(activeBird.startPosition.x, activeBird.startPosition.y, -.001f));
    }

    private void Update()
    {
        if(activeBird.isDragging)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(1, new Vector3(activeBird.transform.position.x, activeBird.transform.position.y, -.001f));
        }
        else
        {
            lineRenderer.enabled = false;
        }
        
    }
}
