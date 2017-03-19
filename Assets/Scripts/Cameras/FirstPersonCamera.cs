using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public bool lockCursor;
    public float mouseSensitivty = 5;
    public Transform target;
    public float dstFromTarget = 3f;

    int zoom = 20;
    int normal = 60;
    float smooth = 5f;

    private bool isZoomed = false;

    public Vector2 pitchMinMax = new Vector2(-25, 80);
    public Vector2 yawMinMax = new Vector2(-35, 35);

    public float rotationSmothTime = 0.12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    float yaw;
    float pitch;

    public float distance = 5f;

    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivty;
        //yaw = Mathf.Clamp(yaw, yawMinMax.x, yawMinMax.y);
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivty;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmothTime);
        transform.eulerAngles = currentRotation;
        transform.Translate(Vector3.forward * mouseSensitivty * Time.deltaTime);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);
        transform.position = target.position - transform.forward * dstFromTarget;

        CameraZoom();
    }

    void CameraZoom()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isZoomed = !isZoomed;
        }
        if (isZoomed)
        {
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, zoom, Time.deltaTime * smooth);
        }

        else
        {
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, normal, Time.deltaTime * smooth);
        }
    }

}
