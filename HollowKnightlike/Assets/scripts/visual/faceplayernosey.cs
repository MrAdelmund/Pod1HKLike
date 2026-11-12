using UnityEngine;

public class faceplayernosey : MonoBehaviour
{
    public class FacePlayerNoZ : MonoBehaviour
    {
        public Transform player; // Assign the player transform in the Inspector
        public float rotationSpeed = 5.0f;

        void Update()
        {
            if (player == null) return;

            // 1. Get direction from this object to the player
            Vector3 direction = player.position - transform.position;
            direction.z = 0f; // Zero out the Z component to prevent tilting on the Z-axis

            if (direction != Vector3.zero)
            {
                // 2. Calculate the target rotation on the Z-axis using Atan2 (for 2D/XY plane)
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

                // 3. Smoothly rotate toward the target rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

}
