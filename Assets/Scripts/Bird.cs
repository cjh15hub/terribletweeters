using System.Collections;
using System.Linq;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField]
    public Color selectedColor;

    [SerializeField]
    public float launchForce = 500;

    [SerializeField]
    public float maxDragDistance = 3f;

    // component references
    private Camera mainCamera;
    private new Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    private Entity[] allEntities;


    public Vector2 startPosition { get; private set; }
    private bool ready;
    private bool onCooldown;

    public bool isDragging { get; private set; }


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        allEntities = FindObjectsOfType<Entity>();

        mainCamera = Camera.main;

        startPosition = rigidbody.position;
    }

    private void Start()
    {
        rigidbody.isKinematic = true;

        ready = true;
    }

    private void Update()
    {
        if (!ready && !onCooldown && !EntitiesAreMoving())
        {
            onCooldown = true;
            StartCoroutine(ResetAfterDelay(3));
        }
    }

    private void FixedUpdate()
    {

    }

    private void OnMouseDown()
    {
        if(ready)
        {
            spriteRenderer.color = selectedColor;

            isDragging = true;
        }
    }

    private void OnMouseUp()
    {
        if(ready)
        {
            spriteRenderer.color = Color.white;

            rigidbody.isKinematic = false;

            // subtract staring position from current position and normilize the result (terms of 1 unit vector)
            Vector2 direction = (startPosition - rigidbody.position).normalized;

            // magnitude represents the pull distance, up to maxDragDistance
            float magnitude = (rigidbody.position - startPosition).magnitude;

            // apply normilized force vector multiplied by magnitude of pull distance 
            rigidbody.AddForce(direction * (launchForce * magnitude));
        }

        isDragging = false;
    }

    private void OnMouseDrag()
    {
        if(ready)
        {
            // copy in mouse position as a Vector2
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 desiredposition = mousePosition;

            // clamp the distance the bird can be dragged away from the start position
            float distance = Vector2.Distance(desiredposition, startPosition);
            if(distance > maxDragDistance)
            {
                // determine the direction being dragged
                Vector2 direction = (desiredposition - startPosition).normalized;
                // multiply the direction by the furthest allowed point, and add on the starting point
                desiredposition = (direction * maxDragDistance) + startPosition; // desiredposition = startPosition + (direction * maxDragDistance); // could also be written like this
            }

            // if drag position is to the right of the starting position; do not allow
            if (desiredposition.x > startPosition.x) desiredposition.x = startPosition.x;

            rigidbody.position = desiredposition;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(ready)
        {
            ready = false;
            StartCoroutine(ResetAfterDelay(15));
        }
        
    }

    private bool EntitiesAreMoving()
    {
        return allEntities.Any(e => e.isActiveAndEnabled && !e.isDead && e.GetComponent<Rigidbody2D>().velocity.magnitude > 0.0001);
    }

    private IEnumerator ResetAfterDelay(int delay)
    {
        yield return new WaitForSeconds(delay);
        rigidbody.isKinematic = true;
        rigidbody.velocity = Vector2.zero;
        rigidbody.position = startPosition;
        onCooldown = false;
        ready = true;

        //StartCoroutine(ReadyAfterDelay());
    }

    private IEnumerator ReadyAfterDelay()
    {
        yield return new WaitForSeconds(.5f);
        ready = true;
    }
}
