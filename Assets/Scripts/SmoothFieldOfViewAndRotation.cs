using UnityEngine;


// Help from Chatgpt has been taken to write and debug the script :)

public class SmoothFieldOfViewAndRotation : MonoBehaviour
{
    public float startFOV = 60f;          // Starting Field of View
    public float endFOV = 39f;            // Ending Field of View
    public float startRotationX = -50f;   // Starting X-axis rotation
    public float endRotationX = 3f;       // Ending X-axis rotation
    public float totalDuration = 3f;      // Total duration of the transitions

    private Camera mainCamera;            
    private float rotationDuration;       // Duration for the rotation phase
    private float fovDuration;            // Duration for the FOV phase

    void Start()
    {
        mainCamera = Camera.main;

        // Calculate the durations for each phase (split equally by default)
        rotationDuration = totalDuration / 2f;
        fovDuration = totalDuration / 2f;

        // Set the starting field of view and rotation
        mainCamera.fieldOfView = startFOV;
        mainCamera.transform.rotation = Quaternion.Euler(startRotationX, mainCamera.transform.rotation.eulerAngles.y, mainCamera.transform.rotation.eulerAngles.z);

        // Start the sequence of transitions
        StartCoroutine(ChangeRotationThenFOV());
    }

    private System.Collections.IEnumerator ChangeRotationThenFOV()
    {
        // Phase 1: Change Rotation
        float elapsedTime = 0f;
        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;

            // Smoothly interpolate the rotation
            float currentRotationX = Mathf.Lerp(startRotationX, endRotationX, elapsedTime / rotationDuration);
            mainCamera.transform.rotation = Quaternion.Euler(currentRotationX, mainCamera.transform.rotation.eulerAngles.y, mainCamera.transform.rotation.eulerAngles.z);

            // Wait for the next frame
            yield return null;
        }

        // Ensure the final rotation value is set precisely
        mainCamera.transform.rotation = Quaternion.Euler(endRotationX, mainCamera.transform.rotation.eulerAngles.y, mainCamera.transform.rotation.eulerAngles.z);

        // Phase 2: Change Field of View
        elapsedTime = 0f;
        while (elapsedTime < fovDuration)
        {
            elapsedTime += Time.deltaTime;

            // Smoothly interpolate the field of view
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, endFOV, elapsedTime / fovDuration);

            // Wait for the next frame
            yield return null;
        }

        // Ensure the final field of view value is set precisely
        mainCamera.fieldOfView = endFOV;
    }
}
