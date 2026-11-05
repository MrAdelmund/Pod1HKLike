using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{


    // Drag your Player GameObject into this slot in the Inspector
    public Transform playerTransform;
    public float rotationSpeed = 5f; // Higher numbers mean faster tracking

    void Update()
    {
        if (playerTransform != null)
        {
            // 1. Get the direction vector from the image to the player
            Vector2 direction = playerTransform.position - transform.position;

            // 2. Calculate the angle in degrees
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // 3. Create the target rotation (adjusting by -90 if your image faces "up" by default)
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 0f);

            // 4. Smoothly rotate towards the player over time
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
