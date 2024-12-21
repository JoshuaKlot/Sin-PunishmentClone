using UnityEngine;

public class MoveAlongTrack : MonoBehaviour
{
    public Transform[] waypoints; // Array of waypoints
    public Quaternion[] rotations; // Array of target rotations
    public GameObject camera;
    public float speed = 5f; // Movement speed
    public float rotationSpeed = 2f; // Rotation speed for smoothing
    private int currentWaypointIndex = 0;

    void Update()
    {
        if (waypoints.Length == 0) return;

        // Move the parent towards the current waypoint
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Smoothly rotate the camera to the target rotation
        if (rotations.Length > 0 && currentWaypointIndex < rotations.Length)
        {
            camera.transform.rotation = Quaternion.Slerp(
                camera.transform.rotation,
                rotations[currentWaypointIndex],
                Time.deltaTime * rotationSpeed
            );
        }

        // Update the child with the CharacterController manually
        foreach (Transform child in transform)
        {
            CharacterController controller = child.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.Move(direction * speed * Time.deltaTime); // Synchronize the movement
            }
        }

        // Check if close enough to the current waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Loop to the next waypoint
        }
    }
}
