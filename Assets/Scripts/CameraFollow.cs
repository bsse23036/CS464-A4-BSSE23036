using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;
    public Vector3 targetOffset = new Vector3(0f, 1.5f, 0f);
    public float distance = 5.0f;

    [Header("Orbit Sensitivity")]
    public float xSpeed = 120.0f;
    public float ySpeed = 80.0f;

    [Header("Angle Limits")]
    public float yMinLimit = -10.0f;
    public float yMaxLimit = 80.0f;

    private float x = 0.0f;
    private float y = 0.0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        if (!PauseMenu.IsPaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void LateUpdate()
    {
        if (target == null || PauseMenu.IsPaused || Time.timeScale == 0f) return;

        // Re-lock cursor if clicked during active gameplay (e.g., after clicking Game view in Editor)
        if (Input.GetMouseButtonDown(0) && Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
        y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

        y = Mathf.Clamp(y, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 focusPoint = target.position + targetOffset;
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + focusPoint;

        transform.rotation = rotation;
        transform.position = position;
    }
}