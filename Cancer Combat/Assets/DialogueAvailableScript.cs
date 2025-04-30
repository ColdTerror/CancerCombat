using UnityEngine;

public class DialogueAvailableScript : MonoBehaviour
{
    public float bobSpeed = 2f;      // Adjust this to control the speed of the bobbing
    public float bobHeight = 0.1f;   // Adjust this to control the height of the bob

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition; // Store the original local position
    }
    
    /*
    void Update()
    {
        // Calculate the vertical offset using Mathf.Sin()
        float verticalOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        Vector3 bobDirection = Vector3.up; // Default to bobbing along the local Y-axis

        // Check if the object is rotated around the X-axis (approximately 90 degrees)
        if (transform.localEulerAngles.x != 0f)
        {
            bobDirection = Vector3.forward; // Bob along the local Z-axis
        }

        // Apply the offset in the determined direction
        transform.localPosition = initialPosition + bobDirection * verticalOffset;
    }
    */
    
}