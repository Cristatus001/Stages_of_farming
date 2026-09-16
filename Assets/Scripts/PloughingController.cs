using UnityEngine;

public class PloughingController : MonoBehaviour
{
    public GameObject unploughed; // Reference to the Unploughed GameObject
    public GameObject ploughed;    // Reference to the Ploughed GameObject
    public float ploughingDistance = 0.5f; // Distance to trigger ploughing effect

    void Update()
    {
        MoveTractor();

        // Check for ploughing
        if (Input.GetKeyDown(KeyCode.Space)) // Use Space key to plough
        {
            PloughArea();
        }
    }

    void MoveTractor()
    {
        float moveSpeed = 5f;
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(moveX, moveY, 0);
    }

    void PloughArea()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f); // Adjust radius

        foreach (var collider in colliders)
        {
            if (collider.gameObject == unploughed)
            {
                unploughed.SetActive(false); // Hide unploughed
                ploughed.SetActive(true);    // Show ploughed
                break; // Exit loop after ploughing one area
            }
        }
    }
}
